!macro linkFile junction target
  System::Call "kernel32::CreateSymbolicLinkW(w `${junction}`, w `${target}`, i 0) i .s"
!macroend

;------------------------------------------------------------------------------
; GetParent
; input, top of stack  (i.e. C:\Program Files\Poop)
; output, top of stack (replaces, with i.e. C:\Program Files)
; modifies no other variables.
;
; Usage:
;   Push "C:\Program Files\Directory\Whatever"
;   Call GetParent
;   Pop $0
;   ; at this point $0 will equal "C:\Program Files\Directory"
Function GetParent
  Exch $0 ; old $0 is on top of stack
  Push $1
  Push $2
  StrCpy $1 -1
  loop:
    StrCpy $2 $0 1 $1
    StrCmp $2 "" exit
    StrCmp $2 "\" exit
    IntOp $1 $1 - 1
  Goto loop
  exit:
    StrCpy $0 $0 $1
    Pop $2
    Pop $1
    Exch $0 ; put $0 on top of stack, restore $0 to original value
FunctionEnd

;copypasted for UN
Function un.GetParent
  Exch $0 ; old $0 is on top of stack
  Push $1
  Push $2
  StrCpy $1 -1
  loop:
    StrCpy $2 $0 1 $1
    StrCmp $2 "" exit
    StrCmp $2 "\" exit
    IntOp $1 $1 - 1
  Goto loop
  exit:
    StrCpy $0 $0 $1
    Pop $2
    Pop $1
    Exch $0 ; put $0 on top of stack, restore $0 to original value
FunctionEnd


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

!macro getVs2010SchemasFolder un
  !insertmacro getVs2010ExFolder
  ;MessageBox MB_OK $0
  StrCmp $0 "" GetSchemaEnd 0
  Push $0
  Call ${un}GetParent
  Pop $0
  Push $0
  Call ${un}GetParent
  Pop $0
  Push $0
  Call ${un}GetParent
  Pop $0
  StrCpy $0 "$0\Xml\Schemas\"
  ;MessageBox MB_OK $0
  
  GetSchemaEnd:
  
!macroend



