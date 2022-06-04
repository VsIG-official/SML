﻿using System.Collections.Generic;
using SML.Polynomials;
using Xunit;

namespace SML.Tests.PolynomialTests;

public class PolynomialTests
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

    public static IEnumerable<object[]> DifferentNonZeroValuesData => new List<object[]>
    {
        new object[] { 2, 1 },
        new object[] { -1, 2 },
        new object[] { 1, -1 },
        new object[] { -3.222, 9 },
        new object[] { -1000.123, 9999.1 },
        new object[] { int.MaxValue, int.MinValue },
        new object[] { int.MinValue, int.MaxValue },
    };

    #endregion PremadeData

    #region Constructor

    [Fact]
    public void Constructor_NoneValues_CorrectValues()
    {
        // Act
        Polynomial polynomial = new();

        // Assert
        Assert.Equal(0, polynomial.Count);
        Assert.Equal(0, polynomial.Degree);
    }

    [Theory]
    [MemberData(nameof(SameValuesData))]
    public void Constructor_PolynomialMember_CorrectValues(double expected)
    {
        // Arrange
        int expectedMembersCount = 1;
        PolynomialMember member = new(expected, expected);

        // Act
        Polynomial polynomial = new(member);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
    }

    [Theory]
    [MemberData(nameof(SameValuesData))]
    public void Constructor_PolynomialMemberList_CorrectValues(double expected)
    {
        // Arrange
        int expectedMembersCount = 5;
        
        List<PolynomialMember> members = new();
        PolynomialMember member = new(expected, expected);

        for (int i = 0; i < expectedMembersCount; i++)
        {
            members.Add(member);
        }

        // Act
        Polynomial polynomial = new(members);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
    }

    [Theory]
    [MemberData(nameof(SameValuesData))]
    public void Constructor_PolynomialMemberTuple_CorrectValues(double expected)
    {
        // Arrange
        int expectedMembersCount = 1;
        (double, double) member = (expected, expected);

        // Act
        Polynomial polynomial = new(member);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
    }

    [Theory]
    [MemberData(nameof(SameValuesData))]
    public void Constructor_PolynomialMemberTupleList_CorrectValues(double expected)
    {
        // Arrange
        int expectedMembersCount = 5;

        List<(double, double)> members = new();
        (double, double) member = (expected, expected);

        for (int i = 0; i < expectedMembersCount; i++)
        {
            members.Add(member);
        }

        // Act
        Polynomial polynomial = new(members);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
    }

    #endregion Constructor

    #region AddMember

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void AddMember_PolynomialMember_CorrectValues
        (double degree, double coefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        Polynomial polynomial = new();
        PolynomialMember member = new(degree, coefficient);

        // Act
        polynomial.AddMember(member);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void AddMember_Tuple_CorrectValues
        (double degree, double coefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        (double, double) member = (degree, coefficient);

        // Act
        Polynomial polynomial = new();
        polynomial.AddMember(member);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
    }

    #endregion AddMember
}
