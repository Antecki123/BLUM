using UnityEngine;

public class PlayerAttack
{
    private PlayerEntity player;

    public PlayerAttack(PlayerEntity player)
    {
        this.player = player;
    }

    public void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCapsuleAll(player.PlayerSettings.AttackPoint.position,
            player.PlayerSettings.AttackPointSize, CapsuleDirection2D.Horizontal, 0f);

        foreach (var h in hitColliders)
        {
            var damageable = h.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage();
            }
        }
    }
}
