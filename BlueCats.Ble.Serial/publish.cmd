@IF NOT defined _echo @echo off
IF "%1"=="" GOTO PrintHelp
IF "%1"=="-h" GOTO PrintHelp
ECHO ************************************************************************
ECHO * NuGet Package Publish Script
ECHO ************************************************************************

SET pkg_path1=".\bin\Release\BlueCats.Ble.Serial.%1.nupkg"
SET pkg_path2=".\bin\Release\netstandard2.0\publish\BlueCats.Ble.Serial.%1.nupkg"

IF EXIST %pkg_path1% (
	CALL nuget push %pkg_path1% -Source https://api.nuget.org/v3/index.json -NonInteractive 
	IF NOT ERRORLEVEL 1 GOTO End
) ELSE ( 
	IF EXIST %pkg_path1% (
		CALL nuget push %pkg_path1% -Source https://api.nuget.org/v3/index.json -NonInteractive 
		IF NOT ERRORLEVEL 1 GOTO End
	) ELSE (
		ECHO.ERROR No package exists with version %1
		EXIT /b 1
	)
)
IF NOT ERRORLEVEL 1 GOTO End
ECHO.ERROR Failed to publish package!
EXIT /b 1

:PrintHelp
ECHO Usage: publish.cmd package_version

:End
ECHO ************************************************************************
ECHO * Success
ECHO ************************************************************************
