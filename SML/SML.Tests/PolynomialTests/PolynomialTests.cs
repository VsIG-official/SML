using System.Collections.Generic;
using SML.Polynomials;
using SML.Polynomials.Exceptions;
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

    public static IEnumerable<object[]> AdditionData => new List<object[]>
    {
        new object[] { 3, 2, 4 },
        new object[] { 1, 1, 2 },
        new object[] { -1 , -1, -2 },
        new object[] { 2, -1.23, -2.46 },
    };

    public static IEnumerable<object[]> SubtractionData => new List<object[]>
    {
        new object[] { 3, 4, 2, 2 },
        new object[] { 1, 2, 1, 1 },
        new object[] { -1 , -2, 3, -5 },
        new object[] { 2, 2.46, -1.2, 3.66 },
    };

    public static IEnumerable<object[]> MultiplyData => new List<object[]>
    {
        new object[] { 3, 2, 3, 6 },
        new object[] { 1, 2, 1, 2 },
        new object[] { -1 , -2, 3, -6 },
        new object[] { 2, 2.46, -1.2, -2.952 },
    };

    #endregion PremadeData

    #region Constructor

    [Fact]
    public void Constructor_NoneValues_CorrectValues()
    {
        // Arrange
        int expectedMembersCount = 0;
        int expectedMembersDegree = 0;

        // Act
        Polynomial polynomial = new();

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedMembersDegree, polynomial.Degree);
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
    
    [Fact]
    public void AddMember_NullPolynomialMember_ThrowsPolynomialArgumentNullException()
    {
        // Arrange
        Polynomial polynomial = new();

        // Assert
        Assert.Throws<PolynomialArgumentNullException>(() => polynomial.AddMember(null));
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void AddMember_ExistingPolynomialMember_ThrowsPolynomialArgumentException
        (double deegree, double coefficient)
    {
        // Arrange
        Polynomial polynomial = new();
        PolynomialMember member = new(deegree, coefficient);
        polynomial.AddMember(member);

        // Assert
        Assert.Throws<PolynomialArgumentException>(() => polynomial.AddMember(member));
    }

    [Fact]
    public void AddMember_MeaninglessPolynomialMember_ThrowsPolynomialArgumentException()
    {
        // Arrange
        int degree = 1;
        int coefficient = 0;
        
        Polynomial polynomial = new();
        PolynomialMember member = new(degree, coefficient);

        // Assert
        Assert.Throws<PolynomialArgumentException>(() => polynomial.AddMember(member));
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void AddMember_MeaninglessTuple_ThrowsPolynomialArgumentException
        (double deegree, double coefficient)
    {
        // Arrange
        Polynomial polynomial = new();
        (double, double) member = (deegree, coefficient);
        polynomial.AddMember(member);

        // Assert
        Assert.Throws<PolynomialArgumentException>(() => polynomial.AddMember(member));
    }

    #endregion AddMember

    #region RemoveMember

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void RemoveMember_WithMember_True
        (double degree, double coefficient)
    {
        // Arrange
        int expectedMembersCount = 0;
        bool expectedDoesMemberExisted = true;
        
        Polynomial polynomial = new();
        PolynomialMember member = new(degree, coefficient);
        polynomial.AddMember(member);

        // Act
        var doesMemberExisted = polynomial.RemoveMember(degree);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedDoesMemberExisted, doesMemberExisted);
    }

    [Theory]
    [MemberData(nameof(SameValuesData))]
    public void RemoveMember_WithoutMember_False
        (double degree)
    {
        // Arrange
        int expectedMembersCount = 0;
        bool expectedDoesMemberExisted = false;

        Polynomial polynomial = new();

        // Act
        var doesMemberExisted = polynomial.RemoveMember(degree);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedDoesMemberExisted, doesMemberExisted);
    }

    #endregion RemoveMember

    #region ContainsMember

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void ContainsMember_WithMember_True
        (double degree, double coefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        bool expectedDoesMemberExisted = true;

        Polynomial polynomial = new();
        PolynomialMember member = new(degree, coefficient);
        polynomial.AddMember(member);

        // Act
        var doesMemberExists = polynomial.ContainsMember(degree);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedDoesMemberExisted, doesMemberExists);
    }

    [Theory]
    [MemberData(nameof(SameValuesData))]
    public void ContainsMember_WithoutMember_False
        (double degree)
    {
        // Arrange
        int expectedMembersCount = 0;
        bool expectedDoesMemberExisted = false;

        Polynomial polynomial = new();

        // Act
        var doesMemberExists = polynomial.ContainsMember(degree);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedDoesMemberExisted, doesMemberExists);
    }

    #endregion ContainsMember

    #region FindMember

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void FindMember_WithMember_True
        (double degree, double coefficient)
    {
        // Arrange
        int expectedMembersCount = 1;

        Polynomial polynomial = new();
        PolynomialMember expectedMember = new(degree, coefficient);
        polynomial.AddMember(expectedMember);

        // Act
        var foundMember = polynomial.Find(degree);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedMember, foundMember);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void FindMember_WithoutMember_False
        (double degree, double coefficient)
    {
        // Arrange
        int expectedMembersCount = 0;

        Polynomial polynomial = new();
        PolynomialMember expectedMember = new(degree, coefficient);

        // Act
        var foundMember = polynomial.Find(degree);

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.NotEqual(expectedMember, foundMember);
    }

    #endregion FindMember

    #region Indexer

    #region Get

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void IndexerGet_WithMember_CorrectCoefficient
        (double degree, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;

        Polynomial polynomial = new();
        PolynomialMember expectedMember = new(degree, expectedCoefficient);
        polynomial.AddMember(expectedMember);

        // Act
        var foundCoefficient = polynomial[degree];

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedCoefficient, foundCoefficient);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void IndexerGet_WithoutMember_CorrectCoefficient
        (double degree, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 0;

        Polynomial polynomial = new();

        // Act
        var foundCoefficient = polynomial[degree];

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(0, foundCoefficient);
    }

    #endregion Get

    #region Set

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void IndexerSet_WithMemberAndValueNotEqualZero_Coefficient
        (double degree, double coefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        double expectedCoefficient = 5;

        Polynomial polynomial = new();
        PolynomialMember expectedMember = new(degree, coefficient);
        polynomial.AddMember(expectedMember);

        // Act
        polynomial[degree] = expectedCoefficient;
        var foundCoefficient = polynomial[degree];

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedCoefficient, foundCoefficient);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void IndexerSet_WithMemberAndValueEqualZero_Coefficient
        (double degree, double coefficient)
    {
        // Arrange
        int expectedMembersCount = 0;
        double expectedCoefficient = 0;

        Polynomial polynomial = new();
        PolynomialMember expectedMember = new(degree, coefficient);
        polynomial.AddMember(expectedMember);

        // Act
        polynomial[degree] = expectedCoefficient;
        var foundCoefficient = polynomial[degree];

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedCoefficient, foundCoefficient);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void IndexerSet_WithoutMemberAndValueNotEqualZero_Coefficient
        (double degree, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;

        Polynomial polynomial = new();

        // Act
        polynomial[degree] = expectedCoefficient;
        var foundCoefficient = polynomial[degree];

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedCoefficient, foundCoefficient);
    }

    #endregion Get

    #endregion Indexer

    #region ToArray

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void ToArray_CorrectArray
        (double degree, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;

        Polynomial polynomial = new();
        PolynomialMember member = new(degree, expectedCoefficient);
        polynomial.AddMember(member);

        PolynomialMember[] expectedArray = new[] { member };

        // Act
        var foundArray = polynomial.ToArray();

        // Assert
        Assert.Equal(expectedMembersCount, polynomial.Count);
        Assert.Equal(expectedArray, foundArray);
    }

    #endregion ToArray

    #region Addition

    #region PlusOperator

    [Theory]
    [MemberData(nameof(AdditionData))]
    public void PlusOperator_PolynomialMember_CorrectValues
        (double degree, double coefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        PolynomialMember member = new(degree, coefficient);
        
        Polynomial polynomial1 = new(member);
        Polynomial polynomial2 = new(member);

        // Act
        Polynomial result = polynomial1 + polynomial2;
        var actualAddCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualAddCoefficient, expectedCoefficient);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void PlusOperator_NullFirstPolynomial_PolynomialMember_ThrowsPolynomialArgumentNullException
        (double degree, double coefficient)
    {
        // Arrange
        PolynomialMember member = new(degree, coefficient);

        Polynomial polynomial1 =  null;
        Polynomial polynomial2 = new(member);

        // Assert
        Assert.Throws<PolynomialArgumentNullException>(() => polynomial1 + polynomial2);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void PlusOperator_NullSecondPolynomial_PolynomialMember_ThrowsPolynomialArgumentNullException
        (double degree, double coefficient)
    {
        // Arrange
        PolynomialMember member = new(degree, coefficient);

        Polynomial polynomial1 = new(member);
        Polynomial polynomial2 = null;

        // Assert
        Assert.Throws<PolynomialArgumentNullException>(() => polynomial1 + polynomial2);
    }

    [Theory]
    [MemberData(nameof(AdditionData))]
    public void PlusOperator_Tuple_CorrectValues
    (double degree, double coefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        (double degree, double coefficient) member = (degree, coefficient);

        Polynomial polynomial = new(member);

        // Act
        Polynomial result = polynomial + member;
        var actualAddCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualAddCoefficient, expectedCoefficient);
    }

    #endregion PlusOperator

    #region Add

    [Theory]
    [MemberData(nameof(AdditionData))]
    public void Add_PolynomialMember_CorrectValues
    (double degree, double coefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        PolynomialMember member = new(degree, coefficient);

        Polynomial polynomial1 = new(member);
        Polynomial polynomial2 = new(member);

        // Act
        Polynomial result = polynomial1.Add(polynomial2);
        var actualAddCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualAddCoefficient, expectedCoefficient);
    }

    [Theory]
    [MemberData(nameof(AdditionData))]
    public void Add_Tuple_CorrectValues
        (double degree, double coefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        (double, double) member = new(degree, coefficient);

        Polynomial polynomial = new(member);

        // Act
        Polynomial result = polynomial.Add(member);
        var actualAddCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualAddCoefficient, expectedCoefficient);
    }

    #endregion Add

    #endregion Addition

    #region Subtraction

    #region MinusOperator

    [Theory]
    [MemberData(nameof(SubtractionData))]
    public void MinusOperator_PolynomialMember_CorrectValues
        (double degree, double coefficient,
        double subCoefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        PolynomialMember member1 = new(degree, coefficient);
        PolynomialMember member2 = new(degree, subCoefficient);

        Polynomial polynomial1 = new(member1);
        Polynomial polynomial2 = new(member2);

        // Act
        Polynomial result = polynomial1 - polynomial2;
        var actualSubCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualSubCoefficient, expectedCoefficient);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void MinusOperator_NullFirstPolynomial_PolynomialMember_ThrowsPolynomialArgumentNullException
        (double degree, double coefficient)
    {
        // Arrange
        PolynomialMember member = new(degree, coefficient);

        Polynomial polynomial1 = null;
        Polynomial polynomial2 = new(member);

        // Assert
        Assert.Throws<PolynomialArgumentNullException>(() => polynomial1 - polynomial2);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void MinusOperator_NullSecondPolynomial_PolynomialMember_ThrowsPolynomialArgumentNullException
        (double degree, double coefficient)
    {
        // Arrange
        PolynomialMember member = new(degree, coefficient);

        Polynomial polynomial1 = new(member);
        Polynomial polynomial2 = null;

        // Assert
        Assert.Throws<PolynomialArgumentNullException>(() => polynomial1 - polynomial2);
    }

    [Theory]
    [MemberData(nameof(SubtractionData))]
    public void MinusOperator_Tuple_CorrectValues
        (double degree, double coefficient,
        double subCoefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        (double, double) member1 = new(degree, coefficient);
        (double, double) member2 = new(degree, subCoefficient);

        Polynomial polynomial = new(member1);

        // Act
        Polynomial result = polynomial - member2;
        var actualSubCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualSubCoefficient, expectedCoefficient);
    }

    #endregion MinusOperator

    #region Sub

    [Theory]
    [MemberData(nameof(SubtractionData))]
    public void Subtraction_PolynomialMember_CorrectValues
        (double degree, double coefficient,
        double subCoefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        PolynomialMember member1 = new(degree, coefficient);
        PolynomialMember member2 = new(degree, subCoefficient);

        Polynomial polynomial1 = new(member1);
        Polynomial polynomial2 = new(member2);

        // Act
        Polynomial result = polynomial1.Subtraction(polynomial2);
        var actualAddCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualAddCoefficient, expectedCoefficient);
    }

    [Theory]
    [MemberData(nameof(SubtractionData))]
    public void Subtraction_Tuple_CorrectValues
        (double degree, double coefficient,
        double subCoefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        (double, double) member1 = new(degree, coefficient);
        (double, double) member2 = new(degree, subCoefficient);

        Polynomial polynomial = new(member1);

        // Act
        Polynomial result = polynomial.Subtraction(member2);
        var actualAddCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualAddCoefficient, expectedCoefficient);
    }

    #endregion Sub

    #endregion Subtraction

    #region Multiply

    #region MultiplyOperator

    [Theory]
    [MemberData(nameof(MultiplyData))]
    public void MultiplyOperator_PolynomialMember_CorrectValues
        (double degree, double coefficient,
        double subCoefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        PolynomialMember member1 = new(degree, coefficient);
        PolynomialMember member2 = new(degree, subCoefficient);

        Polynomial polynomial1 = new(member1);
        Polynomial polynomial2 = new(member2);

        // Act
        Polynomial result = polynomial1 * polynomial2;
        var actualSubCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualSubCoefficient, expectedCoefficient);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void MultiplyOperator_NullFirstPolynomial_PolynomialMember_ThrowsPolynomialArgumentNullException
        (double degree, double coefficient)
    {
        // Arrange
        PolynomialMember member = new(degree, coefficient);

        Polynomial polynomial1 = null;
        Polynomial polynomial2 = new(member);

        // Assert
        Assert.Throws<PolynomialArgumentNullException>(() => polynomial1 * polynomial2);
    }

    [Theory]
    [MemberData(nameof(DifferentNonZeroValuesData))]
    public void MultiplyOperator_NullSecondPolynomial_PolynomialMember_ThrowsPolynomialArgumentNullException
        (double degree, double coefficient)
    {
        // Arrange
        PolynomialMember member = new(degree, coefficient);

        Polynomial polynomial1 = new(member);
        Polynomial polynomial2 = null;

        // Assert
        Assert.Throws<PolynomialArgumentNullException>(() => polynomial1 * polynomial2);
    }

    [Theory]
    [MemberData(nameof(MultiplyData))]
    public void MultiplyOperator_Tuple_CorrectValues
        (double degree, double coefficient,
        double subCoefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        (double, double) member1 = new(degree, coefficient);
        (double, double) member2 = new(degree, subCoefficient);

        Polynomial polynomial = new(member1);

        // Act
        Polynomial result = polynomial * member2;
        var actualSubCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualSubCoefficient, expectedCoefficient);
    }

    #endregion MultiplyOperator

    #region Mul

    [Theory]
    [MemberData(nameof(MultiplyData))]
    public void Multiply_PolynomialMember_CorrectValues
        (double degree, double coefficient,
        double mulCoefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        PolynomialMember member1 = new(degree, coefficient);
        PolynomialMember member2 = new(degree, mulCoefficient);

        Polynomial polynomial1 = new(member1);
        Polynomial polynomial2 = new(member2);

        // Act
        Polynomial result = polynomial1.Multiply(polynomial2);
        var actualAddCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualAddCoefficient, expectedCoefficient);
    }

    [Theory]
    [MemberData(nameof(MultiplyData))]
    public void Multiply_Tuple_CorrectValues
        (double degree, double coefficient,
        double mulCoefficient, double expectedCoefficient)
    {
        // Arrange
        int expectedMembersCount = 1;
        (double, double) member1 = new(degree, coefficient);
        (double, double) member2 = new(degree, mulCoefficient);

        Polynomial polynomial = new(member1);

        // Act
        Polynomial result = polynomial.Multiply(member2);
        var actualAddCoefficient = result.ToArray()[0].Coefficient;

        // Assert
        Assert.Equal(expectedMembersCount, result.Count);
        Assert.Equal(actualAddCoefficient, expectedCoefficient);
    }

    #endregion Mul

    #endregion Multiply
}
