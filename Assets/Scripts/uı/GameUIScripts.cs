using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIScripts : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Health playerHealth;

    [Header("UI references")]

    [SerializeField] private TextMeshProUGUI playerHealthValue;

    [SerializeField] private TextMeshProUGUI enemyCount;

    [SerializeField] private TextMeshProUGUI currentWaveCount;
    [SerializeField] private TextMeshProUGUI maxWaveCount;

    [SerializeField] private GameObject getReadyMessage;

    [SerializeField] private GameObject endGameTitle;
    [SerializeField] private Button endGameButton;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }


    public void UpdatePlayerHealth(string value) 
    {
        playerHealthValue.text = value;
    }

    public void UpdateEnemyCount(string value) 
    {
        enemyCount.text = value;
    }

    public void UpdateCurrentWaveCount(string value) 
    {
        currentWaveCount.text = value;
    }

    public void UpdateMaxWaveCount(string value) 
    {
        maxWaveCount.text = value;
    }

    public void EnableDisableGetReadyMessage() 
    {
        if (getReadyMessage.activeSelf) getReadyMessage.SetActive(false);
        else getReadyMessage.SetActive(true);
    }

    public void EnableEndGameTitle() 
    {
        endGameTitle.SetActive(true);
    }

    public void EnableEndGameButton()
    {
        endGameButton.gameObject.SetActive(true);
        endGameButton.onClick.AddListener(delegate { Application.Quit();});
    }
}