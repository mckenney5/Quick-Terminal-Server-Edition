Imports System.Net
Imports System.Text
Imports System.Console
Imports System.Net.Sockets
Public Class NetworkClass
    Public CurrentPort As UInt16 = 0
    Dim senddata As String
    Dim uClient As New TcpClient
    Dim DataStream As NetworkStream
    Dim Packet(1024) As Byte
    Dim Oports As New ArrayList
    Dim Address As String
    Dim Startport As UInt16
    Dim Endport As UInt16
    Dim Startportt As UInt16
    Dim Endportt As UInt16

    Public Sub DnsLookUp(ByVal Server As String)
        Console.WriteLine("Looking up " & Server)
        Dim Hostname As IPHostEntry = Dns.GetHostEntry(Server)
        Dim ip As IPAddress() = Hostname.AddressList
        Dim stat As String
        stat = ip(0).ToString
        Console.WriteLine(stat)
        Console.WriteLine()
        Main2()
    End Sub

    Public Function Ping_Server(ByVal Server As String)
        Try
            Return My.Computer.Network.Ping(Server)
        Catch
            Er(Err.Number, Err.Description)
            Return "Error"
        End Try
    End Function

    Public Function GetLocalIpAddress() As System.Net.IPAddress
        Dim strHostName As String
        Dim addresses() As System.Net.IPAddress
        strHostName = System.Net.Dns.GetHostName()
        addresses = System.Net.Dns.GetHostAddresses(strHostName)
        ' Find an IpV4 address
        For Each address As System.Net.IPAddress In addresses
            If address.ToString.Contains(".") Then
                Return address
            End If
        Next
        ' No IpV4 address? Return the loopback address.
        Return System.Net.IPAddress.Loopback
    End Function

    Public Sub Scan(ByVal Address1 As String, Optional ByVal StartPort1 As UInt16 = 20, Optional ByVal EndPort1 As UInt16 = 82) 'KILL ME PLEASE
        Address = Address1
        Startport = StartPort1
        Endport = EndPort1
        Writeline("Scanning started. You will get a message when its done.")
        Writeline("This scan is slow, so don't hold your breath.")
        Dim t1 As New System.Threading.Thread(AddressOf ScanThread)
        t1.Priority = Threading.ThreadPriority.Highest
        t1.Start()
        Main2()
    End Sub

    Private Sub ScanThread()
        TRunning += 1
        Dim byteBuffer = Encoding.ASCII.GetBytes(vbNewLine)
        Dim OpenPorts As UInt16 = 0
        Dim ClosedPorts As UInt16 = 0
        CurrentPort = Startport
        Try
a:
            Dim Client As New TcpClient
            Dim Data As NetworkStream
            Dim test(15) As Byte
            Client.Connect(Address, CurrentPort)
            Data = Client.GetStream
            Data.Write(byteBuffer, 0, byteBuffer.Length)
            Oports.Add(CurrentPort)
            OpenPorts = OpenPorts + 1
            Client.Close()
            Data.Close()
            Data.Flush()
            If CurrentPort < Endport Then
                CurrentPort = CurrentPort + 1
                GoTo a
            Else
                System.IO.File.WriteAllText(Address & ".log", "Scanned " & Endport - Startport & " ports. Open: " & OpenPorts & " Closed: " & ClosedPorts)
                System.IO.File.AppendAllText(Address & ".log", vbNewLine & "Open Ports: " & vbNewLine)
                Dim i As UInt16 = 0
                Do Until i = Oports.Count
                    System.IO.File.AppendAllText(Address & ".log", Oports.Item(i) & vbNewLine)
                    i += 1
                Loop
                MsgBox("Scan of " & Address & " finished!", MsgBoxStyle.Information, "Quick Terminal Scan")
                Dim result1 As Microsoft.VisualBasic.MsgBoxResult = MsgBox("Would you like to view the results?", MsgBoxStyle.YesNo, "Quick Terminal Scan")
                If result1 = MsgBoxResult.Yes Then
                    Process.Start(Address & ".log")
                End If
                CurrentPort = 0
                TRunning -= 1
                Exit Sub
            End If
        Catch
            'If Not Err.Number = 5 Then
            'WriteLine("Error (" & Err.Number & ") " & Err.Description)
            'End If
            If CurrentPort < Endport Then
                ClosedPorts = ClosedPorts + 1
                CurrentPort = CurrentPort + 1
                GoTo a
            Else
                System.IO.File.WriteAllText(Address & ".log", "Scanned " & Endport - Startport & " ports. Open: " & OpenPorts & " Closed: " & ClosedPorts)
                System.IO.File.AppendAllText(Address & ".log", vbNewLine & "Open Ports: " & vbNewLine)
                Dim i As UInt16 = 0
                Do Until i = Oports.Count
                    System.IO.File.AppendAllText(Address & ".log", Oports.Item(i) & vbNewLine)
                    i += 1
                Loop
                MsgBox("Scan of " & Address & " finished!", MsgBoxStyle.Information, "Quick Terminal Scan")
                Dim result1 As Microsoft.VisualBasic.MsgBoxResult = MsgBox("Would you like to view the results?", MsgBoxStyle.YesNo, "Quick Terminal Scan")
                If result1 = MsgBoxResult.Yes Then
                    Process.Start(Address & ".log")
                End If
                TRunning -= 1
                Exit Sub
            End If
            GoTo a
        End Try
        TRunning -= 1
    End Sub

    Public Sub Connect(ByVal Address As String, ByVal Port As Integer)
        Try
            WriteLine("Connecting...")
            WriteLine()
            Shell("nc " & Address & " " & Port, AppWinStyle.NormalFocus, True)
            WriteLine("Disconnected")
        Catch
            Er(Err.Number, Err.Description)
        End Try
    End Sub

    Public Sub Download(ByVal Address As String, ByVal FileName As String)
        Try
            Dim w As New WebClient
            w.DownloadFile("http://" & Address, FileName)
            WriteLine("Done.")
        Catch
            Er(Err.Number, Err.Description)
        End Try
    End Sub

    Public Sub Upload(ByVal Address As String, ByVal File2 As String)
        Try
            If IO.File.Exists(File2) = False Then
                WriteLine("File not found")
                Main2()
            End If
            Dim w As New WebClient
            w.UploadFile(Address, File2)
        Catch
            Er(Err.Number, Err.Description)
        End Try
    End Sub

    Public Sub Grab(ByVal Address As String, Optional Port As UInt16 = 80, Optional Message As String = "HEAD / HTTP/1.1")
        Try
            Dim tcpClient As New System.Net.Sockets.TcpClient()
            tcpClient.Connect(Address, Port)
            Dim networkStream As NetworkStream = tcpClient.GetStream()
            If networkStream.CanWrite And networkStream.CanRead Then
                Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(Message & vbCrLf & vbCrLf)
                networkStream.Write(sendBytes, 0, sendBytes.Length)
                Dim bytes(tcpClient.ReceiveBufferSize - 20) As Byte
                networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize - 20))
                Dim returndata As String = Encoding.ASCII.GetString(bytes).Trim
                WriteLine(returndata.Trim.Trim)
            Else
                If Not networkStream.CanRead Then
                    Console.WriteLine("cannot not read data to this stream")
                    tcpClient.Close()
                Else
                    If Not networkStream.CanWrite Then
                        Console.WriteLine("cannot write data from this stream")
                        tcpClient.Close()
                    End If
                End If
            End If
            Main2()
        Catch
            Er(Err.Number, Err.Description)
        End Try
    End Sub

    Public Sub RBrowse(ByVal Address As String) 'Lynx got nothing on me
        Try
            Dim wc As New WebClient
            wc.Headers(HttpRequestHeader.Accept) = "*/*"
            wc.Headers(HttpRequestHeader.UserAgent) = "Mozilla/1.0 (compatible; MSIE 1.0; Windows NT 1.0; Trident/1.0)"
            Dim meow As String = wc.DownloadString("http://" & Address)
            meow = meow.Replace("<td>", "")
            meow = meow.Replace("</td>", "")
            meow = meow.Replace("<tr>", "")
            meow = meow.Replace("</tr>", "")
            meow = meow.Replace("<html>", "")
            meow = meow.Replace("</html>", "")
            meow = meow.Replace("<a href=", "")
            meow = meow.Replace("</a>", "")
            meow = meow.Replace("  ", "")
            meow = meow.Replace("</br>", vbNewLine)
            meow = meow.Replace("  ", "")
            meow = meow.Replace("<table>", "")
            meow = meow.Replace("</table>", "")
            meow = meow.Replace("<frame>", "")
            meow = meow.Replace("</frame>", "")
            meow = meow.Replace("<", "")
            meow = meow.Replace(">", " ")
            meow = meow.Replace("  ", "")
            Writeline(meow)
            Writeline()
            Main2()
        Catch
            Er(Err.Number, Err.Description)
        End Try
    End Sub

    Public Function SearchPage(ByVal Address As String, ByVal Term As String) As Boolean
        Try
            Dim wc As New WebClient
            wc.Headers(HttpRequestHeader.Accept) = "*/*"
            wc.Headers(HttpRequestHeader.UserAgent) = "Mozilla/1.0 (compatible; MSIE 1.0; Windows NT 1.0; Trident/1.0)"
            Dim meow As String = wc.DownloadString("http://" & Address)
            If meow.Contains(Term) = True Then
                Return True
            Else
                Return False
            End If
        Catch
            Er(Err.Number, Err.Description)
        End Try
    End Function
End Class
