$myNewProjectName = 'SqlReportApi'
  
$myExtensions = @("*.cs", "*.csproj", "*.sln", "*.ps1 -exclude clone-project.ps1", "*.sh", "*.json", "*.xml", "*.ncrunchsolution", "*.user", "*.toml")

foreach ($extension in $myExtensions) {
  # Replace the names in the files and the files
  $configFiles = iex "Get-ChildItem -Path .\* $extension -rec"
  foreach ($file in $configFiles) {
      ((Get-Content $file.PSPath) -replace "WebApiStarter", "$myNewProjectName" ) |
      out-file $file.PSPath
      rename-item -path $file.FullName -NewName ($file.name -replace 'WebApiStarter', "$myNewProjectName")
  }
}

gci . -rec -Directory -Filter *WebApiStarter* | foreach-object { rename-item -path $_.FullName -NewName ($_.name -replace 'WebApiStarter', "$myNewProjectName") }
$myNewProjectNameLower = $myNewProjectName.tolower()
((Get-Content .\deploy\docker\docker-compose.yml) -replace "REPLACE_ME", "$myNewProjectNameLower") | out-file -encoding ascii .\deploy\docker\docker-compose.yml


#gci . -rec -Directory -Filter *Company* | foreach-object { rename-item -path $_.FullName -NewName ($_.name -replace 'Cortside', "Company") }
