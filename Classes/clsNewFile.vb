Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Net
Imports System.Net.Configuration

Public Class clsNewFile
    Implements IDisposable

    Public Event JobStarted(ByVal Sender As clsNewFile)
    Public Event JobError(ByVal Sender As clsNewFile, ByVal ErrorMessage As String)
    Public Event JobWaiting(ByVal Sender As clsNewFile)
    Public Event BeginningUpload(ByVal Sender As clsNewFile, ByVal FileSize As Long)
    Public Event JobUploading(ByVal Sender As clsNewFile, PercentComplete As Integer)
    Public Event JobComplete(ByVal Sender As clsNewFile, Success As Boolean, LocalFile As String, UploadPath As String, ElapsedSeconds As Long, ErrorMsg As String)


    Private Enum FileStatus As Integer
        [None] = 0
        [Added] = 1
        [Monitoring] = 2
        [BeginningUpload] = 3
        [Uploading] = 4
        [UploadingComplete] = 5
    End Enum

    Private moJobData As clsEchoJobs.JobDetail
    Private msFilename As String
    Private miCurrentStatus As FileStatus
    Private mlLastFileSize As Long
    Private moLastInfo As FileInfo

    Private moMonitorThread As System.Threading.Thread
    Private WithEvents moFtp As clsFtp
    Private disposedValue As Boolean

    Public ReadOnly Property FileName As String
        Get
            Return msFilename
        End Get
    End Property

    Public ReadOnly Property JobData As clsEchoJobs.JobDetail
        Get
            Return moJobData
        End Get
    End Property

    Public Sub New(ByVal NewFilename As String, ByVal JobData As clsEchoJobs.JobDetail)
        msFilename = NewFilename
        moJobData = JobData
        miCurrentStatus = FileStatus.Added
        mlLastFileSize = 0
        moFtp = New clsFtp(moJobData.FtpServer, moJobData.FtpLogin, moJobData.FtpPassword)
    End Sub

    Public Sub StartProcess()
        moLastInfo = New FileInfo(FileName)
        miCurrentStatus = FileStatus.Added
        moMonitorThread = New System.Threading.Thread(AddressOf MonitorFileStatus)
        moMonitorThread.IsBackground = True
        moMonitorThread.Start()
    End Sub

    Public Sub Abort()
        Try
            If moMonitorThread IsNot Nothing AndAlso moMonitorThread.IsAlive Then
                moMonitorThread.Abort()
            End If
        Catch ex As Exception
        Finally
            moMonitorThread = Nothing
        End Try
        RaiseEvent JobError(Me, "Job aborted by user request")
    End Sub

    Private Sub moFtp_UploadComplete(Sender As clsFtp, Success As Boolean, LocalFile As String, UploadPath As String, ElapsedSeconds As Long, ErrorMsg As String) Handles moFtp.UploadComplete
        miCurrentStatus = FileStatus.UploadingComplete
        RaiseEvent JobComplete(Me, Success, LocalFile, UploadPath, ElapsedSeconds, ErrorMsg)
    End Sub

    Private Sub moFtp_UploadProgress(Sender As clsFtp, PercentComplete As Integer) Handles moFtp.UploadProgress
        RaiseEvent JobUploading(Me, PercentComplete)
    End Sub


    Private Sub MonitorFileStatus()
        Dim pbDone As Boolean = False
        Dim ptDelayStart As Date = Date.Now
        Dim ptMonitorStart As Date

        RaiseEvent JobStarted(Me)

        Try
            Do While Not pbDone
                Select Case miCurrentStatus
                    Case FileStatus.Added
                        RaiseEvent JobWaiting(Me)
                        mlLastFileSize = 0
                        miCurrentStatus = FileStatus.Monitoring
                        ptDelayStart = DateTime.Now
                        ptMonitorStart = DateTime.Now

                    Case FileStatus.Monitoring
                        If mlLastFileSize < moLastInfo.Length OrElse FileIsLocked(moLastInfo) Then
                            mlLastFileSize = moLastInfo.Length
                            moLastInfo.Refresh()
                            ptDelayStart = DateTime.Now
                            'timeout?
                            If DateTime.Now.Subtract(ptMonitorStart).TotalMinutes > 60 Then
                                RaiseEvent JobError(Me, "Timeout waiting for incoming FTP to complete: " & moLastInfo.FullName & ".")
                                miCurrentStatus = FileStatus.UploadingComplete
                            End If
                        Else
                            ' filesize >= current size and not locked so start the upload!
                            miCurrentStatus = FileStatus.BeginningUpload
                        End If

                    Case FileStatus.BeginningUpload
                        RaiseEvent BeginningUpload(Me, mlLastFileSize)

                        ' build output filename
                        Dim psDestFile As String
                        Dim psDirectoryName As String
                        Dim psPathRoot As String
                        Dim psRemoteURI As String
                        Dim psBaseFolder As String
                        Dim piPos As Integer

                        ' Directory name = C:\temp2\test\backup0004
                        psDirectoryName = System.IO.Path.GetDirectoryName(msFilename)
                        ' Path root = C:\
                        psPathRoot = System.IO.Path.GetPathRoot(msFilename)
                        ' start with comlete filename including windows path
                        psDestFile = msFilename
                        ' keep what's been requested from source file path and name if root specified
                        If moJobData.MonitorRootFolder.Length > 0 Then
                            psDestFile = psDestFile.Replace(moJobData.MonitorRootFolder, "")
                        End If
                        ' remove drive letter if it's still there
                        psDestFile = psDestFile.Replace(psPathRoot, "")
                        ' dest file should now be something like "startingfolder1\folder2\filename.ext", make sure no beginning slashie
                        If psDestFile.StartsWith("\") Or psDestFile.StartsWith("/") Then
                            psDestFile = psDestFile.Substring(1)
                        End If
                        ' now tack on our ftp root value
                        If moJobData.FtpRootFolder = "" Then
                            psDestFile = "/" & psDestFile
                        Else
                            If moJobData.FtpRootFolder.EndsWith("/") Or moJobData.FtpRootFolder.EndsWith("\") Then
                                psDestFile = moJobData.FtpRootFolder & psDestFile
                            Else
                                psDestFile = moJobData.FtpRootFolder & "/" & psDestFile
                            End If
                        End If
                        ' replace windows slashies with unix slashies
                        psDestFile = psDestFile.Replace("\", "/")


                        psRemoteURI = moJobData.FtpServer & psDestFile
                        ' make base folder 
                        piPos = psRemoteURI.LastIndexOf("/")
                        psBaseFolder = psRemoteURI.Substring(0, piPos)
                        If moFtp.FolderCreate(psBaseFolder, True) Then
                            ' folder exists or was created, start the upload 
                            RaiseEvent JobUploading(Me, 0)
                            moFtp.UploadFile(msFilename, psRemoteURI, True)
                            miCurrentStatus = FileStatus.Uploading
                        Else
                            Dim psTemp As String = "clsNewFile.MonitorFIleStatus - Unable to create folder path on destination FTP server: {0}Folder {1}{0}Code: {2} {3}"
                            psTemp = String.Format(psTemp, vbCrLf, psBaseFolder, moFtp.LastStatusCode, moFtp.LastResponseMessage)
                            goLogger.LogEntry(psTemp, EventLogEntryType.Warning)
                            RaiseEvent JobError(Me, "Unable to create folder path on destination FTP server: " & psBaseFolder & " status code was: " & moFtp.LastStatusCode & " " & moFtp.LastResponseMessage)
                            miCurrentStatus = FileStatus.UploadingComplete
                        End If

                    Case FileStatus.Uploading
                        ' do nothing, we're waiting for the FTP to complete

                    Case FileStatus.UploadingComplete
                        pbDone = True
                End Select
                If Not pbDone Then
                    System.Threading.Thread.Sleep(500)
                End If
            Loop


        Catch ThreadEx As System.Threading.ThreadAbortException
            ' don't care

        Catch ex As Exception
            goLogger.LogException("clsNewFile.MonitorFileStatus(" & msFilename & ")", ex)
        End Try
        moMonitorThread = Nothing
    End Sub


    Private Function FileIsLocked(ByVal FileData As FileInfo) As Boolean
        Try
            Using stream As FileStream = FileData.Open(FileMode.Open, FileAccess.Read, FileShare.None)
                stream.Close()
            End Using

        Catch Ex As IOException
            Return True

        Catch BaseEx As Exception
            goLogger.LogException("clsNewFile.FileIsLocked(" & FileData.FullName & ")", BaseEx)
            Return True
        End Try

        Return False
    End Function


#Region "  iDisposable Code "
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
                Try
                    If moMonitorThread IsNot Nothing AndAlso moMonitorThread.IsAlive Then
                        moMonitorThread.Abort()
                    End If
                Catch ex As Exception
                    'don't care
                Finally
                    moMonitorThread = Nothing
                End Try
                Try
                    moFtp = Nothing
                Catch ex As Exception
                    ' don't care
                End Try
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region






End Class











'
