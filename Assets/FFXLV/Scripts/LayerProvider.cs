using UnityEngine;

namespace FFXLV
{
    public class LayerProvider : MonoBehaviour
    {

        [SerializeField] private GameObject normalLayer;
        [SerializeField] private GameObject clearLayer;
        [SerializeField] private GameObject failedLayer;
        [SerializeField] private Vector3 distanceVector;

        public GameObject NormalLayer => normalLayer;
        public GameObject ClearLayer => clearLayer;
        public GameObject FailedLayer => failedLayer;
        public Vector3 DistanceVector => distanceVector;
    }
}
