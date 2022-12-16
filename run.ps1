[cmdletBinding()]
Param()

Push-Location "$PSScriptRoot/src/Cortside.SqlReportApi.WebApi"

cmd /c start cmd /k "title Cortside.SqlReportApi.WebApi & dotnet run"

Pop-Location
