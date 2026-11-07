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
            SceneManager.LoadScene("Battle");
            Debug.Log("New Game");
        }

        if (navigator.index == 1) // Continue
        {
            //
            Debug.Log("Continue");
        }

        if (navigator.index == 2) // Load Game
        {
            //
            Debug.Log("Load Game");
        }

        if (navigator.index == 3) // Settings
        {
            mainMenu.gameObject.SetActive(false);
            //loadGameMenu.gameObject.SetActive(false);
            settingsMenu.gameObject.SetActive(true);
            Debug.Log("Settings");
        }

        if (navigator.index == 4) // Quit
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }


}
