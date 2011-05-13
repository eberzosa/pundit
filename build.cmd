SET FVER=1.0.0.13
SET VER=1.0.0.0

pundit utils -u:asminfo -fv:%FVER% -av:%VER%

msbuild src/Pundit.sln /p:Configuration=Release
msbuild src/Vsix.sln /p:Configuration=Release
del /f/s/q *.rar

rar a -ep -x*vshost* pundit-%FVER%.rar bin\core\*.dll bin\core\*.exe

rar a -ep pundit-vsix-%FVER%.rar src\Pundit.Vsix\bin\Release\*.vsix