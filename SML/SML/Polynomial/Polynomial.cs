using SML.Polynomial.Exceptions;

namespace SML.Polynomial;

public sealed class Polynomial
{
    private readonly List<PolynomialMember> _monomials;

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

    public void AddMember(PolynomialMember member)
    {
        if (member == null)
        {
            throw new PolynomialArgumentNullException("Member is null");
        }

        if (ContainsMember(member.Degree))
        {
            throw new PolynomialArgumentException
                ("Member with such degree already exist in polynomial");
        }

        if (member.Coefficient == 0 && member.Degree != 0)
        {
            throw new PolynomialArgumentException("Don't need to add this member");
        }

        _monomials.Add(member);
    }

    public void AddMember((double degree, double coefficient) member)
    {
        if (ContainsMember(member.degree) || member.coefficient == 0)
        {
            throw new PolynomialArgumentException
                ("Member with such degree already exist in polynomial");
        }

        AddMember(new PolynomialMember(member.degree, member.coefficient));
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

        return new PolynomialMember(0, 0);
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

    public static Polynomial operator +(Polynomial a, Polynomial b)
    {
        if (a == null || b == null)
        {
            throw new PolynomialArgumentNullException();
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
            throw new PolynomialArgumentNullException();
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
            throw new PolynomialArgumentNullException();
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

    public Polynomial Add(Polynomial polynomial)
    {
        if (polynomial is null)
        {
            throw new PolynomialArgumentNullException();
        }

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
}
