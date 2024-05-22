using UnityEngine;

namespace MutationNeuralNetworkAI
{
    public abstract class Bot : MonoBehaviour
    {
        protected Trainer Trainer;
        protected Transform Transform;
        protected Vector3 OriginPosition;

        protected bool IsOperating = true;

        [SerializeField] protected bool ShowDebug;

        public abstract void AddFitness(float fitness);
    }
}
