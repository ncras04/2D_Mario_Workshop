using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Manager { get; private set; }

    [SerializeField]
    private TextMeshProUGUI m_coinDisplay;

    private void Awake()
    {
        if (Manager != null)
        {
            Destroy(gameObject);
            return;
        }
        Manager = this;
    }

    public void SetCoinUI(int _newAmount)
    {
        m_coinDisplay.text = $"Coins: {_newAmount}";
    }
}
