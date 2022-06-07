using System.Collections.Generic;
using System.Linq;
using SML.ExtensionMethods;
using Xunit;

namespace SML.Tests.ArrayExtensionTests;

public class ArrayExtensionTests
{
    #region PremadeData

    public static IEnumerable<object[]> OneElementData =>
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
            },
        },
    };

    public static IEnumerable<object[]> TwoElementsData =>
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
                    
    
    #endregion PremadeData

    #region GetRow

    [Theory]
    [MemberData(nameof(OneElementData))]
    [MemberData(nameof(TwoElementsData))]
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
    [MemberData(nameof(OneElementData))]
    [MemberData(nameof(TwoElementsData))]
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
}
