using TMPro;
using UnityEngine;

public class ScoreDisplayUI : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private FloatVariable scoreValue;

    private void OnEnable() => PlayerEntity.OnScore += UpdateScore;
    private void OnDisable() => PlayerEntity.OnScore -= UpdateScore;

    public void UpdateScore() => scoreText.text = scoreValue.value.ToString();

}