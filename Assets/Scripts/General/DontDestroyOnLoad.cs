using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to reference each GameObject we want to keep between scenes.
/// Ex: Player, game managers, UI...
/// </summary>
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
