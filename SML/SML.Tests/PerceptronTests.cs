using System;
using System.Collections.Generic;
using System.Linq;
using SML.ExtensionMethods;
using SML.Perceptrons;
using Xunit;

namespace SML.Tests.PerceptronTests;

public class PerceptronTests
{
    #region PremadeData

    public static IEnumerable<object[]> XorData =>
        new List<object[]>
    {
        new object[]
        {
            new double[,]
            {
                { 0, 0 },
                { 0, 1 },
                { 1, 0 },
                { 1, 1 }
            },
            new double[,]
            {
                { 0 },
                { 1 },
                { 1 },
                { 0 }
            }
        },
    };

    #endregion PremadeData

    #region XOR

    [Theory]
    [MemberData(nameof(XorData))]
    public void XorGate_ShouldReturnTrue
        (double[,] input, double [,] output)
    {
        var smth = input.GetRow(0).ToArray();
        double[,] xTest1 = new double[1, 2];

        Buffer.BlockCopy(
    smth, // src
    0, // srcOffset
    xTest1, // dst
    1 * xTest1.GetLength(1) * sizeof(int), // dstOffset
    smth.Length * sizeof(int));

        var xTest5 = input.GetColumn(0);
        double[,] xTest2 = { { 0, 1 } };
        double[,] xTest3 = { { 1, 0 } };
        double[,] xTest4 = { { 1, 1 } };

        Perceptron perceptron = new(input);

        perceptron.Start();
        perceptron.Train(input, output, 10000);

        double expected = output[0, 0];
        double actual = Math.Round(perceptron.Predict(xTest1)[0, 0]);

        Assert.Equal(expected, actual);

        expected = output[1, 0];
        actual = Math.Round(perceptron.Predict(xTest2)[0, 0]);

        Assert.Equal(expected, actual);

        expected = output[2, 0];
        actual = Math.Round(perceptron.Predict(xTest3)[0, 0]);

        Assert.Equal(expected, actual);

        expected = output[3, 0];
        actual = Math.Round(perceptron.Predict(xTest4)[0, 0]);

        Assert.Equal(expected, actual);
    }

    #endregion XOR
}
