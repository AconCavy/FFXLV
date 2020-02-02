using UnityEngine;

namespace FFXLV
{
    public class TutorialLayerBehaviour : LayerBehaviour
    {
        [SerializeField] private GameObject logo;
        [SerializeField] private Vector3 firstPosition;
        public bool IsMoved { get; private set; }

        public void Move(float deltaTime)
        {
            if (logo.activeSelf)
            {
                logo.SetActive(false);
            }
            
            transform.position += (firstPosition - transform.position) * deltaTime;
            if (Vector3.Distance(transform.position, firstPosition) > 0.125f) return;
            IsMoved = true;
        }
        public void Initialize(float bestAngle, float angularMagnitude, float bestDistance, float distanceMagnitude)
        {
            Initialize();
            ChangeState(State.None);
            transform.position = firstPosition;
            handBehaviour.Initialize(bestAngle, angularMagnitude, bestDistance, distanceMagnitude, 50);
        }
    }
}