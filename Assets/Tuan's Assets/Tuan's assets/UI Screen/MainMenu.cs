using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startScene;

    //Hàm xử lý bắt đầu game
    public void StartGame()
    {
        PlayerPrefs.DeleteAll();
        ContinueGame();
    }

    public void ContinueGame()
    {
        if(Prefs.LEVEL > 0)
        {
            SceneManager.LoadScene(Prefs.LEVEL);
        }
        else
        {
            Prefs.LEVEL++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


    //Hàm xử lý thoát game
    public void QuitGame()
    {
        Application.Quit();
    }
}
