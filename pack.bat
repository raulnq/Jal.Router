packages\NuGet.CommandLine.2.8.6\tools\nuget pack Jal.Router\Jal.Router.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Router.Nuget

packages\NuGet.CommandLine.2.8.6\tools\nuget pack Jal.Router.Installer\Jal.Router.Installer.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Router.Nuget

pause;