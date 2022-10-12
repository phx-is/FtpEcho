Imports System.IO

Module modMain
    Public Const CONFIG_FILE As String = "ftpecho.xml"
    Public Const ERROR_LOG_FILE As String = "error_log.txt"
    Public Const DATA_FOLDER As String = "FtpEcho"


    Public goLogger As clsServiceLogger
    Public goConfig As clsConfigInfo
    Public goJobList As clsEchoJobs



    Public Function GetConfigFileName() As String
        Return Path.Combine(GetConfigPath(), CONFIG_FILE)
    End Function

    Public Function GetErrorLogFilename() As String
        Return Path.Combine(GetConfigPath(), ERROR_LOG_FILE)
    End Function


    Public Function GetConfigPath() As String
        Dim psPath As String = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)
        psPath = System.IO.Path.Combine(psPath, DATA_FOLDER)
        If System.IO.Directory.Exists(psPath) = False Then
            System.IO.Directory.CreateDirectory(psPath)
        End If
        Return psPath
    End Function






    Public Sub Initialize()
        ' initialize log file, purges it down to 100k as well
        goLogger = New clsServiceLogger(clsConfigInfo.LoggingLevels.Informational, GetConfigPath, ERROR_LOG_FILE)

        ' read in config
        goConfig = New clsConfigInfo()
        If Not goConfig.LoadConfigurationFile(GetConfigFileName()) Then
            goLogger.LogError("modMain.Initialize", "Unable to load configuration file, using defaults.")
            goLogger.Clear()
        End If

        ' get job list ready
        goJobList = New clsEchoJobs
        ' set job list from config XML string
        goJobList.JobsListXML = goConfig.ConfigValue("EchoJobs")



    End Sub


















#Region " Safe Functions "
    Public Function SafeStr(ByVal Anything As Object, Optional ByVal bTrimIt As Boolean = True, Optional ByVal bUCase As Boolean = False, Optional ByVal bProperCase As Boolean = False) As String
        Dim psNew As String = ""

        Try
            If Anything Is System.DBNull.Value Then
                Return psNew
            Else
                If Anything Is Nothing Then
                    Return psNew
                Else
                    psNew = Anything.ToString
                End If
            End If
        Catch ex As System.Exception
            psNew = ""
        Finally
            If bTrimIt Then
                psNew = psNew.Trim
            End If
            If bUCase Then
                psNew = StrConv(psNew, VbStrConv.Uppercase)
            Else
                If bProperCase Then
                    psNew = StrConv(psNew, VbStrConv.ProperCase)
                End If
            End If
        End Try

        Return psNew
    End Function

    Public Function SafeByte(ByVal Anything As Object) As Byte
        Dim pyNewByte As Byte = 0

        Try
            If Anything Is System.DBNull.Value Then
                Return pyNewByte
            Else
                If Anything Is Nothing Then
                    Return pyNewByte
                Else
                    pyNewByte = Anything
                End If
            End If
        Catch ex As System.Exception
            Try
                pyNewByte = CByte(Anything)
            Catch ex2 As System.Exception
                Try
                    pyNewByte = Val(Anything)
                Catch ex3 As System.Exception
                    pyNewByte = 0
                End Try
            End Try
        End Try

        Return pyNewByte
    End Function

    Public Function SafeInt(ByVal Anything As Object) As Integer
        Dim piNewInt As Integer = 0

        Try
            If Anything Is System.DBNull.Value Then
                Return 0
            Else
                If Anything Is Nothing Then
                    Return 0
                Else
                    If Not Integer.TryParse(Anything, piNewInt) Then
                        Return CInt(Anything)
                    End If
                End If
            End If
        Catch ex As System.Exception
            Try
                piNewInt = CInt(Anything)
            Catch ex2 As System.Exception
                Try
                    piNewInt = Val(Anything)
                Catch ex3 As System.Exception
                    piNewInt = 0
                End Try
            End Try
        End Try

        Return piNewInt
    End Function

    Public Function SafeShort(ByVal Anything As Object) As Short
        Dim phNewShort As Short = 0S

        Try
            If Anything Is System.DBNull.Value Then
                Return phNewShort
            Else
                If Not Short.TryParse(Anything, phNewShort) Then
                    Return 0
                End If
            End If
        Catch ex As System.Exception
            Try
                phNewShort = CShort(Anything)
            Catch ex2 As System.Exception
                Try
                    phNewShort = Val(Anything)
                Catch ex3 As System.Exception
                    phNewShort = 0S
                End Try
            End Try
        End Try

        Return phNewShort
    End Function

    Public Function SafeLong(ByVal Anything As Object) As Long
        Dim plNewLong As Long = 0

        Try
            If Anything Is System.DBNull.Value Then
                Return plNewLong
            Else
                If Anything Is Nothing Then
                    Return plNewLong
                Else
                    If Not Long.TryParse(Anything, plNewLong) Then
                        Return 0
                    End If
                End If
            End If
        Catch ex As System.Exception
            Try
                plNewLong = CLng(Anything)
            Catch ex2 As System.Exception
                Try
                    plNewLong = Val(Anything)
                Catch ex3 As System.Exception
                    plNewLong = 0
                End Try
            End Try
        End Try

        Return plNewLong
    End Function

    Public Function SafeDollar(ByVal Anything As Object) As Decimal
        Dim pcNewDecimal As Decimal = 0D
        Dim psFormat As String

        Try
            If Anything Is System.DBNull.Value Then
                Return pcNewDecimal
            Else
                If Anything Is Nothing Then
                    Return pcNewDecimal
                Else
                    If Not Decimal.TryParse(Anything, pcNewDecimal) Then
                        Return 0D
                    End If
                End If
            End If
        Catch ex As System.Exception
            Try
                pcNewDecimal = CDec(Anything)
            Catch ex2 As System.Exception
                Try
                    pcNewDecimal = Val(Anything)
                Catch ex3 As System.Exception
                    pcNewDecimal = 0D
                End Try
            End Try
        End Try
        psFormat = Format(pcNewDecimal, "#0.00")

        Return CDec(psFormat)
    End Function

    Public Function SafeDec(ByVal Anything As Object) As Decimal
        Dim pcNewDecimal As Decimal = 0D

        Try
            If Anything Is System.DBNull.Value Then
                Return pcNewDecimal
            Else
                If Anything Is Nothing Then
                    Return pcNewDecimal
                Else
                    If Not Decimal.TryParse(Anything, pcNewDecimal) Then
                        Return 0D
                    End If
                End If
            End If
        Catch ex As System.Exception
            Try
                pcNewDecimal = CDec(Anything)
            Catch ex2 As System.Exception
                Try
                    pcNewDecimal = Val(Anything)
                Catch ex3 As System.Exception
                    pcNewDecimal = 0D
                End Try
            End Try
        End Try

        Return pcNewDecimal
    End Function

    Public Function SafeSng(ByVal Anything As Object) As Single
        Dim pfNewSng As Single = 0.0F

        Try
            If Anything Is System.DBNull.Value Then
                Return 0.0F
            Else
                If Anything Is Nothing Then
                    Return 0.0F
                Else
                    If Not Single.TryParse(Anything, pfNewSng) Then
                        Return 0.0F
                    End If
                End If
            End If
        Catch ex As System.Exception
            Try
                pfNewSng = CSng(Anything)
            Catch ex2 As System.Exception
                Try
                    pfNewSng = Val(Anything)
                Catch ex3 As System.Exception
                    pfNewSng = 0.0F
                End Try
            End Try
        End Try

        Return pfNewSng
    End Function


    Public Function SafeDate(ByVal Anything As Object) As Date
        Dim ptDate As Date

        Try
            If Anything Is System.DBNull.Value Then
                Return Nothing
            Else
                If Anything Is Nothing Then
                    Return Nothing
                Else
                    If Not Date.TryParse(Anything, ptDate) Then
                        Return Nothing
                    End If
                End If
            End If
        Catch ex As System.Exception
            Try
                ptDate = CDate(Anything)
            Catch ex2 As System.Exception
                ptDate = Nothing
            End Try
        End Try

        Return ptDate
    End Function

    Public Function SafeSQLDate(ByVal Anything As Object) As Date
        Try
            If Anything Is Nothing Then
                Return Nothing
            Else
                If Anything Is System.DBNull.Value Then
                    Return Nothing
                Else
                    Dim ptDate As Date
                    If Not Date.TryParse(Anything, ptDate) Then
                        Return Nothing
                    Else
                        If ptDate < System.Data.SqlTypes.SqlDateTime.MinValue.Value Then
                            ptDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value
                        Else
                            If ptDate > System.Data.SqlTypes.SqlDateTime.MaxValue.Value Then
                                ptDate = System.Data.SqlTypes.SqlDateTime.MaxValue.Value
                            End If
                        End If
                        Return ptDate
                    End If
                End If
            End If
        Catch ex As System.Exception
            Return Nothing
        End Try
    End Function

    Public Function SafeBool(ByVal Anything As Object) As Boolean
        Try
            If Anything Is System.DBNull.Value Then
                Return False
            Else
                If Anything Is Nothing Then
                    Return False
                Else
                    SafeBool = Anything
                End If
            End If
        Catch ex As System.Exception
            SafeBool = False
        End Try
    End Function
#End Region








End Module








'