using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtectedGameMode : MonoBehaviour
{
    public float GameLifeTime = 60f;

    public float LifeTime
    {
        get { return _currentTimer; }
    }

    private float _currentTimer;
    private bool _isStopped = false;

    void Start()
    {
        _currentTimer = GameLifeTime;
    }

    void Update()
    {
        if (_isStopped) return;

        _currentTimer -= Time.deltaTime;
        if (_currentTimer <= 0)
        {
            Win();
        }
    }

    public void GameOver()
    {
        TimerStop();
        StartCoroutine(LoadSceneAfterDelay("LoseScreen", 2f));
    }

    public void Win()
    {
        TimerStop();
        _currentTimer = 0;
        StartCoroutine(LoadSceneAfterDelay("WinScreen", 2f));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void TimerStop()
    {

    }
}
