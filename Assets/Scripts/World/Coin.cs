using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool m_isCollected = false;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (m_isCollected)
            return;

        if (_collision.CompareTag("Player"))
        {
            m_isCollected = true;
            gameObject.SetActive(false);
            Game.Manager.GetCoin();
        }
    }
}
