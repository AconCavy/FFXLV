using UnityEngine;

namespace FFXLV
{
    public class TutorialLayerBehaviour : LayerBehaviour
    {
        [SerializeField] private GameObject child;
        [SerializeField] private Vector3 firstPosition;
        public bool IsMoved { get; private set; }

        public void Move(float deltaTime)
        {
            child.transform.position += (firstPosition - child.transform.position) * deltaTime;
            if (Vector3.Distance(child.transform.position, firstPosition) > 0.125f) return;
            IsMoved = true;
        }
    }
}