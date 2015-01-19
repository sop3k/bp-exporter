@echo off

echo BASEPROTECT DATABASE FIX TOOL
echo Made by PROSOFT 2012 (soop3k@gmail.com)
echo " "
echo " "

set str=%1
set input=%str:~1,-1%

:GETTEMPNAME
set TMPFILE="dump-%RANDOM%.tmp"
if exist "%TMPFILE%" GOTO :GETTEMPNAME 

setlocal enabledelayedexpansion
set INTEXTFILE=%TMPFILE%

echo Backup old database ...
move "%input%" "%input%_backup"

echo Dumping database "%input%_backup" %INTEXTFILE%
echo .dump | sqlite3.exe "%input%_backup" > %INTEXTFILE%
rbk_to_cmt.exe %INTEXTFILE%

echo "Saving fixed database ..."
echo .read %INTEXTFILE% | sqlite3.exe "%input%"
del %INTEXTFILE%
del "%input%_backup"