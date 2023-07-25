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

    $entityPath = Join-Path $location "./templates/apistd/src/Entity"
    # 获取模块
    $entityFiles = Get-ChildItem -Path $entityPath -Filter "*.cs" -Recurse |`
        Select-String -Pattern "\[Module" -List |`
        Select-Object -ExpandProperty Path

    # 模块名称
    $modulesNames = @()

    # 获取模块名称
    $regex = '\[Module\("(.+?)"\)\]';
    foreach ($file in $entityFiles) {
        $content = Get-Content $file
        $match = $content | Select-String -Pattern $regex -AllMatches | Select-Object -ExpandProperty Matches
        $moduleName = $match.Groups[1].Value

        # add modulename  to modulesNames if not exist 
        if ($modulesNames -notcontains $moduleName) {
            $modulesNames += $moduleName
        }
    }
    $solutionPath = Join-Path $location "./templates/apistd"

    foreach ($moduleName in $moduleNames) {
        TempModule($solutionPath, $moduleName)
    }

    # pack
    dotnet pack -c release -o ./nuget

    foreach ($moduleName in $moduleNames) {
        RestoreModule($solutionPath, $moduleName)
    }

    # delete tmp directory
    # Remove-Item $tmp -Force -Recurse

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

# 移动模块到临时目录
function TempModule($solutionPath, $moduleName) {
    # temp save
    $tmp = Join-Path $solutionPath "./.tmp"
    if (!(Test-Path $tmp)) {
        New-Item -Path $tmp -ItemType Directory -Force | Out-Null
    }
    $destDir = Join-Path $tmp $moduleName
    # move entity to tmp   
    $entityPath = Join-Path $solutionPath "./src/Entity" $moduleName+"Entities"
    $entityDestDir = Join-Path $destDir "Entities"
    Move-Item -Path $entityPath -Destination $entityDestDir -Force

    # get entity names by cs file name without extension
    $entityNames = Get-ChildItem -Path $entityPath -Filter "*.cs" | ForEach-Object { $_.BaseName }

    # move store to tmp
    $applicationPath = Join-Path $solutionPath "./src/Application"
    $applicationDestDir = Join-Path $destDir "Application"

    foreach ($entityName in $entityNames) {
        $storePaths = @(Join-Path $applicationPath "CommandStore" $entityName+"CommandStore.cs",
            Join-Path $applicationPath "QueryStore", $entityName+"QueryStore.cs")
        foreach ($storePath in $storePaths) {
            Move-Item -Path $storePath -Destination $applicationDestDir -Force
        }   
    }

    # DatastoreContext.cs
    $contextPath = Join-Path $applicationPath "DatastoreContext.cs"
    # 保存原文件
    if (!Test-Path (Join-Path $applicationDestDir "DatastoreContext.cs")) {
        Move-Item -Path $contextPath -Destination $applicationDestDir
    }

    $contextContent = Get-Content $contextPath
    foreach ($entityName in $entityNames) {
        $name = $string.Substring(0, 1).ToLower() + $string.Substring(1)
        $contextContent = $contextContent | Where-Object { $_ -notmatch $name + "Query" -and $_ -notmatch $name + "Command" } 
    }
    Set-Content $contextPath $contextContent -Force
}

# 复原模块内容
function RestoreModule ($solutionPath, $moduleName) {
    $tmp = Join-Path $solutionPath "./.tmp"
    if (!(Test-Path $tmp)) {

        # restore module  entity and application from tmp
        $destDir = Join-Path $tmp $moduleName
        $entityDestDir = Join-Path $destDir "Entities"
        $applicationDestDir = Join-Path $destDir "Application"

        $entityPath = Join-Path $solutionPath "./src/Entity" $moduleName+"Entities"
        $applicationPath = Join-Path $solutionPath "./src/Application"

        Move-Item -Path $entityDestDir -Destination $entityPath -Force

        $entityNames = Get-ChildItem -Path $entityPath -Filter "*.cs" | ForEach-Object { $_.BaseName }

        foreach ($entityName in $entityNames) {
            $queryStorePath = Join-Path $applicationPath "QueryStore" $entityName+"QueryStore.cs"
            $queryStoreDestPath = Join-Path $applicationDestDir $entityName+"QueryStore.cs"
            Move-Item -Path $queryStoreDestPath -Destination $queryStorePath -Force

            $commandStorePath = Join-Path $applicationPath "CommandStore" $entityName+"CommandStore.cs"
            $commandStoreDestPath = Join-Path $applicationDestDir $entityName+"CommandStore.cs"
            Move-Item -Path $commandStoreDestPath -Destination $commandStorePath -Force
        }

        # DataStoreContext.cs
        $contextPath = Join-Path $applicationPath "DatastoreContext.cs"
        $contextDestPath = Join-Path $applicationDestDir "DatastoreContext.cs"
        Move-Item -Path $contextDestPath -Destination $contextPath -Force
    }
}
    
