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
            },
        },
    };

    #endregion PremadeData

    #region XOR

    [Theory]
    [MemberData(nameof(XorData))]
    public void XorGate_ShouldReturnTrue
        (double[,] input, double [,] output)
    {
        Perceptron perceptron = new(input);

        perceptron.Start();
        perceptron.Train(input, output, 10000);

        for (int i = 0; i < 4; i++)
        {
            double[] row = input.GetRow(i).ToArray();
            double[,] xTest = new double[1, 2];
            xTest[0, 0] = row[0];
            xTest[0, 1] = row[1];

            //Buffer.BlockCopy(
            //    row, // src
            //    0, // srcOffset
            //    xTest, // dst
            //    1 * xTest.GetLength(1) * sizeof(double), // dstOffset
            //    row.Length * sizeof(double));

            double expected = output[i, 0];
            double actual = Math.Round(perceptron.Predict(xTest)[0, 0]);

            Assert.Equal(expected, actual);
        }
    }

    #endregion XOR
}
