using System;
using FFXLV.Enum;
using FFXLV.Utilities;
using UnityEngine;

namespace FFXLV
{
    public class TransformChanger : MonoBehaviour
    {
        [SerializeField] private float bestAngle = 45;
        [SerializeField] private float bestAngleMargin = 5;
        [SerializeField] private float goodAngleMargin = 20;
        [SerializeField] private float bestDistance = 1;
        [SerializeField] private float bestDistanceMargin = 5;
        [SerializeField] private float goodDistanceMargin = 20;

        public float AngularMagnitude { get; private set; } = 0.5f;
        public float DistanceMagnitude { get; private set; } = 0.5f;
        public TransformState CurrentState { get; private set; }

        private float angle;
        private float distance;
        private float coefficient;
        private float attackVelocity;
        private AngleScoreCalculator angleScoreCalculator;
        private DistanceScoreCalculator distanceScoreCalculator;

        public void Initialize(float angularMagnitude, float distanceMagnitude)
        {
            this.distance = 1;
            this.coefficient = 1;
            this.angleScoreCalculator =
                new AngleScoreCalculator(this.bestAngle, this.bestAngleMargin, this.goodAngleMargin, 100, 75, 30);
            this.distanceScoreCalculator = new DistanceScoreCalculator(this.bestDistance, this.bestDistanceMargin,
                this.goodDistanceMargin, 100, 75, 30);
            this.AngularMagnitude = angularMagnitude;
            this.DistanceMagnitude = distanceMagnitude;
            this.CurrentState = TransformState.Angle;
        }

        public void NextState()
        {
            if (CurrentState.Equals(TransformState.Distance))
            {
                this.attackVelocity = GetCurrentVector().magnitude;
            }

            this.CurrentState = CurrentState.Equals(TransformState.Effect) ? TransformState.None : CurrentState + 1;
        }

        public Vector3 GetCurrentVector()
        {
            return transform.localPosition;
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            dt = dt > 0.5f ? dt : 0.5f;
            switch (this.CurrentState)
            {
                case TransformState.Angle:
                    this.angle += this.AngularMagnitude * Mathf.PI * dt;
                    break;
                case TransformState.Distance:
                    if (this.distance < 0)
                    {
                        this.coefficient = 1;
                    }
                    else if (this.distance > this.DistanceMagnitude)
                    {
                        this.coefficient = -1;
                    }

                    this.distance += this.coefficient * this.DistanceMagnitude * dt;
                    break;
                case TransformState.Attack:
                    distance = distance < 0 ? 0 : distance - attackVelocity * dt;
                    if (Math.Abs(distance) < 0.05f)
                    {
                        NextState();
                    }

                    break;
                case TransformState.Effect:
                    // TODO VFX and SFX
                    NextState();
                    break;
            }

            transform.localPosition = new Vector3(Mathf.Abs(Mathf.Cos(angle)), Mathf.Abs(Mathf.Sin(angle)))
                                      * distance;
        }
    }
}