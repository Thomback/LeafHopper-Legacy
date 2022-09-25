using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private LoaderPoint[] myLoaders;

    private void Start()
    {
        for (int i = 0; i < myLoaders.Length; i++)
        {
            myLoaders[i].LinkNewLevelLoader(this);
        }
    }

    public void TransitionStart(string sceneName, int nextSceneLoadingNumber, LoaderPoint.LoaderDirection transitionDirection)
    {
        LevelLoaderManager.Instance.Transitionning(sceneName, nextSceneLoadingNumber, transitionDirection);
    }
}
