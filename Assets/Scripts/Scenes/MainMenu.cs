using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;

class MainMenu : MonoBehaviour
{
    #region Script paremeters
#pragma warning disable 649
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameObject mainInterface;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] AudioProperty audioProrerty;
    [SerializeField] Scenes scenes;
#pragma warning restore 649


    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        Time.timeScale = 1.0f;
    }
    #endregion

    #region Custom methods
    public void LoadMainGameScene() => SceneManager.LoadScene(scenes.mainGameScene);

    public void SetGameSound(float volume) => audioMixer.SetFloat(audioProrerty.gameSound, volume);

    public void SetMusicSound(float volume) => audioMixer.SetFloat(audioProrerty.musicSound, volume);

    public void SetSFXSound(float volume) => audioMixer.SetFloat(audioProrerty.sfx, volume);

    public void SetGraphicsQuality(int quality) => QualitySettings.SetQualityLevel(quality);
    #endregion

    #region Nested classes
    [System.Serializable]
    class AudioProperty
    {
        [SerializeField] internal string gameSound = "GameSound";
        [SerializeField] internal string musicSound = "MusicSound";
        [SerializeField] internal string sfx = "SFX";
    }
    [System.Serializable]
    class Scenes
    {
        [SerializeField] internal string mainGameScene = "MainGameScene";
    }
    #endregion
}
