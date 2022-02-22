using SML.Matrices.Exceptions;
using System.Text;

namespace SML.Matrices;

public class Matrix : ICloneable
{
    #region Fields

    public int Rows { get; }

    public int Columns { get; }

    public double[,] Array { get; }

    #endregion Fields

    #region Constructors

    public Matrix(int rows, int columns)
    {
        if (rows < 0 || columns < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rows),
                "Rows should be more than 0");
        }
        else if (columns < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(columns),
                "Columns should be more than 0");
        }

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
            if (row < 0 || column < 0)
            {
                throw new ArgumentException("indexes can't be less than 0");
            }

            return Array[row, column];
        }
        set
        {
            if (row < 0 || column < 0)
            {
                throw new ArgumentException("indexes can't be less than 0");
            }

            Array[row, column] = value;
        }
    }

    public object Clone() => new Matrix(Array);

    public Matrix Add(Matrix matrix)
    {
        if (matrix == null)
        {
            throw new ArgumentNullException(nameof(matrix),
                "matrix shouldn't be null");
        }

        if (Rows != matrix.Rows ||
            Columns != matrix.Columns)
        {
            throw new MatrixException("Matrixes should have same dimensions");
        }

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
        if (matrix == null)
        {
            throw new ArgumentNullException(nameof(matrix),
                "matrix shouldn't be null");
        }

        if (Rows != matrix.Rows ||
            Columns != matrix.Columns)
        {
            throw new MatrixException("Matrixes should have same dimensions");
        }

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
        if (matrix == null)
        {
            throw new ArgumentNullException(nameof(matrix),
                "matrix shouldn't be null");
        }

        if (Columns != matrix.Rows)
        {
            throw new MatrixException("Matrixes should have same dimensions");
        }

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
        if (matrix == null)
        {
            throw new ArgumentNullException(nameof(matrix),
                "matrix shouldn't be null");
        }

        if (Columns != matrix.Columns || Rows != matrix.Rows)
        {
            throw new MatrixException("Matrixes should have same dimensions");
        }

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
        if (matrix1 == null)
        {
            throw new ArgumentNullException(nameof(matrix1),
                "matrix1 shouldn't be null");
        }
        else if (matrix2 == null)
        {
            throw new ArgumentNullException(nameof(matrix2),
                "matrix2 shouldn't be null");
        }

        if (matrix1.Rows != matrix2.Rows ||
            matrix1.Columns != matrix2.Columns)
        {
            throw new MatrixException("Matrixes should have same dimensions");
        }

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
        if (matrix1 == null)
        {
            throw new ArgumentNullException(nameof(matrix1),
                "matrix1 shouldn't be null");
        }
        else if (matrix2 == null)
        {
            throw new ArgumentNullException(nameof(matrix2),
                "matrix2 shouldn't be null");
        }

        if (matrix1.Rows != matrix2.Rows ||
            matrix1.Columns != matrix2.Columns)
        {
            throw new MatrixException("Matrixes should have same dimensions");
        }

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
        if (matrix1 == null)
        {
            throw new ArgumentNullException(nameof(matrix1),
                "matrix1 shouldn't be null");
        }
        else if (matrix2 == null)
        {
            throw new ArgumentNullException(nameof(matrix2),
                "matrix2 shouldn't be null");
        }

        if (matrix1.Columns != matrix2.Rows)
        {
            throw new MatrixException("Matrixes should have same dimensions");
        }

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
}
