<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tspJobs = New System.Windows.Forms.ToolStrip()
        Me.btnAdd = New System.Windows.Forms.ToolStripButton()
        Me.btnEdit = New System.Windows.Forms.ToolStripButton()
        Me.btnDelete = New System.Windows.Forms.ToolStripButton()
        Me.lvwJobs = New System.Windows.Forms.ListView()
        Me.lvwJobStatus = New System.Windows.Forms.ListView()
        Me.btnLog = New System.Windows.Forms.ToolStripButton()
        Me.imlSmall = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tspJobs.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.Color.Black
        Me.SplitContainer1.Panel1.Controls.Add(Me.lvwJobs)
        Me.SplitContainer1.Panel1.Controls.Add(Me.tspJobs)
        Me.SplitContainer1.Panel1.ForeColor = System.Drawing.Color.White
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lvwJobStatus)
        Me.SplitContainer1.Size = New System.Drawing.Size(1200, 727)
        Me.SplitContainer1.SplitterDistance = 260
        Me.SplitContainer1.TabIndex = 0
        '
        'tspJobs
        '
        Me.tspJobs.BackColor = System.Drawing.Color.DarkSlateGray
        Me.tspJobs.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tspJobs.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tspJobs.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.tspJobs.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAdd, Me.btnEdit, Me.btnDelete, Me.btnLog})
        Me.tspJobs.Location = New System.Drawing.Point(0, 0)
        Me.tspJobs.Name = "tspJobs"
        Me.tspJobs.Size = New System.Drawing.Size(260, 39)
        Me.tspJobs.TabIndex = 0
        Me.tspJobs.Text = "ToolStrip1"
        '
        'btnAdd
        '
        Me.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAdd.Image = Global.FtpEcho.My.Resources.Resources.add2
        Me.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(36, 36)
        '
        'btnEdit
        '
        Me.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEdit.Image = Global.FtpEcho.My.Resources.Resources.edit
        Me.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(36, 36)
        '
        'btnDelete
        '
        Me.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnDelete.Image = Global.FtpEcho.My.Resources.Resources.delete2
        Me.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(36, 36)
        '
        'lvwJobs
        '
        Me.lvwJobs.BackColor = System.Drawing.Color.Black
        Me.lvwJobs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwJobs.ForeColor = System.Drawing.Color.White
        Me.lvwJobs.HideSelection = False
        Me.lvwJobs.Location = New System.Drawing.Point(0, 39)
        Me.lvwJobs.Name = "lvwJobs"
        Me.lvwJobs.Size = New System.Drawing.Size(260, 688)
        Me.lvwJobs.TabIndex = 1
        Me.lvwJobs.UseCompatibleStateImageBehavior = False
        '
        'lvwJobStatus
        '
        Me.lvwJobStatus.BackColor = System.Drawing.Color.Black
        Me.lvwJobStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwJobStatus.ForeColor = System.Drawing.Color.White
        Me.lvwJobStatus.HideSelection = False
        Me.lvwJobStatus.Location = New System.Drawing.Point(0, 0)
        Me.lvwJobStatus.Name = "lvwJobStatus"
        Me.lvwJobStatus.Size = New System.Drawing.Size(936, 727)
        Me.lvwJobStatus.TabIndex = 2
        Me.lvwJobStatus.UseCompatibleStateImageBehavior = False
        '
        'btnLog
        '
        Me.btnLog.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnLog.Image = Global.FtpEcho.My.Resources.Resources.folder_view
        Me.btnLog.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLog.Name = "btnLog"
        Me.btnLog.Size = New System.Drawing.Size(36, 36)
        Me.btnLog.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnLog.ToolTipText = "View log file"
        '
        'imlSmall
        '
        Me.imlSmall.ImageStream = CType(resources.GetObject("imlSmall.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlSmall.TransparentColor = System.Drawing.Color.Transparent
        Me.imlSmall.Images.SetKeyName(0, "ftp_icon")
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 727)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FtpEcho"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.tspJobs.ResumeLayout(False)
        Me.tspJobs.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents tspJobs As ToolStrip
    Friend WithEvents btnAdd As ToolStripButton
    Friend WithEvents btnEdit As ToolStripButton
    Friend WithEvents btnDelete As ToolStripButton
    Friend WithEvents lvwJobs As ListView
    Friend WithEvents lvwJobStatus As ListView
    Friend WithEvents btnLog As ToolStripButton
    Friend WithEvents imlSmall As ImageList
End Class
