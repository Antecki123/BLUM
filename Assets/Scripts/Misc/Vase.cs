using UnityEngine;

public class Vase : MonoBehaviour, IDamageable
{
    private Animator animator;
    private Collider2D collisionBox;

    private static readonly int DestroyAnimation = Animator.StringToHash("Destroy");

    [SerializeField] private GameObject contents;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collisionBox = GetComponent<Collider2D>();

        if (contents != null)
        {
            contents = Instantiate(contents, transform.position, transform.rotation);
            contents.SetActive(false);
            contents.transform.SetParent(transform);
        }
    }

    public void TakeDamage()
    {
        animator.SetTrigger(DestroyAnimation);
        collisionBox.enabled = false;

        if (contents != null)
        {
            contents.SetActive(true);
        }
    }
}