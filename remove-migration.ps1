$repo = "Cortside.LoanServicing"
$project = "src/$repo.Repository"
$startup = "src/$repo.WebApi"
$context = "LoanContext"

echo "removing last migration for $context context in project $project"

dotnet ef migrations remove  --project "$project" --startup-project "$startup" --context "$context"

echo "done"
