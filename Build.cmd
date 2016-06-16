@ECHO OFF 
REM SET PATH=%SystemRoot%\Microsoft.NET\Framework\v2.0.50727;%PATH%
SET PATH=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319;%PATH%
SET PATH=%SystemRoot%\Microsoft.NET\Framework\v3.5;%PATH%

@ECHO ON
MSBuild Energy.Core.sln /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v2.0
