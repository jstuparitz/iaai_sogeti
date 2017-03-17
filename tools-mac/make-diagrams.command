BASEDIR=$(dirname "$0")
echo -------------------------------
cd "$BASEDIR"
sh convert2png.sh "$BASEDIR/../diagrams/GraphViz" "$BASEDIR/../../Wiki/images"
echo Completed.
exit 0