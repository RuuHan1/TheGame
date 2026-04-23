using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : ISceneState
{
    private SceneController _controller;
    public MainMenuState(SceneController sceneController)
    {
        _controller = sceneController;
    }
    public void Enter()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }
    public void StartGame(int difficulty)
    {
        //_controller.ChangeState(new LoadingState(_controller, new GameplayState(_controller, difficulty)));
    }
}
