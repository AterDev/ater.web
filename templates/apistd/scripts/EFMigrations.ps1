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
dotnet ef migrations add $Name -c CommandDbContext
Set-Location $location