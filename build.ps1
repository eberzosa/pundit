msbuild /property:Configuration=Release src\Pundit.sln

$punditExe = $(Get-Item .\out\Release\pundit.exe).FullName

$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($punditExe).FileVersion

pundit pack pundit.xml -v $version