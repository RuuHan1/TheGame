using System;
using UnityEngine;

public class GameplayState : ISceneState
{
    private SceneController _controller;
    private int _difficulty;

    public GameplayState(SceneController controller, int difficulty)
    {
        _controller = controller;
        _difficulty = difficulty;
    }

    public void Enter()
    {
        GameEvents.PlayerDied += OnPlayerDied;
        GameEvents.GoMainMenu += OnGoMainMenu;
    }



    public void Tick()
    {
        // Oyun döngüsü. Fizik ve hareket iþlemleri genellikle MonoBehaviour'larda (Player, Enemy) yapýlýr.
        // Ancak oyun süresi sayacý (timer) gibi sahneye özgü genel kontrolleri buraya yazabilirsin.
    }

    public void Exit()
    {
        GameEvents.PlayerDied -= OnPlayerDied;
        GameEvents.GoMainMenu += OnGoMainMenu;
    }

    private void OnGoMainMenu()
    {
        _controller.ChangeState(
            new LoadingState(
                _controller,
                "MainMenu",
                new MainMenuState(_controller)
            )
        );
    }

    private void OnPlayerDied()
    {

        //_controller.ChangeState(
        //    new LoadingState(
        //        _controller,
        //        "MainMenu",
        //        new MainMenuState(_controller)
        //    )
        //);
    }

}