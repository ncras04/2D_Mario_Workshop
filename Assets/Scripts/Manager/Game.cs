using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    public static Game Manager { get; private set; }

    [SerializeField]
    float switchToMainMenuCount;

    [SerializeField]
    TextMeshProUGUI coinUI;

    private int coinAmount = 0;



    private void Awake()
    {
        if (Manager != null)
        {
            Destroy(gameObject);
            return;
        }
        Manager = this;


        Init();
    }

    private void Init()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(PlayerContr.Player.CurrentState == EPlayerStates.DEAD)
        {
            ChangeMainMenu();
            Time.timeScale = 0;
        }    

    }

    public void GetCoin()
    {
        coinAmount += 1;
        coinUI.text = "Coins: " + coinAmount.ToString();
        Audio.Manager.PlaySound(ESounds.COIN);
    }
    public void ChangeMainMenu()
    {
        switchToMainMenuCount -= Time.unscaledDeltaTime;

        if (switchToMainMenuCount < 0)
            SceneManager.LoadScene(0);
    }
}
