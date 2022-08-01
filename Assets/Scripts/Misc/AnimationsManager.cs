using UnityEngine;

public class AnimationsManager
{
    [Header("Animations")]
    private static readonly int PatrolAnimation = Animator.StringToHash("Patrol");
    private static readonly int DeathAnimation = Animator.StringToHash("Death");
    private static readonly int HitAnimation = Animator.StringToHash("Hit");
    private static readonly int AttackAnimation = Animator.StringToHash("Attack");

    [Header("Player Animations")]
    private static readonly int HorizontalSpeed = Animator.StringToHash("Horizontal_speed");
    private static readonly int VerticalSpeed = Animator.StringToHash("Vertical_speed");
    private static readonly int Grounded = Animator.StringToHash("IsGrounded");
    private static readonly int Pushing = Animator.StringToHash("isPushing");

    private readonly Animator animator;

    public AnimationsManager(Animator animator)
    {
        this.animator = animator;
    }

    internal void Movement(bool state) => animator.SetBool(PatrolAnimation, state);
    internal void Movement(float valueX, float valueY, bool groundedState, bool pushingState)
    {
        animator.SetFloat(HorizontalSpeed, Mathf.Abs(valueX));
        animator.SetFloat(VerticalSpeed, valueY);
        animator.SetBool(Grounded, groundedState);
        animator.SetBool(Pushing, pushingState);
    }

    internal void Death() => animator.SetTrigger(DeathAnimation);
    internal void Death(bool state) => animator.SetBool(DeathAnimation, state);
    internal void Hit() => animator.SetTrigger(HitAnimation);
    internal void Attack() => animator.SetTrigger(AttackAnimation);
}