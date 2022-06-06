using System;
using System.Collections.Generic;
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
        (double[][] separatedInputValues, double [,] output)
    {
        // Arrange
        int iterations = 10000;
        double[,] input = separatedInputValues.To2D();
        Perceptron perceptron = new(input);

        perceptron.Start();
        perceptron.Train(input, output, iterations);

        double[,] xTest = new double[1, 2];

        for (int i = 0; i < input.GetLowerBound(0); i++)
        {
            for (int j = 0; j < input.GetLowerBound(1); j++)
            {
                xTest[0, j] = separatedInputValues[i][j];
            }

            // Act
            double expected = output[i, 0];
            double actual = Math.Round(perceptron.Predict(xTest)[0, 0]);

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    #endregion XOR
}
