@echo off
FOR /F "tokens=*" %%a in ('C:\Git\bin\git.exe status --short') do SET OUTPUT=%%a
FOR /F "tokens=*" %%a in ('C:\Git\bin\git.exe rev-parse --short HEAD') do SET REV=%%a
if "%OUTPUT%" == "" (goto ok) else (goto fail)
:ok
"C:\Program Files (x86)\Inno Setup 5\iscc.exe" ..\Installer\install_x64.iss /dRevision="%REV%"
exit 0
:fail
echo "GIT DIRTY. PLEASE COMMIT"
exit 1