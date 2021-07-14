





cd $1
dir
dotnet pack "$1.csproj" -c:Release
read stop