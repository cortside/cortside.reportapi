[cmdletbinding()]
param(
)

$repo = "Cortside.SqlReportApi"
$project = "src/$repo.Data"
$startup = "src/$repo.WebApi"
$context = "DatabaseContext"

echo "removing last migration from $context context in project $project"

dotnet ef migrations remove --project "$project" --startup-project "$startup" --context "$context"

echo "done"
