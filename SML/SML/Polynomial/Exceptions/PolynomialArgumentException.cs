namespace SML.Polynomials.Exceptions;

[Serializable]
public class PolynomialArgumentException : Exception
{
    public PolynomialArgumentException() { }

    public PolynomialArgumentException(string message) :
        base (message) { }
}
