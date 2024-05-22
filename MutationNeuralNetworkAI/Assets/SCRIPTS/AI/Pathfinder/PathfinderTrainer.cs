using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace MutationNeuralNetworkAI
{
    [DisallowMultipleComponent]
    public sealed class PathfinderTrainer : Trainer
    {
        private int _failedBots;
        private PathfinderNN[] _neuralNetworks;
        private PathfinderBot[] _bots;

        [SerializeField] private GameObject _prefab;

        private void Start()
        {
            Launch();
        }

        protected override void Launch()
        {
            Time.timeScale = TimeScale;
            _failedBots = 0;
            InitNeuralNetworks();
            InitBots();
        }

        protected override void InitNeuralNetworks()
        {
            if (Population % 2 != 0) Population++;

            _neuralNetworks = new PathfinderNN[Population];
            var path = Path.Combine("Assets", LoadFromPath);

            if (File.Exists(path))
            {
                for (int i = 0; i < Population; i++)
                {
                    var nn = new PathfinderNN();
                    nn.Load(path);
                    _neuralNetworks[i] = nn;
                }
            }
            else
            {
                for (int i = 0; i < Population; i++)
                {
                    var nn = new PathfinderNN();
                    _neuralNetworks[i] = nn;
                }
            }
        }

        protected override void InitBots()
        {
            _bots = new PathfinderBot[Population];

            for (int i = 0; i < Population; i++)
            {
                _bots[i] = Instantiate(
                    _prefab, Vector3.zero, Quaternion.identity).GetComponent<PathfinderBot>();
                _bots[i].Init(_neuralNetworks[i], this);
            }
        }

        protected override void SortNeuralNetworks()
        {
            Array.Sort(_neuralNetworks);

            _neuralNetworks[^1].Save(Path.Combine("Assets", SaveToPath));

            var semiPopulation = Population / 2;

            for (int i = 0; i < semiPopulation; i++)
            {
                _neuralNetworks[i] = _neuralNetworks[i + semiPopulation].GetCopy();
                _neuralNetworks[i].Mutate(MutatationChance, MutationPower);
            }
        }

        protected override void ResetBots()
        {
            for (int i = 0;i < Population; i++)
            {
                _bots[i].SetNeuralNetwork(_neuralNetworks[i]);
                _bots[i].ResetBot();
            }
        }

        protected override void Restart()
        {
            if (IsRestarting) return;
            IsRestarting = true;
            StartCoroutine(CRT_Restart());
        }

        private IEnumerator CRT_Restart()
        {
            yield return new WaitForEndOfFrame();

            Evolutions++;
            AttemptTime = 0f;
            Time.timeScale = TimeScale;
            _failedBots = 0;

            SortNeuralNetworks();

            yield return new WaitForEndOfFrame();

            ResetRewardTriggers();
            ResetBots();

            GC.Collect();
            IsRestarting = false;
        }

        public override void IntformAbotFailure()
        {
            _failedBots++;
            if (_failedBots > Population / 2)
            {
                Restart();
            }
        }

        private void Update()
        {
            if (IsRestarting) return;

            for (int i = 0; i < Population; i++)
            {
                _bots[i].Operate();
            }

            AttemptTime += Time.deltaTime;
            if (AttemptTime > Duration)
            {
                Restart();
            }
        }

        
    }
}
