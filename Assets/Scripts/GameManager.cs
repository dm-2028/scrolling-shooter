using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    private string[] level;
    private TextAsset levelText;

    private int mScore = 0;

    public GameObject[] prefabs;

    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.streamingAssetsPath +"/level1";
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
        StartCoroutine("SpawnWave");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addScore(int score)
    {
        mScore += score;
        scoreText.text = "Score: " + mScore;
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
