using UnityEngine;

namespace FFXLV.UI
{
    public class TimerMaskController : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float min;
        [SerializeField] private float max;
        [SerializeField] private int step;

        public void SetTimer(float ratio = 1)
        {
            var time = (int) (ratio * step);
            rectTransform.sizeDelta = new Vector2((max - min) * time, rectTransform.sizeDelta.y);
        }
    }
}
