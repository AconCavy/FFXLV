using System;
using TMPro;
using UnityEngine;

namespace FFXLV
{
    public class ResultState : BaseState
    {
        [SerializeField] private GameObject resultBack;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip loseClip;
        
        public void Initialize(int count, int score)
        {
            Initialize();
            resultBack.SetActive(true);
            countText.text = count.ToString();
            scoreText.text = score.ToString();
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.clip = loseClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        public override void Run(float deltaTime)
        {
            base.Run(deltaTime);
            if (!Input.GetKeyDown(KeyCode.Return)) return;
            audioSource.Stop();
            resultBack.SetActive(false);
            IsCompleted = true;
        }
    }
}