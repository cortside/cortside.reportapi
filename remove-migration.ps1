$repo = "Cortside.SqlReportApi"
$project = "src/$repo.Domain"
$startup = "src/$repo.WebApi"
$context = "DatabaseContext"

echo "removing last migration for $context context in project $project"

dotnet ef migrations remove  --project "$project" --startup-project "$startup" --context "$context"

echo "done"
