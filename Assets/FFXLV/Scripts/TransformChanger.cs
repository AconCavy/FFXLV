using FFXLV.Utilities;
using UnityEngine;

namespace FFXLV
{
    public class TransformChanger : MonoBehaviour
    {
        [SerializeField] private float bestAngle = 45;
        [SerializeField] private float bestMargin = 5;
        [SerializeField] private float goodMargin = 20;

        public float AngularMagnitude { get; set; } = 0.5f;
        public float DistanceMagnitude { get; set; } = 0.5f;
        public bool DecidedAngle { get; set; } = false;
        public bool DecidedDistance { get; set; } = false;

        private float angle;
        private float distance;
        private float coefficient;
        private AngleScoreCalculator angleScoreCalculator;
        private DistanceScoreCalculator distanceScoreCalculator;

        private void Start()
        {
            distance = 1;
            coefficient = 1;
            angleScoreCalculator = new AngleScoreCalculator(this.bestAngle, this.bestMargin, this.goodMargin, 100, 75, 30);
            distanceScoreCalculator = new DistanceScoreCalculator(1, 0.1f, 0.3f, 100, 75, 30);
        }

        private void Update()
        {
            if (!DecidedAngle)
            {
                angle += AngularMagnitude * Mathf.PI * Time.deltaTime;
            }
            else
            {
                if (!DecidedDistance)
                {
                    if (distance < 0)
                    {
                        coefficient = 1;
                    }
                    else if (distance > DistanceMagnitude)
                    {
                        coefficient = -1;
                    }

                    distance += coefficient * DistanceMagnitude * Time.deltaTime;
                }
            }

            transform.localPosition = new Vector3(Mathf.Abs(Mathf.Cos(angle)), Mathf.Abs(Mathf.Sin(angle)))
                                      * distance;
        }
    }
}