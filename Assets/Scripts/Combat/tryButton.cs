using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class tryButton : MonoBehaviour
{
    public GameObject textBox;

    void Start()
    {
        textBox.SetActive(false);
    }

    public void GetClicked()
    {
        textBox.SetActive(true);
    }


}
