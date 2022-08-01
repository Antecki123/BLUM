using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private InputControls inputControls;
    private PlayerEntity player;
    private Rigidbody2D rb;
    private PlayerAttack attack;

    private float horizontalMovement;
    private bool facingRight = true;
    private float attackTimer = 0.0f;

    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;

    private void Start()
    {
        player = GetComponent<PlayerEntity>();
        rb = GetComponent<Rigidbody2D>();

        attack = new PlayerAttack(player);
        attackTimer = player.PlayerSettings.AttackDelay;
    }

    private void OnEnable()
    {
        inputControls = new InputControls();
        inputControls.Enable();

        inputControls.Gameplay.HorizontalMovement.performed += ctx => horizontalMovement = ctx.ReadValue<float>();
        inputControls.Gameplay.HorizontalMovement.canceled += ctx => horizontalMovement = 0;

        inputControls.Gameplay.Jump.started += ctx => Jump();
        inputControls.Gameplay.Attack.started += ctx => Attack();

        inputControls.Gameplay.Crouch.performed += ctx => Crouch(true);
        inputControls.Gameplay.Crouch.canceled += ctx => Crouch(false);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Move the character by finding the target velocity
        if (!player.IsDead)
        {
            rb.velocity = new Vector2(player.PlayerSettings.RunSpeed * horizontalMovement, rb.velocity.y);

            if (horizontalMovement > 0 && !facingRight)
                Flip();
            else if (horizontalMovement < 0 && facingRight)
                Flip();
        }
        else
        {
            inputControls.Disable();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        player.Animations.Movement(horizontalMovement, rb.velocity.y, IsGrounded(), IsPushing());
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Jump()
    {
        if (IsGrounded() && !player.IsDead)
        {
            rb.velocity = Vector2.up * player.PlayerSettings.JumpSpeed;
        }
    }

    private void Crouch(bool state)
    {
        if (state)
        {
            virtualCameras[0].m_Priority = 0;
            virtualCameras[1].m_Priority = 10;
        }
        else
        {
            virtualCameras[0].m_Priority = 10;
            virtualCameras[1].m_Priority = 0;
        }
    }

    private void Attack()
    {
        if (player.PlayerSettings.AttackDelay <= attackTimer)
        {
            StopAllCoroutines();
            StartCoroutine(AttackCountdown());

            player.Animations.Attack();
            attack.Attack();
        }
    }

    private IEnumerator AttackCountdown()
    {
        attackTimer = 0.0f;

        while (player.PlayerSettings.AttackDelay > attackTimer)
        {
            attackTimer += Time.deltaTime;
            yield return null;
        }
    }

    private bool IsPushing()
    {
        // Checks if the character is pushing object
        RaycastHit2D raycastHit = Physics2D.BoxCast(player.PlayerSettings.AttackPoint.position,
            player.PlayerSettings.GroundSensorSize, 0.0f, Vector2.zero, 1.0f, 1024);

        return raycastHit;
    }

    private bool IsGrounded()
    {
        // Checks if the character is on the ground
        RaycastHit2D raycastHit = Physics2D.BoxCast(player.PlayerSettings.GroundCollider.position,
            player.PlayerSettings.GroundSensorSize, 0.0f, Vector2.zero);

        return raycastHit;
    }
}