using SML.CircularLinkedList;
using Xunit;

namespace SML.Tests.CircularLinkedListTests;

public class CircularLinkedListNodeTests
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

    [Theory]
    [InlineData("Паляниця")]
    [InlineData("Русский военный корабль")]
    [InlineData("European Union")]
    [InlineData("汉字 and 漢字")]
    [InlineData("الْعَرَبِيَّة")]
    [InlineData("👾🤓😎🥸🤩🥳")]
    public void Constructor_String_ReturnsCorrectValues(string expected)
    {
        // Arrange
        var node = new CircularLinkedListNode<string>(expected);

        // Act
        var actualData = node.Data;
        var actualNext = node.Next;

        // Assert
        Assert.Equal(expected, actualData);
        Assert.Null(actualNext);
    }
}
