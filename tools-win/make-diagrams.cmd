@echo off
set gwpath="C:\Program Files (x86)\Graphviz2.38\bin"
cd %gwpath%
call convert2png.cmd "..\diagrams\GraphViz" "..\..\Wiki\images"
echo Completed.