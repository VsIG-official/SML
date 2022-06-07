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

        (int firstLayerLen, int secondLayerLen) = GetLastElements(Input);

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

    private double[,] ActivateSigmoid(double[,] testValues,
        double[,] layerWeights, bool adjustBias = false)
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
                
                if (adjustBias)
                {
                    layer[i, j] += _bias;
                }
            }
        }

        return layer;
    }

    private static void ActivateSigmoidDerivative(int firstElement,
        int secondElement, double[,] layer, double[,] layerForSigmoid = null)
    {
        layerForSigmoid ??= layer;

        for (int x = 0; x < firstElement; x++)
        {
            for (int y = 0; y < secondElement; y++)
            {
                layer[x, y] = SigmoidDerivative(layerForSigmoid[x, y]);
            }
        }
    }

    public void Train(double[,] xTrain, double[,] yTrain, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            double[,] firstLayer = ActivateSigmoid(xTrain,
                _firstLayerWeights, true);

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

            (int firstLastElement, int secondLastElement) =
                GetLastElements(secondLayer);

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

            (firstLastElement, secondLastElement) =
                GetLastElements(firstLayerDerivative);

            ActivateSigmoidDerivative(firstLastElement,
                secondLastElement, firstLayerDerivative, firstLayer);

            Matrix firstLayerDerivativeMatrix = new(firstLayerDerivative);

            Matrix firstLayerDeltaMatrix = firstLayerDerivativeMatrix.
                Hadamard(firstLayerErrorMatrix);

            // Adjusting the weights
            // Second Weights

            Matrix dotFirstLayerAndSecondLayerDelta = firstLayerMatrix.Transpose()
                .Multiply(secondLayerDeltaMatrix);

            (firstLastElement, secondLastElement) =
                GetLastElements(_secondLayerWeights);

            for (int x = 0; x < firstLastElement; x++)
            {
                for (int y = 0; y < secondLastElement; y++)
                {
                    _secondLayerWeights[x, y] += dotFirstLayerAndSecondLayerDelta[x, y];
                }
            }

            // First Weights
            
            Matrix xTrainMatrix = new(xTrain);

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

    private static (int, int) GetLastElements(double[,] layer)
    {
        int firstElement = GetLastElementOfDimension(layer, 0);
        int secondElement = GetLastElementOfDimension(layer, 1);

        return (firstElement, secondElement);
    }

    private static int GetLastElementOfDimension(double[,] array, int dimension)
    {
        return array.GetUpperBound(dimension) + 1;
    }

    #endregion Methods
}
