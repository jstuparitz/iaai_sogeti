@echo off
cd ../tools
java -jar plantuml.jar -o "..\..\Wiki\images" "..\diagrams\Sequence" 
echo Completed.