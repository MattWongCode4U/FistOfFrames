using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameAudioManager audioManager;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu();
        }
    }

    //Start Game button opens the game scene
    public void startGame()
    {
        audioManager.buttonSFX();
        SceneManager.LoadScene("Game");
    }

    //Loads the Main menu scene
    public void mainMenu()
    {
        audioManager.buttonSFX();
        SceneManager.LoadScene("Menu");
    }
}
