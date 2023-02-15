using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private string[] level;
    private TextAsset levelText;

    private int mScore = 0;

    public GameObject[] prefabs;
    public GameObject[] playerShips;
    public GameObject pauseMenu;
    public GameObject controlsDirection;
    public Slider healthBar;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private bool gamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "High Score: " + MainManager.Instance.highScore;
        SpawnPlayer();
        LoadLevel();
        StartCoroutine("SpawnWave");
        Invoke("DisableControlMessage", 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            TogglePause();
        }
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
        string path = Application.streamingAssetsPath + "/level1";
        levelText = Resources.Load<TextAsset>("level1");
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
        Instantiate(playerShips[index], new Vector3(0, -4, 0), playerShips[index].transform.rotation);
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
    
    public void UpdateHealth(int health)
    {
        healthBar.value = health;
    }
    IEnumerator SpawnWave()
    {
        foreach(string line in level)
        {
            ParseLine(line);
            yield return new WaitForSeconds(2f);
        }

        yield return null;
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
                default:
                    break;
            }
            if(toSpawn != null)
            {
                Instantiate(toSpawn, new Vector3(-8f + i, 5, 0), toSpawn.transform.rotation);
            }
        }
    }
}
