using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to place once per level, references all entrypoints to the level
/// </summary>
public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private LoaderPoint[] myLoaders;                // List of loaderPoints, to reference in editor

    private void Start()
    {
        for (int i = 0; i < myLoaders.Length; i++)
        {
            myLoaders[i].LinkNewLevelLoader(this);
        }
    }

    /// <summary>
    /// Level transition animation starter
    /// </summary>
    /// <param name="sceneName">Name of next scene</param>
    /// <param name="nextSceneLoadingNumber">myLoaders[] index for NEXT SCENE</param>
    /// <param name="transitionDirection">Direction of transition</param>
    public void TransitionStart(string sceneName, int nextSceneLoadingNumber, LoaderPoint.LoaderDirection transitionDirection)
    {
        LevelLoaderManager.Instance.Transitionning(sceneName, nextSceneLoadingNumber, transitionDirection);
    }
}
