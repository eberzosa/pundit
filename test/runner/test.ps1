function LegacyTest
{
    Write-Host "Running Legacy Bin folder test..."
    Clean
    Resolve pundit-Legacy-bin-folder.xml
    CheckFiles "lib\file1.dll", "lib\file2.dll"
}

function NormalTest
{
    Write-Host "Running Normal test..."
    Clean
    Resolve pundit.xml
    CheckFiles "lib\file1.dll", "lib\file2.dll" "lib\file1-any.dll", "lib\file2-any.dll" "lib\file1-release.dll", "lib\file2-release.dll"
    CheckFiles "lib\net461\file1-net461.dll", "lib\net461\file2-net461.dll" "lib\netstandard2.0\file1-standard20.dll", "lib\netstandard2.0\file2-standard20.dll" 
}


function NormalTestRelease
{
    Write-Host "Running Normal (Release) test..."
    Clean
    ResolveWithConfig pundit.xml Release
    CheckFiles "lib\file1.dll", "lib\file2.dll" "lib\file1-any.dll", "lib\file2-any.dll" "lib\file1-release.dll", "lib\file2-release.dll"
    CheckFiles "lib\net461\file1-net461.dll", "lib\net461\file2-net461.dll" "lib\netstandard2.0\file1-standard20.dll", "lib\netstandard2.0\file2-standard20.dll" 
}

function NormalTestDebug
{
    Write-Host "Running Normal (Debug) test..."
    Clean
    ResolveWithConfig pundit.xml Debug
    CheckFiles "lib\file1.dll", "lib\file2.dll" "lib\file1-any.dll", "lib\file2-any.dll" "lib\file1-debug.dll", "lib\file2-debug.dll"
    CheckFiles "lib\net461\file1-net461.dll", "lib\net461\file2-net461.dll" "lib\netstandard2.0\file1-standard20.dll", "lib\netstandard2.0\file2-standard20.dll" 
}

function CheckFiles([string[]]$files)
{
    foreach ($file in $files)
    {
        CheckFile($file)
    }
}

function CheckFile([string]$fileName)
{
    if (-Not (Test-Path $fileName))
    {
        Write-Host "   File '$fileName' does not exist." -ForegroundColor Red
    }
}

function Resolve([string]$resolveFile)
{
    & "..\src\Pundit\bin\Debug\net461\pundit.exe" resolve -p repo -f -m $resolveFile > NULL
}

function ResolveWithConfig([string]$resolveFile, [string]$config)
{
    & "..\src\Pundit\bin\Debug\net461\pundit.exe" resolve --repositoryPath repo -f --manifest $resolveFile --configuration $config > NULL
}

function Clean 
{
	Remove-Item lib -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item .pundit-install-index -Force -ErrorAction SilentlyContinue
}


LegacyTest
NormalTest
NormalTestRelease
NormalTestDebug
