;Pundit Installer for Windows
;Installs:
; - console tools (updates invironment vars as well)
; - Visual Studio 2010 extension

!include "configure.nsh"
!include "EnvVarUpdate.nsh"

!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_DIR_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define LICENSE_FILE "License.rtf"
!define VSIX_INSTALLER_2010_64 "C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\VSIXInstaller.exe"
!define VSIX_INSTALLER_2010_32 "C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\VSIXInstaller.exe"

OutFile "..\..\bin\pundit-setup-${PRODUCT_VERSION}.exe"

;zlib|bzip2|lzma (lowest -> highest)
SetCompressor /SOLID lzma
RequestExecutionLevel admin
BrandingText "${PRODUCT_NAME}" ;text shown at the bottom of the install window
Caption "${PRODUCT_NAME} ${PRODUCT_VERSION}"

; MUI 1.67 compatible ------
!include "MUI.nsh"

; MUI Settings
!define MUI_ABORTWARNING
;!define MUI_ICON "..\Pundit.WinForms.Core\app.ico"
;!define MUI_UNICON "..\Pundit.WinForms.Core\app.ico"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_RIGHT
!define MUI_HEADERIMAGE_BITMAP "Banner.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "Dialog.bmp"

; Welcome page
;!define MUI_WELCOMEPAGE_TITLE_3LINES
;!define MUI_WELCOMEPAGE_TITLE "Welcome to the ${PRODUCT_SHORT_NAME} Setup Wizard"
!insertmacro MUI_PAGE_WELCOME

; License page
!define MUI_LICENSEPAGE_RADIOBUTTONS
;!define MUI_PAGE_HEADER_TEXT "lic_header"
;!define MUI_PAGE_HEADER_SUBTEXT "Please review the license terms before installing ${PRODUCT_SHORT_NAME}"
;!define MUI_LICENSEPAGE_TEXT_TOP "top"
;!define MUI_LICENSEPAGE_TEXT_BOTTOM "If you accept the terms of the agreement, select the first option below. You must accept the agreement to install ${PRODUCT_SHORT_NAME}. Click Next to continue."
!insertmacro MUI_PAGE_LICENSE ${LICENSE_FILE}

; Directory page
!insertmacro MUI_PAGE_DIRECTORY

; Instfiles page
!insertmacro MUI_PAGE_INSTFILES

; Finish page
!define MUI_FINISHPAGE_TITLE_3LINES
;!define MUI_FINISHPAGE_RUN "$INSTDIR\AppMainExe.exe"
;!define MUI_PAGE_CUSTOMFUNCTION_PRE PreFinishPage
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; Reserve files
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS

; MUI end ------

;Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
Name "${PRODUCT_SHORT_NAME}"
InstallDir "C:\Program Files\${PRODUCT_DIR_NAME}"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

;Add version information to output exe
VIProductVersion "${PRODUCT_VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "${PRODUCT_NAME}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "${PRODUCT_WEB_SITE}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "${PRODUCT_PUBLISHER}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" "${PRODUCT_NAME} is a trademark of ${PRODUCT_PUBLISHER}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright ${PRODUCT_PUBLISHER}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "${PRODUCT_NAME} installer"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "${PRODUCT_VERSION}"

Section "!Core" SCCONSOLE
  SectionIn 1
  SetOverwrite ifdiff
  SetShellVarContext all

  SetOutPath "$INSTDIR\core"

  File "..\..\bin\core\*.exe"
  File "..\..\bin\core\*.dll"
  File ${LICENSE_FILE}
  
  ${EnvVarUpdate} $0 "PATH" "A" "HKLM" "$INSTDIR\core" ; Append

  ;RMDir /r /REBOOTOK "$SMPROGRAMS\${PRODUCT_DIR_NAME}"
  ;CreateDirectory "$SMPROGRAMS\${PRODUCT_DIR_NAME}"
  ;createShortCut "$SMPROGRAMS\${PRODUCT_DIR_NAME}\Global Settings.lnk" "$INSTDIR\pundit-gui.exe" "--global" "$INSTDIR\pundit-gui.exe"
  ;createShortCut "$SMPROGRAMS\${PRODUCT_DIR_NAME}\License Agreement.lnk" "$INSTDIR\License.rtf"
  ;createShortCut "$SMPROGRAMS\${PRODUCT_DIR_NAME}\Documentation.lnk" "http://pundit.codeplex.com"
  ;createShortCut "$SMPROGRAMS\${PRODUCT_DIR_NAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe"

SectionEnd

Section "!VSIX" SCVSIX
  SectionIn 1

  SetOutPath "$INSTDIR\vs2010"
  File "..\..\bin\vsix\extension.vsixmanifest"
  File "..\..\bin\vsix\*.dll"
  File "..\..\bin\vsix\Pundit.pkgdef"

  Var /GLOBAL installer_path
  
  IfFileExists "${VSIX_INSTALLER_2010_64}" 0 +4
    StrCpy $installer_path "${VSIX_INSTALLER_2010_64}"
  IfFileExists "${VSIX_INSTALLER_2010_32}" 0 +2
    StrCpy $installer_path "${VSIX_INSTALLER_2010_32}"
  DetailPrint "VS2010 installer: $installer_path"
  
  StrCmp "" "$installer_path" +4 0
    ;ExecWait '"$installer_path" "$INSTDIR\Pundit.vsix"' $0
    StrCpy $0 "0" ;stub
    StrCmp "0" "$0" +2 0
      Abort "failed to install Visual Studio 2010 extension, error code: $0"

SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninstall.exe"
  ;WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\AppMainExe.exe"

  ;add uninstall info
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninstall.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\bin\MimecastServicesForExchange.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  ;WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "MseRegion" "${PRODUCT_REGION}"

SectionEnd

Section Uninstall
  SetShellVarContext all
  
  ${un.EnvVarUpdate} $0 "PATH" "R" "HKLM" "$INSTDIR\core" ; Append
  
  ;delete all program files (don't set REBOOTOK flag!)
  ;MessageBox MB_ICONINFORMATION|MB_OK "removing: $INSTDIR"
  RMDir /r "$INSTDIR"
  
  RMDir /r /REBOOTOK "$SMPROGRAMS\${PRODUCT_DIR_NAME}"

SectionEnd