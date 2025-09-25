@echo off
echo Starting TodoApp...

echo.
echo [1/2] Starting API...
start "TodoApp API" cmd /k "cd /d src\TodoApp.API && dotnet run --urls https://localhost:7001"

timeout /t 3 /nobreak >nul

echo.
echo [2/2] Starting Frontend...
start "TodoApp Frontend" cmd /k "cd /d todo-web && ng serve --port 4200"

echo.
echo ✅ Both services starting...
echo 📡 API: https://localhost:7001
echo 🌐 Frontend: http://localhost:4200
echo.
pause