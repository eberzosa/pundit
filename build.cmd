SET FVER=1.0.0.26
SET VER=1.0.0.26

pundit utils -u:asminfo -fv:%FVER% -av:%VER%
pundit utils -u:asminfo -i:**/Pundit.Vsix/**/AssemblyInfo.cs -fv:%FVER% -av:%FVER%
pundit utils -u:regex -i:**/*.vsixmanifest "-s:<Version>.*</Version>" -r:"<Version>%FVER%</Version>"
pundit utils -u:regex -i:**/configure.nsh -s:"PRODUCT_VERSION\s"(.*)"" -r:"PRODUCT_VERSION \"%FVER%\""

msbuild src/Pundit.sln /p:Configuration=Release
msbuild src/Vsix.sln /p:Configuration=Release
del /f/s/q *.zip
del /f/s/q *.rar
del /f/q *.vsix

rem winrar a -afzip -ep -x*vshost* pundit-%FVER%.zip bin\core\*.dll bin\core\*.exe

copy src\Pundit.Vsix\bin\Release\Pundit.vsix .\pundit.vsix
makensis src\installer\installer.nsi

rem winrar a -afzip -ep pundit-vsix-%FVER%.zip src\Pundit.Vsix\bin\Release\*.vsix