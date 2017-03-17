@echo off
set gwpath="C:\Program Files (x86)\Graphviz2.38\bin"
set src-path="..\diagrams\GraphViz"
set img-path="..\..\Wiki\images"
%gwpath%\dot %src-path%\context.dot -Tpng -o %img-path%\context.png
