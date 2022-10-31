using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class to place on level entrance/exit to load next zone
/// </summary>
public class LoaderPoint : MonoBehaviour
{
    public LoaderDirection myDirection = LoaderDirection.East;  // Which level direction the loader is facing, default to East
    public Transform mySpawnPoint;                              // Where the player will spawn when appearing on this loader
    [SerializeField]
    private string myLoadingSceneName = "";                     // Name of scene I'll be loading when used
    [SerializeField]
    private int myNextSceneLoadingNumber = 0;                   // Index of loader point I'm linked to in next scene

    private LevelLoader myLevelLoader;                          // Unique LevelLoader linked to me in this scene
    private bool isTriggerActive = true;                        // Am I active or not

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
