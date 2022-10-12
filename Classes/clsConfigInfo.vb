
Imports System.IO

Public Class clsConfigInfo

    Public Enum LoggingLevels
        [None] = 0
        [Informational] = 1
        [Warnings] = 2
        [Errors] = 3
    End Enum



    Private moConfigTB As New DataTable("ConfigData")



    Public Property ConfigValue(ByVal ColumnName As String) As Object
        Get
            If moConfigTB.Columns.Contains(ColumnName) Then
                Return moConfigTB.Rows(0).Item(ColumnName)
            Else
                Return Nothing
            End If
        End Get
        Set(value As Object)
            If moConfigTB.Columns.Contains(ColumnName) Then
                moConfigTB.Rows(0).Item(ColumnName) = value
                moConfigTB.AcceptChanges()
            End If
        End Set
    End Property



    Public Sub New()
        moConfigTB.Columns.Clear()
        moConfigTB.Columns.Add("LoggingLevel", GetType(Integer))
        moConfigTB.Columns.Add("EchoJobs", GetType(String))
        moConfigTB.AcceptChanges()
        Me.Clear()
    End Sub

    Public Sub Clear()
        moConfigTB.Rows.Clear()
        Dim poRow As DataRow = moConfigTB.NewRow
        poRow("LoggingLevel") = CInt(LoggingLevels.Informational)
        poRow("EchoJobs") = ""
        moConfigTB.Rows.Add(poRow)
        moConfigTB.AcceptChanges()
    End Sub

    Public Function SaveConfigFile(ByVal Filename As String) As Boolean
        Dim pbResult As Boolean = False

        Try
            If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Filename)) = False Then
                ' create it
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Filename))
            End If


            If System.IO.File.Exists(Filename) Then
                System.IO.File.Delete(Filename)
            End If

            Using poFS As New FileStream(Filename, FileMode.Create)
                Using poSW As New StreamWriter(poFS)
                    moConfigTB.WriteXml(poSW, XmlWriteMode.WriteSchema)
                    poSW.Flush()
                    poSW.Close()
                End Using
                poFS.Flush()
                poFS.Close()
            End Using
            pbResult = True

        Catch ex As Exception
            goLogger.LogException("clsConfigInfo.LoadConfigurationFile", ex)
        End Try

        Return pbResult
    End Function


    Public Function LoadConfigurationFile(ByVal Filename As String) As Boolean
        Dim pbSuccess As Boolean = False
        Dim psCurrElem As String = ""

        Me.Clear()

        Dim poTB As New DataTable("ConfigData")
        Try
            If System.IO.File.Exists(Filename) Then
                Using poFS As New FileStream(Filename, FileMode.Open)
                    Using poSR As New StreamReader(poFS)
                        poTB.ReadXml(poSR)
                        poSR.Close()
                    End Using
                    poFS.Close()
                End Using
                ' copy columns over
                If poTB IsNot Nothing AndAlso poTB.Rows.Count > 0 Then
                    For Each poCol As DataColumn In moConfigTB.Columns
                        If poTB.Columns.Contains(poCol.ColumnName) Then
                            moConfigTB.Rows(0).Item(poCol.ColumnName) = poTB.Rows(0).Item(poCol.ColumnName)
                        End If
                    Next
                    moConfigTB.Rows(0).AcceptChanges()
                End If
                pbSuccess = True
            End If

        Catch ex As Exception
            goLogger.LogException("clsConfigInfo.LoadConfigurationFile", ex)
        End Try

        Return pbSuccess
    End Function

End Class
