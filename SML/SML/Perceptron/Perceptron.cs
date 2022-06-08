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

    public double[,] Predict(double[,] test)
    {
        (double[,] firstLayer, _) = ActivateSigmoid(test, _firstLayerWeights);
        (double[,] secondLayer, _) = ActivateSigmoid(firstLayer, _secondLayerWeights);

        return secondLayer;
    }

    private (double[,], Matrix) ActivateSigmoid(double[,] train,
        double[,] layerWeights, bool adjustBias = false)
    {
        Matrix trainMatrix = new(train);
        Matrix weights = new(layerWeights);

        Matrix trainDotWeights = trainMatrix
            .Multiply(weights);

        double[,] layer = trainDotWeights.Array;

        for (int i = 0; i < trainDotWeights.Rows; i++)
        {
            for (int j = 0; j < trainDotWeights.Columns; j++)
            {
                layer[i, j] = Sigmoid(layer[i, j]);

                if (adjustBias)
                {
                    layer[i, j] += _bias;
                }
            }
        }

        return (layer, trainDotWeights);
    }

    private static void ActivateSigmoidDerivative(double[,] layer,
        double[,] sigmoidLayer = null)
    {
        (int firstElement, int secondElement) = GetLastElements(layer);
            
        sigmoidLayer ??= layer;

        for (int x = 0; x < firstElement; x++)
        {
            for (int y = 0; y < secondElement; y++)
            {
                layer[x, y] = SigmoidDerivative(sigmoidLayer[x, y]);
            }
        }
    }

    public void Train(double[,] xTrain, double[,] yTrain, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            (double[,] firstLayer, _) = ActivateSigmoid(xTrain,
                _firstLayerWeights, true);

            (double[,] secondLayer, Matrix firstLayerAndSecondLayerWeights) =
                ActivateSigmoid(firstLayer, _secondLayerWeights);
            
            // Calculate the prediction error
            Matrix secondLayerErrorMatrix = CalculateError(yTrain,
                firstLayerAndSecondLayerWeights, secondLayer);

            ActivateSigmoidDerivative(secondLayer);

            Matrix secondLayerMatrix = new(secondLayer);

            Matrix secondLayerDeltaMatrix = secondLayerMatrix.
                Hadamard(secondLayerErrorMatrix);

            Matrix firstLayerDelta = GetFirstLayerDelta(firstLayer,
                secondLayerDeltaMatrix, _secondLayerWeights);

            // Adjusting the weights
            // Second Weights
            ApplyDeltaValues(firstLayer,
                secondLayerDeltaMatrix, _secondLayerWeights);

            // First Weights
            ApplyDeltaValues(xTrain,
                firstLayerDelta, _firstLayerWeights);
        }
    }
    
    private static Matrix GetFirstLayerDelta(double[,] firstLayer,
        Matrix secondLayerDelta, double[,] layerWeights)
    {
        Matrix secondLayerWeightsMatrix = new(layerWeights);

        Matrix secondLayerWeightsMatrixTransposed =
            secondLayerWeightsMatrix.Transpose();
        
        double[,] firstLayerDerivative = firstLayer;
        ActivateSigmoidDerivative(firstLayerDerivative, firstLayer);

        Matrix firstLayerDerivativeMatrix = new(firstLayerDerivative);

        Matrix firstLayerError = secondLayerDelta.
            Multiply(secondLayerWeightsMatrixTransposed);

        Matrix firstLayerDelta = firstLayerDerivativeMatrix.
            Hadamard(firstLayerError);

        return firstLayerDelta;
    }

    private static Matrix CalculateError(double[,] yTrain,
        Matrix dotProduct, double[,] layer)
    {
        double[,] secondLayerError = dotProduct.Array;

        for (int x = 0; x < dotProduct.Rows; x++)
        {
            for (int y = 0; y < dotProduct.Columns; y++)
            {
                secondLayerError[x, y] = yTrain[x, y] - layer[x, y];
            }
        }

        Matrix secondLayerErrorMatrix = new(secondLayerError);

        return secondLayerErrorMatrix;
    }

    private static void ApplyDeltaValues(double[,] train,
        Matrix delta, double[,] layer)
    {
        var trainTransposed = new Matrix(train).Transpose();

        Matrix dotTrainTransposedAndDelta =
            trainTransposed.Multiply(delta);

        (int firstLastElement, int secondLastElement) =
            GetLastElements(layer);

        for (var i = 0; i < firstLastElement; i++)
        {
            for (var j = 0; j < secondLastElement; j++)
            {
                layer[i, j] +=
                    dotTrainTransposedAndDelta[i, j];
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
