try {
    dotnet pack 
    # get package info
    $VersionNode = Select-Xml -Path ./Pack.csproj -XPath '/Project//PropertyGroup/PackageVersion'
    $PackageNode = Select-Xml -Path ./Pack.csproj -XPath '/Project//PropertyGroup/PackageId'
    $Version = $VersionNode.Node.InnerText
    $PackageId = $PackageNode.Node.InnerText

    #re install package
    dotnet new --uninstall $PackageId
    dotnet new -i .\nuget\$PackageId.$Version.nupkg
}
catch {
    Write-Host $_.Exception.Message
}

