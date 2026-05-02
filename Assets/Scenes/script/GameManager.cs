using UnityEngine;
using TMPro; // Đừng quên thêm TextMeshPro nếu bạn dùng nó

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI missText;
    public TextMeshProUGUI timerText;

    private int score = 0;
    private int misses = 0;
    private float timer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Cập nhật thời gian chơi
        timer += Time.deltaTime;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void AddMiss()
    {
        misses++;
        if (misses >= 3)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score: " + score;
        if (missText) missText.text = "Miss: " + misses + "/3";
        if (timerText) timerText.text = "Time: " + timer.ToString("F2") + "s";
    }

    void GameOver()
    {
        Debug.Log("GAME OVER!");
        Time.timeScale = 0; // Dừng game
    }
}
