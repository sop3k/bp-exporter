@echo off

echo BASEPROTECT DATABASE FIX TOOL
echo Made by PROSOFT 2012 (soop3k@gmail.com)
echo " "
echo " "

setlocal enabledelayedexpansion
set INTEXTFILE=dumptmp.sql 

echo Opening database "%1" 
echo .dump | sqlite3.exe "%1" > %INTEXTFILE%
rbk_to_cmt.exe %INTEXTFILE%

echo "Backup old database ..."
move "%1" "%1_backup"

echo "Saving fixed database ..."
echo .read %INTEXTFILE% | sqlite3.exe "%1"

del %INTEXTFILE%