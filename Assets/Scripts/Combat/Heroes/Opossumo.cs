using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossumo : hero
{
    public override int skill1(actionStep playerTSU)
    {
        if (playerTSU == actionStep.SKILLRESOLVE)
        {
            Debug.Log("Skill 1 de Opossumo en SKILL. Power : " + getSelfUnit().getCurrentPower());
            //On veut une attaque à hauteur de la power.
            return getSelfUnit().getCurrentPower();
        }
        if (playerTSU == actionStep.FURESOLVE)
        {
            Debug.Log("Skill 1 de Opossumo en FU. Power : " + getSelfUnit().getCurrentPower());
            //On veut une attaque à hauteur de la power.
            return getSelfUnit().getCurrentPower();
        }
        return 0;
    }
}