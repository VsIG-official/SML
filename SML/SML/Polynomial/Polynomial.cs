﻿using SML.Polynomials.Exceptions;

namespace SML.Polynomials;

public sealed class Polynomial
{
    #region Fields

    private readonly List<PolynomialMember> _monomials;
    private const string NullMemberMessage = "Member is null";
    private const string ExistingMemberMessage =
        "Member with such degree already exist in polynomial";
    private const string MeaninglessMemberMessage =
        "Don't need to add this member";

    public int Count
    {
        get
        {
            int counter = 0;

            foreach (PolynomialMember member in _monomials)
            {
                if (member != null)
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    public double Degree
    {
        get
        {
            double maxDegree = 0;

            foreach (PolynomialMember member in _monomials)
            {
                if (member.Degree > maxDegree)
                {
                    maxDegree = member.Degree;
                }
            }

            return maxDegree;
        }
    }

    #endregion Fields

    #region Constructors

    public Polynomial()
    {
        _monomials = new List<PolynomialMember>();
    }

    public Polynomial(PolynomialMember member) : this()
    {
        _monomials.Add(member);
    }

    public Polynomial(IEnumerable<PolynomialMember> members) : this()
    {
        foreach (PolynomialMember member in members)
        {
            _monomials.Add(member);
        }
    }

    public Polynomial((double degree, double coefficient) member) :
        this(new PolynomialMember(member.degree, member.coefficient))
    { }

    public Polynomial(IEnumerable<(double degree, double coefficient)> members) :
        this(members.Select(member => new
        PolynomialMember(member.degree, member.coefficient)))
    { }

    #endregion Constructors

    #region Methods

    public void AddMember(PolynomialMember member)
    {
        CheckPolynomialMember(member);

        _monomials.Add(member);
    }

    private void CheckPolynomialMember(PolynomialMember member)
    {
        CheckNullMember(member);
        CheckExistingMember(member);
        CheckMeaninglessMember(member);
    }

    private static void CheckNullMember(PolynomialMember member)
    {
        if (member == null)
        {
            throw new PolynomialArgumentNullException(NullMemberMessage);
        }
    }

    private void CheckExistingMember(PolynomialMember member)
    {
        if (ContainsMember(member.Degree))
        {
            throw new PolynomialArgumentException
                (ExistingMemberMessage);
        }
    }

    private static void CheckMeaninglessMember(PolynomialMember member)
    {
        if (member.Coefficient == 0 && member.Degree != 0)
        {
            throw new PolynomialArgumentException(MeaninglessMemberMessage);
        }
    }

    public void AddMember((double degree, double coefficient) member)
    {
        CheckTupleMember(member);

        AddMember(new PolynomialMember(member.degree, member.coefficient));
    }

    private void CheckTupleMember((double degree, double coefficient) member)
    {
        if (ContainsMember(member.degree) || member.coefficient == 0)
        {
            throw new PolynomialArgumentException
                (ExistingMemberMessage);
        }
    }

    public bool RemoveMember(double degree)
    {
        for (var i = 0; i < Count; i++)
        {
            if (_monomials[i].Degree == degree)
            {
                _monomials.RemoveAt(i);

                return true;
            }
        }

        return false;
    }

    public bool ContainsMember(double degree)
    {
        for (var i = 0; i < Count; i++)
        {
            if (_monomials[i].Degree == degree)
            {
                return true;
            }
        }

        return false;
    }

    public PolynomialMember Find(double degree)
    {
        for (var i = 0; i < Count; i++)
        {
            if (_monomials[i].Degree == degree)
            {
                return _monomials[i];
            }
        }

        return null;
    }

    public double this[double degree]
    {
        get
        {
            if (ContainsMember(degree))
            {
                return Find(degree).Coefficient;
            }

            return 0;
        }
        set
        {
            if (ContainsMember(degree))
            {
                if (value != 0)
                {
                    Find(degree).Coefficient = value;
                }
                else
                {
                    RemoveMember(degree);
                }
            }
            else if (value != 0)
            {
                var member = new PolynomialMember(degree, value);

                AddMember(member);
            }
        }
    }

    public PolynomialMember[] ToArray()
    {
        return _monomials.ToArray();
    }

    public Polynomial Add(Polynomial polynomial)
    {
        return this + polynomial;
    }

    public Polynomial Subtraction(Polynomial polynomial)
    {
        return this - polynomial;
    }

    public Polynomial Multiply(Polynomial polynomial)
    {
        return this * polynomial;
    }

    public Polynomial Add((double degree, double coefficient) member)
    {
        return Add(new Polynomial(member));
    }

    public Polynomial Subtraction((double degree, double coefficient) member)
    {
        return Subtraction(new Polynomial(member));
    }

    public Polynomial Multiply((double degree, double coefficient) member)
    {
        return Multiply(new Polynomial(member));
    }

    #endregion Methods

    #region Operators

    public static Polynomial operator +(Polynomial a, Polynomial b)
    {
        if (a == null || b == null)
        {
            throw new PolynomialArgumentNullException(NullMemberMessage);
        }

        var result = new Polynomial(a._monomials);

        foreach (PolynomialMember item in b._monomials)
        {
            result[item.Degree] += item.Coefficient;
        }

        return result;
    }

    public static Polynomial operator -(Polynomial a, Polynomial b)
    {
        if (a == null || b == null)
        {
            throw new PolynomialArgumentNullException(NullMemberMessage);
        }

        var result = new Polynomial(a._monomials);

        for (var i = 0; i < a._monomials.Count; ++i)
        {
            if (a._monomials[i].Coefficient == 0)
            {
                return new Polynomial();
            }
        }

        foreach (PolynomialMember member in b._monomials)
        {
            result[member.Degree] -= member.Coefficient;
        }

        return result;
    }

    public static Polynomial operator *(Polynomial a, Polynomial b)
    {
        if (a == null || b == null)
        {
            throw new PolynomialArgumentNullException(NullMemberMessage);
        }

        var result = new Polynomial();

        foreach (PolynomialMember memberA in a._monomials)
        {
            foreach (PolynomialMember memberB in b._monomials)
            {
                double degreeSum = memberA.Degree + memberB.Degree;
                double coefficientMul = memberA.Coefficient * memberB.Coefficient;

                if (coefficientMul != 0)
                {
                    if (result.Find(coefficientMul) == null)
                    {
                        result.AddMember(new PolynomialMember
                            (degreeSum, coefficientMul));
                    }
                    else
                    {
                        result[degreeSum] += coefficientMul;
                    }
                }
            }
        }

        return result;
    }

    public static Polynomial operator +(Polynomial a,
        (double degree, double coefficient) b)
    {
        return a + new Polynomial(b);
    }

    public static Polynomial operator -(Polynomial a,
        (double degree, double coefficient) b)
    {
        return a - new Polynomial(b);
    }

    public static Polynomial operator *(Polynomial a,
        (double degree, double coefficient) b)
    {
        return a * new Polynomial(b);
    }

    #endregion Operators
}
