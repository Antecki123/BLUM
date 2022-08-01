using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public interface IPlayer
{
    public void TakeDamage(Transform origin);
}

public class PlayerEntity : MonoBehaviour, IPlayer
{
    public static Action OnTakeDamage;
    public static Action OnScore;

    [Header("HUD References")]
    [SerializeField] private FloatVariable healthValue;
    [SerializeField] private FloatVariable scoreValue;

    private Rigidbody2D rb2D;
    private float immunityTimer = 0;

    public bool IsDead { get; private set; } = false;
    public AnimationsManager Animations { get; private set; }
    [field: SerializeField] public Settings PlayerSettings { get; private set; }

    private void Awake()
    {
        Animations = new AnimationsManager(GetComponentInChildren<Animator>());
        rb2D = GetComponent<Rigidbody2D>();

        immunityTimer = PlayerSettings.ImmunitykDelay;
    }

    public async void TakeDamage(Transform origin)
    {
        var animationTime = 667;    //millis

        if (PlayerSettings.ImmunitykDelay <= immunityTimer && !IsDead)
        {
            healthValue.value--;
            OnTakeDamage?.Invoke();

            if (healthValue.value <= 0)
            {
                IsDead = true;
                Animations.Death();

                await Task.Delay(animationTime);
                gameObject.SetActive(false);

                StopAllCoroutines();

                return;
            }

            HitEffect();
            StartCoroutine(Knockback(origin));
            StartCoroutine(ImmunityCountdown());
        }
    }

    private async void HitEffect()
    {
        var animationTime = 333;    //millis

        PlayerSettings.HitSparkle.SetActive(true);
        await Task.Delay(animationTime);
        PlayerSettings.HitSparkle.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();

            scoreValue.value++;
            OnScore?.Invoke();
        }

        if (collision.gameObject.TryGetComponent(out Enemy _))
        {

            TakeDamage(collision.gameObject.transform);
        }
    }

    private IEnumerator ImmunityCountdown()
    {
        immunityTimer = 0.0f;

        while (PlayerSettings.ImmunitykDelay > immunityTimer)
        {
            immunityTimer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Knockback(Transform origin)
    {
        var knockbackTimer = 0.1f;

        var direction = (transform.position - origin.position).normalized;
        float knockbackAmount = PlayerSettings.KnockbackForce;

        //Debug.DrawRay(origin.position, direction, Color.red, 10f);

        while (knockbackTimer > 0)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(10.0f * knockbackAmount * direction);

            knockbackTimer -= Time.deltaTime;
            yield return null;
        }
    }

    [Serializable]
    public class Settings
    {
        [field: SerializeField, Range(0.0f, 50.0f)] public float RunSpeed { get; private set; } = 15.0f;
        [field: SerializeField, Range(0.0f, 50.0f)] public float JumpSpeed { get; private set; } = 30.0f;
        [field: SerializeField, Range(0.0f, 5.0f)] public float AttackDelay { get; private set; } = 1.0f;
        [field: SerializeField, Range(0.0f, 5.0f)] public float ImmunitykDelay { get; private set; } = 1.0f;
        [field: SerializeField, Range(0.0f, 30.0f)] public float KnockbackForce { get; private set; } = 6.0f;

        [field: SerializeField] public Transform GroundCollider { get; private set; }
        [field: SerializeField] public Transform AttackPoint { get; private set; }
        [field: SerializeField] public GameObject HitSparkle { get; private set; }

        [field: SerializeField] public Vector2 AttackPointSize { get; private set; } = new Vector2(1.0f, 0.5f);
        [field: SerializeField] public Vector2 GroundSensorSize { get; private set; } = new Vector2(0.3f, 0.1f);
    }
}