namespace SML.ExtensionMethods
{
    public static class ArrayExtension
    {
        public static IEnumerable<T> GetColumn<T>(this T[,] array, int column)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                yield return array[i, column];
            }
        }

        public static IEnumerable<T> GetRow<T>(this T[,] array, int row)
        {
            for (var i = 0; i < array.GetLength(1); i++)
            {
                yield return array[row, i];
            }
        }

        private static IEnumerable<T> IterateThroughArray<T>(T[,] array, int arrayLength, int index)
        {
            for (var i = 0; i < arrayLength; i++)
            {
                yield return array[i, index];
            }
        }

        public static T[,] To2D<T>(this T[][] source)
        {
            try
            {
                int firstDim = source.Length;
                int secondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[firstDim, secondDim];
                for (var i = 0; i < firstDim; ++i)
                {
                    for (var j = 0; j < secondDim; ++j)
                    {
                        result[i, j] = source[i][j];
                    }
                }

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }
    }
}
