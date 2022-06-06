using SML.Matrices;

namespace SML.Perceptrons;

public class Perceptron
{
    #region Fields

    public double[,] Input { get; set; }
    public int Iterations { get; set; } = 10000;

    private readonly double _bias = 0.03;

    private readonly Random _random = new();

    private double[,] _firstLayerWeights = new double[0, 0];
    private double[,] _secondLayerWeights = new double[0, 0];

    #endregion Fields

    #region Constructors

    public Perceptron(double[,] input)
    {
        Input = input;
    }

    #endregion Constructors

    #region Methods

    public void Start()
    {
        GenerateWeights();
    }

    private void GenerateWeights()
    {
        // During the training phase, the network is trained by adjusting
        // these weights to be able to predict the correct class for the input.

        int firstLayerLen = Input.GetUpperBound(0) + 1;
        int secondLayerLen = Input.GetUpperBound(1) + 1;

        _firstLayerWeights = new double[secondLayerLen, firstLayerLen];
        AssignWeights(_firstLayerWeights, secondLayerLen, firstLayerLen);

        _secondLayerWeights = new double[firstLayerLen, 1];
        AssignWeights(_secondLayerWeights, firstLayerLen, 1);
    }

    private void AssignWeights(double[,] weights,
        int firstLayerLength, int secondLayerLength)
    {
        for (int i = 0; i < firstLayerLength; i++)
        {
            for (int j = 0; j < secondLayerLength; j++)
            {
                weights[i, j] = GetRandomDouble();
            }
        }
    }

    private double GetRandomDouble()
    {
        return _random.NextDouble();
    }

    private static double Sigmoid(double x)
    {
        return 1 / (1 + (float)Math.Exp(-x));
    }

    private static double SigmoidDerivative(double x)
    {
        return Sigmoid(x) * (1 - Sigmoid(x));
    }

    public double[,] Predict(double[,] xTest)
    {
        double[,] firstLayer = ActivateSigmoid(xTest, _firstLayerWeights);

        double[,] secondLayer = ActivateSigmoid(firstLayer, _secondLayerWeights);

        return secondLayer;
    }

    private static double[,] ActivateSigmoid(double[,] testValues,
        double[,] layerWeights)
    {
        Matrix testValuesMatrix = new(testValues);
        Matrix layerWeightsMatrix = new(layerWeights);

        Matrix testValuesDotLayerWeights = testValuesMatrix
            .Multiply(layerWeightsMatrix);

        double[,] layer = testValuesDotLayerWeights.Array;

        for (int i = 0; i < testValuesDotLayerWeights.Rows; i++)
        {
            for (int j = 0; j < testValuesDotLayerWeights.Columns; j++)
            {
                layer[i, j] = Sigmoid(layer[i, j]);
            }
        }

        return layer;
    }

    private static void ActivateSigmoidDerivative(int firstElement,
        int secondElement, double[,] layer, double[,] layerInSigmoid = null)
    {
        layerInSigmoid ??= layer;

        for (int x = 0; x < firstElement; x++)
        {
            for (int y = 0; y < secondElement; y++)
            {
                layer[x, y] = SigmoidDerivative(layerInSigmoid[x, y]);
            }
        }
    }

    public void Train(double[,] xTrain, double[,] yTrain, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            Matrix xTrainMatrix = new(xTrain);

            Matrix firstLayerWeightsMatrix = new(_firstLayerWeights);

            Matrix dotXTrainAndFirstLayerWeigth = xTrainMatrix
                .Multiply(firstLayerWeightsMatrix);

            double[,] firstLayer = dotXTrainAndFirstLayerWeigth.Array;

            // Adjusting with bias and activating training

            for (int x = 0; x < dotXTrainAndFirstLayerWeigth.Rows; x++)
            {
                for (int y = 0; y < dotXTrainAndFirstLayerWeigth.Columns; y++)
                {
                    firstLayer[x, y] = Sigmoid(firstLayer[x, y]);
                    firstLayer[x, y] += _bias;
                }
            }

            Matrix firstLayerMatrix = new(firstLayer);

            Matrix secondLayerWeightsMatrix = new(_secondLayerWeights);

            Matrix dotFirstLayerAndSecondLayerWeights =
                firstLayerMatrix.Multiply(secondLayerWeightsMatrix);

            double[,] secondLayer = dotFirstLayerAndSecondLayerWeights.Array;

            for (int x = 0; x < dotFirstLayerAndSecondLayerWeights.Rows; x++)
            {
                for (int y = 0; y < dotFirstLayerAndSecondLayerWeights.Columns; y++)
                {
                    secondLayer[x, y] = Sigmoid(secondLayer[x, y]);
                }
            }

            // Calculate the prediction error

            double[,] secondLayerError = dotFirstLayerAndSecondLayerWeights.Array;

            for (int x = 0; x < dotFirstLayerAndSecondLayerWeights.Rows; x++)
            {
                for (int y = 0; y < dotFirstLayerAndSecondLayerWeights.Columns; y++)
                {
                    secondLayerError[x, y] = yTrain[x, y] - secondLayer[x, y];
                }
            }

            Matrix secondLayerErrorMatrix = new(secondLayerError);

            int firstLastElement = GetLastElementOfDimension(secondLayer, 0);
            int secondLastElement = GetLastElementOfDimension(secondLayer, 1);

            ActivateSigmoidDerivative(firstLastElement,
                secondLastElement, secondLayer);

            Matrix secondLayerMatrix = new(secondLayer);

            Matrix secondLayerDeltaMatrix = secondLayerMatrix.
                Hadamard(secondLayerErrorMatrix);

            Matrix secondLayerWeightsMatrixTransposed =
                secondLayerWeightsMatrix.Transpose();

            Matrix firstLayerErrorMatrix = secondLayerDeltaMatrix.
                Multiply(secondLayerWeightsMatrixTransposed);

            double[,] firstLayerDerivative = firstLayer;
            
            firstLastElement = GetLastElementOfDimension(firstLayerDerivative, 0);
            secondLastElement = GetLastElementOfDimension(firstLayerDerivative, 1);

            ActivateSigmoidDerivative(firstLastElement,
                secondLastElement, firstLayerDerivative, firstLayer);

            Matrix firstLayerDerivativeMatrix = new(firstLayerDerivative);

            Matrix firstLayerDeltaMatrix = firstLayerDerivativeMatrix.
                Hadamard(firstLayerErrorMatrix);

            // Adjusting the weights
            // Second Weights

            Matrix dotFirstLayerAndSecondLayerDelta = firstLayerMatrix.Transpose()
                .Multiply(secondLayerDeltaMatrix);

            firstLastElement = GetLastElementOfDimension(_secondLayerWeights, 0);
            secondLastElement = GetLastElementOfDimension(_secondLayerWeights, 1);

            for (int x = 0; x < firstLastElement; x++)
            {
                for (int y = 0; y < secondLastElement; y++)
                {
                    _secondLayerWeights[x, y] += dotFirstLayerAndSecondLayerDelta[x, y];
                }
            }

            // First Weights

            Matrix xTrainTransposedMatrix = xTrainMatrix.Transpose();

            Matrix dotXTrainTransposedAndFirstLayerDeltaMatrix =
                xTrainTransposedMatrix.Multiply(firstLayerDeltaMatrix);

            firstLastElement = GetLastElementOfDimension(_firstLayerWeights, 0);
            secondLastElement = GetLastElementOfDimension(_firstLayerWeights, 1);

            for (int x = 0; x < firstLastElement; x++)
            {
                for (int y = 0; y < secondLastElement; y++)
                {
                    _firstLayerWeights[x, y] +=
                        dotXTrainTransposedAndFirstLayerDeltaMatrix[x, y];
                }
            }
        }
    }

    private static int GetLastElementOfDimension(double[,] array, int dimension)
    {
        return array.GetUpperBound(dimension) + 1;
    }

    #endregion Methods
}
