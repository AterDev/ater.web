try {
    if (Test-Path ./nuget) {
        Remove-Item ./nuget -Force -Recurse
    }
    if (Test-Path ./templates/apistd/src/Http.API/Migrations) {
        Remove-Item ./templates/apistd/src/Http.API/Migrations -Force -Recurse
    }
    
    dotnet pack -c release -o ./nuget
    # get package info
    $VersionNode = Select-Xml -Path ./Pack.csproj -XPath '/Project//PropertyGroup/PackageVersion'
    $PackageNode = Select-Xml -Path ./Pack.csproj -XPath '/Project//PropertyGroup/PackageId'
    $Version = $VersionNode.Node.InnerText
    $PackageId = $PackageNode.Node.InnerText

    #re install package
    dotnet new uninstall $PackageId
    dotnet new install .\nuget\$PackageId.$Version.nupkg
}
catch {
    Write-Host $_.Exception.Message
}

