cd $1
dotnet ef migrations add $2

dotnet ef database update
