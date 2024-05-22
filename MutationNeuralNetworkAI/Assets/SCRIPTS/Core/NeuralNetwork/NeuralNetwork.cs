using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MutationNeuralNetworkAI
{
    [Serializable]
    public abstract class NeuralNetwork<I, O, N> : IComparable<NeuralNetwork<I, O, N>>
    {
        [SerializeField] protected Node[] Nodes;

        public float Fitness { get; protected set; }

        public abstract N GetCopy();

        public abstract void SetInput(I input);

        public abstract O GetOutput();

        public void SetData(int nodeIndex, float[][] data)
        {
            Nodes[nodeIndex].SetData(data);
        }

        public void Save(string path)
        {
            var bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                bf.Serialize(fs, Nodes);
            }
        }

        public void Load(string path)
        {
            var bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Nodes = (Node[])bf.Deserialize(fs);
            }
        }

        public void Mutate(float chance, float power)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Mutate(chance, power);
            }
        }

        public void Addfitness(float fitness)
        {
            Fitness += fitness;
        }

        public void ResetFitness()
        {
            Fitness = 0f;
        }

        public int CompareTo(NeuralNetwork<I, O, N> other)
        {
            if (other == null) return 1;

            if (Fitness > other.Fitness) return 1;
            else if (Fitness < other.Fitness) return -1;
            else return 0;
        }
    }
}
