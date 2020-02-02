using UnityEngine;

namespace FFXLV
{
    public class TutorialState : BaseState
    {
        [SerializeField] private TutorialLayerBehaviour tutorialLayer;
        [SerializeField] private LayerProvider tutorialLayerProvider;
        [SerializeField] private LayerProvider nextLayerProvider;
        [SerializeField] private LayerBehaviour nextLayer;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip gameBGM;

        private State currentState;
        private readonly Vector3 arrivalPoint = new Vector3(1, 0.5f, -11);

        private enum State
        {
            None,
            Move,
            Game,
            Skip
        }

        public void Skip()
        {
            var bestAngle = 45;
            var bestDistance = Random.Range(2, 4);
            var angleMagnitude = 0.2f;
            var distanceMagnitude = 1.6f;
            nextLayer.gameObject.SetActive(true);
            nextLayer.Initialize(bestAngle, angleMagnitude, bestDistance, distanceMagnitude, Vector3.zero,
                nextLayerProvider);
            nextLayer.Activate();
            currentState = State.Skip;
        }

        private void TransformLayers(float deltaTime)
        {
            var tutorialLayerTransform = tutorialLayer.transform;
            var position = tutorialLayerTransform.position;
            position += (arrivalPoint - position) * (5 * deltaTime);
            tutorialLayerTransform.position = position;

            if (Vector3.Distance(tutorialLayerTransform.position, arrivalPoint) > 0.125f) return;
            IsCompleted = true;
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
                    tutorialLayer.Move(deltaTime);
                    if (tutorialLayer.IsMoved)
                    {
                        currentState = State.Game;
                        tutorialLayer.Initialize(45, 0.2f, 1, 1.6f, tutorialLayerProvider);
                        tutorialLayer.Activate();
                    }

                    break;
                case State.Game:
                    tutorialLayer.Run(deltaTime);
                    if (tutorialLayer.IsCompleted)
                    {
                        Skip();
                    }

                    break;
                case State.Skip:
                    TransformLayers(deltaTime);
                    break;
            }
        }
    }
}