Imports System.Drawing.Text
Imports System.IO
Imports System.Text

Public Class clsEchoMonitor
    Implements IDisposable

    Public Event JobStatus(ByVal JobInfo As clsEchoJobs.JobDetail, ByVal FileName As String, ByVal Status As JobStatuses, ByVal Message As String)

    Private disposedValue As Boolean


    Public Enum JobStatuses As Integer
        [None] = 0
        [AddedToList] = 1
        [Waiting] = 2
        [Uploading] = 3
        [UploadComplete] = 4
        [UploadFailed] = 5
        [Error] = 6
    End Enum



    Private WithEvents moPoolTimer As Timer
    Private WithEvents FileWatcher As FileSystemWatcher

    Public Structure MonitorInfo
        Public JobInfo As clsEchoJobs.JobDetail
        Public FileWatcher As FileSystemWatcher
        Public AddedFilesList As List(Of String)


    End Structure


    Private moMonitorList As New List(Of MonitorInfo)
    Private moXferList As New List(Of clsNewFile)
    Private miMaxConcurrentXfers As Integer = 10


    Public Sub New()

        moPoolTimer = New Timer
        moPoolTimer.Interval = 1000
        moPoolTimer.Enabled = True
    End Sub


    Public Sub AddJobToList(ByVal oJobData As clsEchoJobs.JobDetail)
        Dim poNew As New MonitorInfo
        poNew.JobInfo = oJobData
        poNew.FileWatcher = New FileSystemWatcher(oJobData.MonitorFolder)
        poNew.FileWatcher.NotifyFilter = NotifyFilters.Attributes _
                            Or NotifyFilters.CreationTime _
                            Or NotifyFilters.DirectoryName _
                            Or NotifyFilters.FileName _
                            Or NotifyFilters.LastAccess _
                            Or NotifyFilters.LastWrite _
                            Or NotifyFilters.Security _
                            Or NotifyFilters.Size
        poNew.FileWatcher.Filter = "*.*"
        poNew.FileWatcher.IncludeSubdirectories = True
        poNew.AddedFilesList = New List(Of String)
        AddHandler poNew.FileWatcher.Created, AddressOf FileWatcher_Created
        SyncLock moMonitorList
            moMonitorList.Add(poNew)
            poNew.FileWatcher.EnableRaisingEvents = True
        End SyncLock
    End Sub

    Public Sub ClearAll()
        Dim piIndex As Integer
        Dim piFtp As Integer
        For piIndex = moMonitorList.Count - 1 To 0 Step -1
            Dim poInfo As MonitorInfo = moMonitorList(piIndex)
            ' remove any in progress xfers
            SyncLock moXferList
                For piFtp = moXferList.Count - 1 To 0 Step -1
                    Dim poFtp As clsNewFile = moXferList(piFtp)
                    If poFtp.JobData.JobName = poInfo.JobInfo.JobName Then
                        Call RemoveXferEventHandlers(poFtp) 'remove handler before abort so event gets discarded
                        poFtp.Abort()
                        moXferList.RemoveAt(piFtp)
                    End If
                Next
            End SyncLock
            ' remove the job itself
            poInfo.FileWatcher.EnableRaisingEvents = False
            RemoveHandler poInfo.FileWatcher.Created, AddressOf FileWatcher_Created
            moMonitorList.RemoveAt(piIndex)
        Next
        moMonitorList.Clear()
    End Sub

    Public Sub RemoveJob(ByVal sJobName As String)
        SyncLock moMonitorList
            Dim piIndex As Integer
            Dim piFtp As Integer
            For piIndex = 0 To moMonitorList.Count - 1
                Dim poInfo As MonitorInfo = moMonitorList(piIndex)
                If poInfo.JobInfo.JobName = sJobName Then
                    ' remove any in progress xfers
                    SyncLock moXferList
                        For piFtp = moXferList.Count - 1 To 0 Step -1
                            Dim poFtp As clsNewFile = moXferList(piFtp)
                            If poFtp.JobData.JobName = poInfo.JobInfo.JobName Then
                                poFtp.Abort()
                                Call RemoveXferEventHandlers(poFtp) 'remove handler after abort so event gets raised
                                moXferList.RemoveAt(piFtp)
                            End If
                        Next
                    End SyncLock
                    ' remove the job itself
                    poInfo.FileWatcher.EnableRaisingEvents = False
                    RemoveHandler poInfo.FileWatcher.Created, AddressOf FileWatcher_Created
                    moMonitorList.RemoveAt(piIndex)
                    Exit For
                End If
            Next
        End SyncLock
    End Sub


    Private Sub FileWatcher_Created(sender As Object, e As FileSystemEventArgs) Handles FileWatcher.Created
        Dim poFile As FileInfo = New FileInfo(e.FullPath)
        If poFile.Attributes.HasFlag(FileAttributes.Directory) Then
            ' skip this one
            Exit Sub
        End If
        ' not a directory, add to list
        SyncLock moMonitorList
            For piIndex = 0 To moMonitorList.Count - 1
                Dim poInfo As MonitorInfo = moMonitorList(piIndex)
                If poInfo.FileWatcher Is sender Then
                    ' add this file to the list 
                    If Not poInfo.AddedFilesList.Contains(e.FullPath) Then
                        poInfo.AddedFilesList.Add(e.FullPath)
                        moMonitorList(piIndex) = poInfo
                        RaiseEvent JobStatus(poInfo.JobInfo, e.FullPath, JobStatuses.AddedToList, "Job added to list")
                    End If
                    Exit For
                End If
            Next
        End SyncLock
    End Sub

    Private Sub moPoolTimer_Tick(sender As Object, e As EventArgs) Handles moPoolTimer.Tick
        ' see if there are unprocessed files in the added list
        Dim piIndex As Integer
        SyncLock moMonitorList
            For piIndex = 0 To moMonitorList.Count - 1
                Dim poInfo As MonitorInfo = moMonitorList(piIndex)
                If poInfo.AddedFilesList.Count > 0 And moXferList.Count <= miMaxConcurrentXfers Then
                    Dim poNewFile As New clsNewFile(poInfo.AddedFilesList(0), poInfo.JobInfo)
                    Call AddXferEventHandlers(poNewFile)
                    moXferList.Add(poNewFile)
                    poNewFile.StartProcess()
                    ' take one off the list and update it, if there's more than on it'll get caught next time timer fires
                    poInfo.AddedFilesList.RemoveAt(0)
                    moMonitorList(piIndex) = poInfo
                End If
            Next
        End SyncLock
    End Sub







    '===========================================================================================================================
    '===========================================================================================================================
    '===========================================================================================================================
    '===========================================================================================================================
    '===========================================================================================================================
    '===========================================================================================================================
#Region "  clsNewFile Event Handlers "
    Private Sub AddXferEventHandlers(ByRef oNewFile As clsNewFile)
        AddHandler oNewFile.JobStarted, AddressOf NewFile_JobStarted
        AddHandler oNewFile.JobError, AddressOf NewFile_JobError
        AddHandler oNewFile.JobWaiting, AddressOf NewFile_JobWaiting
        AddHandler oNewFile.JobUploading, AddressOf NewFile_JobUploading
        AddHandler oNewFile.JobComplete, AddressOf NewFile_JobComplete
    End Sub

    Private Sub RemoveXferEventHandlers(ByRef oNewFile As clsNewFile)
        RemoveHandler oNewFile.JobStarted, AddressOf NewFile_JobStarted
        RemoveHandler oNewFile.JobError, AddressOf NewFile_JobError
        RemoveHandler oNewFile.JobWaiting, AddressOf NewFile_JobWaiting
        RemoveHandler oNewFile.JobUploading, AddressOf NewFile_JobUploading
        RemoveHandler oNewFile.JobComplete, AddressOf NewFile_JobComplete
    End Sub



    Private Sub NewFile_JobStarted(ByVal Sender As clsNewFile)
        RaiseEvent JobStatus(Sender.JobData, Sender.FileName, JobStatuses.Waiting, "FTP Job started: " & Sender.FileName & ".")
    End Sub

    Private Sub NewFile_JobError(ByVal Sender As clsNewFile, ByVal ErrorMessage As String)
        goLogger.LogError("clsEchoMonitor.JobError", Sender.JobData.JobName & "(" & Sender.FileName & ") " & ErrorMessage)
        RaiseEvent JobStatus(Sender.JobData, Sender.FileName, JobStatuses.Error, ErrorMessage)
    End Sub

    Private Sub NewFile_JobWaiting(ByVal Sender As clsNewFile)
        RaiseEvent JobStatus(Sender.JobData, Sender.FileName, JobStatuses.Waiting, "FTP waiting for file upload to complete: " & Sender.FileName & ".")
    End Sub

    Private Sub NewFile_JobUploading(ByVal Sender As clsNewFile, PercentComplete As Integer)
        RaiseEvent JobStatus(Sender.JobData, Sender.FileName, JobStatuses.Uploading, "FTP file echo uploading: " & PercentComplete & "%.")
    End Sub

    Private Sub NewFile_JobComplete(ByVal Sender As clsNewFile, Success As Boolean, LocalFile As String, UploadPath As String, ElapsedSeconds As Long, ErrorMsg As String)
        Dim psTemp As String
        If Success Then
            psTemp = String.Format("FTP completed successfully in {0} for file {1}.", FullDisplayElapsed(ElapsedSeconds), UploadPath)
            RaiseEvent JobStatus(Sender.JobData, Sender.FileName, JobStatuses.UploadComplete, psTemp)
        Else
            psTemp = String.Format("FTP FAILED for file {0}, {1}", UploadPath, ErrorMsg)
            RaiseEvent JobStatus(Sender.JobData, Sender.FileName, JobStatuses.UploadFailed, psTemp)
        End If
    End Sub





#End Region


#Region "  iDisposable implementation code "

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
            End If
            Try
                Dim piIndex As Integer
                Dim piFtp As Integer
                For piIndex = moMonitorList.Count - 1 To 0 Step -1
                    Dim poInfo As MonitorInfo = moMonitorList(piIndex)
                    ' remove any in progress xfers
                    For piFtp = moXferList.Count - 1 To 0 Step -1
                        Dim poFtp As clsNewFile = moXferList(piFtp)
                        If poFtp.JobData.JobName = poInfo.JobInfo.JobName Then
                            ' remove handlers before abort to discard the event
                            Call RemoveXferEventHandlers(poFtp)
                            poFtp.Abort()
                            moXferList.RemoveAt(piFtp)
                        End If
                    Next
                    ' remove the job itself
                    poInfo.FileWatcher.EnableRaisingEvents = False
                    RemoveHandler poInfo.FileWatcher.Created, AddressOf FileWatcher_Created
                    moMonitorList.RemoveAt(piIndex)
                Next
            Catch ex As Exception
                ' don't care at this point
            End Try

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
