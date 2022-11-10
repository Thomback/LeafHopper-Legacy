using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renardo : hero
{
    public override int skill1(actionStep playerTSU)
    {
        if (playerTSU == actionStep.SKILLRESOLVE)
        {
            Debug.Log("Skill 1 de Renardo en SKILL. Power : " + getSelfUnit().getCurrentPower());
            //On veut une attaque à hauteur de la power.
            return getSelfUnit().getCurrentPower();
        }
        if (playerTSU == actionStep.FURESOLVE)
        {
            Debug.Log("Skill 1 de Renardo en FU. Power : " + getSelfUnit().getCurrentPower());
            //On veut une attaque à hauteur de la power.
            return getSelfUnit().getCurrentPower();
        }
        return 0;
    }
}
