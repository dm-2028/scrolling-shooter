using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string[] level;
    private TextAsset levelText;
    private static int levelNumber = 1;
    private static int maxLevels = 3;

    private static int mScore = 0;
    private float lerpSpeed = 5.0f;
    private static int lives = 2;

    public GameObject[] prefabs;
    public GameObject[] playerShips;
    public GameObject bossPrefab;
    public GameObject pauseMenu;
    public GameObject gameOverScreen;
    public GameObject controlsDirection;
    public GameObject scroller;
    public GameObject player;
    public Slider healthBar;
    private bool bossFight = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI remainingLives;

    private bool gamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "High Score: " + MainManager.Instance.highScore;
        scoreText.text = "Score: " + mScore;
        UpdateLives();
        SpawnPlayer();
        LoadLevel();
        StartCoroutine("SpawnWave");
        if(levelNumber == 1)
        {
            controlsDirection.SetActive(true);
            Invoke("DisableControlMessage", 5.0f);
        }
        else
        {
            controlsDirection.SetActive(false);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            TogglePause();
        }
    }

    private void UpdateLives()
    {
        remainingLives.text = "Lives: " + lives;
    }

    private void DisableControlMessage()
    {
        controlsDirection.gameObject.SetActive(false);
    }

    private void TogglePause()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            gamePaused = false;
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void LoadLevel()
    {
        string path = Application.streamingAssetsPath + "/level" + levelNumber;
        levelText = Resources.Load<TextAsset>("level" + levelNumber);
        level = levelText.text.Split('\n');
        Debug.Log(path);
        if (File.Exists(path))
        {
            Debug.Log("load file");
            level = File.ReadAllLines(path);
        }
        else
        {
            Debug.Log("did not load file");
        }
    }

    private void SpawnPlayer()
    {
        int index = MainManager.Instance.shipSelection;
        player = Instantiate(playerShips[index], new Vector3(0, -4, 0), playerShips[index].transform.rotation);
        UpdateHealth(10);
    }

    public void addScore(int score)
    {
        mScore += score;
        scoreText.text = "Score: " + mScore;
        if(mScore > MainManager.Instance.highScore)
        {
            MainManager.Instance.highScore = mScore;
            MainManager.Instance.SavePlayerInfo();
        }
    }

    public void ShipDestroyed()
    {
        lives--;
        if(lives < 0)
        {
            GameOver();
        }
        else
        {
            UpdateLives();
            Invoke("SpawnPlayer", 2.0f);
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }
    
    public void UpdateHealth(int health)
    {
        healthBar.value = health;
    }
    IEnumerator SpawnWave()
    {
        foreach(string line in level)
        {
            ParseLine(line);
            Debug.Log("boss fight " + bossFight);
            yield return new WaitForSeconds(2f);
        }
        Debug.Log("boss fight after for loop " + bossFight);
        if (bossFight)
        {
            StartCoroutine("BossFight");
        }
        else
        {
            StartCoroutine("FinishLevel");
        }
    }

    IEnumerator BossFight()
    {
        GameObject boss = Instantiate(bossPrefab, new Vector3(0, 6, 0), bossPrefab.transform.rotation);
        Vector3 startPos = boss.transform.position;
        Vector3 endPos = new Vector3(0, 2.6f, 0);
        float i = 0.0f;
        float rate = 1.0f / 3.0f;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            boss.transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
        boss.GetComponent<Boss>().isActive = true;
    }

    IEnumerator FinishLevel()
    {
        scroller.GetComponent<MoveBackground>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        Vector3 startPos = player.transform.position;
        Vector3 endPos = new Vector3(player.transform.position.x, 6, player.transform.position.z);
        float distance = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fraction = distanceCovered / distance;

        while (fraction < 1)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fraction = distanceCovered / distance;
            player.transform.position = Vector3.Lerp(startPos, endPos, fraction);
            yield return null;
        }
        LoadNextLevel();
    }
    private void LoadNextLevel()
    {
        levelNumber++;
        if (levelNumber > maxLevels)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    private void ParseLine(string line)
    {
        for(int i = 0; i < line.Length; i++)
        {
            GameObject toSpawn = null;
            switch (line[i])
            {
                case ('x'):
                    toSpawn = prefabs[0];
                    break;
                case ('y'):
                    toSpawn = prefabs[1];
                    break;
                case ('z'):
                    toSpawn = prefabs[2];
                    break;
                case ('a'):
                    toSpawn = prefabs[3]; 
                    break;
                case ('b'):
                    bossFight = true;
                    return;
                case ('-'):
                    return;
                default:
                    break;
            }
            if(toSpawn != null)
            {
                Instantiate(toSpawn, new Vector3(-8f + i, 5, 0), toSpawn.transform.rotation);
            }
            
        }

    }

    public void Restart()
    {
        Time.timeScale = 1;
        levelNumber = 1;
        mScore = 0;
        SceneManager.LoadScene(1);
    }
}
