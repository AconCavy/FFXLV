using FFXLV.Enum;
using UnityEngine;

namespace FFXLV
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TitleState titleState;
        [SerializeField] private TutorialState tutorialState;
        [SerializeField] private ActionState actionState;
        [SerializeField] private ResultState resultState;
        public GameState CurrentGameState { get; private set; }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void Start()
        {
            CurrentGameState = GameState.Title;
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            Debug.Log(CurrentGameState);
            switch (CurrentGameState)
            {
                case GameState.Title:
                    titleState.Run(dt);
                    if (titleState.IsCompleted)
                    {
                        CurrentGameState = GameState.Tutorial;
                    }

                    break;
                case GameState.Tutorial:
                    tutorialState.Run(dt);
                    if (tutorialState.IsCompleted)
                    {
                        CurrentGameState = GameState.Action;
                    }

                    break;
                case GameState.Action:
                    actionState.Run(dt);
                    if (actionState.IsCompleted)
                    {
                        CurrentGameState = GameState.Result;
                    }

                    break;
                case GameState.Result:
                    resultState.Run(dt);
                    if (resultState.IsCompleted)
                    {
                        CurrentGameState = GameState.Title;
                    }

                    break;
            }
        }
    }
}