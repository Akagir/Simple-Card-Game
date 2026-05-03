using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject SettingsMenu;

    void Start()
    {
        CloseSettingsMenu();
    }

    public void StartMainGame()
    {
        SceneManager.LoadSceneAsync("MainGameScene");
    }

    public void OpenSettingsMenu()
    {
        SettingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        SettingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
