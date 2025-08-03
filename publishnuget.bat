@echo off

rem Check if version number is provided as argument
if "%~1"=="" (
    echo "Please provide the version number as an argument."
    exit /b 1
)

rem Set the version number
set Version=%~1

rem Define the directory containing the solution file
set SolutionPath=D:\TrainingProject\MinPlatform\MinPlatform.sln

rem Define the output directory for NuGet packages
set NugetOutputDirectory=D:\Outputs\Nugets

rem List of projects to exclude from building
set ExcludedProjects=MinPlatform.Test.csproj MinPlatform.NetService.csproj

rem Build the solution in Release configuration with the specified version number
dotnet build "%SolutionPath%" -c Release /p:Version="%Version%"

rem Pack each project in the solution to create a NuGet package
for /r %%i in (*.csproj) do (
    set "IsExcluded="
    for %%j in (%ExcludedProjects%) do (
        if /i "%%~nxi"=="%%~nj" set IsExcluded=true
    )
    if not defined IsExcluded (
        dotnet pack "%%i" -c Release -o "%NugetOutputDirectory%" /p:Version="%Version%"
    )
)

echo "NuGet packages for version %Version% generated successfully."

rem Optional: Clean up the output directory by deleting any temporary files
del /q "%NugetOutputDirectory%\*.nupkg.sha512"
del /q "%NugetOutputDirectory%\*.symbols.nupkg"

echo "Temporary files cleaned up."
