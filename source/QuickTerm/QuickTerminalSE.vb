Option Explicit On 'Required for something awhile back
Imports System.Console 'easier Console Actions
Imports System.Threading 'Multi-Threading

Module QuickTerminalSE

#Region "License"
    '*  GNU License Agreement
    '*  ---------------------
    '*  This program is free software; you can redistribute it and/or modify
    '*  it under the terms of the GNU General Public License version 3 as
    '*  published by the Free Software Foundation.
    '*
    '*  This program is distributed in the hope that it will be useful,
    '*  but WITHOUT ANY WARRANTY; without even the implied warranty of
    '*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    '*  GNU General Public License for more details.
    '*
    '*  You should have received a copy of the GNU General Public License
    '*  along with this program; if not, write to the Free Software
    '*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA
    '*
    '*  http://www.gnu.org/licenses/gpl-3.0.txt
#End Region

#Region "Some Quick Notes"
    'This program is full of spelling mistakes
    '
    'I have not worked on this project in along while!
    'I originally made this program to be used by me on
    'my Windows 2003 server. After some turn of events
    'I switched my computer and my server to GNU/Linux,
    'more specificaly Debian. I, there for, have no use
    'for this program besides a hobby.
    '
    'The reason this was created in VB.NET was its simplicity.
    'I can dream in VB because it's so easy! I know its ironic
    'that it's writen in VB but doesn't contain a GUI but who
    'cares?
    '
    'You guys can email me if you have questions or comments about
    'this program. Let's make RMS proud.
#End Region

#Region "Vars"
    '[Public]
    Public ReadOnly ver As String = "1.0.0 BETA"
    Public ReadOnly ProgramLocation As String = Environment.CurrentDirectory
    Public ErrorLog As New ArrayList
    Public lstUrls As New ArrayList
    Public lstEmails As New ArrayList
    Public TRunning As UInt16 = 1
    Public inpt As String
    Public Filee As String = ""
    Public WorkingDir As String = Environment.CurrentDirectory 'FIX THIS PLEASE, WorkingDir has been such an issue
    Public nomsg As Boolean = False

    '[Declarations]
    Dim QuickInfo As New Info
    Dim Net As New NetworkClass
    Dim Core As New Core
    Dim QIO As New QuickIO

    '[Booleans]
    Dim back As Boolean = False
    Dim exitafter = False

    '[Ints]
    Dim cmdssaid As UInt16 = 0

    '[Multi-Array]
    Dim Args()

    '[Strings]
    Dim Prompt As String = "==> "

    '[Threads]
    Dim worker As Threading.Thread

#End Region

#Region "Script Vars"
    Public errorhandle As String = "default"
    Public replyhandle As String = "default"
    Public invisable As Boolean = False
    Public Scripting As Boolean = False
    Dim QtScriptFile As String = Nothing
    Dim QtFile() As String
    Public i2 As UInteger = 0
    Public loopp As UInteger
    Public looppfor As UInteger
    Public loopped As UInteger = 1
    Public ifthen As Boolean = False
#End Region

#Region "Config Vars"
    'backup
    Dim BackUpImportance As String = "Normal"
    Dim Filestobackup As New ArrayList
    Dim BackUpLocation As String
    'Telnet
    Dim TelnetRunning As Boolean = False
    Dim AllowTelnet As Boolean = False
    Dim EnableLogging As Boolean = True
    Dim LogFailedLogins As Boolean = False
    Dim LogKicks As Boolean = True
    Dim ServerPort As UInt16 = 23
    Dim EnableAsciiArt As Boolean = True
    Dim TelnetLogLocation As String = "Default"
    Dim TelnetLogName As String = "TelnetServer.log"
    Dim TelnetUserName As String = "Default"
    Dim TelnetPassword As String = "1234"
    Dim LogCommands As Boolean = True
#End Region

#Region "UI"
    Sub Main(Optional sArgs() As String = Nothing)
        Try
            If sArgs.Length = 0 Then 'if no args were passed to the program
                Main2()
            Else
                If sArgs(0) = "-l" Then
                    If ReadConfig() = True Then
                        Login()
                    Else
                        MsgBox("Error reading main.conf! Args are disabled until file is found.", 16, )
                        Environment.Exit(0)
                    End If
                ElseIf sArgs(0) = "-i" Then
                    invisable = True
                ElseIf sArgs(0) = "-h" Then
                    exitafter = True
                    QuickInfo.Help()
                ElseIf sArgs(0) = "-x" Then
                    exitafter = True
                ElseIf sArgs(0) = "-c" Then
                    back = True
                    Dim temp As String
                    Dim i As UInt16 = 2
                    If sArgs.Length = 1 Then
                        Writeline("Missing Syntax")
                    ElseIf sArgs.Length = 2 Then
                        If ReadConfig() = True Then
                            Commands(sArgs(1))
                        Else
                            MsgBox("Error reading main.conf! Args are disabled until file is found.", 16, )
                            Environment.Exit(0)
                        End If
                        'Commands(sArgs(1))
                    ElseIf sArgs.Length = 3 AndAlso sArgs(1) = "-x" Then
                        exitafter = True
                        If ReadConfig() = True Then
                            Commands(sArgs(2))
                        Else
                            MsgBox("Error reading main.conf!", 16, )
                            Environment.Exit(0)
                        End If

                    Else
                        If sArgs(1) = "-x" Then
                            exitafter = True
                            temp = sArgs(2)
                            i += 1
                        Else
                            temp = sArgs(1)
                        End If
                        Do Until i >= sArgs.Length
                            temp = temp & " " & sArgs(i)
                            i += 1
                        Loop
                        If ReadConfig() = True Then
                            Commands(temp)
                        Else
                            MsgBox("Error reading main.conf! Args are disabled until file is found.", 16, )
                            Environment.Exit(0)
                        End If

                    End If
                ElseIf sArgs(0) = "-s" Then
                    back = True
                    Dim temp As String
                    Dim i As UInt16 = 2
                    If sArgs.Length = 1 Then
                        Writeline("Missing Syntax")
                    ElseIf sArgs.Length = 2 Then
                        If ReadConfig() = True Then
                            ReadFile(sArgs(1))
                        Else
                            MsgBox("Error reading main.conf! Args are disabled until file is found.", 16, )
                            Environment.Exit(0)
                        End If

                    Else
                        temp = sArgs(1)
                        Do Until i = sArgs.Length
                            temp = temp & " " & sArgs(i)
                            i += 1
                        Loop
                        If ReadConfig() = True Then
                            ReadFile(temp)
                        Else
                            MsgBox("Error reading main.conf! Args are disabled until file is found.", 16, )
                            Environment.Exit(0)
                        End If

                    End If
                Else
                    exitafter = True
                    QuickInfo.Help()
                End If
            End If
        Catch
            Er()
        End Try
    End Sub

    Public Sub Main2()
        Try
            If exitafter = True Then
                ForegroundColor = ConsoleColor.Black
                ReadKey()
                Environment.Exit(0)
            End If
            If Scripting = True Then
                i2 += 1
                QtCont()
            End If
            If back = True Then
                GoTo a
            End If
            If ReadConfig() = False Then
                MsgBox("Error reading main.conf! Args are disabled until file is found but Commands will still work.", 16, )
            End If
            WorkingDir = Environment.CurrentDirectory 'use this instead of public var
            ForegroundColor = ConsoleColor.White
            Write("Greetings ")
            ForegroundColor = ConsoleColor.Red
            Write(Environment.UserName)
            ForegroundColor = ConsoleColor.White
            Write("@")
            ForegroundColor = ConsoleColor.Red
            WriteLine(My.Computer.Name)
a:
            Title = "Quick Terminal Server Edition"
            BackgroundColor = ConsoleColor.Black
            ForegroundColor = ConsoleColor.Cyan
            Write(Prompt)
            ForegroundColor = ConsoleColor.Gray
            inpt = ReadLine()
            back = True
            If inpt = "" Then
                GoTo a
            End If
            Commands(inpt)
        Catch
            Er()
        End Try
    End Sub

    Public Sub Commands(ByVal command As String)
        Try
            cmdssaid += 1 'this is a horrible patch to stop a stack overflow
            If cmdssaid >= 30 Then 'if 30+ commands were made
                Dim temp As String = command
                command = Nothing 'wipes vars
                Args = Nothing
                cmdssaid = 0 'resets counter
                command = temp 'return to normal
            End If
            If TelnetRunning = True AndAlso LogCommands = True Then
                TLog("(C) " & command)
            End If
            ForegroundColor = ConsoleColor.White
            Dim i As UInt16 = 0
            'Begin Commands
            Args = Split(command)
            If Args.Length = 0 Then
                If Scripting = True Then
                    i += 1
                    QtCont()
                End If
                WriteLine("Missing Args")
                Main2()
            End If

            If Args(0) = "telnet" Then
                If AllowTelnet = False Then
                    WriteLine("Telnet Server is disabled on this server")
                    ForegroundColor = ConsoleColor.Black
                    ReadKey()
                    WriteLine()
                    Main2()
                ElseIf TelnetRunning = True Then
                    WriteLine("Telnet Server is still running! Please close it and try again.")
                    ForegroundColor = ConsoleColor.Black
                    ReadKey()
                    WriteLine()
                    Main2()
                End If
                WriteLine("Starting QT-Telnet server")
                WriteLine("To stop the server simply close this window.")
                ForegroundColor = ConsoleColor.Yellow
                WriteLine("Warning: Telnet connections arent secure!")
                ForegroundColor = ConsoleColor.White
                TLog("(?) Server Started on " & DateTime.Now.ToString("dddd, MM/dd/yyyy") & ".")
                Shell("nc -L -p " & ServerPort & " -e " & Chr(34) & "Quicktermse.exe -l" & Chr(34), , True)
            ElseIf Args(0) = "telnet2" Then
                If AllowTelnet = False Then
                    WriteLine("Telnet Server is disabled on this server")
                    ForegroundColor = ConsoleColor.Black
                    ReadKey()
                    WriteLine()
                    Main2()
                ElseIf TelnetRunning = True Then
                    WriteLine("Telnet Server is still running! Please close it and try again.")
                    ForegroundColor = ConsoleColor.Black
                    ReadKey()
                    WriteLine()
                    Main2()
                End If
                WriteLine("To stop the server simply close this window.")
                ForegroundColor = ConsoleColor.Yellow
                WriteLine("Warning: Telnet connections arent secure!")
                ForegroundColor = ConsoleColor.White
                Shell("nc.exe -L -p " & ServerPort & " -e cmd.exe")
                WriteLine("Telnet Server started on port 23")
            ElseIf Args(0) = "exit" Then
                Environment.Exit(0)
            ElseIf Args(0) = "kill" Then
                Core.ProcKill(command.Remove(0, 5))
            ElseIf Args(0) = "shutdown" Then
                Writeline("Shuting down NOW")
                Shell("shutdown -s -t 2")
            ElseIf Args(0) = "logoff" Or Args(0) = "log off" Then
                Shell("shutdown -l")
            ElseIf Args(0) = "restart" Then
                Shell("shutdown -r -t 0")
            ElseIf Args(0) = "lock" Then
                LockWorkStation()
            ElseIf Args(0) = "cdtray" Then
                worker = New Threading.Thread(AddressOf Core.cd)
                worker.Start()
            ElseIf Args(0) = "run" Then
                If command.EndsWith(".qts") = True Then
                    ReadFile(WorkingDir & "\" & command.Remove(0, 4))
                Else
                    If IO.File.Exists(WorkingDir & "\" & command.Remove(0, 4)) = True Then
                        Process.Start(WorkingDir & "\" & command.Remove(0, 4))
                    Else
                        Process.Start(command.Remove(0, 4))
                    End If
                End If
            ElseIf Args(0) = "info" Then
                Console.WriteLine("Username:                   " & Environment.UserName)
                Console.WriteLine("Computer Name:              " & My.Computer.Name)
                Console.WriteLine("Operating System:           " & My.Computer.Info.OSFullName)
                Console.WriteLine("OS Version:                 " & My.Computer.Info.OSVersion)
                Console.WriteLine("System Dir:                 " & Environment.SystemDirectory)
                Console.WriteLine("Number of CPU(s):           " & Environment.ProcessorCount)
                Console.WriteLine("Local IPv4:                 " & Net.GetLocalIpAddress().ToString)
                Console.WriteLine("Total VMem:                 " & Math.Round(My.Computer.Info.TotalVirtualMemory / 1024 / 1024).ToString & " MB")
                Console.WriteLine("Total PMem:                 " & Math.Round(My.Computer.Info.TotalPhysicalMemory / 1024 / 1024).ToString & " MB")
            ElseIf Args(0) = "cmd" Then
                Core.Cmd2()
            ElseIf Args(0) = "ver" Or Args(0) = "version" Then
                Writeline(ver)
            ElseIf Args(0) = "cls" Or Args(0) = "clear" Then
                Clear()
            ElseIf Args(0) = "msg" Then
                If Args.Length > 1 Then
                    Core.msgMbox = command.Remove(0, 4)
                    worker = New Threading.Thread(AddressOf Core.msg3)
                    worker.Start()
                Else
                    Writeline("Missing message")
                End If
            ElseIf Args(0) = "con" Or Args(0) = "conn" Then
                'Conn("localhost", 80)
                If Args.Length <= 2 Then
                    Writeline("Missing Args")
                ElseIf Args.Length = 3 Then
                    Net.Connect(Args(1), Args(2))
                Else
                    Writeline("Wrong syntax. Proper syntax is: con <Address> <Port>")
                End If
            ElseIf Args(0) = "ping" Then
                If Net.Ping_Server(Args(1)) = False Then
                    Writeline(Args(1) & " seems to be offline or blocking ping packets.")
                Else
                    Writeline(Args(1) & " seems to be online.")
                End If
            ElseIf Args(0) = "scan" Then
                If Args.Length = 1 Or Args.Length = 3 Then
                    Writeline("Missing Args")
                ElseIf Args.Length = 4 Then
                    Net.Scan(Args(1), Args(2), Args(3))
                ElseIf Args.Length = 2 Then
                    Net.Scan(Args(1))
                Else
                    Writeline("Wrong syntax. Proper syntax is: scan <target> <start> <end>")
                End If
            ElseIf Args(0) = "error" Or Args(0) = "errors" Then
                Dim l As UInt16 = 0
                Do Until l = ErrorLog.Count
                    Writeline(ErrorLog.Item(l))
                    l += 1
                Loop
            ElseIf Args(0) = "beep" Then
                Beep()
            ElseIf Args(0) = "beta" Then
                QuickInfo.BetaCommands()
            ElseIf Args(0) = "dev" Then
                Core.dev()
            ElseIf Args(0) = "play" Then
                If Args(1) = "-l" Then
                    My.Computer.Audio.Play(command.Remove(0, 8), AudioPlayMode.BackgroundLoop)
                Else
                    My.Computer.Audio.Play(command.Remove(0, 5), AudioPlayMode.Background)
                End If
            ElseIf Args(0) = "time" Then
                Writeline(Date.Now.ToString("hh:mm.ss tt"))
            ElseIf Args(0) = "date" Then
                Writeline(DateTime.Now.ToString("dddd, MM/dd/yyyy"))
            ElseIf Args(0) = "echo" Or Args(0).ToString.StartsWith("echo ") = True Then
                If command = "echo" Then
                    Writeline()
                Else
                    If cmdssaid >= 30 Then
                        Dim temp As String = command
                        command = Nothing
                        Args = Nothing
                        cmdssaid = 0
                        command = temp
                    End If
                    Dim temp2 As String = command.Remove(0, 5)
                    Writeline(temp2)
                    temp2 = Nothing
                    temp2 = Nothing
                End If
            ElseIf Args(0) = "dns" Then
                Net.DnsLookUp(Args(1))
            ElseIf Args(0) = "read" Then
                If Args.Length = 1 Then
                    Writeline("Missing file to read")
                Else
                    QIO.Read2(command.Remove(0, 5))
                End If
            ElseIf Args(0) = "write" Then
                If Args.Length = 1 Then
                    Writeline("Missing file to write")
                Else
                    Dim thefile As String
                    thefile = command.Remove(0, 6)
                    QIO.WriteFile(thefile)
                End If
            ElseIf Args(0) = "make" Then
                If Args.Length = 1 Then
                    Writeline("Missing file to write")
                Else
                    QIO.WriteFile(command.Remove(0, 5))
                End If
            ElseIf Args(0) = "delete" Then
                If Args.Length = 1 Then
                    Writeline("Missing file to delete")
                Else
                    QIO.DelFile(command.Remove(0, 7))
                End If
            ElseIf Args(0) = "del" Then
                If Args.Length = 1 Then
                    Writeline("Missing file to delete")
                Else
                    QIO.DelFile(command.Remove(0, 4))
                End If
            ElseIf Args(0) = "delete1" Then
                If Args.Length = 1 Then
                    Writeline("Missing file to delete")
                Else
                    QIO.DelFile(command.Remove(0, 7))
                End If
            ElseIf Args(0) = "delete2" Then
                If Args.Length = 1 Then
                    Writeline("Missing file to delete")
                Else
                    Filee = command.Remove(0, 8)
                    worker = New Threading.Thread(AddressOf QIO.DelFileThread)
                    worker.Start()
                End If
            ElseIf Args(0) = "delete3" Then
                If Args.Length = 1 Then
                    Writeline("Missing file to delete")
                Else
                    Filee = command.Remove(0, 8)
                    worker = New Threading.Thread(AddressOf QIO.SecureDelete)
                    worker.Start()
                End If
            ElseIf Args(0) = "ip" Then
                Writeline("Local IPv4: " & Net.GetLocalIpAddress.ToString)
            ElseIf Args(0) = "whoami" Then
                Writeline(Environment.UserName)
            ElseIf Args(0) = "define" Or Args(0) = "lookup" Then
                If Args.Length > 1 Then
                    Dim lookupp As String
                    Dim lookuppp As String
                    lookupp = command.Remove(0, 7)
                    lookuppp = lookupp.Replace(" ", "+")
                    Process.Start("http:/" & "/www.thefreedictionary.com/" & lookuppp)
                Else
                    Writeline("Missing term to look up")
                End If
            ElseIf Args(0) = "search" Then
                If Args.Length > 1 Then
                    Dim lookupp As String
                    Dim lookuppp As String
                    lookupp = command.Remove(0, 7)
                    lookuppp = lookupp.Replace(" ", "+")
                    Process.Start("https:/" & "/duckduckgo.com/?q=define%3A" & lookuppp) 'duckduckgo for tin-foil hats out there
                Else
                    Writeline("Missing search terms")
                End If
            ElseIf Args(0) = "dir" Or Args(0) = "ls" Then
                QIO.GetDir()
            ElseIf Args(0) = "cred" Or Args(0) = "credits" Then
                QuickInfo.Credits()
            ElseIf Args(0) = "enc" Or Args(0) = "encrypt" Then
                Dim encstring As String
                Writeline("Text to encrypt")
                ForegroundColor = ConsoleColor.Cyan
                Write("> ")
                ForegroundColor = ConsoleColor.Gray
                encstring = ReadLine()
                Dim securePwd As New System.Security.SecureString()
                Dim key As ConsoleKeyInfo
                ForegroundColor = ConsoleColor.Cyan
                Write("Key: ")
                ForegroundColor = ConsoleColor.Gray
                Do
                    key = Console.ReadKey(True)
                    securePwd.AppendChar(key.KeyChar)
                Loop While key.Key <> ConsoleKey.Enter
                Writeline()
                securePwd.MakeReadOnly()
                Writeline(Core.Encrypt(encstring, securePwd))
            ElseIf Args(0) = "dec" Or Args(0) = "decrypt" Then
                If Args.Length >= 2 Then
                    Dim decfile As String
                    If Args(0) = "dec" Then
                        decfile = command.Remove(0, 4)
                    Else
                        decfile = command.Remove(0, 8)
                    End If

                    If System.IO.File.Exists(decfile) = False Then
                        Writeline("File not found")
                        Main2()
                    End If
                    Dim securePwd As New System.Security.SecureString()
                    Dim key As ConsoleKeyInfo
                    ForegroundColor = ConsoleColor.Cyan
                    Write("Key: ")
                    ForegroundColor = ConsoleColor.Gray
                    Do
                        key = Console.ReadKey(True)
                        securePwd.AppendChar(key.KeyChar)
                    Loop While key.Key <> ConsoleKey.Enter
                    securePwd.MakeReadOnly()
                    Writeline()
                    Writeline(Core.Decrypt(System.IO.File.ReadAllText(decfile), securePwd))
                Else
                    'Else
a2:
                    Dim encstring As String
                    Dim inpt2 As String
                    Writeline("From file? [Y/N]")
a:
                    ForegroundColor = ConsoleColor.Cyan
                    Write("> ")
                    ForegroundColor = ConsoleColor.Gray
                    inpt2 = ReadLine()
                    If inpt2.ToLower = "y" Or inpt2.ToLower.StartsWith("y") Then
                        Dim decfilee
                        ForegroundColor = ConsoleColor.Cyan
                        Write("File> ")
                        ForegroundColor = ConsoleColor.Gray
                        decfilee = ReadLine()
                        If System.IO.File.Exists(decfilee) = False Then
                            Writeline("File not found")

                        End If
                        Dim securePwd2 As New System.Security.SecureString()
                        Dim key2 As ConsoleKeyInfo
                        Dim decfilee2 As String = System.IO.File.ReadAllText(decfilee)
                        ForegroundColor = ConsoleColor.Cyan
                        Write("Key: ")
                        ForegroundColor = ConsoleColor.Gray
                        Do
                            key2 = Console.ReadKey(True)
                            securePwd2.AppendChar(key2.KeyChar)
                        Loop While key2.Key <> ConsoleKey.Enter
                        Writeline()
                        securePwd2.MakeReadOnly()
                        Writeline(Core.Decrypt(decfilee2, securePwd2))
                    ElseIf inpt2.ToLower = "n" Or inpt2.ToLower.StartsWith("N") Then
                    Else
                        GoTo a
                    End If
                    Writeline("Text to decrypt")
                    ForegroundColor = ConsoleColor.Cyan
                    Write("> ")
                    ForegroundColor = ConsoleColor.Gray
                    encstring = ReadLine()
b:
                    Dim securePwd As New System.Security.SecureString()
                    Dim key As ConsoleKeyInfo
                    ForegroundColor = ConsoleColor.Cyan
                    Write("Key: ")
                    ForegroundColor = ConsoleColor.Gray
                    Do
                        key = Console.ReadKey(True)
                        securePwd.AppendChar(key.KeyChar)
                    Loop While key.Key <> ConsoleKey.Enter
                    Writeline()
                    securePwd.MakeReadOnly()
                    Writeline(Core.Decrypt(encstring, securePwd))
                End If
            ElseIf Args(0) = "thread" Or Args(0) = "threads" Then
                Writeline("Current threads running from this program: " & TRunning)
            ElseIf Args(0) = "sp" Then
                Writeline("Current port being scanned is port " & Net.CurrentPort)
            ElseIf Args(0) = "refresh" Then
                worker = New Thread(AddressOf refresh)
                worker.Start()
            ElseIf Args(0) = "download" Then
                If Args.Length = 3 Then
                    Net.Download(Args(1), command.Remove(0, Args(1).ToString.Length + 9))
                Else
                    Writeline("Wrong Syntax")
                End If
            ElseIf Args(0) = "upload" Then
                If Args.Length = 3 Then
                    Net.Download(Args(1), Args(2))
                    Writeline("Done.")
                Else
                    Writeline("Wrong Syntax")
                End If
            ElseIf Args(0) = "update" Then
                Writeline("Checking for updates...")
                Writeline()
                If UptoDate() = True Then
                    Writeline("You are up to date!")
                Else
                    Writeline("There is a newer version available! Please head over to:")
                    Writeline("http://www.quitetiny.com/downloads")
                End If
            ElseIf Args(0) = "sc" Then
                If Args.Length = 1 Then
                    Writeline("3")
                    Thread.Sleep(1000)
                    Writeline("2")
                    Thread.Sleep(1000)
                    Writeline("1")
                    Thread.Sleep(1000)
                    If Core.SaveWholeScreen("ScreenShot") = True Then
                        Writeline("Done!")
                    Else
                        Writeline("Error!")
                    End If
                ElseIf Args.Length >= 2 Then
                    Writeline("3")
                    Thread.Sleep(1000)
                    Writeline("2")
                    Thread.Sleep(1000)
                    Writeline("1")
                    Thread.Sleep(1000)
                    If Core.SaveWholeScreen(command.Remove(0, 3)) = True Then
                        Writeline("Done!")
                    Else
                        Writeline("Error!")
                    End If
                End If
            ElseIf Args(0) = "cap" Then
                If Core.SaveScreen("QuickTerm.sc") = True Then
                    Writeline("Done!")
                Else
                    Writeline("Error!")
                End If
            ElseIf Args(0) = "md5" Then
                If Args.Length >= 2 Then
                    Dim thefile As String
                    thefile = command.Remove(0, 4)
                    Writeline(QIO.GetMD5(thefile))
                Else
                    Writeline("Missing file name")
                End If
            ElseIf Args(0) = "vscan" Then '====To be removed====
                If Args.Length >= 2 Then
                    If Args(1) = "download" Then
                        Writeline("Downloading database file 'md5List.txt'")
                        Net.Download("quitetiny.com/Downloads/virusmd5.txt", "md5List.txt")
                    Else
                        'QIO.VirusScan(command.Remove(0, 6))
                    End If
                Else
                    Writeline("Please specifiy a text file with md5 hashes")
                    Writeline("To create one type 'md5log files.txt' to download one from us type")
                    Writeline("vscan download")
                End If
            ElseIf Args(0) = "ran" Or Args(0) = "random" Then
                Dim ran As New Random
                If Args.Length = 1 Then
                    Writeline(ran.Next)
                ElseIf Args.Length = 3 Then
                    Writeline(ran.Next(Args(1), Args(2)))
                Else
                    Writeline("Invaild Syntax")
                End If
            ElseIf Args(0) = "folder" Then
                QIO.MakeFolder(command.Remove(0, 7))
            ElseIf Args(0) = "remove" Then
                QIO.DelFolder(command.Remove(0, 7))
            ElseIf Args(0) = "deleteall" Then
                QIO.DeleteAll(command.Remove(0, 10))
            ElseIf Args(0) = "md5log" Then '====To be removed====
                QIO.Md5Log(command.Remove(0, 7))
            ElseIf Args(0) = "script" Then
                If Args.Length >= 2 Then
                    ReadFile(command.Remove(0, 7))
                Else
                    Writeline("Missing script file")
                End If
            ElseIf Args(0) = "copy" Then
                If Args.Length >= 2 Then
                    If command.Contains("|") = False Then
                        Writeline("Invalid syntax")
                        Main2()
                    End If
                    Dim dafile() As String = command.Remove(0, 5).Split("|")
                    QIO.CopyFile(dafile(0), dafile(1))
                Else
                    Writeline("Invalid syntax")
                End If
            ElseIf Args(0) = "move" Then
                If Args.Length >= 2 Then
                    If command.Contains("|") = False Then
                        Writeline("Invalid syntax")
                        Main2()
                    End If
                    Dim dafile() As String = command.Remove(0, 5).Split("|")
                    QIO.MoveFile(dafile(0), dafile(1))
                Else
                    Writeline("Invalid syntax")
                End If
            ElseIf Args(0) = "qwrite" Then
                If Args.Length >= 2 Then
                    If command.Contains("|") = False Then
                        Writeline("Invalid syntax")
                        Main2()
                    End If
                    Dim dafile() As String = command.Remove(0, 7).Split("|")
                    QIO.QuickWrite(dafile(0), dafile(1))
                Else
                    Writeline("Invalid syntax")
                End If
            ElseIf Args(0) = "install" Then 'fix me (does shell cmd QuickTermSE -s <scriptname>)
                IO.File.WriteAllText("qt.reg", "Windows Registry Editor Version 5.00" & vbNewLine & _
                "[HKEY_CURRENT_USER\Software\Classes\Quick Terminal Script\shell\open\command]" & vbNewLine & _
                "@=" & Chr(34) & "QuickTerm.exe" & Chr(34) & " \" & Chr(34) & "%1\" & Chr(34) & Chr(34) & vbNewLine & _
                "[HKEY_CURRENT_USER\Software\Classes\.qts]" & vbNewLine & _
                "@=" & Chr(34) & "Quick Terminal Script" & Chr(34))
                Thread.Sleep(100)
                Process.Start("qt.reg")
                Write("Press any key to continue...")
                ForegroundColor = ConsoleColor.Black
                ReadKey()
                Writeline()
                ForegroundColor = ConsoleColor.White
                IO.File.Delete("qt.reg")
            ElseIf Args(0) = "prompt" Then
                If Args.Length = 1 Then
                    Writeline("Missing prompt string!")
                Else
                    Prompt = command.Remove(0, 7)
                End If
            ElseIf Args(0) = "errortest" Then
                i = i / 0
            ElseIf Args(0) = "grab" Then
                If Args.Length = 2 Then
                    Net.Grab(Args(1))
                ElseIf Args.Length = 3 Then
                    Net.Grab(Args(1), Args(2))
                Else
                    Writeline("Invalid Syntax")
                End If
            ElseIf Args(0) = "poke" Then
                Net.RBrowse(Args(1))
            ElseIf Args(0) = "ssh" Then
                Writeline("QuickTermSE does not come with an ssh client.")
                Writeline("For Windows we sugest Putty")
                Writeline("For GNU/Linux we sugest OpenSSH")
            ElseIf Args(0) = "timer" Then
                If Args.Length = 1 Then
                    Writeline("Missing switch")
                Else
                    If Args(1) = "start" Then
                        worker = New Thread(AddressOf Core.Timerz)
                        worker.Start()
                        Writeline("Timer running!")
                    ElseIf Args(1) = "on" Then
                        worker = New Thread(AddressOf Core.Timerz)
                        worker.Start()
                        Writeline("Timer running!")
                    ElseIf Args(1) = "stop" Then
                        Core.TimerStop = True
                    ElseIf Args(1) = "off" Then
                        Core.TimerStop = True
                    Else
                        Writeline("Invalid syntax")
                    End If
                End If
            ElseIf Args(0) = "privacy" Then
                QuickInfo.Privacy()
            ElseIf Args(0) = "syntax" Then
                QuickInfo.Syntax()
            ElseIf Args(0) = "help" Then
                QuickInfo.Help()
            ElseIf Args(0) = "zip" Then
                If Args.Length < 2 Or command.Contains("|") = False Then
                    Writeline("Invalid syntax")
                Else
                    Dim temp() = Split(command.Remove(0, 4), "|")
                    QIO.CreateZip(temp(0), temp(1))
                End If

            ElseIf Args(0) = "cd" Then
                If command.Remove(0, 3) = ".." Then
                    '?
                ElseIf IO.Directory.Exists(command.Remove(0, 3)) = True Then
                    Environment.CurrentDirectory = command.Remove(0, 3)
                ElseIf IO.Directory.Exists(command.Remove(0, 3).ToLower) = True Then
                    Environment.CurrentDirectory = command.Remove(0, 3).ToLower
                Else
                    Writeline("Directory not found")
                End If
                WorkingDir = Environment.CurrentDirectory
            ElseIf Args(0) = "backup" Then
                QIO.Backup()
            ElseIf Args(0) = "pwd" Then
                Writeline(WorkingDir)
            ElseIf Args(0) = "call" Then
                Shell(command.Remove(0, 5), , True)
                Write("Press any key to continue...")
                ForegroundColor = ConsoleColor.Black
                ReadKey()
                Writeline()
                ForegroundColor = ConsoleColor.White
            Else
                If Scripting = True Then
                    If errorhandle = "default" Then
                        ForegroundColor = ConsoleColor.Red
                        GoTo lol
                    ElseIf errorhandle = "log" Then
                        System.IO.File.AppendAllText("QtErrors.log", "The command '" & command & "' is not found." & vbNewLine)
                    ElseIf errorhandle = "msg" Then
                        Core.msgMbox = "The command '" & command & "' is not found."
                        worker = New Threading.Thread(AddressOf Core.msg3)
                        worker.Start()
                    ElseIf errorhandle = "hide" Then

                    End If
                    i2 += 1
                    QtCont()
                End If
lol:
                If command.EndsWith(".qts") = True Then
                    ReadFile(command)
                Else
                    Writeline("The command '" & command & "' is not found.")
                    ForegroundColor = ConsoleColor.White
                End If
            End If
        Catch
            Er()
        End Try
        Main2()
    End Sub
#End Region

#Region "Using .dlls"
    Private Declare Function LockWorkStation Lib "user32.dll" () As Long
    Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" _
    (ByVal lpszCommand As String, ByVal lpszReturnString As String, _
    ByVal cchReturnLength As Long, ByVal hwndCallback As Long) As Long

#End Region

#Region "Misc"
    Public Function Writeline(Optional ByVal Text As String = Nothing) As Boolean
        Try
            If Scripting = True Then
                If replyhandle = "msg" Then
                    Core.msgMbox = Text
                    Dim worker As New Thread(AddressOf Core.msg3)
                    worker.Start()
                ElseIf replyhandle = "hide" Then

                ElseIf replyhandle = "log" Then
                    IO.File.AppendAllText("QuickTerm.log", Text & vbNewLine)
                ElseIf Text.Contains(" << ") = True Then
                    Dim temp() As String = Text.Split(" << ")
                    IO.File.AppendAllText(temp(1), temp(0) & vbNewLine)
                    Console.WriteLine(temp(0))
                End If
            Else
                If Text.Contains(" >> ") = True Then
                    Text = Text.Replace(" >> ", "▬")
                    Dim temp() As String = Text.Split("▬")
                    IO.File.AppendAllText(temp(1), temp(0) & vbNewLine)
                    Console.WriteLine("File: " & temp(1))
                    Console.WriteLine("Text: " & temp(0))
                Else
                    Console.WriteLine(Text)
                End If
            End If
            Return True
        Catch
            Er(Err.Number, Err.Description)
            Return False
        End Try
    End Function

    Private Function ReadConfig() As Boolean
        Dim i As UInt16 = 0
        Try
            Dim fc() As String = System.IO.File.ReadAllLines("main.conf")
            Do Until i = fc.Length
                Dim f As String = fc(i).ToLower
                If f.StartsWith("BackUpImportance: ".ToLower) = True Then
                    BackUpImportance = f.Remove(0, 18)
                ElseIf f.StartsWith("#") = True Then
                    'foo
                ElseIf f.StartsWith("--- # files to backup") = True Then
                    i += 1
                    Do Until f.StartsWith("- ") = False
                        Filestobackup.Add(f.Remove(0, 2))
                        i += 1
                    Loop
                ElseIf f.StartsWith("BackUpLocation: ".ToLower) = True Then
                    BackUpLocation = f.Remove(0, 16)
                ElseIf f.StartsWith("AllowTelnet: ".ToLower) = True Then
                    If f.Remove(0, 13) = "true" Then
                        AllowTelnet = True
                    ElseIf f.Remove(0, 13) = "false" Then
                        AllowTelnet = False
                    End If
                ElseIf f.StartsWith("EnableLogging: ") = True Then
                    If f.Remove(0, 15) = True Then
                        EnableLogging = True
                    ElseIf f.Remove(0, 15) = False Then
                        EnableLogging = False
                    End If
                ElseIf f.StartsWith("LogFailedLogins: ") = True Then
                    If f.Remove(0, 16) = "true" Then
                        LogFailedLogins = True
                    ElseIf f.Remove(0, 16) = "false" Then
                        LogFailedLogins = False
                    End If
                ElseIf f.StartsWith("LogKicks: ") = True Then
                    If f.Remove(0, 10) = "true" Then
                        LogKicks = True
                    ElseIf f.Remove(0, 10) = "false" Then
                        LogKicks = False
                    End If
                ElseIf f.StartsWith("ServerPort: ") = True Then
                    ServerPort = f.Remove(0, 12)
                ElseIf f.StartsWith("EnableAsciiArt: ") = True Then
                    If f.Remove(0, 16) = "true" Then
                        EnableAsciiArt = True
                    ElseIf f.Remove(0, 16) = "false" Then
                        EnableAsciiArt = False
                    End If
                ElseIf f.StartsWith("TelnetLogLocation: ") = True Then
                    TelnetLogLocation = f.Remove(0, 19)
                ElseIf f.StartsWith("LogCommands: ") = True Then
                    If f.Remove(0, 13) = "true" Then
                        LogCommands = True
                    ElseIf f.Remove(0, 13) = "false" Then
                        LogCommands = False
                    End If
                ElseIf f.StartsWith("TelnetLogName: ") = True Then
                    TelnetLogName = f.Remove(0, 15)
                ElseIf f.StartsWith("TelnetUserName: ") = True Then
                    TelnetUserName = f.Remove(0, 16)
                ElseIf f.StartsWith("TelnetPassword: ") = True Then
                    TelnetPassword = f.Remove(0, 16)
                End If
                i += 1
            Loop
            Return True
        Catch
            Return False
            Er()
        End Try
    End Function

    Private Function UptoDate() As Boolean
        Try
            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("http://quitetiny.com/Updates/Ver_QuickTermSE.txt") 'I should change this
            Dim response As System.Net.HttpWebResponse = request.GetResponse()

            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())

            Dim newestversion As String = sr.ReadToEnd()
            Dim currentversion As String = ver

            If newestversion.Contains(currentversion) Then
                'MessageBox.Show("You have the current version")
                Return True
            Else
                Return False
            End If
        Catch
            Er()
            Return Nothing
        End Try
    End Function

    Private Sub refresh()
        Process.Start("QuickTerm-SE.exe")
        Thread.Sleep(100)
        Environment.Exit(0)
        Exit Sub
    End Sub

    Private Sub Login()
        If AllowTelnet = False Then
            Writeline("Telnet Server is disabled on this server")
            ReadKey()
            Environment.Exit(0)
        End If
        TLog("(?) Connection Established.")
        Try
            If EnableAsciiArt = False Then
                GoTo a
            End If
            Writeline()
            Writeline("                  |ZZzzz>")
            Writeline("                  |")
            Writeline("                  |")
            Writeline("     |ZZzzz>     /^\            |ZZzzz>")
            Writeline("     |          |~~~|           |")
            Writeline("     |        |^^^^^^^|        / \")
            Writeline("    /^\       |[]+    |       |~~~|")
            Writeline(" |^^^^^^^|    |    +[]|       |   |")
            Writeline(" |    +[]|/\/\/\/\^/\/\/\/\/|^^^^^^^|")
            Writeline(" |+[]+   |~~~~~~~~~~~~~~~~~~|    +[]|")
            Writeline(" |       |  []   /^\   []   |+[]+   |")
            Writeline(" |   +[]+|  []  || ||  []   |   +[]+|")
            Writeline(" |[]+    |      || ||       |[]+    |")
            Writeline(" |_______|------------------|_______|")
            Writeline("══════════════════════════════════════════")
            Writeline("Quick Term Server Edition Ver. " & ver)
            Writeline("══════════════════════════════════════════")
A:
            Dim i As UInt16 = 0
            Dim ran As New Random
            Do Until i >= ran.Next(2, 5)
                Writeline()
                Write("User: ")
                Dim user As String = ReadLine()
                'Dim moo As Int16 = 4 'for error test
                'moo = moo / 0
                Writeline()
                Thread.Sleep(100)
                Writeline()
                Write(user & "'s Password: ")
                Dim Pass As String = ReadLine()
                If TelnetUserName.ToLower = "default" Then
                    TelnetUserName = Environment.UserName
                End If
                'MsgBox("(" & user & ") (" & Pass & ")")
                If user = TelnetUserName Then
                    If Pass = TelnetPassword Then
                        TLog("(?) Correct login.")
                        TelnetRunning = True
                        Writeline("Welcome to " & user & " on " & My.Computer.Name)
                        'Read()
                        'Clear()
                        Writeline()
                        back = True
                        Main2()
                    Else
                        i += 1
                    End If
                Else
                    i += 1
                End If
                If LogFailedLogins = True Then
                    If TLog("(!) Invaild login.") = False Then
                        Writeline("An internal error has occured!")
                        Environment.Exit(10)
                    End If
                End If
                Writeline("Fail")
            Loop
            Writeline()
            TLog("(!) Invaild login, client kicked from server.")
            Writeline("Too many attempts! This has been logged.")
            Environment.Exit(0)
        Catch ex As Exception
            If TLog("(E) " & ex.Message) = False Then
                Writeline("An internal error has occured!")
                Environment.Exit(10)
            End If
B:
            Writeline("An internal error has occured!")
            Environment.Exit(10)
        End Try
    End Sub

    Private Function TLog(ByVal Messagee As String) As Boolean
        Try
            If EnableLogging = True Then
                If TelnetLogLocation = "Default" Then
                    TelnetLogLocation = ProgramLocation
                End If
                System.IO.File.AppendAllText(TelnetLogLocation & "\" & TelnetLogName _
                                             , "[" & Date.Now.ToString("hh:mm.ss tt") & "] " & Messagee & vbNewLine)
            Else

            End If
            Return True
        Catch
            Return False
        End Try
    End Function
    Public Sub Er(Optional ByVal ErrorNum As UInt16 = Nothing, Optional ByVal ErrorDesc As String = Nothing, Optional ByVal EndOnFinish As Boolean = False)
        If Not ErrorNum = Nothing AndAlso Not ErrorDesc = Nothing Then
            Err.Number = ErrorNum
            Err.Description = ErrorDesc
        End If
        ErrorLog.Add("Error: (" & Err.Number & ") " & Err.Description)
        If Scripting = True Then
            If errorhandle = "default" Then
                GoTo a
            ElseIf errorhandle = "log" Then
                System.IO.File.AppendAllText("QtErrors.log", "Error: (" & Err.Number & ") " & Err.Description & vbNewLine)
            ElseIf errorhandle = "msg" Then
                Core.msgMbox = "Error: (" & Err.Number & ") " & Err.Description
                worker = New Threading.Thread(AddressOf Core.msg3)
                worker.Start()
            ElseIf errorhandle = "hide" Then

            End If
            i2 += 1
            QtCont()
        End If
a:
        ForegroundColor = ConsoleColor.Red
        If Err.Number = 0 Or Err.Number = Nothing Then
            Writeline("Error: (" & Err.Number & ") Unknown Error.")
        ElseIf Err.Number = 9 Then
            Writeline("Error: (9) Missing Args")
        ElseIf Err.Number = 6 AndAlso Err.Description = "Arithmetic operation resulted in an overflow." Then
            Writeline("Error: (6) The number is too high and resulted in an overflow.")
        ElseIf Err.Description.StartsWith("Index was out of range. Must be non-negative and less than the size") = True Then
            Writeline("Search may not yet be completed (if this error keeps occuring then turn on auto logging)")
        Else
            Writeline("Error: (" & Err.Number & ") " & Err.Description)
        End If
        Err.Clear()
        If EndOnFinish = True Then
            ForegroundColor = ConsoleColor.Black
            Read()
            Environment.Exit(1000)
        End If
        If Core.back2 = True Then
            Core.Cmd2()
        End If
        ForegroundColor = ConsoleColor.White
        Main2()
    End Sub
#End Region

#Region "Scripting"
    Public Sub ReadFile(ByVal QtScript As String) 'opens the qts file for reading
        Try
            QtFile = IO.File.ReadAllLines(QtScript)
            Scripting = True 'tells UI that the scripting is true so after running command it goes back to scripting class
            QtScriptFile = QtScript
            IO.File.WriteAllText(ProgramLocation & "\vars.tmp", Nothing) 'overwrites/creates a tmp file to hold vars
            IO.File.SetAttributes(ProgramLocation & "\vars.tmp", IO.FileAttributes.Temporary) 'makes vars a temp file
            QtCont() 'do work
        Catch
            Er(Err.Number, Err.Description) 'er is the error handler, change it for TTOS's sub
        End Try
    End Sub

    Public Sub QtCont()
        Try
a:
            Do Until i2 >= QtFile.Length
                'replace vars
                If QtFile(i2).StartsWith("//") = True Then 'comment
                    i2 += 1
                    GoTo a
                ElseIf QtFile(i2).StartsWith("/*") = True Then 'multi line comment
                    Do Until QtFile(i2).EndsWith("*/") = True
                        i2 += 1
                    Loop
                    i2 += 1
                    GoTo a
                ElseIf QtFile(i2).StartsWith("FIXME ") = True Then 'fixme for errors (e.x FIXME echo ~moo)
                    i2 += 1
                    GoTo a
                    '<Global Vars>
                ElseIf QtFile(i2).Contains("~user") = True AndAlso QtFile(i2).Contains("'~user'") = False Then
                    QtFile(i2) = QtFile(i2).Replace("~user", Environment.UserName)
                    GoTo a 'to check for more vars
                ElseIf QtFile(i2).Contains("~ver") = True AndAlso QtFile(i2).Contains("'~ver'") = False Then
                    QtFile(i2) = QtFile(i2).Replace("~ver", ver) 'Replace ver to the var that contains TTOS ver number
                    GoTo a
                ElseIf QtFile(i2).Contains("~time") = True AndAlso QtFile(i2).Contains("'~time'") = False Then
                    QtFile(i2) = QtFile(i2).Replace("~time", Date.Now.ToString("hh:mm.ss tt"))
                    GoTo a
                ElseIf QtFile(i2).Contains("~backupname") = True AndAlso QtFile(i2).Contains("'~backupname'") = False Then
                    QtFile(i2) = QtFile(i2).Replace("~backupname", Date.Now.ToString(".hh.mm.") & DateTime.Now.ToString("MM.dd.yyyy."))
                    GoTo a
                ElseIf QtFile(i2).Contains("~date") = True AndAlso QtFile(i2).Contains("'~date'") = False Then
                    QtFile(i2) = QtFile(i2).Replace("~date", DateTime.Now.ToString("dddd, MM/dd/yyyy"))
                    GoTo a
                ElseIf QtFile(i2).Contains("~ip") = True AndAlso QtFile(i2).Contains("'~ip'") = False Then
                    QtFile(i2) = QtFile(i2).Replace("~ip", Net.GetLocalIpAddress.ToString)
                    GoTo a

                    '</Global Vars>

                    '<Temp Vars>
                ElseIf IO.File.ReadAllLines(ProgramLocation & "\vars.tmp").Length > 0 AndAlso QtFile(i2).Contains("~") = True AndAlso QtFile(i2).Contains("'~") = False Then
                    Dim temp() As String = IO.File.ReadAllLines(ProgramLocation & "\vars.tmp")
                    If temp.Length > 65535 Then
                        ForegroundColor = ConsoleColor.Red
                        WriteLine("Error! You can not have more then 65535 vars!")
                        ClearVars()
                        Main2() 'goes back to the UI
                    End If
                    Dim ii As UInt16 = 0
                    Dim davars() As String = Split(QtFile(i2), "~")
                    Do Until ii = temp.Length
                        'MsgBox(davars(1))
                        'MsgBox("if " & temp(ii) & " starts with " & davars(1).Trim & "▬ then")
                        If temp(ii).StartsWith(davars(1).Trim & "▬") = True Then
                            'MsgBox("Found")
                            Dim tempp() As String = Split(temp(ii), "▬")
                            'MsgBox("~" & davars(1).Trim & tempp(1))
                            QtFile(i2) = QtFile(i2).Replace("~" & davars(1).Trim, tempp(1))
                            GoTo a
                        End If
                        ii += 1
                    Loop
                    'MsgBox("not found")
                    'QtFile(i2) = QtFile(i2).Replace("~" & temp1.Item(0), temp1.Item(1))
                Else
                    'if it isnt a var then go to the commands
                    GoTo b
                End If
b:
                'commands
                If QtFile(i2) = Nothing Then 'if line is empty then
                    i2 += 1 'goes to next line
                    GoTo a 'checks for vars
                ElseIf QtFile(i2).StartsWith("#e.") = True Then 'handles errors
                    If QtFile(i2) = "#e.hide" Then
                        errorhandle = "hide"
                    ElseIf QtFile(i2) = "#e.msg" Then
                        errorhandle = "msg"
                    ElseIf QtFile(i2) = "#e.log" Then
                        errorhandle = "log"
                    ElseIf QtFile(i2) = "#e.default" Then
                        errorhandle = "default"
                    End If
                ElseIf QtFile(i2) = "#invisable" Then ':)
                    If invisable = False Then
                        Shell("QuickTermSE.exe -i -s " & QtScriptFile, AppWinStyle.Hide)
                    End If
                    i2 += 1
                    GoTo a
                ElseIf QtFile(i2).StartsWith("#r.") = True Then 'Finish
                    If QtFile(i2) = "#r.msg" Then 'nope
                        replyhandle = "msg"
                    ElseIf QtFile(i2) = "#r.log" Then
                        replyhandle = "log"
                    End If
                ElseIf QtFile(i2).StartsWith(":") = True Then
                    'foo
                ElseIf QtFile(i2).StartsWith("goto ") = True Then
                    Dim temp = QtFile(i2).Remove(0, 5)
                    i2 = 0
                    Do Until QtFile(i2) = ":" & temp
                        i2 += 1
                    Loop
                    i2 += 1
                    GoTo a
                ElseIf QtFile(i2).StartsWith("title ") = True Then
                    Title = QtFile(i2).Remove(0, 6)
                ElseIf QtFile(i2) = "do" Then 'do loop forever
                    loopp = i2
                ElseIf QtFile(i2).StartsWith("do ") = True Then 'do loop for specific ammount
                    looppfor = QtFile(i2).Remove(0, 3)
                    loopp = i2
                ElseIf QtFile(i2) = "loop" Then 'end of a loop
                    If looppfor <> Nothing Then
                        If loopped = looppfor Then
                            i2 += 1
                            GoTo a
                        Else
                            loopped += 1
                            i2 = loopp
                        End If
                    Else
                        i2 = loopp
                    End If
                ElseIf QtFile(i2).StartsWith("if ") = True Then 'if statement
                    QtFile(i2) = QtFile(i2).Remove(0, 3) 'removes "if "
                    Dim newReplace = QtFile(i2).Replace(" = ", "■") 'so a string can contain an equals
                    Dim statement() As String = newReplace.Split("■") 'splits vars to tell them apart
                    Dim s1 = statement(0).ToString() '1st var
                    Dim s2 = statement(1).ToString() '2nd var
                    If s1.ToString() = s2.ToString() Then 'if var1 == var2
                        i2 += 1 'continue running code
                        GoTo a 'top of code
                    Else 'if var1 =/= var2
                        Do Until QtFile(i2) = "end if" 'ignore code until end if
                            If QtFile(i2) = "else" Then 'or ignore code until an else is found
                                i2 += 1 'goes to next line
                                GoTo a 'top of code
                            End If
                            i2 += 1
                        Loop
                    End If
                ElseIf QtFile(i2) = "end if" Then
                    'foo
                ElseIf QtFile(i2) = "else" Then
                    Do Until QtFile(i2) = "end if" 'ignore code until end if
                        i2 += 1
                    Loop
                    GoTo a
                ElseIf QtFile(i2).StartsWith("input ") = True Then
                    Dim ytemp As String = QtFile(i2).Remove(0, 6)
                    ytemp = ytemp.Replace(" ", "")
                    ForegroundColor = ConsoleColor.Cyan
                    Write("> ")
                    ForegroundColor = ConsoleColor.Gray
                    Dim xtemp As String = ReadLine()
                    ForegroundColor = ConsoleColor.White
                    IO.File.AppendAllText(ProgramLocation & "\vars.tmp", ytemp & "▬" & xtemp & vbNewLine)
                ElseIf QtFile(i2).StartsWith("dim ") = True Then 'creates vars
                    QtFile(i2) = QtFile(i2).Remove(0, 4)
                    QtFile(i2) = QtFile(i2).Replace(" = ", "▬")
                    Dim temp() As String = QtFile(i2).Split("▬")
                    IO.File.AppendAllText(ProgramLocation & "\vars.tmp", QtFile(i2) & vbNewLine)
                ElseIf QtFile(i2).Contains("~random") = True AndAlso QtFile(i2).Contains("'~random'") = False Then 'random number gen
                    Dim ran As New Random
                    QtFile(i2) = QtFile(i2).Replace("~random", ran.Next(1000)) 'replaces with a random number between 1 - 1000
                ElseIf QtFile(i2) = "break" Then 'like pause
                    Write("Press any key to continue...")
                    ForegroundColor = ConsoleColor.Black
                    ReadKey()
                    WriteLine()
                    ForegroundColor = ConsoleColor.White
                ElseIf QtFile(i2) = "pause" Then 'like pause
                    Write("Press any key to continue...")
                    ForegroundColor = ConsoleColor.Black
                    ReadKey()
                    WriteLine()
                    ForegroundColor = ConsoleColor.White
                ElseIf QtFile(i2).StartsWith("wait ") = True Then 'sleeps
                    Threading.Thread.Sleep(QtFile(i2).Remove(0, 5))
                ElseIf QtFile(i2) = "refresh" Then
                    WriteLine("Cant do that in scripting!")
                ElseIf QtFile(i2).StartsWith("write") = True Then
                    WriteLine("Cant do that in scripting! Use qwrite instead!")
                Else
                    Threading.Thread.Sleep(100) 'to help prevent stack overflows
                    Commands(QtFile(i2)) 'goes to the UI and does the command that is in the line
                End If
                i2 += 1
            Loop
        Catch
            ForegroundColor = ConsoleColor.Red
            Write("(")
            ForegroundColor = ConsoleColor.White
            Write("!")
            ForegroundColor = ConsoleColor.Red
            Write(") Script Stopped! Error on line ")
            ForegroundColor = ConsoleColor.White
            WriteLine(i2 + 1)
            ClearVars() 'allows for more scripts to be run later
            Er(Err.Number, Err.Description)
        End Try
        ClearVars()
        Main2() 'goes back to the UI
    End Sub

    Public Function ClearVars()
        'refreshes all vars back to default values
        i2 = 0
        QtScriptFile = Nothing
        errorhandle = "default"
        replyhandle = "default"
        invisable = False
        Scripting = False
        QtFile = Nothing
        loopp = Nothing
        loopped = 1
        looppfor = Nothing
        Dim worker As New Thread(AddressOf QIO.SecureDelete)
        'deletes vars.tmp
        Filee = ProgramLocation & "\vars.tmp"
        nomsg = True
        worker.Priority = ThreadPriority.Highest
        On Error Resume Next
        worker.Start()
        Return "Done"
    End Function
#End Region

End Module
