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

    void Start()
    {
        _currentTimer = GameLifeTime;
    }

    void Update()
    {
        _currentTimer -= Time.deltaTime;

        if (_currentTimer <= 0)
        {
            Win();
        }
    }

    public void GameOver()
    {
        StartCoroutine(LoadSceneAfterDelay("LoseScreen", 2f));
    }

    public void Win()
    {
        StartCoroutine(LoadSceneAfterDelay("WinScreen", 2f));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
