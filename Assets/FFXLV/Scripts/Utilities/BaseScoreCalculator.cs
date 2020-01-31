namespace FFXLV.Utilities
{
    public abstract class BaseScoreCalculator
    {
        protected readonly float bestScore;
        protected readonly float goodScore;
        protected readonly float badScore;

        public BaseScoreCalculator(float bestScore, float goodScore, float badScore)
        {
            this.bestScore = bestScore;
            this.goodScore = goodScore;
            this.badScore = badScore;
        }

        public abstract float GetScore(float value);
    }
}