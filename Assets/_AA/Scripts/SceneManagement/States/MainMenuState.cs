using System;
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

        GameEvents.PlayButtonClicked += OnPlayClicked;
    }

    private void OnPlayClicked(int obj)
    {
        Debug.Log("registered");
        _controller.ChangeState(new LoadingState(_controller,"GameScene",new GameplayState(_controller,obj)));
    }

    public void Exit()
    {
        GameEvents.PlayButtonClicked -= OnPlayClicked;
    }

    public void Tick()
    {
    }
    public void StartGame(int difficulty)
    {
        //_controller.ChangeState(new LoadingState(_controller, new GameplayState(_controller, difficulty)));
    }
}
