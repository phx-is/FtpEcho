
Imports System.Dynamic
Imports System.IO
Imports System.Runtime.CompilerServices

Public Class clsEchoJobs

    Public Structure JobDetail
        Public JobName As String
        Public MonitorFolder As String
        Public MonitorRootFolder As String
        Public FtpServer As String
        Public FtpLogin As String
        Public FtpPassword As String
        Public FtpRootFolder As String
    End Structure

    Private moCypher As Crypto

    '     Encryption key = "*6mZ;}|.sv:7nS53r{VR~PHh0ZQM8{qP"
    Private mbValueA() As Byte = {42, 54, 109, 90, 59, 125, 124, 46, 115 _
                              , 118, 58, 55, 110, 83, 53, 51, 114, 123, 86 _
                              , 82, 126, 80, 72, 104, 48, 90, 81, 77, 56 _
                              , 123, 113, 80}

    '     Encrypted Value BackupLogin
    Private Const ENC_VALUE1 As String = "+SJ2X7GpnZ6wztJBP984OQ=="
    '     Encrypted Value 3upHa2rA_7ufr;r!za+&
    Private Const ENC_VALUE2 As String = "sRHXOjz6bTX1U2yQKo9BmcGvXazLpQ38mioXr7a0kR4="

    Private moJobsTB As New DataTable("JobsList")


    Public Property JobsListXML As String
        Get
            Dim psVal As String = ""
            Using poMS As New System.IO.MemoryStream
                Using poSW As New StreamWriter(poMS)
                    moJobsTB.WriteXml(poSW, XmlWriteMode.WriteSchema)
                    poSW.Flush()
                    poMS.Position = 0
                    Using poSR As New StreamReader(poMS)
                        psVal = poSR.ReadToEnd()
                        poSR.Close()
                    End Using
                End Using
                poMS.Close()
            End Using
            Return psVal
        End Get

        Set(value As String)
            moJobsTB.Rows.Clear()
            If value <> "" Then
                Dim poTB As New DataTable("JobsList")
                'poTB = moJobsTB.Clone
                Dim pyBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(value)
                Using poMS As New System.IO.MemoryStream(pyBytes)
                    poMS.Position = 0
                    Using poSR As New StreamReader(poMS)
                        poTB.ReadXml(poSR)
                        poSR.Close()
                    End Using
                    poMS.Close()
                End Using
                If poTB IsNot Nothing And poTB.Rows.Count > 0 Then
                    For Each poRow As DataRow In poTB.Rows
                        Dim poNewRow As DataRow = moJobsTB.NewRow
                        For Each poCol As DataColumn In moJobsTB.Columns
                            If poTB.Columns.Contains(poCol.ColumnName) Then
                                poNewRow.Item(poCol.ColumnName) = poTB.Rows(0).Item(poCol.ColumnName)
                            End If
                        Next
                        ' add the job if the name's not blank
                        If SafeStr(poNewRow.Item("JobName")) <> "" Then
                            moJobsTB.Rows.Add(poNewRow)
                        End If
                    Next
                    moJobsTB.AcceptChanges()
                End If
            End If
        End Set
    End Property


    Public Sub New()
        moJobsTB.Columns.Add("JobName", GetType(String))
        moJobsTB.Columns.Add("MonitorFolder", GetType(String))
        moJobsTB.Columns.Add("MonitorRootFolder", GetType(String))
        moJobsTB.Columns.Add("FtpServer", GetType(String))
        moJobsTB.Columns.Add("FtpLogin", GetType(String))
        moJobsTB.Columns.Add("FtpPassword", GetType(String))
        moJobsTB.Columns.Add("FtpRootFolder", GetType(String))
        moJobsTB.AcceptChanges()
    End Sub


    Public Function JobsList() As List(Of JobDetail)
        Dim poList As New List(Of JobDetail)
        For Each poRow As DataRow In moJobsTB.Rows
            Dim poNew As New JobDetail
            poNew.JobName = SafeStr(poRow("JobName"))
            poNew.MonitorFolder = SafeStr(poRow("MonitorFolder"))
            poNew.MonitorRootFolder = SafeStr(poRow("MonitorRootFolder"))
            poNew.FtpServer = SafeStr(poRow("ftpServer"))
            poNew.FtpLogin = DecryptValue(SafeStr(poRow("ftpLogin")))
            poNew.FtpPassword = DecryptValue(SafeStr(poRow("FtpPassword")))
            poNew.FtpRootFolder = SafeStr(poRow("FtpRootFolder"))
            poList.Add(poNew)
        Next
        Return poList
    End Function


    Public Function AddJob(ByVal JobName As String, ByVal MonitorFolder As String, ByVal MonitorRootFolder As String, ByVal FtpServer As String, ByVal FtpUser As String, ByVal FtpPass As String, ByVal FtpRoot As String) As Boolean
        For Each poRow As DataRow In moJobsTB.Rows
            If SafeStr(poRow("JobName")).Trim.ToLower = JobName.Trim.ToLower Then
                Return False
            End If
        Next
        'safe to add
        Dim poNew As DataRow = moJobsTB.NewRow
        poNew("JobName") = JobName
        poNew("MonitorFolder") = MonitorFolder
        poNew("MonitorRootFolder") = MonitorRootFolder
        poNew("FtpServer") = FtpServer
        poNew("FtpLogin") = EncryptValue(FtpUser)
        poNew("FtpPassword") = EncryptValue(FtpPass)
        poNew("FtpRootFolder") = FtpRoot
        moJobsTB.Rows.Add(poNew)
        moJobsTB.AcceptChanges()
        Return True
    End Function
    Public Function AddJob(ByVal oJobDetail As JobDetail) As Boolean
        Return AddJob(oJobDetail.JobName, oJobDetail.MonitorFolder, oJobDetail.MonitorRootFolder, oJobDetail.FtpServer, oJobDetail.FtpLogin, oJobDetail.FtpPassword, oJobDetail.FtpRootFolder)
    End Function


    Public Function UpdateJob(ByVal JobName As String, ByVal MonitorFolder As String, ByVal MonitorRootFolder As String, ByVal FtpServer As String, ByVal FtpUser As String, ByVal FtpPass As String, ByVal FtpRoot As String) As Boolean
        Dim piIndex As Integer

        For piIndex = 0 To moJobsTB.Rows.Count - 1
            Dim poRow As DataRow = moJobsTB.Rows(piIndex)
            If SafeStr(poRow("JobName")).Trim.ToLower = JobName.Trim.ToLower Then
                poRow("MonitorFolder") = MonitorFolder
                poRow("MonitorRootFolder") = MonitorRootFolder
                poRow("FtpServer") = FtpServer
                poRow("FtpLogin") = EncryptValue(FtpUser)
                poRow("FtpPassword") = EncryptValue(FtpPass)
                poRow("FtpRootFolder") = FtpRoot
                poRow.AcceptChanges()
                moJobsTB.AcceptChanges()
                Return True
            End If
        Next
        Return False
    End Function
    Public Function UpdateJob(ByVal oJobDetail As JobDetail) As Boolean
        Return UpdateJob(oJobDetail.JobName, oJobDetail.MonitorFolder, oJobDetail.MonitorRootFolder, oJobDetail.FtpServer, oJobDetail.FtpLogin, oJobDetail.FtpPassword, oJobDetail.FtpRootFolder)
    End Function



    Public Function RemoveJob(ByVal JobName As String) As Boolean
        Dim piIndex As Integer

        For piIndex = 0 To moJobsTB.Rows.Count - 1
            Dim poRow As DataRow = moJobsTB.Rows(piIndex)
            If SafeStr(poRow("JobName")).Trim.ToLower = JobName.Trim.ToLower Then
                poRow.Delete()
                poRow.AcceptChanges()
                moJobsTB.AcceptChanges()
                Return True
            End If
        Next
        Return False
    End Function

    Public Function GetJob(ByVal JobName As String) As JobDetail
        Dim piIndex As Integer
        Dim poJD As New JobDetail

        For piIndex = 0 To moJobsTB.Rows.Count - 1
            Dim poRow As DataRow = moJobsTB.Rows(piIndex)
            If SafeStr(poRow("JobName")).Trim.ToLower = JobName.Trim.ToLower Then
                poJD.MonitorFolder = SafeStr(poRow("MonitorFolder"))
                poJD.MonitorRootFolder = SafeStr(poRow("MonitorRootFolder"))
                poJD.JobName = SafeStr(poRow("JobName"))
                poJD.FtpServer = SafeStr(poRow("FtpServer"))
                poJD.FtpLogin = DecryptValue(SafeStr(poRow("FtpLogin")))
                poJD.FtpPassword = DecryptValue(SafeStr(poRow("FtpPassword")))
                poJD.FtpRootFolder = SafeStr(poRow("FtpRootFolder"))
                Return poJD
            End If
        Next
        Return Nothing
    End Function


    Private Function EncryptValue(ByVal PlainText As String) As String
        Try
            If PlainText = "" Then
                Return ""
            Else
                Crypto.Encoding = Crypto.EncodingType.BASE_64
                Crypto.EncryptionAlgorithm = Crypto.Algorithm.Rijndael
                Crypto.Key = System.Text.Encoding.UTF8.GetString(mbValueA)
                Crypto.EncryptString(PlainText)
                Return Crypto.Content
            End If

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function DecryptValue(ByVal EncryptedText As String) As String
        Try
            Crypto.Encoding = Crypto.EncodingType.BASE_64
            Crypto.EncryptionAlgorithm = Crypto.Algorithm.Rijndael
            Crypto.Key = System.Text.Encoding.UTF8.GetString(mbValueA)
            Crypto.Content = EncryptedText
            Crypto.DecryptString()
            Return Crypto.Content

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function


End Class
