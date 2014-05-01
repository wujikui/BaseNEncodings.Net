@echo off
copy setting.nuspec ..\BaseNEncodings\BaseNEncodings.nuspec /y > nul
bin\NuGet.exe pack ..\BaseNEncodings\BaseNEncodings.csproj -BasePath .\ -Prop Configuration=Release
del ..\BaseNEncodings\BaseNEncodings.nuspec > nul
echo.
pause