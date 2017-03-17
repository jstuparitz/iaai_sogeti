echo reading $1/*.dot
echo converting to $2/*.png
cd $1
for tname in *.dot
do
  fname=${tname%\.*}
  echo "   > $fname"
  dot -o"$2/$fname.png" -Tpng "$fname.dot"
done

exit 0