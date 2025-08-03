@echo off

rem Check if version number is provided as argument
if "%~1"=="" (
    echo "Please provide the version number as an argument."
    exit /b 1
)

rem Set the version number
set Version=%~1

rem Define the directory containing the project file
set ProjectPath=D:\TrainingProject\MinPlatform\MinPlatform.sln

rem Define the output directory
set OutputDirectory=D:\Outputs

rem Publish the project in Release configuration with the specified version
dotnet publish "%ProjectPath%" -c Release -o "%OutputDirectory%" /p:Version="%Version%"

echo "Release version %Version% of the class library published successfully."

rem Define the specific files to delete from the output directory
set DLLFiles=MinPlatform.Test.exe MinPlatform.NetService.dll MinPlatform.NetService.exe

rem Delete specific DLL files from the output directory
for %%F in (%DLLFiles%) do (
    del /q "%OutputDirectory%\%%F"
)

echo "Specific DLL files removed successfully from the output directory."
