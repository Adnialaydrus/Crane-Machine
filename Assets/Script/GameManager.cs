using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;

    public static GameManager Instance;

    public TextMeshProUGUI coinText;
    private int coin = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DecreaseCoin(int amount)
    {
        coin -= amount;
        UpdateCoinUI();
    }

    public void IncreaseCoin(int amount)
    {
        coin += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = "Coin: " + coin.ToString();
    }

    private void Update()
    {
        if(coin == 0)
        {
            text.text = "You Lose!";
            panel.SetActive(true);
        }
    }
}
