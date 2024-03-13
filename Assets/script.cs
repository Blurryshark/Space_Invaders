using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class script : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void resetGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void resetHigh()
    {
        PlayerPrefs.DeleteAll();
    }
}
