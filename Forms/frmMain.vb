Imports System.IO

Public Class frmMain


    Private WithEvents foMonitorList As New clsEchoMonitor

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        frmEditJob.LockJobName = False
        frmEditJob.JobName = "New Job"
        frmEditJob.MonitorFolder = ""
        frmEditJob.MonitorRoot = ""
        frmEditJob.FtpServer = "ftp://96.95.143.241"
        frmEditJob.FtpUser = "BackupLogin"
        frmEditJob.FtpPassword = "3upHa2rA_7ufr;r!za+&"
        frmEditJob.FtpRoot = "/"
        If frmEditJob.ShowDialog(Me) = DialogResult.OK Then
            If goJobList.AddJob(frmEditJob.JobName, frmEditJob.MonitorFolder, frmEditJob.MonitorRoot, frmEditJob.FtpServer, frmEditJob.FtpUser, frmEditJob.FtpPassword, frmEditJob.FtpRoot) Then
                goConfig.ConfigValue("EchoJobs") = goJobList.JobsListXML
                goConfig.SaveConfigFile(GetConfigFileName())
                Call LoadJobsList()
            Else
                MsgBox("Job names must be unique.", MsgBoxStyle.Exclamation, "Duplicate Job Name")
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If lvwJobs.SelectedItems.Count = 1 Then
            Dim psName As String = lvwJobs.SelectedItems(0).Tag
            Dim poJD As clsEchoJobs.JobDetail = goJobList.GetJob(psName)
            If poJD.JobName <> "" Then
                If MsgBox("Are you sure you wish to delete the job '" & psName & "'?", MsgBoxStyle.YesNo, "Confirm Job Deletion") = MsgBoxResult.Yes Then
                    If goJobList.RemoveJob(poJD.JobName) Then
                        goConfig.ConfigValue("EchoJobs") = goJobList.JobsListXML
                        goConfig.SaveConfigFile(GetConfigFileName())
                        Call LoadJobsList()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If lvwJobs.SelectedItems.Count = 1 Then
            Dim psName As String = lvwJobs.SelectedItems(0).Tag
            Dim poJD As clsEchoJobs.JobDetail = goJobList.GetJob(psName)
            If poJD.JobName <> "" Then
                frmEditJob.LockJobName = True
                frmEditJob.JobName = poJD.JobName
                frmEditJob.MonitorFolder = poJD.MonitorFolder
                frmEditJob.MonitorRoot = poJD.MonitorRootFolder
                frmEditJob.FtpServer = poJD.FtpServer
                frmEditJob.FtpUser = poJD.FtpLogin
                frmEditJob.FtpPassword = poJD.FtpPassword
                frmEditJob.FtpRoot = poJD.FtpRootFolder
                If frmEditJob.ShowDialog(Me) = DialogResult.OK Then
                    If goJobList.UpdateJob(poJD.JobName, frmEditJob.MonitorFolder, frmEditJob.MonitorRoot, frmEditJob.FtpServer, frmEditJob.FtpUser, frmEditJob.FtpPassword, frmEditJob.FtpRoot) Then
                        goConfig.ConfigValue("EchoJobs") = goJobList.JobsListXML
                        goConfig.SaveConfigFile(GetConfigFileName())
                        Call LoadJobsList()
                    Else
                        MsgBox("Failed to update job data.", MsgBoxStyle.Exclamation, "Error Updating Job")
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnLog_Click(sender As Object, e As EventArgs) Handles btnLog.Click
        Try
            Process.Start("explorer.exe", "/root," & Chr(34) & GetConfigPath() & "\" & Chr(34))
        Catch ex As Exception
            MsgBox("Unable to open log file folder: " & GetConfigPath(), MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Error starting explorer.")
        End Try
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initailize will load the jobs list from disk XML file
        modMain.Initialize()

        ' setup our imagelist
        imlSmall.Images.Clear()
        imlSmall.ImageSize = New Size(24, 24)
        imlSmall.ColorDepth = ColorDepth.Depth32Bit
        imlSmall.Images.Add("icon_24", My.Resources.ftp_icon_24)
        imlSmall.Images.Add("ready", My.Resources.check)
        imlSmall.Images.Add("running", My.Resources.nav_right_yellow)
        imlSmall.Images.Add("error", My.Resources.delete)
        imlSmall.Images.Add("wait", My.Resources.hourglass)
        ' setup listviews
        With lvwJobs
            .View = View.Details
            .MultiSelect = False
            .FullRowSelect = True
            .GridLines = False
            .Items.Clear()
            .SmallImageList = imlSmall
            .Columns.Clear()
            .Columns.Add("JOB", "Job Name", .Width - 30)
        End With

        With lvwJobStatus
            .View = View.Details
            .MultiSelect = False
            .FullRowSelect = True
            .GridLines = False
            .Items.Clear()
            .SmallImageList = imlSmall
            .Columns.Clear()
            .Columns.Add("JOBNAME", "Job Name", 300)
            .Columns.Add("Status", "Current Status", .Width - 330)
        End With

        ' Load the jobs list into the left-hand list
        Call LoadJobsList()
    End Sub



    Private Sub LoadJobsList()
        ' clear job status first
        lvwJobStatus.SuspendLayout()
        lvwJobStatus.Items.Clear()
        foMonitorList.ClearAll()
        lvwJobStatus.ResumeLayout(True)
        ' now build jobs list 
        lvwJobs.SuspendLayout()
        lvwJobs.Items.Clear()
        For Each poJob As clsEchoJobs.JobDetail In goJobList.JobsList
            Dim poItem As ListViewItem
            poItem = lvwJobs.Items.Add(poJob.JobName, poJob.JobName, "icon_24")
            poItem.Tag = poJob.JobName
            ' add job to monitor list
            foMonitorList.AddJobToList(poJob)
        Next
        lvwJobs.ResumeLayout(True)
    End Sub

    Private Delegate Sub Del_foMonitorList_JobStatus(JobInfo As clsEchoJobs.JobDetail, FileName As String, Status As clsEchoMonitor.JobStatuses, Message As String)
    Private Sub foMonitorList_JobStatus(JobInfo As clsEchoJobs.JobDetail, FileName As String, Status As clsEchoMonitor.JobStatuses, Message As String) Handles foMonitorList.JobStatus
        If lvwJobStatus.InvokeRequired Then
            lvwJobStatus.Invoke(New Del_foMonitorList_JobStatus(AddressOf foMonitorList_JobStatus), New Object() {JobInfo, FileName, Status, Message})
        Else
            Dim poJobLvItem As ListViewItem = Nothing
            For Each poLvItem As ListViewItem In lvwJobStatus.Items
                If poLvItem.Tag = FileName Then
                    poJobLvItem = poLvItem
                    Exit For
                End If
            Next
            If Status = clsEchoMonitor.JobStatuses.AddedToList Then
                If poJobLvItem Is Nothing Then
                    ' new item 
                    poJobLvItem = lvwJobStatus.Items.Add(FileName, JobInfo.JobName, "wait")
                    poJobLvItem.SubItems.Add("New file detected in monitored folder: " & FileName)
                    poJobLvItem.Tag = FileName
                End If
            Else
                If poJobLvItem IsNot Nothing Then
                    Select Case Status

                        Case clsEchoMonitor.JobStatuses.Waiting
                            poJobLvItem.SubItems(1).Text = "Waiting for incoming FTP to complete."
                            poJobLvItem.ImageKey = "wait"

                        Case clsEchoMonitor.JobStatuses.Uploading
                            poJobLvItem.SubItems(1).Text = Message
                            poJobLvItem.ImageKey = "running"

                        Case clsEchoMonitor.JobStatuses.UploadComplete
                            poJobLvItem.SubItems(1).Text = Message
                            poJobLvItem.ImageKey = "ready"
                            goLogger.LogEntry("FTP Echo complete - " & JobInfo.JobName & " (" & FileName & " )", EventLogEntryType.Information)

                        Case clsEchoMonitor.JobStatuses.UploadFailed
                            poJobLvItem.SubItems(1).Text = Message
                            poJobLvItem.ImageKey = "error"
                            goLogger.LogEntry("FTP Echo FAILED - " & JobInfo.JobName & " (" & FileName & " )", EventLogEntryType.Warning)

                        Case clsEchoMonitor.JobStatuses.Error
                            poJobLvItem.SubItems(1).Text = Message
                            poJobLvItem.ImageKey = "error"
                            goLogger.LogEntry("FTP Echo error: " & JobInfo.JobName & " (" & FileName & " ): " & Message, EventLogEntryType.Error)

                        Case Else
                            goLogger.LogEntry("frmMain.foMonitorList_JobStatus - Unhandled status event from clsEchoMonitor(" & Status & "): " & FileName, EventLogEntryType.Warning)
                    End Select
                Else
                    goLogger.LogEntry("frmMain.foMonitorList_JobStatus - Failed to locate filename in listview for update: " & FileName, EventLogEntryType.Warning)
                End If
            End If
        End If


    End Sub
End Class










'