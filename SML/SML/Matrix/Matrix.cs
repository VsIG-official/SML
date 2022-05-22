using SML.Matrices.Exceptions;
using System.Text;

namespace SML.Matrices;

public class Matrix : ICloneable
{
    #region Fields

    public int Rows { get; }

    public int Columns { get; }

    public double[,] Array { get; }

    #region Consts

    private const string MatricesDifferentDimensionsExceptionMessage =
        "Matrices should have same dimensions";
    private const string NullMatrixExceptionMessage =
        "Matrix shouldn't be null";
    private const string RowsLessThanZeroExceptionMessage =
        "Rows should be more than 0";
    private const string ColumnsLessThanZeroExceptionMessage =
       "Columns should be more than 0";

    #endregion Consts

    #endregion Fields

    #region Constructors

    public Matrix(int rows, int columns)
    {
        CheckMatrixDimensions(rows, columns);

        Rows = rows;
        Columns = columns;
        Array = new double[rows, columns];
    }

    public Matrix(double[,] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        Array = array.Clone() as double[,];
        Rows = Array.GetLength(0);
        Columns = Array.GetLength(1);
    }

    #endregion Constructors

    #region Methods

    public double this[int row, int column]
    {
        get
        {
            CheckMatrixDimensions(row, column);

            return Array[row, column];
        }
        set
        {
            CheckMatrixDimensions(row, column);

            Array[row, column] = value;
        }
    }

    public object Clone() => new Matrix(Array);

    public Matrix Add(Matrix matrix)
    {
        CheckNullMatrix(matrix);
        CheckSquareMatrix(matrix);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Array[i, j] = Array[i, j] + matrix[i, j];
            }
        }

        return this;
    }

    public Matrix Subtract(Matrix matrix)
    {
        CheckNullMatrix(matrix);

        CheckSquareMatrix(matrix);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Array[i, j] = Array[i, j] - matrix[i, j];
            }
        }

        return this;
    }

    // Dot product
    public Matrix Multiply(Matrix matrix)
    {
        CheckNullMatrix(matrix);

        CheckMultiplyAvaibility(matrix);

        Matrix result = new(Rows, matrix.Columns);

        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                for (int k = 0; k < matrix.Rows; k++)
                {
                    result[i, j] += Array[i, k] * matrix[k, j];
                }
            }
        }

        return result;
    }

    // Hadamard product (element-wise multiplication)
    public Matrix Hadamard(Matrix matrix)
    {
        CheckNullMatrix(matrix);

        CheckSquareMatrix(matrix);

        Matrix result = new(Rows, matrix.Columns);

        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                result[i, j] = Array[i, j] * matrix[i, j];
            }
        }

        return result;
    }

    public Matrix Transpose()
    {
        Matrix matrix = new(Columns, Rows);

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                matrix[i, j] = Array[j, i];
            }
        }

        return matrix;
    }

    public override bool Equals(object obj)
    {
        bool areEqual = false;

        if (obj is Matrix matrix &&
            Rows == matrix.Rows &&
            Columns == matrix.Columns)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    if (Array[i, j] == matrix[i, j])
                    {
                        areEqual = true;
                    }
                }
            }
        }

        return areEqual;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        StringBuilder matrix = new();

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                matrix.Append(Array[i, j]);
                matrix.Append(' ');
            }
            matrix.Append(Environment.NewLine);
        }

        return matrix.ToString();
    }

    #endregion Methods

    #region Operators

    public static Matrix operator +(Matrix matrix1, Matrix matrix2)
    {
        CheckNullMatrix(matrix1);
        CheckNullMatrix(matrix2);
        CheckSquareMatrix(matrix1, matrix2);

        Matrix result = new(matrix1.Rows, matrix1.Columns);

        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                result[i, j] = matrix1[i, j] + matrix2[i, j];
            }
        }

        return result;
    }

    public static Matrix operator -(Matrix matrix1, Matrix matrix2)
    {
        CheckNullMatrix(matrix1);
        CheckNullMatrix(matrix2);
        CheckSquareMatrix(matrix1, matrix2);

        Matrix result = new(matrix1.Rows, matrix1.Columns);

        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                result[i, j] = matrix1[i, j] - matrix2[i, j];
            }
        }

        return result;
    }

    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
        CheckNullMatrix(matrix1);
        CheckNullMatrix(matrix2);
        CheckMultiplyAvailability(matrix1, matrix2);

        Matrix result = new(matrix1.Rows, matrix2.Columns);

        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                for (int k = 0; k < matrix2.Rows; k++)
                {
                    result[i, j] += matrix1[i, k] * matrix2[k, j];
                }
            }
        }

        return result;
    }

    #endregion Operators

    #region Asserts

    private static void CheckMatrixDimensions(int rows, int columns)
    {
        CheckRowsNum(rows);
        CheckColumnsNum(columns);
    }

    private static void CheckRowsNum(int rows)
    {
        if (rows < 0)
        {
            throw new ArgumentOutOfRangeException
                (RowsLessThanZeroExceptionMessage);
        }
    }

    private static void CheckColumnsNum(int columns)
    {
        if (columns < 0)
        {
            throw new ArgumentOutOfRangeException
                (ColumnsLessThanZeroExceptionMessage);
        }
    }

    private void CheckSquareMatrix(Matrix matrix)
    {
        if (Rows != matrix.Rows ||
            Columns != matrix.Columns)
        {
            throw new MatrixException(MatricesDifferentDimensionsExceptionMessage);
        }
    }

    private static void CheckSquareMatrix(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.Rows != matrix2.Rows ||
            matrix1.Columns != matrix2.Columns)
        {
            throw new MatrixException(MatricesDifferentDimensionsExceptionMessage);
        }
    }

    private static void CheckNullMatrix(Matrix matrix)
    {
        if (matrix == null)
        {
            throw new ArgumentNullException(nameof(matrix),
                NullMatrixExceptionMessage);
        }
    }

    private void CheckMultiplyAvaibility(Matrix matrix)
    {
        if (Columns != matrix.Rows)
        {
            throw new MatrixException(MatricesDifferentDimensionsExceptionMessage);
        }
    }

    private static void CheckMultiplyAvailability(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.Columns != matrix2.Rows)
        {
            throw new MatrixException(MatricesDifferentDimensionsExceptionMessage);
        }
    }

    #endregion Asserts
}
