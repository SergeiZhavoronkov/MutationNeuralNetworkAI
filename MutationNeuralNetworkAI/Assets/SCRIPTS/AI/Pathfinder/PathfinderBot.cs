using System;
using UnityEngine;

namespace MutationNeuralNetworkAI
{
    [DisallowMultipleComponent]
    public sealed class PathfinderBot : Bot
    {
        private const string _raycastMaskName = "Default";
        private const string _rewardTag = "Reward";

        private PathfinderNN _neuralNetwork;
        private RayCaster _rayCaster;
        private float[] _raycastResult;
        private float[][] _output;

        private float _speed;
        private float _rotationY;

        [Header("SETTINGS")]
        [SerializeField][Range(1f, 16f)] private float _raycastDistance;
        [SerializeField][Range(1f, 90f)] private float _rotationSpeed;
        [SerializeField][Range(1f, 5f)] private float _moveSpeed;
       
        public void Init(PathfinderNN neuralNetwork, Trainer trainer)
        {
            _neuralNetwork = neuralNetwork;
            Trainer = trainer;
            Transform = GetComponent<Transform>();
            OriginPosition = Transform.position;

            _rayCaster = new RayCaster(Transform,
                _raycastMaskName, _raycastDistance);
        }

        public void SetNeuralNetwork(PathfinderNN neuralNetwork)
        {
            _neuralNetwork = neuralNetwork;
        }

        public override void AddFitness(float fitness)
        {
            _neuralNetwork.Addfitness(fitness);
        }

        public void ResetBot()
        {
            _neuralNetwork.ResetFitness();
            Transform.SetPositionAndRotation(
                OriginPosition, Quaternion.identity);
            
            IsOperating = true;
        }

        public void Operate()
        {
            if (!IsOperating) return;

            var dt = Time.deltaTime;

            _raycastResult = _rayCaster.GetSemiRoundRaycastResultAxisY(ShowDebug);

            _neuralNetwork.SetInput(_raycastResult);
            _output = _neuralNetwork.GetOutput();

            _speed = _moveSpeed * _output[0][0];
            _rotationY = _rotationSpeed * (_output[1][0] + _output[1][1]);

            Transform.SetPositionAndRotation(
                Transform.position + dt * _speed * Transform.forward,
                Transform.rotation *= Quaternion.Euler(0f, dt * _rotationY, 0f));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsOperating) return;

            if (!other.CompareTag(_rewardTag))
            {
                IsOperating = false;
                AddFitness(-0.5f);
                Trainer.IntformAbotFailure();
            }
        }
    }
}
