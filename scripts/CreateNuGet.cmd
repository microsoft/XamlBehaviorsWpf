@ECHO OFF

WHERE /Q nuget >NUL
IF %ERRORLEVEL% NEQ 0 ( 
    ECHO nuget not found.
    ECHO.
    ECHO Run "%~pd0download-nuget.cmd" to download the latest version, or update PATH as appropriate.
    GOTO END
)

IF EXIST ..\out\BehaviorsSDK\bin\AnyCPU\Release\Microsoft.Xaml.Interactions.dll (
	IF "%1"=="" (
		GOTO PackWithFileVersion
	)
	:PackWithCommandLineVersion
	SET VERSION=%1
	GOTO PACK
	
	:PackWithFileVersion
        SET /p VERSION=<..\src\Version\NuGetPackageVersion.txt
	GOTO PACK
)

ECHO The Microsoft.Xaml.Interactions project has not been built in the "Any CPU - Release" configuration. Please build the project and then try again.
PAUSE
GOTO END

:PACK
SET NUGET_ARGS=^
    -nopackageanalysis ^
    -version %VERSION% ^
    -Verbosity detailed ^
    -Symbols

nuget pack Microsoft.Xaml.Behaviors.WPF.nuspec %NUGET_ARGS%

:END

EXIT /B