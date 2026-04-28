using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingState : ISceneState
{
    private SceneController _controller;
    private ISceneState _nextState;      
    private string _sceneToLoad;         
    private LoadSceneMode _loadMode;     
    private AsyncOperation _asyncOperation;

    public LoadingState(SceneController controller, string sceneToLoad, ISceneState nextState, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        _controller = controller;
        _sceneToLoad = sceneToLoad;
        _nextState = nextState;
        _loadMode = loadMode;
    }

    public void Enter()
    {

        // Hedef sahneyi arka planda asenkron yüklemeye baþla
        _asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad, _loadMode);
    }

    public void Tick()
    {
        if (_asyncOperation == null) return;


        float progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
        Debug.Log($"LoadingState Gönderiyor: {progress}");
        GameEvents.LoadingProgressUpdated_LoadingState?.Invoke(progress);
        if (_asyncOperation.isDone)
        {
            _controller.ChangeState(_nextState);
        }
    }

    public void Exit()
    {
        // Arka plan temizliði, Loading UI'ýn kapatýlmasý vb.
    }
}
