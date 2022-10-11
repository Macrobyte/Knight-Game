using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI enemyCountText;
    [SerializeField] Image healthBar;

    private void Awake()
    {
        Instance = this;  
    }

    public void UpdateHealthBar(float amount)
    {
        healthBar.fillAmount = amount;
    }
    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateEnemyCountText(int amount)
    {
        enemyCountText.text = amount.ToString();
    }
}
