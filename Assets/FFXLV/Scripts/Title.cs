using UnityEngine;

namespace FFXLV
{
    public class Title : MonoBehaviour
    {
        [SerializeField] private AudioClip loopAudio;

        private AudioSource audioSource;

        private bool isAudioLooped;
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!audioSource.isPlaying && !isAudioLooped)
            {
                audioSource.clip = loopAudio;
                audioSource.loop = true;
                isAudioLooped = true;
            }
        }
    }
}

