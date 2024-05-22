using UnityEngine;

namespace MutationNeuralNetworkAI
{
    public sealed class RayCaster
    {
        private readonly Transform _transform;
        private readonly LayerMask _layerMask;
        private readonly int _rayCount;
        private readonly float _raycastDistance;
        private readonly float _angleStep;

        private Ray _ray;
        private float[] _raycastResult;
     
        public RayCaster(Transform transform, string raycastMask, 
            int rayCount, float raycastDistance)
        {
            _transform = transform;
            _layerMask = LayerMask.GetMask(raycastMask);
            _ray = new Ray();
            _rayCount = rayCount;
            _raycastResult = new float[_rayCount];
            _raycastDistance = raycastDistance;
            _angleStep = 360.0f / rayCount;
        }

        public RayCaster(Transform transform, string raycastMask,
            float raycastDistance)
        {
            _transform = transform;
            _layerMask = LayerMask.GetMask(raycastMask);
            _ray = new Ray();
            _rayCount = 11;
            _raycastResult = new float[_rayCount];
            _raycastDistance = raycastDistance;
            _angleStep = 180f / (_rayCount - 1);
        }

        public float[] GetRoundRaycastResultAxisY(bool showDebug = false)
        {
            var position = _transform.position;    
            var forward = _transform.forward;
            
            for (int i = 0; i < _rayCount; i++)
            {
                _raycastResult[i] = GetRaycastResult(
                    position, 
                    Quaternion.Euler(0f, i * _angleStep, 0f) * forward, 
                    showDebug);
            }

            return _raycastResult;
        }

        public float[] GetSemiRoundRaycastResultAxisY(bool showDebug = false)
        {
            var position = _transform.position;
            var forward = _transform.forward;
            var startY = -90f;
            for (int i = 0; i < _rayCount; i++)
            {
                _raycastResult[i] = GetRaycastResult(
                    position,
                    Quaternion.Euler(0f, startY + i * _angleStep, 0f) * forward,
                    showDebug);
            }
            return _raycastResult;
        }

        private float GetRaycastResult(Vector3 position, Vector3 direction, bool showDebug = false)
        {
            _ray.origin = position;
            _ray.direction = direction;

            if (Physics.Raycast(_ray, out RaycastHit hit, _raycastDistance, _layerMask))
            {
                if (showDebug)
                {
                    Debug.DrawRay(position, hit.distance * direction, Color.red);
                }

                return 1f - hit.distance / _raycastDistance;
            }

            if (showDebug )
            {
                Debug.DrawRay(position, _raycastDistance * direction, Color.red);
            }

            return 0f;
        }
    }
}
