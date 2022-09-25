using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{

    [SerializeField]
    private GameObject[] notDestroyedObjects;
    void Start()
    {
        for (int i = 0; i < notDestroyedObjects.Length; i++)
        {
            DontDestroyOnLoad(notDestroyedObjects[i]);
        }
    }
}
