using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FFXLV
{
    public class ActionState : BaseState
    {
        [SerializeField] private LayerBehaviour layer1;
        [SerializeField] private LayerBehaviour layer2;
        [SerializeField] private List<GameObject> prefabs;
        
        public float Score { get; private set; }
        public int Number { get; private set; }
        
        private LayerBehaviour currentLayer;
        private LayerBehaviour previousLayer;
        private bool isTransforming;
        private List<LayerProvider> layerProviders;
        private int previousIndex;

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
            var arrivalPoint = -currentLayer.LayerProvider.DistanceVector + new Vector3(0, 0, -10);
            position += (arrivalPoint - position) * (5 * deltaTime);
            currentLayerTransform.position = position;
            if (Vector3.Distance(currentLayerTransform.position, arrivalPoint) > 0.125f) return;
            var bestAngle = 45;
            var bestDistance = Random.Range(2, 4);
            var angleMagnitude = 0.2f;
            var distanceMagnitude = 1.6f;
            if (Number > 5)
            {
                angleMagnitude *= 1.2f;
                distanceMagnitude *= 1.2f;
            }
            else if (Number > 10)
            {
                angleMagnitude *= 1.5f;
                distanceMagnitude *= 1.5f;
            }
            else if (Number > 15)
            {
                angleMagnitude *= 2;
                distanceMagnitude *= 2;
            }

            var rand = Random.Range(0, layerProviders.Count);
            previousLayer.Finalize();
            previousLayer.Initialize(bestAngle, angleMagnitude, bestDistance, distanceMagnitude, Vector3.zero,
                layerProviders[rand]);
            isTransforming = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            Score = 0;
            Number = 0;
            var index = 0;
            do
            {
                index = Random.Range(0, layerProviders.Count);
            } while (index == previousIndex);

            previousIndex = index;
            currentLayer = layer1;
            currentLayer.Activate();
            previousLayer = layer2;
            previousLayer.Initialize(45, 0.2f, Random.Range(2, 4), 1.6f, Vector3.zero, layerProviders[index]);
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
                Number++;
                Score += currentLayer.GetScore();
                NextLayer();
            }
        }

        private void Start()
        {
            layerProviders = prefabs.Select(x => x.GetComponent<LayerProvider>()).ToList();
            foreach (var prefab in prefabs)
            {
                prefab.SetActive(false);
            }
        }
    }
}