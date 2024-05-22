using System;
using System.Text;
using UnityEngine;

namespace MutationNeuralNetworkAI
{
    [Serializable]
    public abstract class Node
    {
        [SerializeField] protected string Id;
        [SerializeField] protected int OutputLayerIndex;
        [SerializeField] protected int WeightsOffset;
        [SerializeField] protected int BiasesOffset;

        [SerializeField] protected float[][] Data;

        public int InputLength => Data[0].Length;

        public int OutputLength => Data[OutputLayerIndex].Length;

        public abstract float[] GetOutput();

        protected void InitWeightsAndBiases()
        {
            var rnd = new System.Random();
            for (int i = OutputLayerIndex + 1; i < Data.Length; i++)
            {
                for (int j = 0; j < Data[i].Length; j++)
                {
                    Data[i][j] = (float)rnd.NextDouble() - 0.5f;
                }
            }
        }

        protected abstract void FeedForward();

        public void Mutate(float chance, float power)
        {
            var endWeightsIndex = OutputLayerIndex * 2 + 1;
            for (int i = OutputLayerIndex + 1; i < Data.Length; i++)
            {
                for (int j = 0; j < Data[i].Length; j++)
                {
                    if (UnityEngine.Random.value < chance)
                    {
                        Data[i][j] += UnityEngine.Random.Range(-power, power);
                    }

                    if (i > endWeightsIndex)
                    {
                        Data[i][j] = Mathf.Clamp(Data[i][j], -1f, 1f);
                    }
                }
            }
        }

        public void SetInput(float[] input)
        {
            Data[0] = input;
        }

        public void SetData(float[][] data)
        {
            Data = data;
        }

        public float[][] GetData()
        {
            var data = new float[Data.Length][];

            for (int i = 0; i < Data.Length; i++)
            {
                data[i] = new float[Data[i].Length];
                for (int j = 0; j < Data[i].Length; j++)
                {
                    data[i][j] = Data[i][j];
                }
            }

            return data;
        }

        public float[] GetLayer(int i)
        {
            return Data[i];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"NOA ID: {Id}; Array's length: {Data.Length}; OutputLayerIndex: {OutputLayerIndex}; ");
            sb.Append("CAPACITY: ");

            for (int i = 0; i < Data.Length; i++)
            {
                sb.Append($"{Data[i].Length}; ");
            }

            sb.AppendLine("OUTPUTS: ");

            for (int i = 0; i < Data[OutputLayerIndex].Length; i++)
            {
                sb.Append($"output_{i}: {Data[OutputLayerIndex][i]}; ");
            }

            sb.AppendLine("WEIGHTS & BIASES: ");

            for (int i = OutputLayerIndex + 1; i < Data.Length; i++)
            {
                for (int j = 0; j < Data[i].Length; j++)
                {
                    sb.Append($"{Data[i][j]}; ");
                }
            }

            return sb.ToString();
        }
    }
}
