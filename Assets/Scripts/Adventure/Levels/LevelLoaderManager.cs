using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Big papa class for managing all scene transitions and loading
/// </summary>
public class LevelLoaderManager : MonoBehaviour
{
    public static LevelLoaderManager Instance { get; private set; } // Singleton to have a unique instance
    [SerializeField]
    private PlayerController thePlayerController;                   // Reference to player controller
    [SerializeField]
    private Animator UIAnimator;                                    // Reference to the Animator component for UI

    private bool isTransitionning = false;                          // If true, scene transition in process

    /// <summary>
    /// In awake, check if other instances exists, otherwise create one.
    /// There can only be one.
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple LevelLoaderManager Error");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// In Start, find the player controller component if not set in editor.
    /// </summary>
    void Start()
    {
        if(thePlayerController == null)
        {
            thePlayerController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        }
    }


    void Update()
    {
        
    }

    /// <summary>
    /// Start transition to next scene. There is only one n in transitioning, I'm aware.
    /// UNDER CONSTRUCTION
    /// </summary>
    /// <param name="sceneName">Name of next scene</param>
    /// <param name="nextSceneLoadingNumber">myLoaders[] index for NEXT SCENE</param>
    /// <param name="transitionDirection">Direction of transition</param>
    public void Transitionning(string sceneName, int nextSceneLoadingNumber, LoaderPoint.LoaderDirection transitionDirection)
    {
        if (!isTransitionning)
        {
            isTransitionning = true;

            StartCoroutine(Transition(sceneName));
        }
    }

    /// <summary>
    /// Coroutine used to manage transition animation
    /// </summary>
    /// <param name="sceneName">Scene name to load while transitioning</param>
    private IEnumerator Transition(string sceneName)
    {
        UIAnimator.Play("FadeOutBlack");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitForSeconds(0.3f);
        UIAnimator.Play("FadeInBlack");
        yield return new WaitForSeconds(1);
        isTransitionning = false;
    }
}
