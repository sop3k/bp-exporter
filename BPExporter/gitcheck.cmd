FOR /F "tokens=*" %%a in ('git.exe status --short') do SET OUTPUT=%%a
if "%OUTPUT%" == "" (goto ok) else (goto fail)
:ok
exit 0
:fail
echo "GIT DIRTY. PLEASE COMMIT"
exit 1