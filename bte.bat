@ECHO OFF
References\Neovolve\BuildTaskExecutor\BuildTaskExecutor.exe %*

If errorlevel 1 GOTO BuildTaskFailed
GOTO BuildTaskSuccessful

:BuildTaskFailed
exit 1
:BuildTaskSuccessful