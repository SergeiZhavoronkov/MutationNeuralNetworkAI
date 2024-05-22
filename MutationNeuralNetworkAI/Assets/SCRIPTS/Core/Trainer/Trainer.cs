using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MutationNeuralNetworkAI
{
    public abstract class Trainer : MonoBehaviour
    {
        [Header("TRAINING SETTINGS")]
        [SerializeField][Range(2, 1000)] protected int Population;
        [SerializeField][Range(5f, 60f)] protected float Duration;
        [SerializeField][Range(0f, 1f)] protected float MutatationChance;
        [SerializeField][Range(0f, 1f)] protected float MutationPower;
        [SerializeField][Range(1f, 10f)] protected float TimeScale;
        [SerializeField] private RewardTrigger[] _rewardTriggers;

        [Header("SAVE/LOAD PRE-TRAINED")]
        [SerializeField] protected string SaveToPath;
        [SerializeField] protected string LoadFromPath;

        [Header("TRAINING SESSION INFO")]
        [SerializeField] protected int Evolutions;
        [SerializeField] protected float AttemptTime;

        protected bool IsRestarting;

        protected abstract void Launch();

        protected abstract void InitNeuralNetworks();

        protected abstract void InitBots();

        protected abstract void SortNeuralNetworks();

        protected abstract void ResetBots();

        protected abstract void Restart();

        protected void ResetRewardTriggers()
        {
            if (_rewardTriggers == null) return;

            for (int i = 0; i < _rewardTriggers.Length; i++)
            {
                _rewardTriggers[i].Reset();
            }
        }

        public abstract void IntformAbotFailure();
    }
        
}
