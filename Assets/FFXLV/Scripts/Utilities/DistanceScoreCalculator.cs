using UnityEngine;

namespace FFXLV.Utilities
{
    public class DistanceScoreCalculator : BaseScoreCalculator
    {
        private readonly float bestDistance;
        private readonly float bestMargin;
        private readonly float goodMargin;

        public DistanceScoreCalculator(float bestDistance, float bestMargin, float goodMargin, float bestScore,
            float goodScore, float badScore) : base(bestScore, goodScore, badScore)
        {
            this.bestDistance = bestDistance;
            this.bestMargin = bestMargin;
            this.goodMargin = goodMargin;
        }

        public override float GetScore(float value)
        {
            var diff = value - this.bestDistance;
            if (Mathf.Abs(diff) < this.bestMargin) return this.bestScore;
            return Mathf.Abs(diff) < this.goodMargin ? this.goodScore : this.badScore;
        }
    }
}