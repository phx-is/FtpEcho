Imports System.Runtime.InteropServices

Public Class frmEditJob

    Private Const PW_CHAR As String = "●" ' Segoe UI U+25CF


    Public Property JobName As String
        Get
            Return txtJobName.Text.Trim
        End Get
        Set(value As String)
            txtJobName.Text = value
        End Set
    End Property

    Public Property MonitorFolder As String
        Get
            Return txtMonitorFolder.Text.Trim
        End Get
        Set(value As String)
            txtMonitorFolder.Text = value
        End Set
    End Property

    Public Property MonitorRoot As String
        Get
            Return txtMonitorRoot.Text.Trim
        End Get
        Set(value As String)
            txtMonitorRoot.Text = value
        End Set
    End Property

    Public Property FtpServer As String
        Get
            Return txtFtpServer.Text.Trim
        End Get
        Set(value As String)
            txtFtpServer.Text = value
        End Set
    End Property

    Public Property FtpUser As String
        Get
            Return txtFtpUser.Text.Trim
        End Get
        Set(value As String)
            txtFtpUser.Text = value
        End Set
    End Property

    Public Property FtpPassword As String
        Get
            Return txtFtpPass.Text.Trim
        End Get
        Set(value As String)
            txtFtpPass.Text = value
        End Set
    End Property

    Public Property FtpRoot As String
        Get
            Return txtFtpRoot.Text.Trim
        End Get
        Set(value As String)
            txtFtpRoot.Text = value
        End Set
    End Property


    Private Sub btnTogglePass_Click(sender As Object, e As EventArgs) Handles btnTogglePass.Click
        If txtFtpPass.PasswordChar = "" Then
            txtFtpPass.PasswordChar = PW_CHAR
        Else
            txtFtpPass.PasswordChar = ""
        End If
        txtFtpPass.Refresh()
    End Sub

    Private Sub btnMonitorBrowse_Click(sender As Object, e As EventArgs) Handles btnMonitorBrowse.Click
        Using poDlg As New FolderBrowserDialog
            poDlg.SelectedPath = "C:\"
            poDlg.Description = "Select folder to monitor for file changes."
            poDlg.ShowNewFolderButton = False
            If poDlg.ShowDialog(Me) Then
                txtMonitorFolder.Text = poDlg.SelectedPath
                txtMonitorRoot.Text = poDlg.SelectedPath
                txtFtpServer.SelectAll()
                txtFtpServer.Focus()
            End If
        End Using
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtJobName.Text.Trim = "" Then
            MsgBox("Job name cannot be blank.", MsgBoxStyle.Exclamation, "Invalid Entry")
            txtJobName.SelectAll()
            txtJobName.Focus()
            Exit Sub
        End If

        If txtMonitorFolder.Text.Trim = "" Then
            MsgBox("Monitor folder cannot be blank.", MsgBoxStyle.Exclamation, "Invalid Entry")
            txtMonitorFolder.SelectAll()
            txtMonitorFolder.Focus()
            Exit Sub
        Else
            If Not System.IO.Directory.Exists(txtMonitorFolder.Text.Trim) Then
                MsgBox("Monitor folder must point to an existing folder on some drive.", MsgBoxStyle.Exclamation, "Invalid Entry")
                txtMonitorFolder.SelectAll()
                txtMonitorFolder.Focus()
                Exit Sub
            End If
        End If

        If txtFtpServer.Text.Trim = "" Or txtFtpUser.Text.Trim = "" Or txtFtpPass.Text.Trim = "" Then
            MsgBox("You must enter some FTP credentials to use.", MsgBoxStyle.Exclamation, "Invalid Entry")
            txtFtpServer.SelectAll()
            txtFtpServer.Focus()
            Exit Sub
        End If

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub





End Class