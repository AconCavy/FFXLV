using UnityEngine;

namespace FFXLV
{
    public class ActionState : BaseState
    {
        [SerializeField] private LayerBehaviour layer1;
        [SerializeField] private LayerBehaviour layer2;

        private float score;
        private int number;
        private LayerBehaviour currentLayer;
        private LayerBehaviour previousLayer;
        private bool isTransforming;
        private readonly Vector3 arrivalPoint = new Vector3(1, 0, -11);

        private void NextLayer()
        {
            (currentLayer, previousLayer) = (previousLayer, currentLayer);
            isTransforming = true;
            currentLayer.Activate();
        }

        private void TransformLayers(float deltaTime)
        {
            var currentLayerTransform = previousLayer.transform;
            var position = currentLayerTransform.position;
            position += (arrivalPoint - position) * (5 * deltaTime);
            currentLayerTransform.position = position;
            if (Vector3.Distance(currentLayerTransform.position, arrivalPoint) > 0.125f) return;
            var bestAngle = 45;
            var bestDistance = Random.Range(2, 4);
            var angleMagnitude = 0.2f;
            var distanceMagnitude = 1.6f;
            if (number > 5)
            {
                angleMagnitude *= 1.2f;
                distanceMagnitude *= 1.2f;
            }
            else if (number > 10)
            {
                angleMagnitude *= 1.5f;
                distanceMagnitude *= 1.5f;
            }
            else if (number > 15)
            {
                angleMagnitude *= 2;
                distanceMagnitude *= 2;
            }

            previousLayer.Initialize(bestAngle, angleMagnitude, bestDistance, distanceMagnitude, Vector3.zero);
            previousLayer.Deactivate();
            isTransforming = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            score = 0;
            number = 0;
            currentLayer = layer1;
            currentLayer.Initialize(45, 0.2f, 3, 1.6f, Vector3.zero);
            currentLayer.Activate();
            previousLayer = layer2;
            previousLayer.Initialize(45, 0.2f, Random.Range(2, 4), 1.6f, Vector3.zero);
            previousLayer.Deactivate();
        }

        public override void Run(float deltaTime)
        {
            base.Run(deltaTime);
            if (isTransforming)
            {
                TransformLayers(deltaTime);
            }

            currentLayer.Run(deltaTime);
            if (!currentLayer.IsCompleted) return;
            if (currentLayer.IsFailed)
            {
                IsCompleted = true;
            }
            else
            {
                number++;
                score += currentLayer.GetScore();
                NextLayer();
            }
        }
    }
}