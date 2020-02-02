using UnityEngine;

namespace FFXLV
{
    public class EffectVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject effect;

        public bool IsPlaying { get; private set; }
        private float duration;

        public void Initialize()
        {
            IsPlaying = false;
            effect.SetActive(false);
        }
        public void Play(float duration)
        {
            this.duration = duration;
            IsPlaying = true;
            effect.SetActive(true);
        }

        public void Stop()
        {
            IsPlaying = false;
            effect.SetActive(false);
        }

        private void Update()
        {
            if (duration >= 0)
            {
                if (IsPlaying)
                {
                    duration -= Time.deltaTime;
                }
            }
            else
            {
                if (effect.activeSelf)
                {
                    effect.SetActive(false);
                }
                if (!IsPlaying) return;
                IsPlaying = false;
            }
        }
    }
}