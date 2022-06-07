using System.Collections.Generic;
using System.Linq;
using SML.ExtensionMethods;
using Xunit;

namespace SML.Tests.ArrayExtensionTests;

public class ArrayExtensionTests
{
    #region PremadeData

    public static IEnumerable<object[]> OneElement2DData =>
    new List<object[]>
    {
        new object[]
        {
            new double[,]
            {
                { 0 },
                { 1 },
                { 2 },
                { 3 },
                { -1 },
                { int.MinValue },
                { int.MaxValue },
            },
        },
    };

    public static IEnumerable<object[]> TwoElements2DData =>
    new List<object[]>
    {
        new object[]
        {
            new double[,]
            {
                { 0, 1 },
                { 1, 0 },
                { 1, -1 },
                { -1, 1 },
                { -1, -1 },
                { 1, 1 },
                { int.MinValue, int.MaxValue },
                { int.MaxValue, int.MinValue },
            },
        },
    };

    public static IEnumerable<object[]> OneElementJaggedData =>
    new List<object[]>
    {
        new object[]
        {
            new double[][]
            {
                new double[] { 0 },
                new double[] { 1 },
                new double[] { -1 },
                new double[] { int.MinValue },
                new double[] { int.MaxValue },
            },
            new double[,]
            {
                { 0 },
                { 1 },
                { -1 },
                { int.MinValue },
                { int.MaxValue },
            },
        },
};

    public static IEnumerable<object[]> TwoElementsJaggedData =>
    new List<object[]>
    {
        new object[]
        {
            new double[][]
            {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 },
                new double[] { -1, 1 },
                new double[] { 1, -1 },
                new double[] { -1, -1 },
                new double[] { int.MinValue, int.MaxValue },
                new double[] { int.MaxValue, int.MinValue },
            },
            new double[,]
            {
                { 0, 0 },
                { 0, 1 },
                { 1, 0 },
                { 1, 1 },
                { -1, 1 },
                { 1, -1 },
                { -1, -1 },
                { int.MinValue, int.MaxValue },
                { int.MaxValue, int.MinValue },
            },
        },
    };

    #endregion PremadeData

    #region GetRow

    [Theory]
    [MemberData(nameof(OneElement2DData))]
    [MemberData(nameof(TwoElements2DData))]
    public void GetRow_CorrectValues(double[,] array)
    {
        // Arrange
        int columns = array.GetLength(0);
        
        for (var i = 0; i < columns; i++)
        {
            var expectedRowLength = array.GetLength(1);
            double[] expectedRow = new double[expectedRowLength];

            for (var j = 0; j < expectedRowLength; j++)
            {
                expectedRow[j] = array[i, j];
            }

            // Act
            var row = array.GetRow(i).ToArray();

            // Assert
            Assert.Equal(expectedRowLength, row.Length);
            Assert.Equal(expectedRow, row);
        }
    }

    #endregion GetRow

    #region GetColumn

    [Theory]
    [MemberData(nameof(OneElement2DData))]
    [MemberData(nameof(TwoElements2DData))]
    public void GetColumn_CorrectValues(double[,] array)
    {
        // Arrange
        int rows = array.GetLength(1);

        for (var i = 0; i < rows; i++)
        {
            var expectedColumnLength = array.GetLength(0);
            double[] expectedColumn = new double[expectedColumnLength];

            for (var j = 0; j < expectedColumnLength; j++)
            {
                expectedColumn[j] = array[j, i];
            }

            // Act
            var column = array.GetColumn(i).ToArray();

            // Assert
            Assert.Equal(expectedColumnLength, column.Length);
            Assert.Equal(expectedColumn, column);
        }
    }

    #endregion GetColumn

    #region To2D

    [Theory]
    [MemberData(nameof(TwoElementsJaggedData))]
    public void To2D_CorrectValues(double[][] array, double[,] expectedArray)
    {
        // Act
        double[,] actual = array.To2D();

        // Assert
        Assert.Equal(expectedArray.Length, actual.Length);
        Assert.Equal(expectedArray, actual);
    }

    #endregion To2D
}
