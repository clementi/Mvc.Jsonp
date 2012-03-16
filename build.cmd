@echo off

set PATH=%PATH%;c:\Windows\Microsoft.NET\Framework\v4.0.30319\

if not exist output ( mkdir output ) else ( del /q output\*.* )

if "%1" == "clean" ( goto clean )

echo Compiling...
msbuild /nologo /verbosity:quiet Mvc.Jsonp.sln /p:Configuration=Release /t:Clean
msbuild /nologo /verbosity:quiet Mvc.Jsonp.sln /p:Configuration=Release

echo Copying...
copy Mvc.Jsonp\bin\Release\*.* output\

:clean
echo Cleaning...
msbuild /nologo /verbosity:quiet Mvc.Jsonp.sln /p:Configuration=Release /t:Clean

echo Done.