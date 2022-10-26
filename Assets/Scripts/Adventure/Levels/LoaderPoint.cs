using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class to place on level entrance/exit to load next zone
/// </summary>
public class LoaderPoint : MonoBehaviour
{
    public LoaderDirection myDirection = LoaderDirection.East;
    public Transform mySpawnPoint;
    [SerializeField]
    private string myLoadingSceneName = "";
    [SerializeField]
    private int myNextSceneLoadingNumber = 0;

    private LevelLoader myLevelLoader;
    private bool isTriggerActive = true;

    public enum LoaderDirection
    {
        North,
        East,
        South,
        West
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggerActive)
        {
            if(other.gameObject.tag == "Player")
            {
                myLevelLoader.TransitionStart(myLoadingSceneName, myNextSceneLoadingNumber, myDirection);
            }
        }
    }


    public void LinkNewLevelLoader(LevelLoader newLevelLoader)
    {
        myLevelLoader = newLevelLoader;
    }

}
