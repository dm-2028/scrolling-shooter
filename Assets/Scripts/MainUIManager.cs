using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

}
