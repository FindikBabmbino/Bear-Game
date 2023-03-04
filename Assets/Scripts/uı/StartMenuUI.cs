using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    private Button startBtn;
    private Button quitBtn;

    private Scenemanager scenemanager;

    

    private void Awake()
    {
        startBtn = transform.Find("Panel").transform.Find("StartBtn").GetComponent<Button>();
        quitBtn = transform.Find("Panel").transform.Find("QuitBtn").GetComponent<Button>();

        scenemanager = GameObject.FindFirstObjectByType<Scenemanager>();

        startBtn.onClick.AddListener(delegate { scenemanager.CallScene("StartStoryLevel"); });
        quitBtn.onClick.AddListener(Application.Quit);
    }


}
