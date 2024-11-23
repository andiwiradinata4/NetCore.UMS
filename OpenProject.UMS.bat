@echo off
cd /d "C:\Project\NetCore\UMS"
git pull origin
set /p userInput=Open Project Solution? [y/n]:
if /i "%userInput%"=="y" (
	start "" "C:\Project\NetCore\UMS\UMS.Web\UMS.Web.sln"
)
pause