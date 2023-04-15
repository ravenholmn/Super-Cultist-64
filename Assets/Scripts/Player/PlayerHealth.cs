using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    
    public void TakeDamage()
    {
        health--;
        
        RegainHealth();

        if (health <= 0)
        {
            PlayerController.Instance.Die();
        }
    }

    private void RegainHealth()
    {
        StopAllCoroutines();
        StartCoroutine(RegainHealthCor());
    }

    private IEnumerator RegainHealthCor()
    {
        yield return new WaitForSeconds(5);
        health++;
        PlayerController.Instance.HeartUI.UpdateUI(health);
    }

    public void Die()
    {
        health = 0;
    }

    public void Respawn()
    {
        StopAllCoroutines();
        health = 3;
    }
}
