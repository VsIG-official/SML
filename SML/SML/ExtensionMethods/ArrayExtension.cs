namespace SML.ExtensionMethods;

public static class ArrayExtension
{
    #region Fields

    private static string To2DExceptionMessage =
        "The given jagged array is not rectangular";

    #endregion Fields

    #region Methods

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

    public static T[,] To2D<T>(this T[][] array)
    {
        try
        {
            int rows = array.Length;
            int columns = array.GroupBy(row => row.Length).Single().Key;

            var result = new T[rows, columns];
            
            for (var i = 0; i < rows; ++i)
            {
                for (var j = 0; j < columns; ++j)
                {
                    result[i, j] = array[i][j];
                }
            }

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new
                InvalidOperationException(To2DExceptionMessage);
        }
    }

    #endregion Methods
}
