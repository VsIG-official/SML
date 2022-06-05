namespace SML.ExtensionMethods
{
    public static class ArrayExtension
    {
        public static IEnumerable<T> GetRow<T>(this T[,] array, int row)
        {
            for (var i = 0; i < array.GetLength(1); i++)
            {
                yield return array[i, row];
            }
        }

        public static IEnumerable<T> GetColumn<T>(this T[,] array, int column)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                yield return array[i, column];
            }
        }
    }
}
