@echo off
chcp 65001 >nul
echo ========================================================
echo   تشغيل مشروع المترجم العربي (Arabic Compiler WinForms)
echo ========================================================
cd /d "%~dp0ArabicCompiler.UI"
call run.bat
