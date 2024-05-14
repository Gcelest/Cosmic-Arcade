using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

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

    [Header("UI How To Play")]
    [SerializeField] public Image tutorial;
    [SerializeField] public Image catalogSpecies;
    
    [Header("SpawnPoint Variables")]
    [SerializeField] public Transform spawnPoints;

    [SerializeField] public List<Vector3> SpawnPointList = new List<Vector3>();

    [Header("Prefab Characters")]
    [SerializeField] public GameObject slimePrefab;
    [SerializeField] public GameObject friendlySlimePrefab;
    [SerializeField] public GameObject goldenSlimePrefab;

    public List<GameObject> CharactersInScene = new List<GameObject>();

    /*[Header("Audio")]
    [SerializeField] public AudioSource spawnAudioSource;
    [SerializeField] public AudioClip spawnSound;

    [SerializeField] public AudioSource slimeAudioSource;
    [SerializeField] public AudioSource friendlySlimeAudioSource;
    [SerializeField] public AudioSource goldenSlimeAudioSource;

    [SerializeField] public AudioClip slimeDestroySound;
    [SerializeField] public AudioClip friendlySlimeDestroySound;
    [SerializeField] public AudioClip goldenSlimeDestroySound;
*/
    public Transform playerVR;

    private int scoreWhackASlime;

    float durationSlime = 3f;

    [Header("Timer")]
    private float timer;

    private bool canCount = false;
    
    private int minutes;
    
    private int seconds;
    
    string timeStr;

    [Header("UI display")]
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI countdownText;

    //pretty sure i just need buttonPress i dont need buttonPress to make the setactive false
    //public GameObject startButton;

    public GameObject buttonLobby;

    public GameObject countdown;

    public GameObject buttonPress;

   // public GameObject hammerReset;

    private GameObject character;

    [Header("UI Scoreboard")]
    public TextMeshProUGUI highScoreText;

    private int highScoreWhackASlime;

    [Header("Feedback UI")]
    public GameObject scoreFeedbackPrefab; // Reference to the score feedback prefab
    public Transform canvasTransform; // Reference to the canvas transform to instantiate UI elements under
    public float feedbackDuration = 2f;

    private void Start()
    {
        countdown.SetActive(false);
//        hammerReset.SetActive(true);
        highScoreWhackASlime = PlayerPrefs.GetInt("HighScoreWhackASlime", 0);
        UpdateScoreboard();
    }

    public void UpdateScoreboard()
    {
        scoreText.text = "Score: " + scoreWhackASlime;
        highScoreText.text = "High Score: " + highScoreWhackASlime;
    }
    public void StartFunction()
    {
        buttonPress.SetActive(false);
        scoreWhackASlime = 0;
        foreach (Transform item in spawnPoints)
        {
            SpawnPointList.Add(item.localPosition);
        }

        durationSlime = 3f;
        timer = 60f;
        canCount = true;
        timeText.text = "1 : 00";
        scoreText.text = "0";

        InvokeRepeating("GetCharacter", 0f, durationSlime);
    }

    /*public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        instance = this;
        Destroy(this.gameObject);
        Debug.Log("Player playing Whack a Mole ");
    }*/

    public void LoadLobbyScene()
    {
        transitionScene.SetActive(true);
        //int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
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

    private void Update()
    {
        //ResetHighScore();
        if (timer > 0.0f && canCount)
        {
            GameCountDown();
        }
        else if (timer <= 0.0f && canCount)
        {
            Finish();
        }
    }

    public void StartCountdown()
    {
        buttonLobby.SetActive(false);
        buttonPress.SetActive(false);
        countdown.SetActive(true);
        countdownText.gameObject.SetActive(true);
        StartCoroutine(CountdownCoroutine());
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

        countdownText.text = "Whack Em!"; // Display "GO!" when countdown finishes
        yield return new WaitForSeconds(1f); // Wait for 1 second after "GO!"

        countdownText.gameObject.SetActive(false); // Hide countdown text
        StartFunction(); // Start the game
        countdown.SetActive(false);
    }

    public void GameCountDown()
    {
        timer -= Time.deltaTime;
        minutes = Mathf.FloorToInt(timer / 60f);
        seconds = Mathf.FloorToInt(timer - minutes * 60);
        timeStr = string.Format("{0:0} : {1:00}", minutes, seconds);
        timeText.text = timeStr;
    }

    public void Finish()
    {
        //switch score < highScore to reset the highscore count number
        if (scoreWhackASlime > highScoreWhackASlime)
        {
            highScoreWhackASlime = scoreWhackASlime;
            PlayerPrefs.SetInt("HighScoreWhackASlime", highScoreWhackASlime);
        }
        UpdateScoreboard();
        CancelInvoke("GetCharacter");
        buttonLobby.SetActive(true);
        buttonPress.SetActive(true);
        //startButton.SetActive(true);
        canCount = false;
        timeText.text = "0 : 00";
        foreach (GameObject character in CharactersInScene.ToArray())
        {
            CharactersInScene.Remove(character);
            Destroy(character);
        }
    }
    // Was planning to apply Reset Button
    /*public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        highScore = 0; // Update the highScore variable in the script
        UpdateScoreboard(); // Update the UI to reflect the new high score
    }*/

    public void HowtoPlay()
    {
        catalogSpecies.gameObject.SetActive(false);
        tutorial.gameObject.SetActive(true);
        
    }

    public void CatalogSpecies()
    {
        tutorial.gameObject.SetActive(false);
        catalogSpecies.gameObject.SetActive(true);
    }

    public void ShowFeedbackText(Vector3 position, string text)
    {
        // Instantiate the feedback UI prefab
        GameObject feedbackUI = Instantiate(scoreFeedbackPrefab, position, Quaternion.identity, canvasTransform);

        // Get the direction from the feedback UI position to the player's position
        Vector3 directionToPlayer = playerVR.position - position;
        directionToPlayer.y = 0; // Ignore vertical distance

        // Rotate the feedback UI to face the player
        feedbackUI.transform.LookAt(playerVR.position);

        // Set the feedback text
        TextMeshProUGUI feedbackText = feedbackUI.GetComponentInChildren<TextMeshProUGUI>();
        if (feedbackText != null)
        {
            feedbackText.text = text;
        }

        // Hide the feedback UI after a delay
        StartCoroutine(HideFeedbackUI(feedbackUI));
    }


    // Coroutine to hide feedback UI after a delay
    IEnumerator HideFeedbackUI(GameObject feedbackUI)
    {
        yield return new WaitForSeconds(feedbackDuration);
        if (feedbackUI != null)
        {
            Destroy(feedbackUI);
        }
    }
public void GetCharacter()
    {
        if (SpawnPointList.Count > 0)
        {

            //new code starts here
            /* int index = Random.Range(0, SpawnPointList.Count);
             Vector3 worldPosition = SpawnPointList[index];

             bool occupied = IsSpawnPointOccupied(worldPosition);

             // If the spawn point is occupied, find another available spawn point
             while (occupied)
             {
                 index = Random.Range(0, SpawnPointList.Count);
                 worldPosition = SpawnPointList[index];
                 occupied = IsSpawnPointOccupied(worldPosition);
             }
             SpawnPointList.RemoveAt(index); // Optionally remove the spawn point if it shouldn't be reused immediately

             GameObject character;
             // Randomly choose the prefab based on your updated probability
             if (Random.Range(0f, 1f) < 0.7f) // Assuming 70% for slimePrefab, 30% for negativePrefab
                 character = Instantiate(slimePrefab, worldPosition, Quaternion.identity);
             else
                 character = Instantiate(negativePrefab, worldPosition, Quaternion.identity);*/
            //new code ends here

            //Old code here
            Vector3 randomPosition = SpawnPointList[Random.Range(0, SpawnPointList.Count)];
            SpawnPointList.Remove(randomPosition);
            if (!IsSpawnPointOccupied(randomPosition))
            {
                float randomValue = Random.Range(0f, 1f);
                if (randomValue < 0.85f) // Assuming 90% for slimePrefab, 30% for negativePrefab
                    character = Instantiate(slimePrefab, randomPosition, Quaternion.identity);
                else if (randomValue < 0.95f)
                    character = Instantiate(friendlySlimePrefab, randomPosition, Quaternion.identity);
                else
                    character = Instantiate(goldenSlimePrefab, randomPosition, Quaternion.identity);
                //old code ends here

                //Vector3 directionToPlayer = playerVR.position - character.transform.position;
            //directionToPlayer.y = 0; // Ignore vertical distance

            // Rotate the character to face the player
            //character.transform.rotation = Quaternion.LookRotation(directionToPlayer);

                character.transform.LookAt(new Vector3(playerVR.position.x, character.transform.position.y, playerVR.position.z));
                character.SetActive(true);
                CharactersInScene.Add(character);
                //spawnAudioSource.PlayOneShot(spawnSound);
                // Schedule for destruction
                DestroyCharacterAfterDelay(character, 8f); // Pass in the character and the delay time
            }
            else
            {
                Debug.Log("Spawnpoint is occupied Choose another spawn point");
            }
        }
        else
        {
            Debug.Log("Max characters placed");
        }
    }

    private bool IsSpawnPointOccupied(Vector3 position)
    {
        // Check if there's a character already occupying the given spawn point
        foreach (GameObject character in CharactersInScene)
        {
            if (Vector3.Distance(character.transform.position, position) < 0.1f) // Adjust the threshold as needed
            {
                return true;
            }
        }
        return false;
    }

    void DestroyCharacterAfterDelay(GameObject character, float delay)
    {
        StartCoroutine(DestroyAfterDelayCoroutine(character, delay));
    }

    IEnumerator DestroyAfterDelayCoroutine(GameObject character, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Perform the destruction and any associated cleanup
        if (character != null) // Check if the object hasn't already been destroyed
        {
            SpawnPointList.Add(character.transform.localPosition); // Optional, based on your game's logic
            CharactersInScene.Remove(character);
            Destroy(character);
        }
    }

    public void DestroySlime(GameObject character)
    {
        if (CharactersInScene.Contains(character))
        {
            SpawnPointList.Add(character.transform.localPosition);
            CharactersInScene.Remove(character);
            Destroy(character);
            scoreWhackASlime += 10;
            scoreText.text = scoreWhackASlime.ToString(); //increase score
            AdjustSpawningRate();

            Vector3 feedbackPosition = character.transform.position;
            string feedbackText = "+10";
            ShowFeedbackText(feedbackPosition, feedbackText);

            //slimeAudioSource.PlayOneShot(slimeDestroySound);
        }
    }

    public void DestroyFriendlySlime(GameObject character)
    {
        if (CharactersInScene.Contains(character))
        {
            SpawnPointList.Add(character.transform.localPosition);
            CharactersInScene.Remove(character);
            Destroy(character);
            scoreWhackASlime -= 10;
            scoreText.text = scoreWhackASlime.ToString(); // Reduce score
            AdjustSpawningRate();

            Vector3 feedbackPosition = character.transform.position;
            string feedbackText = "-10";
            ShowFeedbackText(feedbackPosition, feedbackText);

            //friendlySlimeAudioSource.PlayOneShot(friendlySlimeDestroySound);
        }
    }

    public void DestroyGoldenCharacter(GameObject character)
    {
        if (CharactersInScene.Contains(character))
        {
            SpawnPointList.Add(character.transform.localPosition);
            CharactersInScene.Remove(character);
            Destroy(character);
            scoreWhackASlime += 50;
            scoreText.text = scoreWhackASlime.ToString(); //increase score
            AdjustSpawningRate();

            Vector3 feedbackPosition = character.transform.position;
            string feedbackText = "+50";
            ShowFeedbackText(feedbackPosition, feedbackText);

            //goldenSlimeAudioSource.PlayOneShot(goldenSlimeDestroySound);
        }
    }
    private void AdjustSpawningRate()
    {
        // Reduce spawning duration based on the number of slimes hit
        if (durationSlime >= 0.06f)
        {
            durationSlime -= 0.002f;
            // Add additional reduction based on the number of slimes hit
            durationSlime -= 0.001f * (scoreWhackASlime / 10); // Assuming 10 points per slime hit
                                                  // Clamp the duration to a minimum value if needed
            durationSlime = Mathf.Max(durationSlime, 0.1f);
            // Cancel and restart the InvokeRepeating with the new duration
            CancelInvoke("GetCharacter");
            InvokeRepeating("GetCharacter", 0f, durationSlime);
        }
    }
}