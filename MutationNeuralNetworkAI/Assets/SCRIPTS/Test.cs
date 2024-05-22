using UnityEngine;

namespace MutationNeuralNetworkAI
{
    public class Test : MonoBehaviour
    {
        private const int _rayCount = 8;
        private RayCaster _roundRayCaster;

        public float[] RaycastResult;
        
        void Start()
        {
            _roundRayCaster = new RayCaster(GetComponent<Transform>(),
                "Default", 10f);

            RaycastResult = new float[_rayCount];
        }

        void Update()
        {
            RaycastResult = _roundRayCaster.GetSemiRoundRaycastResultAxisY(true);
        }
    }
}
