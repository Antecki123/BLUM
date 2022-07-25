using System;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public static Action OnTakeDamage;
    public static Action OnScore;

    [Header("Component References")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private FloatVariable healthValue;
    [SerializeField] private FloatVariable scoreValue;

    private static readonly int DeadAnimation = Animator.StringToHash("Dead");

    [field: SerializeField] public bool IsDead { get; private set; } = false;


    private void Start()
    {
        healthValue.value = 3.0f;
        scoreValue.value = 0.0f;
    }

    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        healthValue.value--;
        OnTakeDamage?.Invoke();

        if (healthValue.value <= 0)
        {
            //Kill character
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetTrigger(DeadAnimation);
            //gameObject.SetActive(false);
        }
    }

    [ContextMenu("Score Points")]
    public void ScorePoints()
    {
        scoreValue.value++;
        OnScore?.Invoke();
    }
}
