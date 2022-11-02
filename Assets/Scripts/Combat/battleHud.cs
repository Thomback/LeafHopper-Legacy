using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battleHud : MonoBehaviour
{
    public Slider hpSlider;

    public void battleHudSetup(units selfUnits)
    {
        hpSlider.maxValue = selfUnits.maxHP;
        hpSlider.value = selfUnits.currentHP;
    }

    public void changeHP(int newValue)
    {
        hpSlider.value = newValue;
    }
}
