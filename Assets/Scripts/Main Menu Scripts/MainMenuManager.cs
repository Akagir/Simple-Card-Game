using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject SettingsMenu;

    void Start()
    {
    }

    public void StartMainGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadSceneAsync("MainGameScene");        
    }

    public void OpenSettingsMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SettingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SettingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        Application.Quit();
    }
}
