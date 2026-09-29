@echo off
chcp 65001 >nul
echo ========================================================
echo   تشغيل مشروع المترجم العربي (Arabic Compiler WinForms)
echo ========================================================
cd /d "%~dp0"

if exist "..\ArabicCompiler.Native\x64\Release\ArabicCompiler.Native.dll" (
    copy /Y "..\ArabicCompiler.Native\x64\Release\ArabicCompiler.Native.dll" "ArabicCompiler.Native.dll" >nul
) else if exist "..\ArabicCompiler.Native\x64\Debug\ArabicCompiler.Native.dll" (
    copy /Y "..\ArabicCompiler.Native\x64\Debug\ArabicCompiler.Native.dll" "ArabicCompiler.Native.dll" >nul
)

dotnet run -c Release --project ArabicCompiler.UI.csproj
if errorlevel 1 (
    echo.
    echo جاري البناء ثم إعادة التشغيل...
    dotnet build -c Release
    dotnet run -c Release --project ArabicCompiler.UI.csproj
)
pause
