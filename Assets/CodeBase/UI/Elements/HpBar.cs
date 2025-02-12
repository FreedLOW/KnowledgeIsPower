﻿using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class HpBar : MonoBehaviour
    {
        public Image healthBar;

        public void SetValue(float currentHP, float maxHP) => 
            healthBar.fillAmount = currentHP / maxHP;
    }
}