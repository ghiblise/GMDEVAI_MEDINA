using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int maxHealth = 100;
    public int curHealth;
    public TextMeshProUGUI healthUI;

    void Start()
    {
        curHealth = maxHealth;
        healthUI.text = "" + curHealth;
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        healthUI.text = "" + curHealth;

        if (curHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
