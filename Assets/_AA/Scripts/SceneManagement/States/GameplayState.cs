using System;
using UnityEngine;

public class GameplayState : ISceneState
{
    private SceneController _controller;
    private int _difficulty;

    // Constructor: Hem yöneticiyi hem de MainMenu'den gelen zorluk bilgisini alýyoruz
    public GameplayState(SceneController controller, int difficulty)
    {
        _controller = controller;
        _difficulty = difficulty;
    }

    public void Enter()
    {
        // Sahne zaten LoadingState tarafýndan yüklendi!
        // Burada sadece oyunu baþlatma hazýrlýklarý yapýyoruz.
        Debug.Log($"Oyun baþladý! Seçilen zorluk: {_difficulty}");
        GameEvents.PlayerDied += OnPlayerDied;
        // TODO: Zorluk seviyesine göre GameManager'a veya Object Pool'lara bilgi gönderilebilir.

    }

    

    public void Tick()
    {
        // Oyun döngüsü. Fizik ve hareket iþlemleri genellikle MonoBehaviour'larda (Player, Enemy) yapýlýr.
        // Ancak oyun süresi sayacý (timer) gibi sahneye özgü genel kontrolleri buraya yazabilirsin.
    }

    public void Exit()
    {
        GameEvents.PlayerDied -= OnPlayerDied;
    }
    private void OnPlayerDied()
    {

        _controller.ChangeState(
            new LoadingState(
                _controller,
                "MainMenu",
                new MainMenuState(_controller)
            )
        );
    }

}