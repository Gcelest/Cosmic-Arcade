using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerLobby : MonoBehaviour
{
    /*public static GameControllerLobby instance;
    // Start is called before the first frame update
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
    }*/

    public Animator transition;
    
    public float transitionTime = 1f;

    public GameObject transitionScene;
    [Header("UI display")]
    public TextMeshProUGUI highScoreWhackASlimeText;

    public TextMeshProUGUI highScoreSkeeBallText;

    public Image hologramInfoFS;

    public Image hologramInfoES;

    public GameObject holograpicFriendlySlime;

    public GameObject holograpicEnemySlime;

    private void Start()
    {
        // Retrieve the high score from PlayerPrefs
        int highScoreWhackASlime = PlayerPrefs.GetInt("HighScoreWhackASlime", 0);
        int highScoreSkeeBall = PlayerPrefs.GetInt("HighScoreSkeeBall", 0);

        // Update the UI with the high score
        highScoreWhackASlimeText.text = "High Score: " + highScoreWhackASlime;
        highScoreSkeeBallText.text = "High Score: " + highScoreSkeeBall;

        //highScoreSkeeBallText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, 2) * Time.deltaTime);

    }
    public void LoadWhackASlimeScene()
    {
        //transitionScene.SetActive(true);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        Debug.Log("Player playing Whack a Mole ");
    }

    IEnumerator LoadLevel(int levelIndex)
    {
            transition.SetTrigger("Start");

            if (levelIndex <= 1)
            {
                SceneManager.LoadScene(levelIndex);
            }
            else
            {
                Debug.LogWarning("No next scene available.");
            }

            yield return new WaitForSeconds(levelIndex);

            SceneManager.LoadScene(levelIndex);
        }

    //transition.SetTrigger("End");
    //}
    /*int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    if (nextSceneIndex <= 1)
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
    else
    {
        Debug.LogWarning("No next scene available.");
    }*/
    public void LoadShootAsteroidScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 2;
        if (nextSceneIndex <= 2)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene available.");
        }
    }

    public void FriendlyHologram()
    {
        hologramInfoES.gameObject.SetActive(false);
        holograpicEnemySlime.SetActive(false);
        hologramInfoFS.gameObject.SetActive(true);
        holograpicFriendlySlime.SetActive(true);
    }

    public void EnemyHologram()
    {
        hologramInfoFS.gameObject.SetActive(false);
        holograpicFriendlySlime.SetActive(false);
        hologramInfoES.gameObject.SetActive(true);
        holograpicEnemySlime.SetActive(true);
    }
}
