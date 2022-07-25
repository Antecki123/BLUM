using UnityEngine;

public class HealthDisplayUI : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Animator[] animators;
    [SerializeField] private FloatVariable healthValue;

    private static readonly int TakeDamageAnimation = Animator.StringToHash("Take_damage");

    private void OnEnable() => PlayerEntity.OnTakeDamage += UpdateHealth;
    private void OnDisable() => PlayerEntity.OnTakeDamage -= UpdateHealth;

    public void UpdateHealth()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if (i >= (int)healthValue.value)
                animators[i].SetTrigger(TakeDamageAnimation);
        }
    }
}