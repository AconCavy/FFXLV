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
        private readonly Vector3 arrivalPoint = new Vector3(0, 0, -15);

        private void NextLayer()
        {
            (currentLayer, previousLayer) = (previousLayer, currentLayer);
            isTransforming = true;
        }

        private void TransformLayers(float deltaTime)
        {
            var currentLayerTransform = previousLayer.transform;
            currentLayerTransform.position += (arrivalPoint - currentLayerTransform.position) * deltaTime;
            if (Vector3.Distance(currentLayerTransform.position, arrivalPoint) > 0.125f) return;
            var bestAngle = 45;
            var bestDistance = Random.Range(2, 4);
            var angleMagnitude = 0.2f;
            var distanceMagnitude = 0.4f;
            if (number > 5)
            {
                angleMagnitude *= 2;
                distanceMagnitude *= 2;
            }
            else if (number > 10)
            {
                angleMagnitude *= 3;
                distanceMagnitude *= 3;
            }
            else if (number > 15)
            {
                angleMagnitude *= 5;
                distanceMagnitude *= 5;
            }

            previousLayer.Initialize(bestAngle, angleMagnitude, bestDistance, distanceMagnitude);
            previousLayer.gameObject.SetActive(false);
            isTransforming = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            score = 0;
            number = 0;
            currentLayer = layer1;
            previousLayer = layer2;
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