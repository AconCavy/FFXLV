using System;
using FFXLV.Enum;
using FFXLV.Utilities;
using UnityEngine;

namespace FFXLV
{
    [RequireComponent(typeof(AudioSource))]
    public class HandBehaviour : BaseState
    {
        [SerializeField] private float baseDistance = 3;
        [SerializeField] private float bestAngleMargin = 5;
        [SerializeField] private float goodAngleMargin = 20;
        [SerializeField] private float bestDistanceMargin = 0.1f;
        [SerializeField] private float goodDistanceMargin = 0.5f;
        [SerializeField] private AudioClip bestSE;
        [SerializeField] private AudioClip strongSE;
        [SerializeField] private AudioClip weakSE;
        [SerializeField] private EffectVisualizer effect;

        public bool IsFailed { get; private set; }
        public float AngularMagnitude { get; private set; } = 0.5f;
        public float DistanceMagnitude { get; private set; } = 0.5f;
        public TransformState CurrentState { get; private set; }
        public float Score { get; private set; }
        public int Count { get; private set; }
        public float DurableValue { get; private set; }

        private float bestAngle;
        private float bestDistance;
        private float angle;
        private float distance;
        private float coefficient;
        private Vector3 attackVector;
        private float attackVelocity;
        private float clearScore;
        private AngleScoreCalculator angleScoreCalculator;
        private DistanceScoreCalculator distanceScoreCalculator;
        private AudioSource audioSource;

        private readonly float bestScore = 100;
        private readonly float goodScore = 70;
        private readonly float badScore = 30;
        private readonly float minDistance = 0.5f;
        private readonly float maxDistance = 1.5f;
        private readonly float maxDurableValue = 3;

        public void Initialize(float bestAngle, float angularMagnitude, float bestDistance, float distanceMagnitude, float clearScore)
        {
            Initialize();
            ResetVariables();
            Score = 0;
            Count = 0;
            this.bestAngle = bestAngle;
            this.bestDistance = bestDistance;
            coefficient = 1;
            angleScoreCalculator = new AngleScoreCalculator(this.bestAngle, bestAngleMargin, goodAngleMargin, bestScore,
                goodScore, badScore);
            distanceScoreCalculator = new DistanceScoreCalculator(this.bestDistance, bestDistanceMargin,
                goodDistanceMargin, bestScore, goodScore, badScore);
            AngularMagnitude = angularMagnitude;
            DistanceMagnitude = distanceMagnitude;
            this.clearScore = clearScore;
            DurableValue = 0;
            IsFailed = false;
            CurrentState = TransformState.None;
        }

        public void ResetVariables()
        {
            angle = 0;
            distance = baseDistance;
        }

        public void Activate()
        {
            CurrentState = TransformState.Angle;
        }

        public override void Run(float deltaTime)
        {
            switch (CurrentState)
            {
                case TransformState.None:
                    return;
                case TransformState.Angle:
                    angle += AngularMagnitude * Mathf.PI * deltaTime;
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        CurrentState = TransformState.Distance;
                    }
                    break;
                case TransformState.Distance:
                    if (distance < minDistance * baseDistance)
                    {
                        coefficient = 1;
                    }
                    else if (distance > maxDistance * baseDistance)
                    {
                        coefficient = -1;
                    }

                    distance += coefficient * DistanceMagnitude * deltaTime;
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        attackVector = transform.localPosition.normalized;
                        attackVelocity = transform.localPosition.magnitude;
                        CurrentState = TransformState.Attack;
                    }
                    break;
                case TransformState.Attack:
                    distance = distance < 0 ? 0 : distance - attackVelocity * deltaTime * 10;
                    if (Math.Abs(distance) < 0.05f)
                    {
                        CurrentState = TransformState.Effect;
                    }

                    break;
                case TransformState.Effect:
                    var angleScore = angleScoreCalculator.GetScore(attackVector);
                    var distanceScore = distanceScoreCalculator.GetScore(attackVelocity);
                    var clip = Math.Abs(angleScore - bestScore) < 1 ? bestSE :
                        attackVelocity > bestDistance ? strongSE : weakSE;
                    audioSource.PlayOneShot(clip);
                    effect.Initialize();
                    effect.Play(1);
                    Score += angleScore + distanceScore;
                    DurableValue += attackVelocity / baseDistance;
                    if (DurableValue >= maxDurableValue)
                    {
                        IsFailed = true;
                        IsCompleted = true;
                    }

                    if (Score >= clearScore)
                    {
                        IsCompleted = true;
                    }
                    else
                    {
                        IsCompleted = false;
                        Count++;
                        ResetVariables();
                        CurrentState = TransformState.Angle;
                    }

                    break;
            }

            var vector = new Vector3(Mathf.Abs(Mathf.Cos(angle)), Mathf.Abs(Mathf.Sin(angle)));
            transform.localPosition = vector * distance;
            transform.localRotation = Quaternion.Euler(0, 0, 90 * Vector3.Dot(Vector3.up, vector));
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}