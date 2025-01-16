using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    public int health = 0;
    public Slider healthBar;    // Reference to the health bar slider
    public Vector3 healthBarOffset = new Vector3(2, 2, 0); // Offset for the health bar above the boss
    private Camera mainCamera;
    public GameObject dropItemPrefab; // Reference to the pickup prefab

    private void Start()
    {
        mainCamera = Camera.main;

        health = maxHealth;

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }

    private void Update()
    {
        // Update the position of the health bar to stay above the boss
        if (healthBar != null)
        {
            Vector3 worldPosition = transform.position + healthBarOffset;
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

            healthBar.transform.position = screenPosition;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageAnimation());

        if (healthBar != null)
        {
            healthBar.value = Mathf.Clamp(health, 0, maxHealth);
        }

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
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }
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
        if (healthBar != null)
        {
            healthBar.value = 0;
        }

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
