using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battleHud : MonoBehaviour
{
    public Slider hpSlider;
    public GameObject highlightPanel;
    public GameObject selectionPanel;

    public void battleHudSetup(unit selfUnit)
    {
        //Set la barre d'HP de l'unit� dans son hud
        hpSlider.maxValue = selfUnit.maxHP;
        hpSlider.value = selfUnit.currentHP;

        //D�sactive le halo de s�lection de l'unit�
        highlightPanel.SetActive(false);
        selectionPanel.SetActive(false);

    }

    //Modifie la barre d'HP de l'unit quand il y a un changement
    public void changeHP(int newValue)
    {
        hpSlider.value = newValue;
    }

    //Routine qui se lance quand c'est le d�but de l'unit
    public void turnActive()
    {
        doHighlightPanel();
    }

    public void doHighlightPanel()
    {
        highlightPanel.SetActive(true);

    }

    public void turnEnd()
    {
        unHighlightPanel();
    }

    public void unHighlightPanel()
    {
        highlightPanel.SetActive(false);

    }

    public void selectableActive()
    {
        selectionPanel.SetActive(true);
    }

    public void selectableDesactive()
    {
        selectionPanel.SetActive(false);
    }
}
