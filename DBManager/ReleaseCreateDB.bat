cd CreateDB

dotnet publish --runtime win-x64 --self-contained true -p:PublishSingleFile=true --configuration Release --output "..\..\Databases\CreateDBexe"

pause