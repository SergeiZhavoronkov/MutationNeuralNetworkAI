using System.Collections.Generic;
using UnityEngine;

namespace MutationNeuralNetworkAI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public sealed class RewardTrigger : MonoBehaviour
    {
        private const string _botTag = "Bot";
        [SerializeField] private float _reward;

        [SerializeField] private List<int> _botHashes;

        private void Start()
        {
            _botHashes = new List<int>();
        }

        public void Reset()
        {
            _botHashes.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_botTag))
            {
                if (other.TryGetComponent(out PathfinderBot bot))
                {
                    var hc = bot.GetHashCode();
                    if (!_botHashes.Contains(hc))
                    {
                        bot.AddFitness(_reward);
                        _botHashes.Add(hc);
                    }
                    
                }
            }
        }
    }
}
