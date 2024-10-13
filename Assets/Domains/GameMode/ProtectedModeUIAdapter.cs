using TMPro;
using UnityEngine;

public class ProtectedModeUIAdapter : MonoBehaviour
{
    [SerializeField] ProtectedGameMode _mode;
    [SerializeField] TMP_Text _timerText;

    void Update()
    {
        _timerText.text = _mode.LifeTime.ToString("F1");
    }
}