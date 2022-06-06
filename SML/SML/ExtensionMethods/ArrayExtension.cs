namespace SML.ExtensionMethods
{
    public static class ArrayExtension
    {
        public static IEnumerable<T> GetRow<T>(this T[,] array, int row)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                yield return array[i, row];
            }
        }

        public static IEnumerable<T> GetColumn<T>(this T[,] array, int column)
        {
            for (var i = 0; i < array.GetLength(1); i++)
            {
                yield return array[i, column];
            }
        }

        public static T[,] To2D<T>(this T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                {
                    for (int j = 0; j < SecondDim; ++j)
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
