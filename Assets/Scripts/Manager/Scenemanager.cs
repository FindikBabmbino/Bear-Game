using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{

    private void Start()
    {
    }

    public void CallScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CallSceneAdditive(string sceneName) 
    {
        SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
    }
}