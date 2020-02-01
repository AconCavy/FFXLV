using FFXLV.Enum;
using UnityEngine;

namespace FFXLV
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField] private Tutorial tutorial;
        public GameState GameState { get; private set; }

        public void NextState()
        {
            this.GameState = this.GameState.Equals(GameState.Result) ? GameState.Title : this.GameState++;
        }

        public void ExitGame()
        {
            Application.Quit();
        }
        
        private void Start()
        {
            this.GameState = GameState.Title;
        }
        
        private void Update()
        {
            var dt = Time.deltaTime;
            switch (GameState)
            {
                case GameState.Title:
                    break;
                case GameState.Tutorial:
                    if (tutorial.IsCompleted)
                    {
                        NextState();
                    }
                    break;
                case GameState.MainGame:
                    break;
                case GameState.Result:
                    break;
            }
        }
    }

}
