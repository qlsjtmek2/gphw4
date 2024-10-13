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
            SceneManager.LoadScene("WinScreen");
        }
    }

    public void OnProtectedDeath(IProtectedState prevState, IProtectedState newState)
    {
        if (newState is DieState)
        {
            Lose();
        }
    }

    void Lose()
    {
        SceneManager.LoadScene("LoseScreen");
    }
}
