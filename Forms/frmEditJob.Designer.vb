<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditJob
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditJob))
        Me.txtJobName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMonitorFolder = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtMonitorRoot = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtFtpUser = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtFtpServer = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFtpPass = New System.Windows.Forms.TextBox()
        Me.btnMonitorBrowse = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnTogglePass = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtFtpRoot = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtJobName
        '
        Me.txtJobName.Location = New System.Drawing.Point(68, 33)
        Me.txtJobName.Name = "txtJobName"
        Me.txtJobName.Size = New System.Drawing.Size(368, 29)
        Me.txtJobName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(68, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 21)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "&Job Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(154, 21)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "&Monitor Local Folder"
        '
        'txtMonitorFolder
        '
        Me.txtMonitorFolder.Location = New System.Drawing.Point(8, 108)
        Me.txtMonitorFolder.Name = "txtMonitorFolder"
        Me.txtMonitorFolder.Size = New System.Drawing.Size(393, 29)
        Me.txtMonitorFolder.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 140)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(151, 21)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Monitor &Root Folder"
        '
        'txtMonitorRoot
        '
        Me.txtMonitorRoot.Location = New System.Drawing.Point(8, 164)
        Me.txtMonitorRoot.Name = "txtMonitorRoot"
        Me.txtMonitorRoot.Size = New System.Drawing.Size(389, 29)
        Me.txtMonitorRoot.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 280)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 21)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "FTP &User"
        '
        'txtFtpUser
        '
        Me.txtFtpUser.Location = New System.Drawing.Point(8, 304)
        Me.txtFtpUser.Name = "txtFtpUser"
        Me.txtFtpUser.Size = New System.Drawing.Size(140, 29)
        Me.txtFtpUser.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 224)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(138, 21)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "FTP Upload &Server"
        '
        'txtFtpServer
        '
        Me.txtFtpServer.Location = New System.Drawing.Point(8, 248)
        Me.txtFtpServer.Name = "txtFtpServer"
        Me.txtFtpServer.Size = New System.Drawing.Size(389, 29)
        Me.txtFtpServer.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(154, 280)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(105, 21)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "FTP &Password"
        '
        'txtFtpPass
        '
        Me.txtFtpPass.Location = New System.Drawing.Point(154, 304)
        Me.txtFtpPass.Name = "txtFtpPass"
        Me.txtFtpPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtFtpPass.Size = New System.Drawing.Size(215, 29)
        Me.txtFtpPass.TabIndex = 12
        '
        'btnMonitorBrowse
        '
        Me.btnMonitorBrowse.BackgroundImage = Global.FtpEcho.My.Resources.Resources.folder_view
        Me.btnMonitorBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnMonitorBrowse.Location = New System.Drawing.Point(407, 108)
        Me.btnMonitorBrowse.Name = "btnMonitorBrowse"
        Me.btnMonitorBrowse.Size = New System.Drawing.Size(29, 29)
        Me.btnMonitorBrowse.TabIndex = 5
        Me.btnMonitorBrowse.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.FtpEcho.My.Resources.Resources.ftp_icon_64
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnTogglePass
        '
        Me.btnTogglePass.BackgroundImage = Global.FtpEcho.My.Resources.Resources.eye
        Me.btnTogglePass.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnTogglePass.Location = New System.Drawing.Point(364, 304)
        Me.btnTogglePass.Name = "btnTogglePass"
        Me.btnTogglePass.Size = New System.Drawing.Size(29, 29)
        Me.btnTogglePass.TabIndex = 14
        Me.btnTogglePass.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 336)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 21)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "FTP R&oot Folder"
        '
        'txtFtpRoot
        '
        Me.txtFtpRoot.Location = New System.Drawing.Point(8, 360)
        Me.txtFtpRoot.Name = "txtFtpRoot"
        Me.txtFtpRoot.Size = New System.Drawing.Size(389, 29)
        Me.txtFtpRoot.TabIndex = 15
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(133, 439)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(150, 50)
        Me.btnSave.TabIndex = 17
        Me.btnSave.Text = "Sa&ve"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(289, 439)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(150, 50)
        Me.btnCancel.TabIndex = 18
        Me.btnCancel.Text = "Ca&ncel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmEditJob
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(451, 501)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtFtpRoot)
        Me.Controls.Add(Me.btnTogglePass)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtFtpPass)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtFtpUser)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtFtpServer)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtMonitorRoot)
        Me.Controls.Add(Me.btnMonitorBrowse)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtMonitorFolder)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtJobName)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEditJob"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FTP Echo Job"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtJobName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtMonitorFolder As TextBox
    Friend WithEvents btnMonitorBrowse As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents txtMonitorRoot As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtFtpUser As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtFtpServer As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtFtpPass As TextBox
    Friend WithEvents btnTogglePass As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents txtFtpRoot As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
End Class
