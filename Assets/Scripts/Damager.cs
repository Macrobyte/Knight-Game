using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] LayerMask damagerLayer;
    [SerializeField] float attackRange;
    [SerializeField] int damage;
    [SerializeField] Transform attackPoint;

    void Attack()
    {
        Collider2D[] enemyCheck = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damagerLayer);

        foreach (Collider2D enemy in enemyCheck)
        {
            if (enemy.TryGetComponent<Health>(out Health health))
            {
                health.ReduceHealth(damage);

                if(enemy.TryGetComponent<EnemyController>(out EnemyController eController))
                    eController.e_animator.Play(enemy.name + "Hurt");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}
