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
        (int firstLayerLength, int secondLayerLength) = GetAllDimensionsLength(Input);

        _firstLayerWeights = new double[secondLayerLength, firstLayerLength];
        SetWeights(_firstLayerWeights, secondLayerLength, firstLayerLength);

        _secondLayerWeights = new double[firstLayerLength, 1];
        SetWeights(_secondLayerWeights, firstLayerLength, 1);
    }

    private void SetWeights(double[,] weights,
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
        (double[,] firstLayer, _) =
            ActivateSigmoid(test, _firstLayerWeights);
        (double[,] secondLayer, _) =
            ActivateSigmoid(firstLayer, _secondLayerWeights);

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
        
        for (var i = 0; i < trainDotWeights.Rows; i++)
        {
            for (var j = 0; j < trainDotWeights.Columns; j++)
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
        (int firstDimensionLength,
            int secondDimensionLength) = GetAllDimensionsLength(layer);
            
        sigmoidLayer ??= layer;

        for (int x = 0; x < firstDimensionLength; x++)
        {
            for (int y = 0; y < secondDimensionLength; y++)
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
            Matrix secondLayerError = CalculateError(yTrain,
                firstLayerAndSecondLayerWeights, secondLayer);

            Matrix secondLayerDelta = GetSecondLayerDeltas
                (secondLayer, secondLayerError);

            Matrix firstLayerDelta = GetFirstLayerDeltas(firstLayer,
                secondLayerDelta, _secondLayerWeights);

            // Adjusting the weights
            // Second Weights
            SetDeltas(firstLayer,
                secondLayerDelta, _secondLayerWeights);

            // First Weights
            SetDeltas(xTrain,
                firstLayerDelta, _firstLayerWeights);
        }
    }
    
    private static Matrix GetFirstLayerDeltas(double[,] layer,
        Matrix layerDelta, double[,] layerWeights)
    {
        Matrix layerWeightsMatrix = new(layerWeights);

        Matrix layerWeightsMatrixTransposed =
            layerWeightsMatrix.Transpose();
        
        double[,] layerDerivative = layer;
        ActivateSigmoidDerivative(layerDerivative, layer);

        Matrix layerDerivativeMatrix = new(layerDerivative);

        Matrix layerError = layerDelta.
            Multiply(layerWeightsMatrixTransposed);

        Matrix firstLayerDelta = layerDerivativeMatrix.
            Hadamard(layerError);

        return firstLayerDelta;
    }

    private static Matrix GetSecondLayerDeltas(double[,] secondLayer,
        Matrix secondLayerError)
    {
        ActivateSigmoidDerivative(secondLayer);

        Matrix secondLayerMatrix = new(secondLayer);
        Matrix secondLayerDelta = secondLayerMatrix.
            Hadamard(secondLayerError);

        return secondLayerDelta;
    }

    private static Matrix CalculateError(double[,] yTrain,
        Matrix dotProduct, double[,] layer)
    {
        double[,] layerError = dotProduct.Array;
        
        for (var i = 0; i < dotProduct.Rows; i++)
        {
            for (var j = 0; j < dotProduct.Columns; j++)
            {
                layerError[i, j] = yTrain[i, j] - layer[i, j];
            }
        }

        Matrix layerErrorMatrix = new(layerError);

        return layerErrorMatrix;
    }

    private static void SetDeltas(double[,] train,
        Matrix delta, double[,] layer)
    {
        var trainTransposed = new Matrix(train).Transpose();

        Matrix dotTrainTransposedAndDelta =
            trainTransposed.Multiply(delta);

        (int firstLastElement, int secondLastElement) =
            GetAllDimensionsLength(layer);

        for (var i = 0; i < firstLastElement; i++)
        {
            for (var j = 0; j < secondLastElement; j++)
            {
                layer[i, j] +=
                    dotTrainTransposedAndDelta[i, j];
            }
        }
    }

    private static (int, int) GetAllDimensionsLength(double[,] layer)
    {
        int firstDimensionLength = GetDimensionLength(layer, 0);
        int secondDimensionLength = GetDimensionLength(layer, 1);

        return (firstDimensionLength, secondDimensionLength);
    }

    private static int GetDimensionLength(double[,] array, int dimension)
    {
        return array.GetUpperBound(dimension) + 1;
    }

    #endregion Methods
}
