@ECHO OFF
CLS
:Start
ECHO 1.Status
ECHO 2.Add changes
ECHO 3.Commit changes
ECHO 4.Push
ECHO 5.Quit
ECHO.

CHOICE /C 12345 /M "Enter your choice:"

:: Note - list ERRORLEVELS in decreasing order
IF ERRORLEVEL 5 GOTO Quit
IF ERRORLEVEL 4 GOTO Push
IF ERRORLEVEL 3 GOTO Commit
IF ERRORLEVEL 2 GOTO Add
IF ERRORLEVEL 1 GOTO Status

:Status
ECHO Status
(git status)
GOTO Start

:Add
ECHO Adding Changes 
(git add -A)
GOTO Start

:Commit
ECHO Committing
set /p CommitMessage=What is your commit message?
(git commit -m"%CommitMessage%")
GOTO Start

:Push
ECHO Pushing 
(git push)
GOTO Start

:Quit
(exit)

