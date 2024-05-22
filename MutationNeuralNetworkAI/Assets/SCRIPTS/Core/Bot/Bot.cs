using UnityEngine;

namespace MutationNeuralNetworkAI
{
    public abstract class Bot : MonoBehaviour
    {
        public abstract void AddFitness(float fitness);
    }
}
