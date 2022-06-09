using System.Text;
using SML.Matrices.Exceptions;

namespace SML.Matrices;

public class Matrix : ICloneable
{
    #region Fields

    public int Rows { get; }

    public int Columns { get; }

    public double[,] Array { get; }

    #region Consts

    private const char StringDelimiter = ' ';

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
        return this + matrix;
    }

    public Matrix Subtract(Matrix matrix)
    {
        return this - matrix;
    }

    // Dot product
    public Matrix Multiply(Matrix matrix)
    {
        return this * matrix;
    }

    // Hadamard product (element-wise multiplication)
    public Matrix Hadamard(Matrix matrix)
    {
        CheckForHadamardOperation(matrix);

        Matrix result = new(Rows, matrix.Columns);

        for (var i = 0; i < result.Rows; i++)
        {
            for (var j = 0; j < result.Columns; j++)
            {
                result[i, j] = Array[i, j] * matrix[i, j];
            }
        }

        return result;
    }

    private void CheckForHadamardOperation(Matrix matrix)
    {
        CheckNullMatrix(matrix);
        CheckSquareMatrix(matrix);
    }

    public Matrix Transpose()
    {
        Matrix matrix = new(Columns, Rows);

        for (var i = 0; i < Columns; i++)
        {
            for (var j = 0; j < Rows; j++)
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
            for (var i = 0; i < matrix.Rows; i++)
            {
                for (var j = 0; j < matrix.Columns; j++)
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

        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                matrix.Append(Array[i, j]);
                matrix.Append(StringDelimiter);
            }
            matrix.Append(Environment.NewLine);
        }

        return matrix.ToString();
    }

    #endregion Methods

    #region Operators

    public static Matrix operator +(Matrix firstMatrix, Matrix secondMatrix)
    {
        CheckForAddOrSubOperations(firstMatrix, secondMatrix);

        Matrix result = new(firstMatrix.Rows, firstMatrix.Columns);

        for (var i = 0; i < result.Rows; i++)
        {
            for (var j = 0; j < result.Columns; j++)
            {
                result[i, j] = firstMatrix[i, j] + secondMatrix[i, j];
            }
        }

        return result;
    }

    public static Matrix operator -(Matrix firstMatrix, Matrix secondMatrix)
    {
        CheckForAddOrSubOperations(firstMatrix, secondMatrix);

        Matrix result = new(firstMatrix.Rows, firstMatrix.Columns);

        for (var i = 0; i < result.Rows; i++)
        {
            for (var j = 0; j < result.Columns; j++)
            {
                result[i, j] = firstMatrix[i, j] - secondMatrix[i, j];
            }
        }

        return result;
    }

    private static void CheckForAddOrSubOperations(Matrix firstMatrix,
        Matrix secondMatrix)
    {
        CheckNullMatrix(firstMatrix);
        CheckNullMatrix(secondMatrix);
        CheckSameDimensionsMatrix(firstMatrix, secondMatrix);
    }

    public static Matrix operator *(Matrix firstMatrix, Matrix secondMatrix)
    {
        CheckForMultiplyOperation(firstMatrix, secondMatrix);

        Matrix result = new(firstMatrix.Rows, secondMatrix.Columns);

        for (var i = 0; i < result.Rows; i++)
        {
            for (var j = 0; j < result.Columns; j++)
            {
                for (var k = 0; k < secondMatrix.Rows; k++)
                {
                    result[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }
        }

        return result;
    }

    private static void CheckForMultiplyOperation(Matrix firstMatrix,
        Matrix secondMatrix)
    {
        CheckNullMatrix(firstMatrix);
        CheckNullMatrix(secondMatrix);
        CheckMultiplyAvailability(firstMatrix, secondMatrix);
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
                (MatrixExceptionDescription.
                RowsLessThanZeroExceptionMessage);
        }
    }

    private static void CheckColumnsNum(int columns)
    {
        if (columns < 0)
        {
            throw new ArgumentOutOfRangeException
                (MatrixExceptionDescription.
                ColumnsLessThanZeroExceptionMessage);
        }
    }

    private void CheckSquareMatrix(Matrix matrix)
    {
        if (Rows != matrix.Rows ||
            Columns != matrix.Columns)
        {
            throw new MatrixException(MatrixExceptionDescription.
                MatricesDifferentDimensionsExceptionMessage);
        }
    }

    private static void CheckSameDimensionsMatrix(Matrix firstMatrix,
        Matrix secondMatrix)
    {
        if (firstMatrix.Rows != secondMatrix.Rows ||
            firstMatrix.Columns != secondMatrix.Columns)
        {
            throw new MatrixException(MatrixExceptionDescription.
                MatricesDifferentDimensionsExceptionMessage);
        }
    }

    private static void CheckNullMatrix(Matrix matrix)
    {
        if (matrix == null)
        {
            throw new ArgumentNullException(nameof(matrix),
                MatrixExceptionDescription.
                    NullMatrixExceptionMessage);
        }
    }

    private static void CheckMultiplyAvailability(Matrix firstMatrix,
        Matrix secondMatrix)
    {
        if (firstMatrix.Columns != secondMatrix.Rows)
        {
            throw new MatrixException(MatrixExceptionDescription.
                MatricesDifferentDimensionsExceptionMessage);
        }
    }

    #endregion Asserts
}
