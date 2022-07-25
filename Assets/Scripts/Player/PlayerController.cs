using System.Collections;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerEntity playerStats;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private Transform groundCollider;
    [SerializeField] private Transform attackPoint;

    [Header("Controller Properties")]
    [SerializeField, Range(0, 50)] private float runSpeed = 15f;
    [SerializeField, Range(0, 50)] private float jumpSpeed = 30f;
    [Space]
    [SerializeField, Range(0, 5)] private float attackDelay = 1.1f;
    private float attackTimer = 0.0f;

    [Header("Animations Hashes")]
    private static readonly int SpeedAnimation = Animator.StringToHash("Speed");
    private static readonly int VerticalVelocityAnimation = Animator.StringToHash("Vertical_velocity");
    private static readonly int AttackAnimation = Animator.StringToHash("Attack");


    private bool isJump;
    private float horizontalMove;
    private bool facingRight = true;

    private readonly Vector2 groundSensorSize = new Vector2(0.5f, 0.1f);
    private readonly Vector2 attackPointSize = new Vector2(1.0f, 0.5f);

    private InputControls inputActions;
    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Awake()
    {
        inputActions = new InputControls();

        inputActions.Gameplay.HorizontalMovement.performed += ctx => horizontalMove = ctx.ReadValue<float>();
        inputActions.Gameplay.HorizontalMovement.canceled += ctx => horizontalMove = 0;

        inputActions.Gameplay.Jump.performed += ctx => isJump = true;
        inputActions.Gameplay.Jump.canceled += ctx => isJump = false;

        inputActions.Gameplay.Attack.performed += ctx => Attack();
    }

    private void FixedUpdate()
    {
        Jump();
        Move(horizontalMove);
    }

    private void LateUpdate()
    {
        animator.SetFloat(VerticalVelocityAnimation, Mathf.Abs(rb2D.velocity.y));
        animator.SetFloat(SpeedAnimation, Mathf.Abs(horizontalMove));
    }

    private void Jump()
    {
        if (IsGrounded() && isJump)
            rb2D.velocity = Vector2.up * jumpSpeed;
    }

    private void Move(float move)
    {
        // Move the character by finding the target velocity
        if (!playerStats.IsDead)
            rb2D.velocity = new Vector2(runSpeed * move, rb2D.velocity.y);
        else
            rb2D.velocity = Vector2.zero;

        if (horizontalMove > 0 && !facingRight)
            Flip();
        else if (horizontalMove < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Attack()
    {
        if (attackDelay <= attackTimer)
        {
            attackTimer = 0.0f;

            animator.SetTrigger(AttackAnimation);
            var hitColliders = Physics2D.OverlapCapsule(attackPoint.position, attackPointSize, CapsuleDirection2D.Horizontal, 0f);
        }

        StartCoroutine(AttackCountdown());
    }

    private IEnumerator AttackCountdown()
    {
        while (attackTimer <= attackDelay)
        {
            attackTimer += Time.deltaTime;
            yield return null;
        }
    }

    private bool IsGrounded()
    {
        // Checks if the character is on the ground
        RaycastHit2D raycastHit = Physics2D.BoxCast(groundCollider.position, groundSensorSize, 0.0f, Vector2.zero);

        return raycastHit;
    }
}