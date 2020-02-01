using FFXLV.Enum;
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
        public TransformState CurrentState { get; private set; }

        private float angle;
        private float distance;
        private float coefficient;
        private AngleScoreCalculator angleScoreCalculator;
        private DistanceScoreCalculator distanceScoreCalculator;

        public void NextState()
        {
            CurrentState = CurrentState.Equals(TransformState.Attack) ? TransformState.None : CurrentState + 1;
        }

        public Vector3 GetCurrentVector()
        {
            return transform.localPosition;
        }

        private void Start()
        {
            distance = 1;
            coefficient = 1;
            angleScoreCalculator =
                new AngleScoreCalculator(this.bestAngle, this.bestMargin, this.goodMargin, 100, 75, 30);
            distanceScoreCalculator = new DistanceScoreCalculator(1, 0.1f, 0.3f, 100, 75, 30);
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case TransformState.Angle:
                    angle += AngularMagnitude * Mathf.PI * Time.deltaTime;
                    break;
                case TransformState.Distance:
                    if (distance < 0)
                    {
                        coefficient = 1;
                    }
                    else if (distance > DistanceMagnitude)
                    {
                        coefficient = -1;
                    }

                    distance += coefficient * DistanceMagnitude * Time.deltaTime;
                    break;
                case TransformState.Attack:
                    break;
                default:
                    break;
            }

            transform.localPosition = new Vector3(Mathf.Abs(Mathf.Cos(angle)), Mathf.Abs(Mathf.Sin(angle)))
                                      * distance;
        }
    }
}