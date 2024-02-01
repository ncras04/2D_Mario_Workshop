using System.Collections;
using UnityEditor;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Manager { get; private set; }

    [SerializeField]
    float m_switchToMainMenuCount;

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
    public void GetCoin()
    {
        coinAmount += 1;
        UI.Manager.SetCoinUI(coinAmount);
        Audio.Manager.PlaySound(ESounds.COIN);
    }
    public void ChangeMainMenu()
    {
        StartCoroutine(SwitchToMain());
    }

    private IEnumerator SwitchToMain()
    {
        yield return new WaitForSeconds(m_switchToMainMenuCount);

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
}
