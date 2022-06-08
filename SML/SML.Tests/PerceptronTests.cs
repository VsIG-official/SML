using System;
using System.Collections.Generic;
using SML.ExtensionMethods;
using SML.Perceptrons;
using Xunit;

namespace SML.Tests.PerceptronTests;

public class PerceptronTests
{
    #region PremadeData

    public static IEnumerable<object[]> XorGateData =>
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

    public static IEnumerable<object[]> AndGateData =>
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
                { 0 },
                { 0 },
                { 1 }
            },
        },
    };

    public static IEnumerable<object[]> OrGateData =>
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
                { 1 }
            },
        },
    };

    public static IEnumerable<object[]> NotGateData =>
    new List<object[]>
    {
        new object[]
        {
            new double[][]
            {
                new double[] { 0 },
                new double[] { 1 },
            },
            new double[,]
            {
                { 1 },
                { 0 },
            },
        },
    };

    #endregion PremadeData

    #region LogicGates

    [Theory]
    [MemberData(nameof(XorGateData))]
    [MemberData(nameof(AndGateData))]
    [MemberData(nameof(OrGateData))]
    [MemberData(nameof(NotGateData))]
    public void LogicGate_True
        (double[][] separatedInputValues, double[,] output)
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

    #endregion LogicGates
}
