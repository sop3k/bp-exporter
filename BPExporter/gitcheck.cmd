@echo off
FOR /F "tokens=*" %%a in ('C:\Git\bin\git.exe status --short') do SET OUTPUT=%%a
if "%OUTPUT%" == "" (goto ok) else (goto fail)
:ok
exit 0
:fail
echo "GIT DIRTY. PLEASE COMMIT"
exit 1