using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWepon : MonoBehaviour
{
    public int attack1Damage = 20;
    public int attack2Damage = 40;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    public void Attack1()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Debug.Log($"Attack Mask: {LayerMask.LayerToName(attackMask.value)}");

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        if (colInfo != null)
        {
            var hero = colInfo.GetComponent<HeroKnight>();
            if (hero != null)
            {
                hero.TakeDamage(attack1Damage);
            }

            colInfo.GetComponent<HeroKnight>().TakeDamage(attack1Damage);
        }
    }

    public void Attack2()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        if (colInfo != null)
        {
            colInfo.GetComponent<HeroKnight>().TakeDamage(attack2Damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
}
