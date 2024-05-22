using System;

namespace MutationNeuralNetworkAI
{
    [Serializable]
    public sealed class PathfinderNN : NeuralNetwork<float[], float[][], PathfinderNN>
    {
        public int InputLength => Nodes[0].InputLength;

        public PathfinderNN() 
        {
            Nodes = new Node[3];

            Nodes[0] = new InputNode("input",
                    11, 16, 15);

            Nodes[1] = new HiddenNode("outputSpeed",
                    new Node[] { Nodes[0] }, 8, 1);

            Nodes[2] = new HiddenNode("outputLookRotation",
                    new Node[] { Nodes[0] }, 14, 12, 2);
            
        }

        public override PathfinderNN GetCopy()
        {
            var copy = new PathfinderNN();
            
            for (int i = 0; i < Nodes.Length; i++)
            {
                copy.SetData(i, Nodes[i].GetData());
            }
            return copy;
        }

        public override void SetInput(float[] input)
        {
            Nodes[0].SetInput(input);
        }

        public override float[][] GetOutput()
        {
            var output = new float[2][];
            output[0] = Nodes[1].GetOutput();
            output[1] = Nodes[2].GetOutput();
            return output;
        }

        
    }
}
