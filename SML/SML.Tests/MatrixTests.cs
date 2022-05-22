using System;
using System.Collections.Generic;
using SML.Matrices;
using SML.Matrices.Exceptions;
using Xunit;

namespace SML.Tests.MatrixTests;

public class MatrixTests
{
    #region PremadeData

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
    public void ConstructorMatrix_NullArray_ReturnsException(double[,] nullArray)
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => new Matrix(nullArray));
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
        Matrix result = bigValuesMatrix - expectedMatrix;

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

    #region Hadamard

    [Fact]
	public void Hadamard2x2_ReturnsTrue()
	{
        double[,] nums =
            {
                { 2, 2 },
                { 2, 2 }
            };

        double[,] expectedNums =
            {
                { 4, 4 },
                { 4, 4 }
            };

        Matrix expected = new (expectedNums);

        Matrix firstMatrix = new(nums);
        Matrix secondMatrix = new(nums);

        Matrix actual = firstMatrix.Hadamard(secondMatrix);

        Assert.Equal(actual.Array, expected.Array);
    }

    [Fact]
    public void Hadamard3x3_ReturnsTrue()
    {
        double[,] nums1 =
            {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 }
            };

        double[,] nums2 =
            {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 }
            };

        double[,] expectedNums =
            {
                { 0, 1, 4 },
                { 9, 16, 25 },
                { 36, 49, 64 }
            };

        Matrix expected = new(expectedNums);

        Matrix firstMatrix = new(nums1);
        Matrix secondMatrix = new(nums2);

        Matrix actual = firstMatrix.Hadamard(secondMatrix);

        Assert.Equal(actual.Array, expected.Array);
    }

    [Fact]
    public void Hadamard3x2_ReturnsTrue()
    {
        double[,] nums1 =
            {
                { 0, 1, 2 },
                { 3, 4, 5 }
            };

        double[,] nums2 =
            {
                { 0, 1, 2 },
                { 3, 4, 5 }
            };

        double[,] expectedNums =
            {
                { 0, 1, 4 },
                { 9, 16, 25 }
            };

        Matrix expected = new(expectedNums);

        Matrix firstMatrix = new(nums1);
        Matrix secondMatrix = new(nums2);

        Matrix actual = firstMatrix.Hadamard(secondMatrix);

        Assert.Equal(actual.Array, expected.Array);
    }

    #endregion Hadamard

    #region Transpose
    
    [Fact]
    public void Transpose3x1_ReturnsTrue()
    {
        double[,] nums1 =
            {
                { 0, 1, 2 }
            };

        Matrix numMatrix = new(nums1);

        double[,] expectedNums =
        {
            { 0 },
            { 1 },
            { 2 }
        };

        Matrix actual = numMatrix.Transpose();

        Matrix expected = new(expectedNums);

        Assert.Equal(actual.Array, expected.Array);
    }

    [Fact]
    public void Transpose3x3_ReturnsTrue()
    {
        double[,] nums1 =
            {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 },
            };

        Matrix numMatrix = new(nums1);

        double[,] expectedNums =
            {
                { 0, 3, 6 },
                { 1, 4, 7 },
                { 2, 5, 8 },
            };

        Matrix actual = numMatrix.Transpose();

        Matrix expected = new(expectedNums);

        Assert.Equal(actual.Array, expected.Array);
    }

    [Fact]
    public void Transpose2x3_ReturnsTrue()
    {
        double[,] nums1 =
            {
                { 0, 1 },
                { 2, 3 },
                { 4, 5 },
            };

        Matrix numMatrix = new(nums1);

        double[,] expectedNums =
            {
                { 0, 2, 4 },
                { 1, 3, 5 }
            };

        Matrix actual = numMatrix.Transpose();

        Matrix expected = new(expectedNums);

        Assert.Equal(actual.Array, expected.Array);
    }

    #endregion Transpose

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
