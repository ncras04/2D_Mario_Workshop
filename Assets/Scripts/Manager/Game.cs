using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Manager { get; private set; }

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
            Time.timeScale = 0;
        }    

    }

    private void ChangeScene()
    {

    }

}
