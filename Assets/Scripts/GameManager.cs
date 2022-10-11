using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int playerScore = 0;

    List<GameObject> enemies = new List<GameObject>();
    [SerializeField]int enemyCount;

    private void Awake()
    {
        Instance = this;
        
    }

    private void OnEnable()
    {
        EnemyController.OnKilledEvent += EnemyController_OnKilledEvent;
    }

    private void Start()
    {
        GameObject[] tempEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject item in tempEnemies)
            enemies.Add(item);

        UIController.Instance.UpdateEnemyCountText(GetEnemyCount());
    }

    private void EnemyController_OnKilledEvent(GameObject obj)
    {
        enemies.Remove(obj);

        UIController.Instance.UpdateEnemyCountText(GetEnemyCount());

        if (GetEnemyCount() == 0) WinGame();
    }

    public int GetEnemyCount()
    {   
        return enemies.Count;
    }

    public void AddScore(int score)
    {
        playerScore += score;

        UIController.Instance.UpdateScoreText(playerScore);
    }

    public void LoseGame()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void WinGame()
    {
        SceneManager.LoadScene("GameWin");
    }

    public int GetScore() => playerScore;
}
