#region 函数定义
# 移动模块到临时目录
function TempModule([string]$solutionPath, [string]$moduleName) {
    Write-Host "move module:"$moduleName
    # temp save
    $tmp = Join-Path $solutionPath "./.tmp"
    if (!(Test-Path $tmp)) {
        New-Item -Path $tmp -ItemType Directory -Force | Out-Null
    }
    $destDir = Join-Path $tmp $moduleName
    # move entity to tmp   
    $entityPath = Join-Path $solutionPath "./src/Entity" $moduleName"Entities"
    $entityDestDir = Join-Path $destDir "Entities"

    if (!(Test-Path $entityDestDir)) {
        New-Item -Path $entityDestDir -ItemType Directory -Force | Out-Null
    }
    # get entity names by cs file name without extension
    $entityNames = Get-ChildItem -Path $entityPath -Filter "*.cs" | ForEach-Object { $_.BaseName }

    Move-Item -Path $entityPath\* -Destination $entityDestDir -Force
    # move store to tmp
    $applicationPath = Join-Path $solutionPath "./src/Application"
    $applicationDestDir = Join-Path $destDir "Application"

    if (!(Test-Path $applicationDestDir)) {
        New-Item -Path $applicationDestDir -ItemType Directory -Force | Out-Null
    }

    foreach ($entityName in $entityNames) {
        $queryStorePath = Join-Path $applicationPath "QueryStore" $entityName"QueryStore.cs"
        $commandStorePath = Join-Path $applicationPath "CommandStore" $entityName"CommandStore.cs"

        if ((Test-Path $queryStorePath)) {
            Move-Item -Path $queryStorePath -Destination $applicationDestDir -Force
        }
        if ((Test-Path $commandStorePath)) {
            Move-Item -Path $commandStorePath -Destination $applicationDestDir -Force
        }
    }

    Write-Host "Update DataStoreContext"
    # DatastoreContext.cs
    $contextPath = Join-Path $applicationPath "DataStoreContext.cs"
    # 保存原文件
    if (!(Test-Path (Join-Path $applicationDestDir "DataStoreContext.cs"))) {
        Copy-Item -Path $contextPath -Destination $applicationDestDir
    }
    $contextContent = Get-Content $contextPath


    foreach ($entityName in $entityNames) {
        $name = $entityName.Substring(0, 1).ToLower() + $entityName.Substring(1)
        $contextContent = $contextContent | Where-Object { $_ -notmatch $name + "Query" -and $_ -notmatch $name + "Command" } 
    }
    Set-Content $contextPath $contextContent -Force
}

# 复原模块内容
function RestoreModule ([string]$solutionPath, [string]$moduleName) {
    Write-Host "restore module:"$moduleName
    $tmp = Join-Path $solutionPath "./.tmp"
    if ((Test-Path $tmp)) {
        # restore module  entity and application from tmp
        $destDir = Join-Path $tmp $moduleName
        $entityDestDir = Join-Path $destDir "Entities"
        $applicationDestDir = Join-Path $destDir "Application"

        $entityPath = Join-Path $solutionPath "./src/Entity" $moduleName"Entities"
        $applicationPath = Join-Path $solutionPath "./src/Application"
        $entityNames = Get-ChildItem -Path $entityDestDir -Filter "*.cs" | ForEach-Object { $_.BaseName }

        Move-Item -Path $entityDestDir\* -Destination $entityPath -Force

        foreach ($entityName in $entityNames) {
            $queryStorePath = Join-Path $applicationPath "QueryStore" $entityName"QueryStore.cs"
            $queryStoreDestPath = Join-Path $applicationDestDir $entityName"QueryStore.cs"

            if ((Test-Path $queryStoreDestPath)) {
                Move-Item -Path $queryStoreDestPath -Destination $queryStorePath -Force
            }

            $commandStorePath = Join-Path $applicationPath "CommandStore" $entityName"CommandStore.cs"
            $commandStoreDestPath = Join-Path $applicationDestDir $entityName"CommandStore.cs"
            if (Test-Path $commandStoreDestPath) {
                Move-Item -Path $commandStoreDestPath -Destination $commandStorePath -Force
            }
        }

        # DataStoreContext.cs
        $contextPath = Join-Path $applicationPath "DataStoreContext.cs"
        $contextDestPath = Join-Path $applicationDestDir "DataStoreContext.cs"

        if (Test-Path $contextDestPath) {
            Move-Item -Path $contextDestPath -Destination $contextPath -Force
        }
    }
}

#endregion
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

$solutionPath = Join-Path $location "./templates/apistd"
$tmp = Join-Path $solutionPath "./.tmp"

try {
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

    Write-Host "find modules:"$modulesNames;

    foreach ($moduleName in $modulesNames) {
        TempModule $solutionPath $moduleName
    }

    # pack
    dotnet pack -c release -o ./nuget

    foreach ($moduleName in $modulesNames) {
        RestoreModule $solutionPath $moduleName
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
    Write-Error $_.Exception.Message
    Remove-Item $tmp -Force -Recurse
}


    
