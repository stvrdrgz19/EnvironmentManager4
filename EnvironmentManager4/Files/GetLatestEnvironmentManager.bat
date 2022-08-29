@ECHO OFF
SET buildDir=\\sp-fileserv-01\Team QA\Tools\Environment Manager\Installers

FOR /F "delims=|" %%I IN ('DIR "%buildDir%\*.*" /B /O:D') DO SET newestBuild=%buildDir%\%%I

ECHO Grabbing the installer. . . This may take a few moments. . .
start "" "%newestBuild%"

taskkill /IM "Environment Manager.exe" /F