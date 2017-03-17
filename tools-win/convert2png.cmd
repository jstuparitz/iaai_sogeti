@echo off
echo reading %1\*.dot
echo converting to %2\*.png
cd %1
for %%f in (*.dot) do (
  dot.exe -o"%2/%%~nf.png" -Tpng "%%~nf.dot"
    echo   - %%~nf
)
