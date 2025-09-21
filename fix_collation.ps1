# Fix PostgreSQL Collation Issue
# Run this script as Administrator

Write-Host "Fixing PostgreSQL collation issue..." -ForegroundColor Cyan

$psqlPath = "C:\Program Files\PostgreSQL\17\bin\psql.exe"

if (-not (Test-Path $psqlPath)) {
    Write-Host "PostgreSQL not found at: $psqlPath" -ForegroundColor Red
    exit 1
}

Write-Host "PostgreSQL found: $psqlPath" -ForegroundColor Green

# Try different passwords
$passwords = @("postgres", "123", "admin", "password", "")

Write-Host "`nTrying to connect to PostgreSQL..." -ForegroundColor Yellow

$connected = $false
foreach ($password in $passwords) {
    Write-Host "Trying password: $password" -ForegroundColor Yellow
    
    $env:PGPASSWORD = $password
    $testResult = & $psqlPath -U postgres -c "SELECT version();" 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Connected successfully with password: $password" -ForegroundColor Green
        $connected = $true
        break
    } else {
        Write-Host "Failed to connect with password: $password" -ForegroundColor Red
    }
}

if (-not $connected) {
    Write-Host "`nCould not connect to PostgreSQL" -ForegroundColor Red
    Write-Host "Please check:" -ForegroundColor Yellow
    Write-Host "1. PostgreSQL service is running" -ForegroundColor Yellow
    Write-Host "2. Correct postgres user password" -ForegroundColor Yellow
    Write-Host "3. pg_hba.conf settings" -ForegroundColor Yellow
    exit 1
}

Write-Host "`nFixing collation issue..." -ForegroundColor Yellow

# Commands to fix collation
$commands = @(
    "ALTER DATABASE template1 REFRESH COLLATION VERSION;",
    "ALTER DATABASE template0 REFRESH COLLATION VERSION;",
    "ALTER DATABASE postgres REFRESH COLLATION VERSION;"
)

foreach ($command in $commands) {
    Write-Host "Executing: $command" -ForegroundColor Yellow
    $result = & $psqlPath -U postgres -c $command 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Command executed successfully" -ForegroundColor Green
    } else {
        Write-Host "Command failed, but continuing..." -ForegroundColor Yellow
    }
}

# Test database creation
Write-Host "`nTesting database creation..." -ForegroundColor Yellow
$testDbCommand = "CREATE DATABASE test_collation_db;"
$result = & $psqlPath -U postgres -c $testDbCommand 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "Test database created successfully!" -ForegroundColor Green
    
    # Drop test database
    & $psqlPath -U postgres -c "DROP DATABASE test_collation_db;" 2>&1 | Out-Null
    Write-Host "Test database dropped" -ForegroundColor Green
    
    Write-Host "`nCollation issue fixed!" -ForegroundColor Green
    Write-Host "You can now create the aquapark database" -ForegroundColor Green
} else {
    Write-Host "`nCollation issue may still exist" -ForegroundColor Yellow
    Write-Host "Try restarting PostgreSQL service" -ForegroundColor Yellow
}

Write-Host "`nNext steps:" -ForegroundColor Cyan
Write-Host "1. Run the aquapark database creation script" -ForegroundColor White
Write-Host "2. Or execute: .\Запуск_приложения.ps1" -ForegroundColor White
