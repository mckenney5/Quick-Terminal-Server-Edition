Imports System.Console
Public Class Info
    Public Sub BetaCommands()
        WriteLine("BETA Commands")
        WriteLine("VERSION: " & QuickTerminalSE.ver & vbNewLine _
                         & "None at the moment" & vbNewLine _
                         )
        Main2()
    End Sub

    Public Sub Credits()
        WriteLine("Credits")
        WriteLine("This whole program is created by Adam McKenney with the exception of")
        WriteLine("update, lock and IP (all of their Sources is Unknown)" & vbNewLine & "and dir & ls command found at:")
        WriteLine("http://www.developerfusion.com/code/3681/list-files-in-a-directory/")
        WriteLine("Icon by:")
        WriteLine("http://www.fasticon.com")
        WriteLine("For info on programs and other things see README.txt")
        Main2()
    End Sub

    Public Sub Privacy()
        WriteLine("Privacy")
        WriteLine("This whole program is designed to keep your information a secret.")
        WriteLine("We DO NOT sell your information, nor do we keep it.")
        WriteLine("Information sent includes: IP Address (if you're not using a proxy)")
        WriteLine("~Your IP is displayed on any commands that requires a tcp connection~")
        WriteLine("Also this program saves logs, if you want to delete them use del3 <File>")
        Main2()
    End Sub

    Public Sub Syntax()
        WriteLine("All commands that require a syntax:")
        WriteLine("VERSION: " & QuickTerminalSE.ver & vbNewLine _
                         & "cd <Directory>" & vbNewLine _
                         & "con <Address> <Port> (Address= an address for a server, can be ip)" & vbNewLine _
                         & "copy <File>|<Location> (File= file you want to copy)" & vbNewLine _
                         & "dec <filename> (filename= a text file [Optional])" & vbNewLine _
                         & "define <word> (a word to look up)" & vbNewLine _
                         & "delete <filename> (filename= file on computer)" & vbNewLine _
                         & "delete2 <filename> (filename= file on computer)" & vbNewLine _
                         & "delete3 <filename> (filename= file on computer)" & vbNewLine _
                         & "dns <Address> (Address= website address)" & vbNewLine _
                         & "download <WebAddress> <FileName>" & vbNewLine _
                         & "echo <input> (input= string)" & vbNewLine _
                         & "kill <program_name> (porgram_name= a running program)" & vbNewLine _
                         & "move <File>|<Location> (File= file you want to move)" & vbNewLine _
                         & "msg <message> (message= message as string)" & vbNewLine _
                         & "ping <Server> (Server= Web Address or IP)" & vbNewLine _
                         & "prompt <Prompt String>" & vbNewLine _
                         & "qwrite <File>|<Text> (File= file name, Text= text the file will contain)" & vbNewLine _
                         & "random <Lowest> <Highest> (Low/High= numbers for random num gen.)" & vbNewLine _
                         & "read <filename> (filename= file on computer)" & vbNewLine _
                         & "run <filename> (filename= file on computer)" & vbNewLine _
                         & "sc <IMG Name> (IMG name= ScreenShot)" & vbNewLine _
                         & "scan <Target> <Start> <End> (target= server start/end = port)" & vbNewLine _
                         & "timer <On/Off>" & vbNewLine _
                         & "upload <WebAddress> <FileName>" & vbNewLine _
                         & "write <filename> (filename= file on computer)" & vbNewLine _
                         & "zip <zipfile>|<File-To-Archive>" & vbNewLine _
                         )
        Main2()
    End Sub

    Public Sub Help()
        WriteLine("VERSION: " & QuickTerminalSE.ver & vbNewLine _
                                  & "-c                  (runs a commands)" & vbNewLine _
                                  & "-h                  (displays this stuff)" & vbNewLine _
                                  & "-l                  (brings you to a login screen [For remote accsess])" & vbNewLine _
                                  & "-s                  (runs a script)" & vbNewLine _
                                  & "-x                  (runs a command then closes. use -c then -x)" & vbNewLine _
                                  & "beta                (shows beta commands, use the commands with caution)" & vbNewLine _
                                  & "cap                 (captures QuickTerm)" & vbNewLine _
                                  & "cd                  (changes current directory)" & vbNewLine _
                                  & "cdtray              (opens/closes cd tray)" & vbNewLine _
                                  & "cls                 (clears screan)" & vbNewLine _
                                  & "clear               (clears screan)" & vbNewLine _
                                  & "cred                (shows credits and sources)" & vbNewLine _
                                  & "cmd                 (runs a shell command)" & vbNewLine _
                                  & "con                 (connects to a server on a specified port)" & vbNewLine _
                                  & "copy                (copys a file to another location)" & vbNewLine _
                                  & "date                (displays date)" & vbNewLine _
                                  & "dec                 (decrypts a string with AES)" & vbNewLine _
                                  & "define              (looks up a word via free dictionary)" & vbNewLine _
                                  & "delete              (deletes a file)" & vbNewLine _
                                  & "delete2             (deletes a file without freezing console)" & vbNewLine _
                                  & "delete3             (deletes and overwrites 16 times)" & vbNewLine _
                                  & "dev                 (shows the dev log)" & vbNewLine _
                                  & "dir                 (list files in the local directory)" & vbNewLine _
                                  & "dns                 (does a dns look up of a server)" & vbNewLine _
                                  & "download            (downloads a file via internet)" & vbNewLine _
                                  & "echo                (echos what you type)" & vbNewLine _
                                  & "enc                 (encrypts a string with AES)" & vbNewLine _
                                  & "errors              (displays all errors that has happened)" & vbNewLine _
                                  & "exit                (closes the program)" & vbNewLine _
                                  & "folder              (creates a new folder)" & vbNewLine _
                                  & "help                (this stuff)" & vbNewLine _
                                  & "info                (shows computer info)" & vbNewLine _
                                  & "install             (makes .qts files be owned by QuickTerm)" & vbNewLine _
                                  & "ip                  (displays local IPv4 Address)" & vbNewLine _
                                  & "kill                (ends a procsess)" & vbNewLine _
                                  & "lock                (locks computer)" & vbNewLine _
                                  & "logoff              (logs off computer)" & vbNewLine _
                                  & "ls                  (list files in the local directory)" & vbNewLine _
                                  & "md5                 (shows the md5 hash of a file)" & vbNewLine _
                                  & "md5log              (logs all md5 hashes to a file in the current dir)" & vbNewLine _
                                  & "move                (moves a file to another location)" & vbNewLine _
                                  & "msg                 (shows a message box)" & vbNewLine _
                                  & "ping                (pings a server with 32 bytes of data)" & vbNewLine _
                                  & "prompt              (changes prompt string '==> ' to a user defined one)" & vbNewLine _
                                  & "pwd                 (prints the working directory)" & vbNewLine _
                                  & "qwrite              (quicky writes a text file)" & vbNewLine _
                                  & "random              (generates random number between 1 - 1000)" & vbNewLine _
                                  & "read                (opens an reads a text file)" & vbNewLine _
                                  & "refresh             (closes current console then opens a new one)" & vbNewLine _
                                  & "remove              (removes a directory)" & vbNewLine _
                                  & "restart             (restarts computer)" & vbNewLine _
                                  & "run                 (runs a program)" & vbNewLine _
                                  & "sc                  (captures the whole screen and saves as .png)" & vbNewLine _
                                  & "scan                (scans a computer for open ports)" & vbNewLine _
                                  & "search              (searches the web via duckduckgo)" & vbNewLine _
                                  & "sp                  (displays current port being scanned)" & vbNewLine _
                                  & "syntax              (displays command syntax)" & vbNewLine _
                                  & "telnet              (uses program nc and QuickTermSE for t-server [Not Secure])" & vbNewLine _
                                  & "telnet2             (uses program nc and cmd.exe for t-server [Not Secure])" & vbNewLine _
                                  & "time                (displays current time)" & vbNewLine _
                                  & "timer               (starts/stops a timer and displays time in seconds)" & vbNewLine _
                                  & "upload              (uploads a file via internet)" & vbNewLine _
                                  & "ver                 (current program version)" & vbNewLine _
                                  & "vscan               (scans the current dir for viruses)" & vbNewLine _
                                  & "whoami              (tells you who you are, very simular to bash)" & vbNewLine _
                                  & "write               (creates and edits a text file [OVERWRITES])" & vbNewLine _
                                  & "zip                 (creates a zip archive)" & vbNewLine _
                                  )
        Main2()
    End Sub
End Class
