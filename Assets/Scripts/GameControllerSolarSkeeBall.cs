using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;

public class GameControllerSolarSkeeBall : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameControllerSolarSkeeBall instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("Transition Scene")]
    [SerializeField] public Animator transition;
    [SerializeField] public float transitionTime = 1f;
    [SerializeField] public GameObject transitionScene;

    [Header("SpawnPoint Variables")]
    [SerializeField] public Transform ballSpawnPoints;

    //[SerializeField] public List<Vector3> SpawnPointList = new List<Vector3>();
    //[Header("UI How To Play")]
    //[SerializeField] public Image tutorial;
    //[SerializeField] public Image catalogSpecies;

    [Header("Prefab Balls")]
    [SerializeField] public GameObject ballPrefab;
    private GameObject currentBall;

    [Header("UI display")]
    public TextMeshProUGUI scoreText; //Track Player current score

    public TextMeshProUGUI ballsLeftText; //Track how many balls went in the game

    public TextMeshProUGUI countdownText; //Count down when player press Start

    //public GameObject gameOverPanel; // Ends when player finish all 9 balls

    public GameObject buttonLobby; //Return Player to the Lobby

    public GameObject countdown; //

    public GameObject buttonPress; //Start Button for game

    public GameObject resetButton;

    [Header("UI Scoreboard")]
    public TextMeshProUGUI highScoreSkeeBallText;

    private int scoreSkeeBall;

    private int highScoreSkeeBall;

    private int ballsLeft = 9;

    private bool ballSpawned = false;

    private bool gameStarted = false;
    //public HoleTrigger[] holes;
    private void Start()
    {
//Instantiate(ballPrefab, ballSpawnPoints.position, ballSpawnPoints.rotation);
        countdown.SetActive(false);
        resetButton.SetActive(false);
        ballsLeftText.text = "Balls Left: " + ballsLeft;
        highScoreSkeeBall = PlayerPrefs.GetInt("HighScoreSkeeBall", 0);
        UpdateScoreboard();
    }
    public void UpdateScoreboard()
    {
        scoreText.text = "Score: " + scoreSkeeBall;
        highScoreSkeeBallText.text = "High Score: " + highScoreSkeeBall;
    }
    public void StartFunction()
    {
        if (gameStarted)
        {
            return;
        }

        gameStarted = true;
        buttonPress.SetActive(false);
        scoreSkeeBall = 0;
        if (!ballSpawned)
        {
            StartCoroutine(SpawnBallAfterCountdown());
        }
        resetButton.SetActive(true);
    }

    public void LoadLobbyScene()
    {
        transitionScene.SetActive(true);
        //int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 2));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        if (levelIndex >= 0)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogWarning("No previous scene available.");
        }

        yield return new WaitForSeconds(levelIndex);

        instance = this;
        Destroy(this.gameObject);
        SceneManager.LoadScene(levelIndex);
        //transition.SetTrigger("End");
    }

    // Update is called once per frame
    public void Update()
    {
        /*if (ballsLeft > 0f && gameStarted)
        {
            BallUsed();
        }
        else if (ballsLeft <= 0f && gameStarted)
        {
            Finish();
        }*/
    }
    public void BallUsed()
    {
        ballsLeft--;
        UpdateBallsLeftText();

        if (ballsLeft <= 0)
        {
            Finish();
        }
    }

    void UpdateBallsLeftText()
    {
        ballsLeftText.text = "Balls Left: " + ballsLeft;
    }

    // Method to update the score
    public void UpdateScore(int points)
    {
        scoreSkeeBall += points;
        UpdateScoreboard();
        Debug.Log("adding point");
    }

    public void StartCountdown()
    {
        //buttonLobby.SetActive(false);
        buttonPress.SetActive(false);
        countdown.SetActive(true);
        countdownText.gameObject.SetActive(true);
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator SpawnBallAfterCountdown()
    {
        yield return StartCoroutine(CountdownCoroutine());

        currentBall = Instantiate(ballPrefab, ballSpawnPoints.position, ballSpawnPoints.rotation);
        ballSpawned = true;

    }
    private IEnumerator CountdownCoroutine()
    {
        int countdownTime = 3; // Countdown from 3

        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString(); // Update UI with countdown time
            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdownTime--; // Decrease countdown time
        }

        countdownText.text = "Roll On!"; // Display "GO!" when countdown finishes
        yield return new WaitForSeconds(1f); // Wait for 1 second after "GO!"

        countdownText.gameObject.SetActive(false); // Hide countdown text
        StartFunction(); // Start the game
        countdown.SetActive(false);
    }

    public void Finish()
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
            currentBall = null;
        }
        
        // Reset game state variables
        ballsLeft = 9;
        ballSpawned = false;
        gameStarted = false;

        // Update UI
        UpdateBallsLeftText();
        //switch score < highScore to reset the highscore count number
        if (scoreSkeeBall > highScoreSkeeBall)
        {
            highScoreSkeeBall = scoreSkeeBall;
            PlayerPrefs.SetInt("HighScoreSkeeBall", highScoreSkeeBall);
        }
        UpdateScoreboard();
        //buttonLobby.SetActive(true);
        resetButton.SetActive(false);
        buttonPress.SetActive(true);
        gameStarted = false;
        Debug.Log("Ball Prefab: " + ballPrefab); // Check the ballPrefab reference

        /*if (ballPrefab != null)
        {
            Destroy(ballPrefab); // Destroy the ballPrefab object
        }
        else
        {
            Debug.LogWarning("ballPrefab is null!"); // Log a warning if ballPrefab is null
        }*/
        //startButton.SetActive(true);
    }

}