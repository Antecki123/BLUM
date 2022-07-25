using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Animator animator;

    [Header("Hashed Animations")]
    private static readonly int Move = Animator.StringToHash("move");
    private static readonly int Shoot = Animator.StringToHash("shoot");
    private static readonly int Death = Animator.StringToHash("death");
    private static readonly int GetHit = Animator.StringToHash("getHit");
    private static readonly int Attack = Animator.StringToHash("attack");


    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void MovementAnimation(bool state)
    {
        animator.SetBool(Move, state);
    }
}
