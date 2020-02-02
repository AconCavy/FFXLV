using UnityEngine;

namespace FFXLV
{
    public class LayerProvider : MonoBehaviour
    {

        [SerializeField]
        GameObject _normalLayer;
        [SerializeField]
        GameObject _clearLayer;
        [SerializeField]
        GameObject _failedLayer;

        public GameObject NormalLayer { get { return _normalLayer; } }
        public GameObject ClearLayer { get { return _clearLayer; } }
        public GameObject FailedLayer { get { return _failedLayer; } }
    }
}
