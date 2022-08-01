using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Action OnWinGame;
    public static Action OnLoseGame;

    [SerializeField] private GameObject endgamePanel;
    [SerializeField] private TextMeshProUGUI endgameText;
    [Space]
    [SerializeField] private FloatVariable health;
    [SerializeField] private FloatVariable score;

    private readonly string winText = "You win!";
    private readonly string loseText = "You lose!";

    private void Start()
    {
        health.value = 3.0f;
        score.value = 0.0f;

        InvokeRepeating(nameof(LoseGameCondition), 0.0f, 1.0f);
    }

    private void OnEnable() => FinishLevel.OnFinishLevel += WinGameCondition;

    private void OnDisable() => FinishLevel.OnFinishLevel -= WinGameCondition;

    private void LoseGameCondition()
    {
        if (health.value <= 0)
        {
            endgameText.text = loseText;
            endgamePanel.SetActive(true);

            OnLoseGame?.Invoke();
            CancelInvoke();
        }
    }

    private void WinGameCondition()
    {
        endgameText.text = winText;
        endgamePanel.SetActive(true);

        OnWinGame?.Invoke();
        CancelInvoke();

        Time.timeScale = 0.0f;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}