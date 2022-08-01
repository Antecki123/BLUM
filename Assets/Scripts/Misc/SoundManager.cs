using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sounds { Win, Lose, Score, TakeDamage, Click }

    [Header("Sound Sources")]
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource mainThemeSource;
    [Space]
    [SerializeField] private List<EffectSettings> effectSettings;

    private void OnEnable()
    {
        PlayerEntity.OnScore += CoinCollect;
        PlayerEntity.OnTakeDamage += TakeDamage;
        GameManager.OnWinGame += WinGame;
        GameManager.OnLoseGame += LoseGame;
    }
    private void OnDisable()
    {
        PlayerEntity.OnScore -= CoinCollect;
        PlayerEntity.OnTakeDamage -= TakeDamage;
        GameManager.OnWinGame -= WinGame;
        GameManager.OnLoseGame -= LoseGame;
    }

    public void ButtonClick() => PlayEffect(Sounds.Click);

    private void CoinCollect() => PlayEffect(Sounds.Score);
    private void TakeDamage() => PlayEffect(Sounds.TakeDamage);
    private void WinGame() => PlayEffect(Sounds.Win);
    private void LoseGame() => PlayEffect(Sounds.Lose);

    private void PlayEffect(Sounds soundClip)
    {
        var clipIndex = effectSettings.FindIndex(e => e.effectName == soundClip.ToString());

        effectsSource.volume = effectSettings[clipIndex].volume;
        effectsSource.PlayOneShot(effectSettings[clipIndex].audioClip);

        if (soundClip == Sounds.Win || soundClip == Sounds.Lose)
        {
            mainThemeSource.volume = 0.05f;
        }
    }

    [System.Serializable]
    public struct EffectSettings
    {
        public string effectName;
        public AudioClip audioClip;
        [Range(0.0f, 1.0f)] public float volume;
    }
}