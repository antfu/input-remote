@echo off
xcopy src\controller bin\current\controller\ /S /Y

C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe src\InputRemote.Client.Receiver\InputRemote.Client.Receiver.csproj /t:Build /p:Configuration=Release,OutDir="..\..\bin\current"
