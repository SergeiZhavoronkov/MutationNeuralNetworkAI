using System;

namespace MutationNeuralNetworkAI
{
    [Serializable]
    public sealed class InputNode : Node
    {
        public InputNode(string id, params int[] layers)
        {
            Id = id;
            OutputLayerIndex = layers.Length - 1;
            WeightsOffset = OutputLayerIndex;
            BiasesOffset = OutputLayerIndex * 2;

            Data = new float[1 + OutputLayerIndex * 3][];

            for (int i = 0; i < layers.Length; i++)
            {
                Data[i] = new float[layers[i]];

                if (i > 0)
                {
                    Data[i + WeightsOffset] = new float[
                        Data[i - 1].Length * Data[i].Length];

                    Data[i + BiasesOffset] = new float[Data[i].Length];
                }
            }

            InitWeightsAndBiases();
        }

        public override float[] GetOutput()
        {
            FeedForward();
            return Data[OutputLayerIndex];
        }

        protected override void FeedForward()
        {
            for (int i = 1; i <= OutputLayerIndex; i++)
            {
                var inputArray = Data[i - 1];
                var weightsArray = Data[i + WeightsOffset];
                var biasArray = Data[i + BiasesOffset];

                for (int j = 0; j < Data[i].Length; j++)
                {
                    var sum = 0f;
                    var step = weightsArray.Length / Data[i].Length;

                    for (int k = 0; k < inputArray.Length; k++)
                    {
                        sum += inputArray[k] * weightsArray[k + step * j];
                    }
                    Data[i][j] = MathF.Tanh(sum + biasArray[j]);
                }
            }
        }
    }
}
