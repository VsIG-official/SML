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
            new double[][]
            {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            },
            new double[,]
            {
                { 0 },
                { 1 },
                { 1 },
                { 0 }
            },
        },
    };

    #endregion PremadeData

    #region XOR

    [Theory]
    [MemberData(nameof(XorData))]
    public void XorGate_ShouldReturnTrue
        (double[][] separateInputValues, double [,] output)
    {
        double[,] input = separateInputValues.To2D();
        Perceptron perceptron = new(input);

        perceptron.Start();
        perceptron.Train(input, output, 10000);

        double[,] xTest = new double[1, 2];

        for (int i = 0; i < 4; i++)
        {
            for (int x = 0; x < 2; x++)
            {
                xTest[0, x] = separateInputValues[i][x];
            }

            double expected = output[i, 0];
            double actual = Math.Round(perceptron.Predict(xTest)[0, 0]);

            Assert.Equal(expected, actual);
        }
    }

    #endregion XOR
}
