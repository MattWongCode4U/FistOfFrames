using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static string GameMode; //vsAI, offPVP, onPVP

    public GameObject menuCanvas;
    public GameObject gameModeCanvas;
    public GameObject instructionsCanvas;
    public GameObject optionsCanvas;
    public GameObject creditsCanvas;
    public AudioManager audioManager;

    int gameWidth = 800;
    int gameHeight = 600;

    private void Awake()
    {
        //Set game window dimensions
        Screen.SetResolution(gameWidth, gameHeight, false);
    }

    void Start()
    {
        //enableMainMenu();
    }

    //Start Game button opens the game scene
    public void startGame(string g)
    {
        GameMode = g;
        audioManager.buttonSFX();
        SceneManager.LoadScene("Game");
    }

    //Loads the Main menu scene
    public void mainMenu()
    {
        audioManager.buttonSFX();
        SceneManager.LoadScene("Menu");
    }

    //Disable all menu canvases
    public void disableMenus()
    {
        menuCanvas.SetActive(false);
        gameModeCanvas.SetActive(false);
        instructionsCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    //Enable mainMenu UI
    public void enableMainMenu()
    {
        disableMenus();
        audioManager.buttonSFX();
        menuCanvas.SetActive(true);
    }

    //Enable game mode menu
    public void enableGameModeMenu()
    {
        disableMenus();
        audioManager.buttonSFX();
        gameModeCanvas.SetActive(true);
    }

    //Enable Instructions UI
    public void enableInstructions()
    {
        disableMenus();
        audioManager.buttonSFX();
        instructionsCanvas.SetActive(true);
    }

    //Enable Options UI
    public void enableOptions()
    {
        disableMenus();
        audioManager.buttonSFX();
        optionsCanvas.SetActive(true);
    }

    //Enable Credits UI
    public void enableCredits()
    {
        disableMenus();
        audioManager.buttonSFX();
        creditsCanvas.SetActive(true);
    }

    //Quits game and closes application
    public void quitGame()
    {
        audioManager.buttonSFX();
        Application.Quit();
    }
}
