@ECHO OFF

IF "%CONFIGURATION%"=="" SET CONFIGURATION=Debug

star %* --resourcedir="%~dp0src\SampleWebsiteTheme\wwwroot" "%~dp0src\SampleWebsiteTheme\bin\%CONFIGURATION%\SampleWebsiteTheme.exe"
