#######################################################
## Back end test projects runner script
## By: Kevin Wang
## Description: 
## Todos: 
##  - Modularize this script
#######################################################

######################################################
## Script Parameters
#####################################################
param (
    [Alias('rd')]
    [string]$rootDir = "C:\Users\cxu\.jenkins\workspace\Back-End-Test-Demo-2\",
    [string]$defaultReportsLocation = "C:\Users\cxu\.jenkins\workspace\Back-End-Test-Demo-2\TestReport\",
    [string]$reportFilePathPattern = "{0}\xunit_report_{1}.xml",
    [string]$xunitTestRunnerPath = "C:\Users\cxu\.jenkins\workspace\Back-End-Test-Demo-2\packages\xunit.runner.console.2.3.1\tools\net452\" 
)

#######################################################
## 1. Scan the folder structure to extract a list of
##    xunit projects' paths
#######################################################


#######################################################
## 1.1 Helper functions
#######################################################

$dotnetProjectExtensionPattern = ".csproj$"

# Check whether a particular folder on a given path (absolute path) contains a .net project (.csproj) 
function checkFolderContainsDotnetProjectFile () {
    param (
        [string]$folderFullPath
    )

    $projectFilesCount = (Get-ChildItem $folderFullPath -File | Where-Object { $_.Name -match $dotnetProjectExtensionPattern }).count

    [bool]$result = $false
    if ($projectFilesCount -gt 0) {
        $result = $true
    } 

    return $result
}

# Get the full path of the .net project (.csproj) file on given path (absolute path):
function getDonetProjectFileFullPath () {
    param (
        [string]$folderFullPath
    )

    $projectFileFullPath = (Get-ChildItem $folderFullPath -File | Where-Object { $_.Name -match $dotnetProjectExtensionPattern })[0].FullName
    return $projectFileFullPath
}

# Check if a .net project file contains "xunit.core"
# Note: we assume that the path passed here exist due to a previous verification :
function checkDotnetProjectFileContainsXunit () {
    param (
        [string]$projectFileFullPath
    )

    [bool]$result = $false
    [xml]$xmlFileContent = Get-Content $projectFileFullPath

    $xmlFileContent.Project | %{ $_.Import.Project } | ForEach-Object {
        $importProject = $_
        if ($importProject.contains("xunit.core")) {
            $result = $true           
        }
    }
    return $result
}

#######################################################
## 1.2 Action functions
#######################################################

# Loop through files in the source url folder to find xunit projects and return an array of the full path of those test folders:
function searchXunitTestProjects () {  
    $testProjectsArray = @()   
    Get-ChildItem $rootDir -Directory -Recurse | ForEach-Object {
        $folderFullPath = $_.FullName
        
        if (checkFolderContainsDotnetProjectFile -folderFullPath $folderFullPath) {
            $projectFileFullPath = getDonetProjectFileFullPath -folderFullPath $folderFullPath
            if (checkDotnetProjectFileContainsXunit -projectFileFullPath $projectFileFullPath) {               
                $testProjectsArray += $folderFullPath
            }
        }
    }
    $numOfTestProjects = $testProjectsArray.Length
    "numOfTestProjects=$numOfTestProjects" | Out-File "$rootDir\xunitProjectsNum.properties" -Encoding ascii
    Write-Host "----------------------------------------------------------test projects found:"
    Write-Host $testProjectsArray
    return $testProjectsArray
}


#######################################################
## 2. Run the xunit test
#######################################################

$assemblyLocationPathWithinProject = "\bin\Release\"
$assemblyExtention = ".dll"

#######################################################
## 2.1 Helper functions
#######################################################

# For each test project, locate the generated assembly (.dll) file:
function getTestAssemblyFullPath () {
    param (
        [string]$testProjectFolderFullPath
    )
    $testProjectFolderName = ($testProjectFolderFullPath.Split('\'))[-1]
    $testAssemblyFileFullPath = $testProjectFolderFullPath + $assemblyLocationPathWithinProject + $testProjectFolderName + $assemblyExtention
    return $testAssemblyFileFullPath
}

# Loop through test projects to find corresponding test .dll file and return an array of the full path of those .dll files:
function searchXunitTestProjectsAssemblies () {
    $assembliesArray = @()
    $testProjectsArray = searchXunitTestProjects
    foreach ($testProjectFullPath in $testProjectsArray) {
        $assemblieFullPath = getTestAssemblyFullPath -testProjectFolderFullPath $testProjectFullPath
        $assembliesArray += $assemblieFullPath
    }
    return $assembliesArray
}

#######################################################
## 2.2 Action functions
#######################################################

# Loop through asseblies (.dlls) to run xunit test and generate test report:
function runXunitTest () {  
    $assembliesArray = searchXunitTestProjectsAssemblies 
    if (Test-Path $defaultReportsLocation) {
        rm $defaultReportsLocation -Recurse -Force
        cd $rootDir
        mkdir xunit_test_result -Force
    }
    foreach ($assembly in $assembliesArray) {
        $assemblyIndex = [array]::IndexOf($assembliesArray, $assembly)
        $testReportFullPath = $reportFilePathPattern -f $defaultReportsLocation, $assemblyIndex
        cd $xunitTestRunnerPath 
        .\xunit.console $assembly -xml $testReportFullPath
    }    
}


## execute the test
runXunitTest