using UnityEngine;

namespace FFXLV
{
    public class TutorialState : BaseState
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip gameBGM;
        [SerializeField] private Vector3 firstPosition;
        [SerializeField] private TransformChanger transformChanger;

        private State currentState;

        private enum State
        {
            None,
            Move,
            Game
        }

        public override void Initialize()
        {
            base.Initialize();
            currentState = State.Move;
            audioSource.clip = gameBGM;
            audioSource.loop = true;
            audioSource.Play();
        }

        public override void Run(float deltaTime)
        {
            base.Run(deltaTime);
            switch (currentState)
            {
                case State.Move:
                    var direction = firstPosition - transform.position;
                    if (direction.magnitude > 0.125f)
                    {
                        transform.position += direction * deltaTime;
                    }
                    else
                    {
                        currentState++;
                    }

                    break;
                case State.Game:
                    // if (!transformChanger.gameObject.activeSelf)
                    // {
                    //     transformChanger.gameObject.SetActive(true);
                    //     transformChanger.Initialize(45, 0.2f, 3, 0.4f);
                    // }

                    break;
            }
        }
    }
}