using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanelController : MonoBehaviour
{
    public static LoadingPanelController instance;
    public SO_ScenesData ScenesData;
    public Transform loadPanel;
    public Image fillImage;
    public Image imageToTurn;
    public bool loadHubWorld;
    private int _nextSceneIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        if (!loadHubWorld)
        {
            ScenesData.nextSceneIndex++;
            ScenesData.nextSceneIndex = Mathf.Clamp(ScenesData.nextSceneIndex, 0, ScenesData.sceneDataList.Count - 1);
            _nextSceneIndex = ScenesData.nextSceneIndex;
        }

        ScenesData.activeSceneIndex = _nextSceneIndex;

        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        var wait = new WaitForEndOfFrame();
        float t = 0;
        float duration = 3f; 
        
        loadPanel.gameObject.SetActive(true);

        while (t < duration)
        {
            t += Time.deltaTime;

            imageToTurn.transform.Rotate(0f, 0f, -1f);
            
            yield return wait;
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(_nextSceneIndex + 1);
        
        while (!operation.isDone)
        {
            imageToTurn.transform.Rotate(0f, 0f, -1f);
            
            yield return wait;
        }
    }
}
