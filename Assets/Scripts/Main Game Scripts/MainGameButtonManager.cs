using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameButtonManager : MonoBehaviour
{
    public GameObject SettingsPanel;
    public void OpenSettingsPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SettingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SettingsPanel.SetActive(false);
    }

    public void BackMainMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadSceneAsync("MainMenuScene");
    }
}