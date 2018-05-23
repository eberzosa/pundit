$Root="X:\"
$ILMerge=$Root + "tools\ilmerge.exe"

$Solution = "src\Pundit.sln"
$ReleaseFolder = "src\Pundit\bin\Release"

$TempFolder = "tmp"
$MergedName = "$TempFolder\Pundit.exe"

MsBuild.exe $Solution /t:Build /p:Configuration=Release

New-Item tmp -ItemType Directory -ErrorAction SilentlyContinue

$MergeParams = '/keyfile:..\eberzosagh.snk', '/ndebug', '/wildcards', '/targetplatform:4.0,C:\Windows\Microsoft.NET\Framework64\v4.0.30319', "/out:$MergedName", "$ReleaseFolder\Pundit.exe", "$ReleaseFolder\*.dll"

Write-Host "Merging..."
& $ILMerge $MergeParams

$punditExe = $(Get-Item $MergedName).FullName

$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($punditExe).FileVersion

Write-Host "Packing $version to $TempFolder"
pundit pack pundit.xml --versionRelease $version --output $TempFolder