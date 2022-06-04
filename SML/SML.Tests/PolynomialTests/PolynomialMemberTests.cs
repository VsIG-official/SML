using Xunit;

namespace SML.Tests.PolynomialTests;

public class PolynomialMemberTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void Constructor_Int_ReturnsCorrectValues(int expected)
    {
        // Arrange
        var node = new CircularLinkedListNode<int>(expected);

        // Act
        var actualData = node.Data;
        var actualNext = node.Next;

        // Assert
        Assert.Equal(expected, actualData);
        Assert.Null(actualNext);
    }
}
