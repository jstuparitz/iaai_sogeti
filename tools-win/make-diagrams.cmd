@echo off
set gwpath="C:\Program Files (x86)\Graphviz2.38\bin"
call convert2png.cmd "..\diagrams\GraphViz" "..\..\..\Wiki\images" %gwpath%
echo Completed.
pause