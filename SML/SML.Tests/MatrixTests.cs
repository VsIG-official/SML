using SML.Matrices;
using Xunit;

namespace SML.Tests;

public class MatrixTests
{
	[Fact]
	public void Hadamard1_ShouldReturnTrue()
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
    public void Hadamard2_ShouldReturnTrue()
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
    public void Hadamard3_ShouldReturnTrue()
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
    public void Transpose1_ShouldReturnTrue()
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
    public void Transpose2_ShouldReturnTrue()
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
    public void Transpose3_ShouldReturnTrue()
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

    public void ToString1_ShouldReturnTrue()
    {
        double[,] nums =
            {
                { 2, 2 },
                { 2, 2 }
            };

        Matrix matrix = new(nums);

        string expected = "2 2 \r\n\r\n2 2 \r\n\r\n";
        string actual = matrix.ToString();

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void ToString2_ShouldReturnTrue()
    {
        double[,] nums =
            {
                { 0, 1, 2 },
                { 3, 4, 5 }
            };

        Matrix matrix = new(nums);

        string expected = "0 1 2 \r\n\r\n3 4 5 \r\n\r\n";
        string actual = matrix.ToString();

        Assert.Equal(actual, expected);
    }
}
