using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int health = 500;

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageAnimation());

        if (health <= 200)
        {
            GetComponent<Animator>().SetBool("IsStage2", true);
        }

        if (health <= 0)
        {
            StartCoroutine(PlayDeathAndWait());
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator DamageAnimation()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 0;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);

            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 1;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    public IEnumerator PlayDeathAndWait()
    {
        // Trigger the animation
        GetComponent<Animator>().SetTrigger("Death");
        
        // Wait until the animation state is active
        while (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            yield return null;
        }

        // Wait until the animation is complete
        while (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        Die();
    }
}
