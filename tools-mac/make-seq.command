BASEDIR=$(dirname "$0")
cd "$BASEDIR/../tools"
java -jar plantuml.jar -o "$BASEDIR/../../Wiki/images" "$BASEDIR/../diagrams/Sequence"
echo Completed.
exit 0