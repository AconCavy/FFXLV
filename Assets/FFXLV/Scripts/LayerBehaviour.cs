using UnityEngine;

namespace FFXLV
{
    public class LayerBehaviour : BaseState
    {
        [SerializeField] protected HandBehaviour handBehaviour;

        public bool IsFailed { get; private set; }
        public int Count { get; private set; }
        public LayerProvider LayerProvider { get; private set; }
        protected GameObject normalStateObject;
        protected GameObject clearStateObject;
        protected GameObject failedStateObject;
        protected GameObject stateObjectsParent;

        private State currentState;

        protected enum State
        {
            None,
            Normal,
            Clear,
            Failed
        }

        protected void ChangeState(State state)
        {
            if (state.Equals(currentState)) return;
            currentState = state;
            switch (state)
            {
                case State.None:
                    normalStateObject.SetActive(false);
                    clearStateObject.SetActive(false);
                    failedStateObject.SetActive(false);
                    break;
                case State.Normal:
                    normalStateObject.SetActive(true);
                    clearStateObject.SetActive(false);
                    failedStateObject.SetActive(false);
                    break;
                case State.Clear:
                    normalStateObject.SetActive(false);
                    clearStateObject.SetActive(true);
                    failedStateObject.SetActive(false);
                    break;
                case State.Failed:
                    normalStateObject.SetActive(false);
                    clearStateObject.SetActive(false);
                    failedStateObject.SetActive(true);
                    break;
            }
        }

        public void Activate()
        {
            handBehaviour.gameObject.SetActive(true);
            handBehaviour.Activate();
            stateObjectsParent.SetActive(true);
            ChangeState(State.Normal);
        }

        public void Deactivate()
        {
            handBehaviour.gameObject.SetActive(false);
            ChangeState(State.None);
            stateObjectsParent.SetActive(false);
        }

        public int GetScore()
        {
            return Mathf.Clamp(100 - 10 * Count, 0, 100);
        }

        public void Initialize(float bestAngle, float angularMagnitude, float bestDistance, float distanceMagnitude,
            Vector3 firstPosition, LayerProvider layerProvider)
        {
            Initialize();
            handBehaviour.gameObject.SetActive(true);
            handBehaviour.Initialize(bestAngle, angularMagnitude, bestDistance, distanceMagnitude, 200);
            LayerProvider = layerProvider;
            stateObjectsParent = LayerProvider.gameObject;
            stateObjectsParent.transform.SetParent(transform);
            stateObjectsParent.transform.localPosition = Vector3.zero;
            stateObjectsParent.transform.localRotation = Quaternion.identity;
            normalStateObject = LayerProvider.NormalLayer;
            clearStateObject = LayerProvider.ClearLayer;
            failedStateObject = LayerProvider.FailedLayer;
            handBehaviour.gameObject.SetActive(false);
            ChangeState(State.None);
            transform.position = firstPosition;
        }

        public void Finalize()
        {
            Deactivate();
            stateObjectsParent.transform.SetParent(null);
        }

        public override void Run(float deltaTime)
        {
            base.Run(deltaTime);
            handBehaviour.Run(deltaTime);
            if (!handBehaviour.IsCompleted) return;
            IsCompleted = true;
            Count = handBehaviour.Count;
            IsFailed = handBehaviour.IsFailed;
            ChangeState(IsFailed ? State.Failed : State.Clear);
        }
    }
}