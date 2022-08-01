using System.Threading.Tasks;
using UnityEngine;

public class GoblinEnemy : Enemy
{
    [Header("Component References")]
    private AnimationsManager animations;

    [Header("Settings")]
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private Transform[] patrolPoints;

    private int positionIndex = 0;
    private bool facingRight = true;
    private bool isAlive = true;
    private bool isAttacking = false;

    private Vector2 lastPosition;
    private Collider2D collisionBox;

    private void Awake()
    {
        animations = new AnimationsManager(GetComponentInChildren<Animator>());
        collisionBox = GetComponent<Collider2D>();

        lastPosition = transform.position;
    }

    private void Update()
    {
        Movement();
        Attack();
    }

    private async void Attack()
    {
        if (!isAlive)
            return;

        var animationTime = 500;    //millis
        var rayCast = Physics2D.RaycastAll(transform.position, transform.right, attackRange);

        foreach (var rc in rayCast)
        {
            var player = rc.collider.GetComponent<IPlayer>();

            if (!isAttacking && player != null)
            {
                isAttacking = true;
                animations.Attack();
                player.TakeDamage(transform);

                await Task.Delay(animationTime);
                isAttacking = false;
            }
        }
    }

    private void Movement()
    {
        Vector2 actualPosition = transform.position;

        if (actualPosition.x - lastPosition.x > 0 && !facingRight)
            Flip();
        else if (actualPosition.x - lastPosition.x < 0 && facingRight)
            Flip();

        if (isAlive && !isAttacking && patrolPoints.Length > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[positionIndex].position,
                Time.deltaTime * movementSpeed);

            if (transform.position == patrolPoints[positionIndex].position)
            {
                var incrementIndex = positionIndex + 1;
                positionIndex = (positionIndex == patrolPoints.Length - 1) ? 0 : incrementIndex;

                //Debug.Log($"[{gameObject.name}] Reached point: {patrolPoints[positionIndex].name}.");
            }
        }

        lastPosition = actualPosition;
        animations.Movement(patrolPoints.Length > 0);
    }
    private void Flip()
    {
        // Switch the way the character is labelled as facing.
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public override async void TakeDamage()
    {
        var animationTime = 500;    //millis
        Health--;

        if (Health <= 0)
        {
            isAlive = false;
            animations.Death();
            collisionBox.enabled = false;

            await Task.Delay(animationTime);
            gameObject.SetActive(false);

            return;
        }
        animations.Hit();
    }
}