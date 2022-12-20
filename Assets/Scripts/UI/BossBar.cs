using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    private Image healthBar;
    private float maxHealth;

    public BossMovement boss;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        maxHealth = boss.bossHP;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)boss.bossHP / maxHealth;
    }
}
