using UnityEngine;

namespace FFXLV.Utilities
{
    public class AngleScoreCalculator : BaseScoreCalculator
    {
        private readonly Vector3 bestVector;
        private readonly float bestThreshold;
        private readonly float goodThreshold;

        public AngleScoreCalculator(float bestAngle, float bestMargin, float goodMargin, float bestScore,
            float goodScore, float badScore) : base(bestScore, goodScore, badScore)
        {
            var bestRad = bestAngle * Mathf.Deg2Rad;
            this.bestVector = new Vector3(Mathf.Cos(bestRad), Mathf.Sin(bestRad)).normalized;

            var bestMarginRad = (bestAngle + bestMargin) * Mathf.Deg2Rad;
            var bestMarginVector = new Vector3(Mathf.Cos(bestMarginRad), Mathf.Sin(bestMarginRad)).normalized;
            this.bestThreshold = Vector3.Dot(this.bestVector, bestMarginVector);

            var goodMarginRad = (bestAngle + goodMargin) * Mathf.Deg2Rad;
            var goodMarginVector = new Vector3(Mathf.Cos(goodMarginRad), Mathf.Sin(goodMarginRad)).normalized;
            this.goodThreshold = Vector3.Dot(this.bestVector, goodMarginVector);
        }

        public float GetScore(Vector3 angleVector)
        {
            var tmp = Vector3.Dot(this.bestVector, angleVector.normalized);
            return this.GetScore(tmp);
        }

        public override float GetScore(float value)
        {
            if (value > bestThreshold) return this.bestScore;
            return value > goodThreshold ? this.goodScore : this.badScore;
        }
    }
}