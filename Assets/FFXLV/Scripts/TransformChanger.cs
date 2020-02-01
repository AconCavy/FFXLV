using System;
using FFXLV.Enum;
using FFXLV.Utilities;
using UnityEngine;

namespace FFXLV
{
    public class TransformChanger : MonoBehaviour
    {
        [SerializeField] private float baseDistance = 3;
        [SerializeField] private float bestAngleMargin = 5;
        [SerializeField] private float goodAngleMargin = 20;
        [SerializeField] private float bestDistanceMargin = 5;
        [SerializeField] private float goodDistanceMargin = 20;
        [SerializeField] private AudioClip bestSE;
        [SerializeField] private AudioClip strongSE;
        [SerializeField] private AudioClip weakSE;
        [SerializeField] private AudioSource audioSource;

        public float AngularMagnitude { get; private set; } = 0.5f;
        public float DistanceMagnitude { get; private set; } = 0.5f;
        public TransformState CurrentState { get; private set; }
        public bool IsCompleted { get; private set; }
        public float Score { get; private set; }

        private float bestAngle;
        private float bestDistance;
        private float angle;
        private float distance;
        private readonly float minDistance = 0.5f;
        private readonly float maxDistance = 1.5f;
        private float coefficient;
        private float attackVelocity;
        private AngleScoreCalculator angleScoreCalculator;
        private DistanceScoreCalculator distanceScoreCalculator;

        private readonly float bestScore = 100;
        private readonly float goodScore = 70;
        private readonly float badScore = 30;

        public void Initialize(float bestAngle, float angularMagnitude, float bestDistance, float distanceMagnitude)
        {
            this.bestAngle = bestAngle;
            this.bestDistance = bestDistance;
            this.distance = this.baseDistance;
            this.coefficient = 1;
            this.angleScoreCalculator =
                new AngleScoreCalculator(this.bestAngle, this.bestAngleMargin, this.goodAngleMargin, this.bestScore,
                    this.goodScore, this.badScore);
            this.distanceScoreCalculator = new DistanceScoreCalculator(this.bestDistance, this.bestDistanceMargin,
                this.goodDistanceMargin, this.bestScore, this.goodScore, this.badScore);
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
            dt = dt < 0.5f ? dt : 0.5f;
            switch (this.CurrentState)
            {
                case TransformState.Angle:
                    this.angle += this.AngularMagnitude * Mathf.PI * dt;
                    break;
                case TransformState.Distance:
                    if (this.distance < this.minDistance * this.baseDistance)
                    {
                        this.coefficient = 1;
                    }
                    else if (this.distance > this.maxDistance * this.baseDistance)
                    {
                        this.coefficient = -1;
                    }

                    this.distance += this.coefficient * this.DistanceMagnitude * dt;
                    break;
                case TransformState.Attack:
                    distance = distance < 0 ? 0 : distance - attackVelocity * dt * 10;
                    if (Math.Abs(distance) < 0.05f)
                    {
                        NextState();
                    }

                    break;
                case TransformState.Effect:
                    var tmp = transform.localPosition;
                    var angleScore = angleScoreCalculator.GetScore(tmp.normalized);
                    var distanceScore = angleScoreCalculator.GetScore(tmp.magnitude);
                    var clip = Math.Abs(angleScore - this.bestScore) < 1 ? bestSE :
                        tmp.magnitude > bestDistance ? strongSE : weakSE;
                    audioSource.PlayOneShot(clip);
                    this.Score += angleScore + distanceScore;
                    if (this.Score >= 200)
                    {
                        this.IsCompleted = true;
                    }
                    else
                    {
                        
                    }

                    break;
            }

            var vector = new Vector3(Mathf.Abs(Mathf.Cos(angle)), Mathf.Abs(Mathf.Sin(angle)));

            transform.localPosition = vector * distance;
            transform.localRotation = Quaternion.Euler(0, 0, 90 * Vector3.Dot(Vector3.up, vector));
        }
    }
}