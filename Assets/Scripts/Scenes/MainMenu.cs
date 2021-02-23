using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;

class MainMenu : MonoBehaviour
{
    #region Script paremeters
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject mainInterface, settingsMenu;
    [SerializeField] private string gameSound = "GameSound", mainGameScene = "MainGameScene", musicSound = "MusicSound", sfx = "SFX";
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    #endregion

    #region Custom methods
    public void LoadMainGameScene()
    {
        SceneManager.LoadScene(mainGameScene);
    }

    public void SetGameSound(float volume)
    {
        audioMixer.SetFloat(gameSound, volume);
    }

    public void SetMusicSound(float volume)
    {
        audioMixer.SetFloat(musicSound, volume);
    }

    public void SetSFXSound(float volume)
    {
        audioMixer.SetFloat(sfx, volume);
    }

    public void SetGraphicsQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }
    #endregion
}
