# 参数
param (
    [Parameter()]
    [string]
    $Name = $null
)
$location = Get-Location
Set-Location ../src/Http.API/
if ([string]::IsNullOrWhiteSpace($Name)) {
    $Name = [DateTime]::Now.ToString("yyyyMMdd-HHmmss")
}
dotnet build
dotnet ef migrations add $Name -c CommandDbContext --no-build
Set-Location $location