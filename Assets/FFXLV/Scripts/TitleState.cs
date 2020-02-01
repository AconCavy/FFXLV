using System;
using UnityEngine;

namespace FFXLV
{
    public class TitleState : BaseState
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip introBGM;
        [SerializeField] private AudioClip loopBGM;

        private bool isLoopApplied;
        public override void Initialize()
        {
            base.Initialize();
            audioSource.clip = introBGM;
            audioSource.loop = false;
            audioSource.Play();
            isLoopApplied = false;
        }

        public override void Run(float deltaTime)
        {
            base.Run(deltaTime);
            if (audioSource.isPlaying) return;
            if (isLoopApplied) return;
            audioSource.clip = loopBGM;
            audioSource.Play();
            audioSource.loop = true;
            isLoopApplied = true;
        }

        public void GameStart()
        {
            IsCompleted = true;
        }
    }
}

