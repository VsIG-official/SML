using System.Collections.Generic;
using SML.Polynomials;
using Xunit;

namespace SML.Tests.PolynomialTests;

public class PolynomialMemberTests
{
    #region PremadeData

    public static IEnumerable<object[]> SameValuesData => new List<object[]>
    {
        new object[] { 0 },
        new object[] { 1 },
        new object[] { -1 },
        new object[] { -1.23 },
        new object[] { 99.9 },
        new object[] { int.MinValue },
        new object[] { int.MaxValue },
    };

    public static IEnumerable<object[]> DifferentValuesData => new List<object[]>
    {
        new object[] { 0, 1 },
        new object[] { -1, 0 },
        new object[] { 1, -1 },
        new object[] { -1.23, 2 },
        new object[] { 99.9, -100 },
        new object[] { int.MinValue, int.MaxValue },
        new object[] { int.MaxValue, int.MinValue },
    };

    #endregion PremadeData

    #region Constructor

    [Theory]
    [MemberData(nameof(SameValuesData))]
    public void Constructor_SameValues_CorrectValues(double expected)
    {
        // Act
        PolynomialMember member = new(expected, expected);

        // Assert
        Assert.Equal(expected, member.Degree);
        Assert.Equal(expected, member.Coefficient);
    }

    [Theory]
    [MemberData(nameof(DifferentValuesData))]
    public void Constructor_DifferentValues_CorrectValues
        (double deegree, double coefficient)
    {
        // Act
        PolynomialMember member = new(deegree, coefficient);

        // Assert
        Assert.Equal(deegree, member.Degree);
        Assert.Equal(coefficient, member.Coefficient);
    }

    #endregion Constructor

    #region Clone

    [Theory]
    [MemberData(nameof(SameValuesData))]
    public void Clone_SameValues_CorrectValues(double expected)
    {
        // Act
        PolynomialMember member = new(expected, expected);
        var clone = (PolynomialMember)member.Clone();

        // Assert
        Assert.Equal(member.Degree, clone.Degree);
        Assert.Equal(member.Coefficient, clone.Coefficient);
    }

    [Theory]
    [MemberData(nameof(DifferentValuesData))]
    public void Clone_DifferentValues_CorrectValues
        (double deegree, double coefficient)
    {
        // Act
        PolynomialMember member = new(deegree, coefficient);
        var clone = (PolynomialMember)member.Clone();

        // Assert
        Assert.Equal(member.Degree, clone.Degree);
        Assert.Equal(member.Coefficient, clone.Coefficient);
    }

    #endregion Clone
}
