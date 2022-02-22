using System;
using SML.Perceptrons;
using Xunit;

namespace SML.Tests;

public class PerceptronTests
{
    [Fact]
    public void XorGate_ShouldReturnTrue()
    {
        double[,] input = new double[,]
        {
            { 0, 0 },
            { 0, 1 },
            { 1, 0 },
            { 1, 1 }
        };

        double[,] output =
        {
            { 0 },
            { 1 },
            { 1 },
            { 0 }
        };

        double[,] xTest1 = { { 0, 0 } };
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
}
