using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public  float gameSpeed { get; set;}
    private Player player;
    private Spawner spawner;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    private float score;
    private int highScore;
    private string filePath;
    private bool isPaused = false;
    public TextMeshProUGUI pausedText;
    private int lastMilestone = 0;
    private float invincibilityIncrease = 200;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void onDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        filePath = Application.persistentDataPath + "/HighScore.json";
        player = FindAnyObjectByType<Player>();
        spawner = FindAnyObjectByType<Spawner>();
        LoadHighScore();  
        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        gameSpeed = initialGameSpeed;
        enabled = true;
        score = 0;
        lastMilestone = 0;
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        pausedText.gameObject.SetActive(false);
        invincibilityIncrease = 200;
        player.init();
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        UpdateHighScore();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
        if (!isPaused)
        {
            gameSpeed += gameSpeedIncrease * Time.deltaTime;
            score += gameSpeed * Time.deltaTime;
            scoreText.text = Mathf.FloorToInt(score).ToString();
            int currentMilestone = Mathf.FloorToInt(score / invincibilityIncrease);
            if (currentMilestone > lastMilestone)
            {
                lastMilestone = currentMilestone;
                Debug.Log(score);
                player.ActivateInvincibility(5f,2f);
                invincibilityIncrease +=  invincibilityIncrease * gameSpeedIncrease;
            }
        }
    }

    private void UpdateHighScore()
    {
        int finalScore = Mathf.FloorToInt(score);

        if (finalScore > highScore)
        {
            highScore = finalScore;
            SaveHighScore();
        }
    }
    private void SaveHighScore()
    {
        HighScoreData data = new HighScoreData { highScore = highScore };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        highScoreText.text = "HighScore: " + highScore;
        Debug.Log("High Score Saved: " + highScore);
    }

    private void LoadHighScore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            highScore = data.highScore;
        }
        else
        {
            highScore = 0;
        }
        highScoreText.text = "HighScore: " + highScore;
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Time.timeScale = 0f;
            pausedText.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            pausedText.gameObject.SetActive(false);
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure the game runs normally when returning
        SceneManager.LoadScene("MainMenu"); // Change "MainMenu" if your scene has a different name
    }
}

[System.Serializable]
public class HighScoreData
{
    public int highScore;
}

