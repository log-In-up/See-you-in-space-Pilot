using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

class GameMaster : MonoBehaviour
{
    #region Script paremeters
    [Header("Properties of GameMaster")]
    [SerializeField] float fadeDelay = 2.0f;
    [SerializeField] float fadeUIDuration = 3.0f;
    [SerializeField] float scoreValue = 5.0f;
    [SerializeField] float timeBetweenWaves = 5.0f;
    [SerializeField] string enemyTag = "Enemy";
    [SerializeField] string textComponent = "Text";
#pragma warning disable 649
    [SerializeField] GameObject pauseMenu, pauseButton, score, wave;
    [SerializeField] Scenes scenes;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Wave[] waves;
#pragma warning restore 649

    CanvasGroup scoreUIFade, waveUIFade;
    Text scoreUI, waveUI;

    enum WaveState { Spawning, Waiting, Counting };
    float waveCountdown, gameScore = 0.0f, searchCountdown = 1.0f;
    int currentWave = 1, nextWave = 0;
    WaveState state;
    #endregion

    #region MonoBehaviour API
    void Awake()
    {
        state = WaveState.Counting;
        Time.timeScale = 1.0f;

        scoreUIFade = score.GetComponent<CanvasGroup>();
        scoreUI = score.transform.Find(textComponent).GetComponent<Text>();

        waveUIFade = wave.GetComponent<CanvasGroup>();
        waveUI = wave.transform.Find(textComponent).GetComponent<Text>();
    }

    void Start()
    {
        scoreUI.text = gameScore.ToString();
        waveUI.text = currentWave.ToString();

        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        WaveController();
    }
    #endregion

    #region Custom methods
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag(enemyTag) == null)
                return false;
        }
        return true;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Transform spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPosition.position, enemy.transform.rotation);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = WaveState.Spawning;

        waveUIFade.alpha = 1.0f;
        waveUI.text = currentWave++.ToString();
        StartCoroutine(Fade(waveUIFade, waveUIFade.alpha, 0.0f));

        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            SpawnEnemy(wave.enemyAI);
            yield return new WaitForSeconds(wave.spawnRate);
        }

        state = WaveState.Waiting;
        yield break;
    }

    IEnumerator Fade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0.0f;
        yield return new WaitForSeconds(fadeDelay);

        while (counter < fadeUIDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / fadeUIDuration);

            yield return null;
        }
        yield break;
    }

    void WaveCompleted()
    {
        state = WaveState.Counting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
            nextWave = 0;
        else
            nextWave++;

        UpdateScore(scoreValue);
    }

    void WaveController()
    {
        if (state == WaveState.Waiting)
            if (!EnemyIsAlive())
                WaveCompleted();
            else
                return;

        if (waveCountdown <= 0 && state != WaveState.Spawning)
            StartCoroutine(SpawnWave(waves[nextWave]));
        else
            waveCountdown -= Time.deltaTime;
    }

    internal void UpdateScore(float enemyScore)
    {
        scoreUIFade.alpha = 1.0f;

        gameScore += enemyScore;
        scoreUI.text = gameScore.ToString();

        StartCoroutine(Fade(scoreUIFade, scoreUIFade.alpha, 0.0f));
    }

    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void RestartLevel() => SceneManager.LoadScene(scenes.mainGameScene);
    public void Exit() => Application.Quit();
    public void MainMenu() => SceneManager.LoadScene(scenes.mainMenu);
    #endregion

    #region Nested classes

    [System.Serializable]
    class Scenes
    {
        [SerializeField] internal string mainGameScene = "MainGameScene";
        [SerializeField] internal string mainMenu = "MainMenu";
    }

    [System.Serializable]
    class Wave
    {
#pragma warning disable 649
        [SerializeField] internal GameObject enemyAI;
#pragma warning restore 649
        [SerializeField] internal float spawnRate = 1.5f;
        [SerializeField] internal int numberOfEnemies = 3;
    }

    #endregion
}