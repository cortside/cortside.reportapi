[cmdletBinding()]
Param()

Push-Location "$PSScriptRoot/src/Acme.WebApiStarter.WebApi"

cmd /c start cmd /k "title Acme.WebApiStarter.WebApi & dotnet run"

Pop-Location
