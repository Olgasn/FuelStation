
cd C:\Data\Projects\FuelStation\FuelStation

dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet tool install -g dotnet-aspnet-codegenerator

dotnet aspnet-codegenerator controller -name TanksController -m Tank -dc FuelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -f

dotnet aspnet-codegenerator controller -name Operations1Controller -m Operation -dc FuelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -f
