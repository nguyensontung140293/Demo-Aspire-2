# ============================================================
# Migration helper script for CQRS dual-database setup
# Usage:  .\scripts\migrations.ps1 -Action add -Name MyMigration
#         .\scripts\migrations.ps1 -Action list
#         .\scripts\migrations.ps1 -Action remove
# ============================================================

param(
    [ValidateSet("add", "list", "remove")]
    [string]$Action = "list",
    [string]$Name = ""
)

$project  = "Weather/Weather.Infrastructure/Weather.Infrastructure.csproj"
$startup  = "Weather/Weather.API/Weather.API.csproj"
$writeCtx = "WeatherDbContext"
$readCtx  = "WeatherReadDbContext"

function Run-Migration([string]$context, [string]$outputDir) {
    switch ($Action) {
        "add" {
            if (-not $Name) { Write-Error "Provide -Name for 'add' action"; exit 1 }
            dotnet ef migrations add $Name `
                --context $context `
                --output-dir $outputDir `
                --project $project `
                --startup-project $startup
        }
        "list" {
            dotnet ef migrations list `
                --context $context `
                --project $project `
                --startup-project $startup
        }
        "remove" {
            dotnet ef migrations remove `
                --context $context `
                --project $project `
                --startup-project $startup
        }
    }
}

Write-Host "=== Write DB ($writeCtx) ===" -ForegroundColor Cyan
Run-Migration $writeCtx "Persistence/Migrations"

Write-Host ""
Write-Host "=== Read DB ($readCtx) ===" -ForegroundColor Cyan
Run-Migration $readCtx "Persistence/Migrations/Read"
