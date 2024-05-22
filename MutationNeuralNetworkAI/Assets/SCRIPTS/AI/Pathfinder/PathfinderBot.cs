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
        private Trainer _trainer;
        private Transform _transform;
        private Vector3 _originPosition;
        private RayCaster _roundRayCaster;
        private float[] _raycastResult;
        private float[][] _output;


        [Header("SETTINGS")]
        [SerializeField][Range(1f, 16f)] private float _raycastDistance;
        [SerializeField][Range(1f, 90f)] private float _rotationSpeed;
        [SerializeField][Range(1f, 5f)] private float _moveSpeed;
        [SerializeField] private bool _showRaycast;

        [Header("DEBUG")]
        [SerializeField] private int _rayCount;

        private float _speed;


        private bool _isOperating = true;

        public void Init(PathfinderNN neuralNetwork, Trainer trainer)
        {
            _neuralNetwork = neuralNetwork;
            _trainer = trainer;
            _transform = GetComponent<Transform>();
            _originPosition = _transform.position;
            _rayCount = _neuralNetwork.InputLength;

            _roundRayCaster = new RayCaster(_transform,
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
            _transform.SetPositionAndRotation(
                _originPosition, Quaternion.identity);
            
            _isOperating = true;
        }

        public void Operate()
        {
            if (!_isOperating) return;

            var dt = Time.deltaTime;

            _raycastResult = _roundRayCaster.GetSemiRoundRaycastResultAxisY(_showRaycast);

            _neuralNetwork.SetInput(_raycastResult);
            _output = _neuralNetwork.GetOutput();

            _speed = _moveSpeed * _output[0][0];
            var rotationY = _rotationSpeed * (_output[1][0] + _output[1][1]);

            _transform.SetPositionAndRotation(
                _transform.position + dt * _speed * _transform.forward,
                _transform.rotation *= Quaternion.Euler(0f, dt * rotationY, 0f));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isOperating) return;

            if (!other.CompareTag(_rewardTag))
            {
                _isOperating = false;
                AddFitness(-0.5f);
                _trainer.IntformAbotFailure();
            }
        }
    }
}
