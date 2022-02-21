using Xunit;

namespace SML.Tests;

public class MatrixTests
{
	[Fact]
	public void Test1()
	{

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
}
