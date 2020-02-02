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

            var position = transform.position;
            position += (firstPosition - position) * (5 * deltaTime);
            transform.position = position;
            if (Vector3.Distance(transform.position, firstPosition) > 0.125f) return;
            IsMoved = true;
        }
        public void Initialize(float bestAngle, float angularMagnitude, float bestDistance, float distanceMagnitude, LayerProvider layerProvider)
        {
            Initialize();
            stateObjectsParent = layerProvider.gameObject;
            stateObjectsParent.transform.SetParent(transform);
            stateObjectsParent.transform.localPosition = Vector3.zero;
            stateObjectsParent.transform.localRotation = Quaternion.identity;
            normalStateObject = layerProvider.NormalLayer;
            clearStateObject = layerProvider.ClearLayer;
            failedStateObject = layerProvider.FailedLayer;
            ChangeState(State.None);
            transform.position = firstPosition;
            handBehaviour.Initialize(bestAngle, angularMagnitude, bestDistance, distanceMagnitude, 50);
        }
    }
}