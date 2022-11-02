using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class units : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int power;

    public int maxHP;
    public int currentHP;

    public void takeDamage(int enemyPower)
    {
        currentHP -= enemyPower;
    }
}
