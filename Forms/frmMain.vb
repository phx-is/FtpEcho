Imports System.IO

Public Class frmMain
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        frmEditJob.JobName = "New Job"
        frmEditJob.MonitorFolder = ""
        frmEditJob.MonitorRoot = ""
        frmEditJob.FtpServer = "96.95.143.241"
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

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        modMain.Initialize()

        imlSmall.Images.Clear()
        imlSmall.ImageSize = New Size(24, 24)
        imlSmall.ColorDepth = ColorDepth.Depth32Bit
        imlSmall.Images.Add("icon_24", My.Resources.ftp_icon_24)
        imlSmall.Images.Add("ready", My.Resources.check)
        imlSmall.Images.Add("running", My.Resources.nav_right_yellow)
        imlSmall.Images.Add("error", My.Resources.delete)


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




        Call LoadJobsList
    End Sub



    Private Sub LoadJobsList()

        lvwJobs.SuspendLayout()
        lvwJobs.Items.Clear()
        For Each poJob As clsEchoJobs.JobDetail In goJobList.JobsList
            Dim poItem As ListViewItem
            poItem = lvwJobs.Items.Add(poJob.JobName, poJob.JobName, "icon_24")
            poItem.Tag = poJob.JobName
        Next
        lvwJobs.ResumeLayout(True)
    End Sub



End Class


'