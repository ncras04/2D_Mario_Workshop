using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    bool isCollected = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCollected)
            return; 

        if (collision.collider.CompareTag("Player"))
        {
            isCollected = true;
            gameObject.SetActive(false);
            Game.Manager.GetCoin();
        }
    }


}
