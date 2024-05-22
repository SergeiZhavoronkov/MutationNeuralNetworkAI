using System;
using UnityEngine;

namespace MutationNeuralNetworkAI
{
    [Serializable]
    public sealed class HiddenNode : Node
    {
        [SerializeField] private readonly Node[] _inputNodes;
        [SerializeField] private readonly int _inputNodesOutputLength;

        public HiddenNode(string id, Node[] inputNodes,
            params int[] layers)
        {
            Id = id;
            OutputLayerIndex = layers.Length - 1;
            WeightsOffset = layers.Length;
            BiasesOffset = layers.Length * 2;
            _inputNodes = inputNodes;

            Data = new float[layers.Length * 3][];

            for (int i = 0; i < layers.Length; i++)
            {
                Data[i] = new float[layers[i]];

                if (i.Equals(0))
                {
                    _inputNodesOutputLength = 0;
                    for (int j = 0; j < _inputNodes.Length; j++)
                    {
                        _inputNodesOutputLength += _inputNodes[j].OutputLength;
                    }

                    Data[i + WeightsOffset] = new float[
                        _inputNodesOutputLength * Data[i].Length];
                }
                else
                {
                    Data[i + WeightsOffset] = new float[
                        Data[i - 1].Length * Data[i].Length];
                }
                Data[i + BiasesOffset] = new float[Data[i].Length];
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
            var inputArray = new float[_inputNodesOutputLength];
            var count = 0;

            int i;

            for (i = 0; i < _inputNodes.Length; i++)
            {
                var output = _inputNodes[i].GetOutput();
                for (int j = 0; j < output.Length; j++)
                {
                    inputArray[count] = output[j];
                    count++;
                }
            }

            var weightsArray = Data[WeightsOffset];
            var biasArray = Data[BiasesOffset];
            var step = weightsArray.Length / Data[0].Length;

            for (i = 0; i < Data[0].Length; i++)
            {
                var sum = 0f;
                for (int k = 0; k < inputArray.Length; k++)
                {
                    sum += inputArray[k] * weightsArray[k + step * i];
                }
                Data[0][i] = MathF.Tanh(sum + biasArray[i]);
            }

            for (i = 1; i <= OutputLayerIndex; i++)
            {
                inputArray = Data[i - 1];
                weightsArray = Data[i + WeightsOffset];
                biasArray = Data[i + BiasesOffset];

                step = weightsArray.Length / Data[i].Length;

                for (int j = 0; j < Data[i].Length; j++)
                {
                    var sum = 0f;
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
