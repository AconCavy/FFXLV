using FFXLV.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FFXLV
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TitleState titleState;
        [SerializeField] private TutorialState tutorialState;
        [SerializeField] private ActionState actionState;
        [SerializeField] private ResultState resultState;
        public GameState CurrentGameState { get; private set; }
        private bool skipTutorial;

        public void ExitGame()
        {
            Application.Quit();
        }

        private void Start()
        {
            CurrentGameState = GameState.Title;
            skipTutorial = false;
        }

        private void Update()
        {
            var dt = Time.deltaTime;
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
                    if (skipTutorial)
                    {
                        tutorialState.Skip();
                    }

                    tutorialState.Run(dt);
                    if (tutorialState.IsCompleted)
                    {
                        CurrentGameState = GameState.Action;
                        skipTutorial = true;
                    }

                    break;
                case GameState.Action:
                    actionState.Run(dt);
                    if (actionState.IsCompleted)
                    {
                        resultState.Initialize(actionState.Number, (int) actionState.Score);
                        CurrentGameState = GameState.Result;
                    }

                    break;
                case GameState.Result:
                    resultState.Run(dt);
                    if (resultState.IsCompleted)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }

                    break;
            }
        }
    }
}