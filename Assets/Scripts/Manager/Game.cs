using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Manager { get; private set; }

    [SerializeField]
    float counter;



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

    }
    public void ChangeMainMenu()
    {
        counter -= Time.unscaledDeltaTime;

        if (counter < 0)
            SceneManager.LoadScene(0);
    }
}
