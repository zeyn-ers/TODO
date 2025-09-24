@echo off
echo Starting TodoApp Development Environment...
echo.

echo Starting .NET API...
start "TodoApp API" cmd /k "cd src\TodoApp.API && dotnet run"

echo Waiting for API to start...
timeout /t 5 /nobreak > nul

echo Starting Angular Frontend...
start "TodoApp Frontend" cmd /k "cd todo-web && npm start"

echo.
echo Both services are starting...
echo API: https://localhost:57683
echo Frontend: http://localhost:4200
echo.
pause