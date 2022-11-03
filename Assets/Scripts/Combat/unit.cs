using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum unitType { GENTIL, MECHANT }

public class unit : MonoBehaviour
{
    public unitType thisUnitType;
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
