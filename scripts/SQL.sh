echo "Please, enter sql file location:"
read destination
dotnet ef migrations script -i -o "${destination}" -c DatabaseContext