using UnityEngine;

namespace FFXLV
{
    public class TutorialState : BaseState
    {
        [SerializeField] private TutorialLayerBehaviour tutorialLayer;
        [SerializeField] private LayerBehaviour nextLayer;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip gameBGM;

        private State currentState;
        private bool isTransforming;
        private readonly Vector3 arrivalPoint = new Vector3(0, 0, -15);

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
            var distanceMagnitude = 0.4f;
            nextLayer.Initialize(bestAngle, angleMagnitude, bestDistance, distanceMagnitude);
            currentState = State.Skip;
        }

        private void TransformLayers(float deltaTime)
        {
            var tutorialLayerTransform = tutorialLayer.transform;
            tutorialLayerTransform.position += (arrivalPoint - tutorialLayerTransform.position) * deltaTime;
            if (Vector3.Distance(tutorialLayerTransform.position, arrivalPoint) > 0.125f) return;
            isTransforming = false;
            IsCompleted = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            currentState = State.Move;
            audioSource.clip = gameBGM;
            audioSource.loop = true;
            audioSource.Play();
            isTransforming = false;
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