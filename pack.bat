packages\NuGet.CommandLine.4.6.2\tools\nuget pack Jal.Router.AzureServiceBus\Jal.Router.AzureServiceBus.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Router.Nuget

packages\NuGet.CommandLine.4.6.2\tools\nuget pack Jal.Router.AzureServiceBus.Installer\Jal.Router.AzureServiceBus.Installer.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Router.Nuget

packages\NuGet.CommandLine.4.6.2\tools\nuget pack Jal.Router.AzureServiceBus.LightInject.Installer\Jal.Router.AzureServiceBus.LightInject.Installer.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Router.Nuget

pause;