@ECHO OFF 
REM SET PATH=%SystemRoot%\Microsoft.NET\Framework\v2.0.50727;%PATH%
REM SET PATH=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319;%PATH%
REM SET PATH=%SystemRoot%\Microsoft.NET\Framework\v3.5;%PATH%

REM @ECHO ON
REM MSBuild Energy.Core.sln /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v2.0

@ECHO ON
dotnet build -c Release Energy.Core.sln
