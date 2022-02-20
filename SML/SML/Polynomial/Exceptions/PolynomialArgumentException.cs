namespace SML.Polynomial.Exceptions;

[Serializable]
public class PolynomialArgumentException : Exception
{
    public PolynomialArgumentException() { }

    public PolynomialArgumentException(string message) :
        base (message) { }
}
