using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;





public class TitleScreenMenu : MonoBehaviour
{

    [Header("Menus")]
  
    [SerializeField] RectTransform mainMenu;

    [SerializeField] RectTransform loadGameMenu;
    [SerializeField] RectTransform settingsMenu;
    [SerializeField] MenuNavigator navigator;
    [SerializeField] GameFlow gameFlow;
    [Header("Sound")]
    [SerializeField] UISound uiSound;

    private void Start()
    {

        mainMenu.gameObject.SetActive(true);
        loadGameMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        if (mainMenu.gameObject.activeSelf)
        {
            if (BattleInput.Confirm())
                ButtonConfirm(navigator.index);
        }

        if (settingsMenu.gameObject.activeSelf)
        {
            if (BattleInput.Cancel())
            {
                settingsMenu.gameObject.SetActive(false);
                mainMenu.gameObject.SetActive(true);
            }
        }
    }

    void ButtonConfirm(int i)
    {
        if (navigator.index == 0) // New Game
        {
            uiSound.PlayConfirm();
            gameFlow.StartGame();
            Debug.Log("New Game");
        }

        if (navigator.index == 1) // Continue
        {
            //
            Debug.Log("Continue");
            uiSound.PlayError();
        }

        if (navigator.index == 2) // Load Game
        {
            //
            Debug.Log("Load Game");
            uiSound.PlayError();
        }

        if (navigator.index == 3) // Settings
        {
            mainMenu.gameObject.SetActive(false);
            settingsMenu.gameObject.SetActive(true);
            uiSound.PlayConfirm();
            Debug.Log("Settings");
        }

        if (navigator.index == 4) // Quit
        {
            Application.Quit();
            uiSound.PlayConfirm();
            Debug.Log("Quit");
        }
    }


}
