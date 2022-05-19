using System;
using System.Collections.Generic;
using SML.Matrices;
using Xunit;

namespace SML.Tests.MatrixTests;

public class MatrixTests
{
    #region PremadeData

    public static IEnumerable<object[]> SquareMatrixData => new List<object[]>
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

    public static IEnumerable<object[]> DifferentDimensionsMatrixData => new List<object[]>
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


    #endregion PremadeData

    #region Constructor

    [Theory]
    [MemberData(nameof(SquareMatrixData))]
    public void Constructor_SameSize_SquareMatrix_ReturnsTrue(double[,] array)
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
    [MemberData(nameof(DifferentDimensionsMatrixData))]
    public void Constructor_DifferentDimensions_SquareMatrix_ReturnsTrue
        (double[,] array)
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


    #endregion Constructor

    [Fact]
	public void Hadamard2x2_ShouldReturnTrue()
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
    public void Hadamard3x3_ShouldReturnTrue()
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
    public void Hadamard3x2_ShouldReturnTrue()
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

    [Fact]
    public void Transpose3x1_ShouldReturnTrue()
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
    public void Transpose3x3_ShouldReturnTrue()
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
    public void Transpose2x3_ShouldReturnTrue()
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

    [Fact]

    public void ToString2x2_ShouldReturnTrue()
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
    public void ToString3x2_ShouldReturnTrue()
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
}
