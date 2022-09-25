using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderManager : MonoBehaviour
{
    public static LevelLoaderManager Instance { get; private set; } //Singleton
    [SerializeField]
    private PlayerController thePlayerController;
    [SerializeField]
    private Animator UIAnimator;

    private bool isTransitionning = false;

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

    public void Transitionning(string sceneName, int nextSceneLoadingNumber, LoaderPoint.LoaderDirection transitionDirection)
    {
        if (!isTransitionning)
        {
            isTransitionning = true;

            StartCoroutine(Transition(sceneName));
        }
    }

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
