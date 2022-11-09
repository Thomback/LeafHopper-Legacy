using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum unitType { GENTIL, MECHANT }


public class unit : MonoBehaviour
{
    public unitType thisUnitType;
    public GameObject selfGO;
    public string unitName;
    public int unitLevel;

    public int power;
    public int powerMultiplier;

    public int maxHP;
    public int currentHP;

    //Test si l'unit a survécu
    public bool isAlive()
    {
        if (this.currentHP > 0)
            return true;
        return false;
    }

    //Routine quand l'unit prend des dégats
    public void takeDamage(int enemyPower)
    {
        currentHP -= enemyPower;
        this.GetComponent<battleHud>().changeHP(currentHP);
        if (!isAlive())
        {
            dying();
        }
    }

    //Routine si l'unit meurt
    public void dying()
    {       
        selfGO.SetActive(false);
    }

    public int getCurrentPower()
    {
        return power * powerMultiplier;
    }
}
