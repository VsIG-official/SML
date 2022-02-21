namespace SML.Polynomials.Exceptions;

[Serializable]
public class PolynomialArgumentNullException : Exception
{
    public PolynomialArgumentNullException() { }

    public PolynomialArgumentNullException(string message) :
        base(message) { }
}
