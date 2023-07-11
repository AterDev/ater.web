try {
    $OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8

    Write-Host "Clean files"
    # delete files
    if (Test-Path ./nuget) {
        Remove-Item ./nuget -Force -Recurse
    }
    if (Test-Path ./templates/apistd/src/Http.API/Migrations) {
        Remove-Item ./templates/apistd/src/Http.API/Migrations -Force -Recurse
    }

    $location = Get-Location
    # 处理模块实体
    $entityPath = Join-Path $location "./templates/apistd/src/Entity"

    # get all directiories in entityPath 
    $entityDirs = Get-ChildItem -Path $entityPath -Directory
    $tmp = Join-Path $entityPath "./.tmp"

    if (!(Test-Path $tmp)) {
        New-Item -Path $tmp -ItemType Directory -Force | Out-Null
    }

    # traverse entityDirs get which name is not SystemEntities and move to tmp directory
    foreach ($dir in $entityDirs) {
        if ($dir.Name -ne "SystemEntities") {
            $dest = Join-Path $tmp $dir.Name
            Move-Item -Path $dir.FullName -Destination $dest -Force
        }
    }

    # pack
    dotnet pack -c release -o ./nuget

    # move back
    $entityDirs = Get-ChildItem -Path $tmp -Directory
    foreach ($dir in $entityDirs) {
        $dest = Join-Path $entityPath $dir.Name
        Move-Item -Path $dir.FullName -Destination $dest -Force
    }

    # delete tmp directory
    Remove-Item $tmp -Force -Recurse

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

