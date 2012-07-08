!macro linkFile junction target
  System::Call "kernel32::CreateSymbolicLinkW(w `${junction}`, w `${target}`, i 0) i .s"
!macroend

;find Visual Studio 2010 installation path
;arguments - none
;returns - if found, $0 contains full directory name, otherwise empty string
!macro getVs2010ExFolder
  StrCpy $0 ""
  ReadRegStr $0 HKLM "SOFTWARE\Microsoft\VisualStudio\10.0" "InstallDir"
  StrCmp $0 "" 0 +3
  DetailPrint "not found in 32bit registry, trying 64bit node..."
  ReadRegStr $0 HKLM "SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0" "InstallDir"
  StrCmp $0 "" +2 0
  StrCpy $0 "$0Extensions"
!macroend



