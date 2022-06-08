using System;
using System.Collections.Generic;
using SML.Matrices;
using SML.Matrices.Exceptions;
using Xunit;

namespace SML.Tests.MatrixTests;

public class MatrixTests
{
    #region PremadeData

    public static IEnumerable<object[]> ZeroAndMoreConstructorValuesData =>
    new List<object[]>
    {
        new object[] { 0, 0 },
        new object[] { 1, 0 },
        new object[] { 0, 1 },
        new object[] { 1, 1 },
    };

    public static IEnumerable<object[]> LessThenZeroConstructorValuesData =>
    new List<object[]>
    {
        new object[] { -1, 1 },
        new object[] { 1, -1 },
        new object[] { -1, -1 }
    };

    public static IEnumerable<object[]> SquareMatrixData =>
        new List<object[]>
    {
        new object[]
        {
            new double[1, 1]
            {
                { 1 }
            }
        },
        new object[]
        {
            new double[2, 2]
            {
                { 2, 2 },
                { 2, 2 }
            }
        },
        new object[]
        {
            new double[5, 5]
            {
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 }
            }
        },
    };

    public static IEnumerable<object[]> NotSquareMatrixData =>
        new List<object[]>
    {
        new object[]
        {
            new double[1, 2]
            {
                { 1, 2 }
            }
        },
        new object[]
        {
            new double[2, 3]
            {
                { 2, 3, 4 },
                { 2, 3, 4 }
            }
        },
        new object[]
        {
            new double[5, 1]
            {
                { 1 },
                { 2 },
                { 3 },
                { 4 },
                { 5 }
            }
        },
    };

    public static IEnumerable<object[]> DifferentDimensionsMatrixData =>
    new List<object[]>
{
        new object[]
        {
            new double[1, 2]
            {
                { 1, 2 }
            },
            new double[2, 1]
            {
                { 1 },
                { 2 }
            }
        },
        new object[]
        {
            new double[2, 3]
            {
                { 2, 3, 4 },
                { 2, 3, 4 }
            },
            new double[3, 2]
            {
                { 2, 3 },
                { 2, 3 },
                { 2, 3 }
            }
        },
        new object[]
        {
            new double[5, 1]
            {
                { 1 },
                { 2 },
                { 3 },
                { 4 },
                { 5 }
            },
            new double[1, 5]
            {
                { 1, 2, 3, 4, 5 }
            }
        },
};

    public static IEnumerable<object[]> DifferentDimensionsMultiplyMatrixData =>
    new List<object[]>
    {
        new object[]
        {
            new double[1, 2]
            {
                { 1, 2 }
            },
            new double[2, 1]
            {
                { 1 },
                { 2 }
            },
            new double[1, 1]
            {
                { 5 }
            }
        },
        new object[]
        {
            new double[2, 1]
            {
                { 1 },
                { 2 }
            },
            new double[1, 2]
            {
                { 1, 2 }
            },
            new double[2, 2]
            {
                { 1, 2 },
                { 2, 4 }
            }
        },
        new object[]
        {
            new double[2, 3]
            {
                { 2, 3, 4 },
                { 2, 3, 4 }
            },
            new double[3, 2]
            {
                { 2, 3 },
                { 2, 3 },
                { 2, 3 }
            },
            new double[2, 2]
            {
                { 18, 27 },
                { 18, 27 }
            }
        },
        new object[]
        {
            new double[3, 2]
            {
                { 2, 3 },
                { 2, 3 },
                { 2, 3 }
            },
            new double[2, 3]
            {
                { 2, 3, 4 },
                { 2, 3, 4 }
            },
            new double[3, 3]
            {
                { 10, 15, 20 },
                { 10, 15, 20 },
                { 10, 15, 20 }
            }
        },
        new object[]
        {
            new double[5, 1]
            {
                { -5 },
                { 4 },
                { 3 },
                { 4 },
                { 5 }
            },
            new double[1, 5]
            {
                { 1, 2, 3, -2, 1 }
            },
            new double[5, 5]
            {
                { -5, -10, -15, 10, -5 },
                { 4, 8, 12, -8, 4 },
                { 3, 6, 9, -6, 3 },
                { 4, 8, 12, -8, 4 },
                { 5, 10, 15, -10, 5 }
            }
        },
    };

    public static IEnumerable<object[]> DifferentDimensionsNotAvailableToMultiplyMatrixData =>
    new List<object[]>
    {
        new object[]
        {
            new double[1, 2]
            {
                { 1, -2 }
            },
            new double[3, 3]
            {
                { 1, 2, 3 },
                { 2, -1, 4 },
                { 2, 1, 4 }
            }
        },
        new object[]
        {
            new double[3, 1]
            {
                { 2 },
                { 2 },
                { -2 }
            },
            new double[3, 2]
            {
                { 2, 3 },
                { 2, -3 },
                { 2, 3 }
            }
        },
        new object[]
        {
            new double[5, 1]
            {
                { -5 },
                { 4 },
                { 3 },
                { 4 },
                { 5 }
            },
            new double[2, 4]
            {
                { 1, 2, 3, -2 },
                { 1, 2, 3, -2 }
            }
        },
    };

    public static IEnumerable<object[]> SameDimensionsAddMatrixData =>
        new List<object[]>
    {
        new object[]
        {
            new double[1, 1]
            {
                { 1 }
            },
            new double[1, 1]
            {
                { 2 }
            }
        },
        new object[]
        {
            new double[2, 2]
            {
                { 2, 2 },
                { 2, 2 }
            },
            new double[2, 2]
            {
                { 4, 4 },
                { 4, 4 }
            }
        },
        new object[]
        {
            new double[5, 5]
            {
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 }
            },
            new double[5, 5]
            {
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 }
            }
        },
    };

    public static IEnumerable<object[]> SameDimensionsSubtractMatrixData =>
    new List<object[]>
    {
        new object[]
        {
            new double[1, 1]
            {
                { 2 }
            },
            new double[1, 1]
            {
                { 1 }
            },
        },
        new object[]
        {
            new double[2, 2]
            {
                { 4, 4 },
                { 4, 4 }
            },
            new double[2, 2]
            {
                { 2, 2 },
                { 2, 2 }
            }
        },
        new object[]
        {
            new double[5, 5]
            {
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 }
            },
            new double[5, 5]
            {
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 }
            }
        },
};

    public static IEnumerable<object[]> SameDimensionsMultiplyMatrixData =>
    new List<object[]>
    {
        new object[]
        {
            new double[1, 1]
            {
                { 1 }
            },
            new double[1, 1]
            {
                { 1 }
            },
        },
        new object[]
        {
            new double[2, 2]
            {
                { 2, 2 },
                { 2, 2 }
            },
            new double[2, 2]
            {
                { 8, 8 },
                { 8, 8 }
            }
        },
        new object[]
        {
            new double[5, 5]
            {
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 }
            },
            new double[5, 5]
            {
                { 15, 30, 45, 60, 75 },
                { 15, 30, 45, 60, 75 },
                { 15, 30, 45, 60, 75 },
                { 15, 30, 45, 60, 75 },
                { 15, 30, 45, 60, 75 }
            }
        },
    };

    public static IEnumerable<object[]> SameDimensionsSubMatrixData =>
    new List<object[]>
    {
        new object[]
        {
            new double[1, 1]
            {
                { 2 }
            },
            new double[1, 1]
            {
                { 1 }
            }
        },
        new object[]
        {
            new double[2, 2]
            {
                { 4, 4 },
                { 4, 4 }
            },
            new double[2, 2]
            {
                { 2, 2 },
                { 2, 2 }
            }
        },
        new object[]
        {
            new double[5, 5]
            {
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 },
                { 2, 4, 6, 8, 10 }
            },
            new double[5, 5]
            {
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 }
            }
        },
    };

    public static IEnumerable<object[]> NullMatrixData =>
    new List<object[]>
    {
        new object[]
        {
            null
        }
    };

    public static IEnumerable<object[]> SameDimensionsTransposeMatrixData =>
    new List<object[]>
    {
        new object[]
        {
            new double[1, 1]
            {
                { 1 }
            },
            new double[1, 1]
            {
                { 1 }
            }
        },
        new object[]
        {
            new double[1, 3]
            {
                { 0, 1, -2 }
            },
            new double[3, 1]
            {
                { 0 },
                { 1 },
                { -2 }
            }
        },
        new object[]
        {
            new double[3, 3]
            {
                { 0, 1, 2 },
                { 3, 4, -5 },
                { -6, 7, 8 },
            },
            new double[3, 3]
            {
                { 0, 3, -6 },
                { 1, 4, 7 },
                { 2, -5, 8 },
            }
        },
        new object[]
        {
            new double[3, 2]
            {
                { 0, -1 },
                { 2, 3 },
                { 4, 5 },
            },
            new double[2, 3]
            {
                { 0, 2, 4 },
                { -1, 3, 5 }
            }
        }
    };

    public static IEnumerable<object[]> SameDimensionsHadamardMatrixData =>
    new List<object[]>
    {
        new object[]
        {
            new double[1, 1]
            {
                { 1 }
            },
            new double[1, 1]
            {
                { 1 }
            }
        },
        new object[]
        {
            new double[2, 2]
            {
                { 2, 2 },
                { 2, 2 }
            },
            new double[2, 2]
            {
                { 4, 4 },
                { 4, 4 }
            }
        },
        new object[]
        {
            new double[3, 3]
            {
                { 0, -1, 2 },
                { 3, 4, 5 },
                { 6, 7, -8 }
            },
            new double[3, 3]
            {
                { 0, 1, 4 },
                { 9, 16, 25 },
                { 36, 49, 64 }
            }
        },
        new object[]
        {
            new double[2, 3]
            {
                { 0, 1, -2 },
                { 3, 4, 5 }
            },
            new double[2, 3]
            {
                { 0, 1, 4 },
                { 9, 16, 25 }
            }
        }
    };

    #endregion PremadeData

    #region Constructor

    [Theory]
    [MemberData(nameof(SquareMatrixData))]
    [MemberData(nameof(NotSquareMatrixData))]
    public void ConstructorMatrix_SameSize_ReturnsTrue(double[,] array)
    {
        // Arrange
        var matrix = new Matrix(array);

        // Act
        var rows = matrix.Rows;
        var columns = matrix.Columns;

        // Assert
        Assert.Equal(array.GetLength(0), rows);
        Assert.Equal(array.GetLength(1), columns);
    }

    [Theory]
    [MemberData(nameof(SquareMatrixData))]
    [MemberData(nameof(NotSquareMatrixData))]
    public void ConstructorValues_SameSize_ReturnsTrue(double[,] array)
    {
        // Arrange
        var arrayRows = array.GetLength(0);
        var arrayColumns = array.GetLength(1);
        var matrix = new Matrix(arrayRows, arrayColumns);

        // Act
        var matrixRows = matrix.Rows;
        var matrixColumns = matrix.Columns;

        // Assert
        Assert.Equal(arrayRows, matrixRows);
        Assert.Equal(arrayColumns, matrixColumns);
    }

    [Theory]
    [MemberData(nameof(NullMatrixData))]
    public void ConstructorMatrix_NullArray_ReturnsArgumentNullException
        (double[,] nullArray)
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => new Matrix(nullArray));
    }

    [Theory]
    [MemberData(nameof(LessThenZeroConstructorValuesData))]
    public void ConstructorMatrix_LessThanZeroRowsAndOrColumns_ReturnsException
        (int rows, int columns)
    {
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(rows, columns));
    }

    [Theory]
    [MemberData(nameof(ZeroAndMoreConstructorValuesData))]
    public void ConstructorMatrix_ZeroOrMoreRowsAndOrColumns_ReturnsException
        (int rows, int columns)
    {
        // Arrange
        Matrix matrix = new(rows, columns);

        // Assert
        Assert.Equal(rows, matrix.Rows);
        Assert.Equal(columns, matrix.Columns);
    }

    #endregion Constructor

    #region Addition

    #region Add

    [Theory]
    [MemberData(nameof(SameDimensionsAddMatrixData))]
    public void Add_SameDimensions_ReturnsTrue(double[,] addMatrix,
        double[,] expectedMatrix)
    {
        // Arrange
        Matrix matrix = new(addMatrix);
        Matrix expected = new(expectedMatrix);

        // Act
        Matrix result = matrix.Add(matrix);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(SameDimensionsAddMatrixData))]
    public void Add_NullMatrix_ThrowsArgumentNullException(double[,] array1,
        double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Act
        matrix2 = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => matrix1.Add(matrix2));
    }

    [Theory]
    [MemberData(nameof(DifferentDimensionsMatrixData))]
    public void Add_DifferentDimensions_ThrowsMatrixException(
        double[,] array1, double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Assert
        Assert.Throws<MatrixException>(() => matrix1.Add(matrix2));
    }

    #endregion Add

    #region PlusOperation

    [Theory]
    [MemberData(nameof(SameDimensionsAddMatrixData))]
    public void PlusOperation_SameDimensions_ReturnsTrue(double[,] addMatrix,
        double[,] expectedMatrix)
    {
        // Arrange
        Matrix matrix = new(addMatrix);
        Matrix expected = new(expectedMatrix);

        // Act
        Matrix result = matrix + matrix;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PlusOperation_NullMatrix_ThrowsArgumentNullException()
    {
        // Arrange
        Matrix matrix = new(2, 2);

        // Act
        matrix = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => matrix + matrix);
    }

    [Theory]
    [MemberData(nameof(DifferentDimensionsMatrixData))]
    public void PlusOperation_DifferentDimensions_ThrowsMatrixException(
    double[,] array1, double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Assert
        Assert.Throws<MatrixException>(() => matrix1 + matrix2);
    }

    #endregion PlusOperation

    #endregion Addition

    #region Subtract

    #region Sub

    [Theory]
    [MemberData(nameof(SameDimensionsSubtractMatrixData))]
    public void Subtract_SameDimensions_ReturnsTrue(double[,] bigValuesArray,
        double[,] smallValuesArray)
    {
        // Arrange
        Matrix bigValuesMatrix = new(bigValuesArray);
        Matrix expectedMatrix = new(smallValuesArray);

        // Act
        Matrix result = bigValuesMatrix.Subtract(expectedMatrix);

        // Assert
        Assert.Equal(expectedMatrix, result);
    }

    [Theory]
    [MemberData(nameof(SameDimensionsSubtractMatrixData))]
    public void Subtract_NullMatrix_ThrowsArgumentNullException(double[,] array1,
        double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Act
        matrix2 = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => matrix1.Subtract(matrix2));
    }

    [Theory]
    [MemberData(nameof(DifferentDimensionsMatrixData))]
    public void Subtract_DifferentDimensions_ThrowsMatrixException(
        double[,] array1, double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Assert
        Assert.Throws<MatrixException>(() => matrix1.Subtract(matrix2));
    }

    #endregion Sub

    #region MinusOperation

    [Theory]
    [MemberData(nameof(SameDimensionsSubtractMatrixData))]
    public void MinusOperation_SameDimensions_ReturnsTrue(double[,] bigValuesArray,
        double[,] smallValuesArray)
    {
        // Arrange
        Matrix bigValuesMatrix = new(bigValuesArray);
        Matrix expectedMatrix = new(smallValuesArray);

        // Act
        Matrix result = bigValuesMatrix - expectedMatrix;

        // Assert
        Assert.Equal(expectedMatrix, result);
    }

    [Fact]
    public void MinusOperation_NullMatrix_ThrowsArgumentNullException()
    {
        // Arrange
        Matrix matrix = new(2, 2);

        // Act
        matrix = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => matrix - matrix);
    }

    [Theory]
    [MemberData(nameof(DifferentDimensionsMatrixData))]
    public void MinusOperation_DifferentDimensions_ThrowsMatrixException(
    double[,] array1, double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Assert
        Assert.Throws<MatrixException>(() => matrix1 - matrix2);
    }

    #endregion MinusOperation

    #endregion Subtract

    #region Multiply

    #region Mul

    [Theory]
    [MemberData(nameof(SameDimensionsMultiplyMatrixData))]
    public void Multiply_SameDimensions_ReturnsTrue(double[,] array,
        double[,] expectedArray)
    {
        // Arrange
        Matrix matrix = new(array);
        Matrix expectedMatrix = new(expectedArray);

        // Act
        Matrix result = matrix.Multiply(matrix);

        // Assert
        Assert.Equal(expectedMatrix, result);
    }

    [Theory]
    [MemberData(nameof(SameDimensionsMultiplyMatrixData))]
    public void Multiply_NullMatrix_ThrowsArgumentNullException(double[,] array1,
        double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Act
        matrix2 = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => matrix1.Multiply(matrix2));
    }

    [Theory]
    [MemberData(nameof(DifferentDimensionsNotAvailableToMultiplyMatrixData))]
    public void Multiply_DifferentDimensions_ThrowsMatrixException(
        double[,] array1, double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Assert
        Assert.Throws<MatrixException>(() => matrix1.Multiply(matrix2));
    }

    #endregion Mul

    #region MultiplyOperation

    [Theory]
    [MemberData(nameof(SameDimensionsMultiplyMatrixData))]
    public void MultiplyOperation_SameDimensions_ReturnsTrue(double[,] array,
        double[,] expectedArray)
    {
        // Arrange
        Matrix matrix = new(array);
        Matrix expectedMatrix = new(expectedArray);

        // Act
        Matrix result = matrix * matrix;

        // Assert
        Assert.Equal(expectedMatrix, result);
    }

    [Fact]
    public void MultiplyOperation_NullMatrix_ThrowsArgumentNullException()
    {
        // Arrange
        Matrix matrix = new(2, 2);

        // Act
        matrix = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => matrix * matrix);
    }

    [Theory]
    [MemberData(nameof(DifferentDimensionsNotAvailableToMultiplyMatrixData))]
    public void MultiplyOperation_DifferentDimensions_ThrowsMatrixException(
    double[,] array1, double[,] array2)
    {
        // Arrange
        Matrix matrix1 = new(array1);
        Matrix matrix2 = new(array2);

        // Assert
        Assert.Throws<MatrixException>(() => matrix1 * matrix2);
    }

    #endregion MultiplyOperation

    #endregion Multiply

    #region Hadamard

    [Theory]
    [MemberData(nameof(SameDimensionsHadamardMatrixData))]
    public void Hadamard2x2_ReturnsTrue(double[,] array, double[,] expectedArray)
    {
        // Arrange
        Matrix matrix = new(array);
        Matrix expected = new(expectedArray);

        // Act
        Matrix actual = matrix.Hadamard(matrix);

        // Assert
        Assert.Equal(expected.Array, actual.Array);
        Assert.Equal(expected, actual);
    }

    #endregion Hadamard

    #region Transpose

    [Theory]
    [MemberData(nameof(SameDimensionsTransposeMatrixData))]
    public void Transpose_ReturnsTrue(double[,] array, double[,] expectedArray)
    {
        // Arrange
        Matrix matrix = new(array);
        Matrix expected = new(expectedArray);

        // Act
        Matrix actual = matrix.Transpose();

        // Assert
        Assert.Equal(expected.Array, actual.Array);
        Assert.Equal(expected, actual);
    }

    #endregion Transpose

    #region Clone

    [Theory]
    [MemberData(nameof(SquareMatrixData))]
    public void Clone_ReturnsTrue(double[,] array)
    {
        // Arrange
        Matrix expected = new(array);

        // Act
        Matrix matrix = (Matrix)expected.Clone();

        // Assert
        Assert.Equal(expected.Array, matrix.Array);
        Assert.Equal(expected, matrix);
    }

    #endregion Clone

    #region ToString

    [Fact]

    public void ToString2x2_ReturnsTrue()
    {
        double[,] nums =
            {
                { 2, 2 },
                { 2, 2 }
            };

        Matrix matrix = new(nums);

        string expected = $"2 2 {Environment.NewLine}2 2 {Environment.NewLine}";
        string actual = matrix.ToString();

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void ToString3x2_ReturnsTrue()
    {
        double[,] nums =
            {
                { 0, 1, 2 },
                { 3, 4, 5 }
            };

        Matrix matrix = new(nums);

        string expected = $"0 1 2 {Environment.NewLine}3 4 5 {Environment.NewLine}";
        string actual = matrix.ToString();

        Assert.Equal(actual, expected);
    }

    #endregion ToString
}
