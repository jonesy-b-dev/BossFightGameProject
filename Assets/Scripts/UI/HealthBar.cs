using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    private float maxHealth = 10;

    public PlayerController pc;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        maxHealth = (float)pc.hp;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)pc.hp / maxHealth;
    }
}
