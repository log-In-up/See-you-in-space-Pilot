using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

class GameMaster : MonoBehaviour
{
    #region Nested classes
    [System.Serializable]
    private class Wave
    {
        [SerializeField] internal GameObject enemyAI;
        [SerializeField] internal float spawnRate = 1.5f;
        [SerializeField] internal int numberOfEnemies = 3;
    }
    #endregion

    #region Script paremeters
    [SerializeField] private GameObject pauseMenu, pauseButton;
    [SerializeField] private Text scoreUI, waveUI;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Wave[] waves;
    [SerializeField] private float scoreValue = 5.0f, timeBetweenWaves = 5.0f;
    [SerializeField] private string enemyTag = "Enemy", mainGameScene = "MainGameScene", mainMenu = "MainMenu";

    private float waveCountdown, searchCountdown = 1.0f, gameScore;
    private int currentWave = 1, nextWave = 0;
    private const int zero = 0;

    private enum WaveState { Spawning, Waiting, Counting };
    WaveState state = WaveState.Counting;
    #endregion

    #region MonoBehaviour API
    private void Awake()
    {
        Time.timeScale = 1.0f;
        gameScore = 0.0f;
    }
    private void Start()
    {
        scoreUI.text = gameScore.ToString();
        waveUI.text = currentWave.ToString();

        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        WaveController();
    }
    #endregion

    #region Custom methods
    private bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag(enemyTag) == null)
            {
                return false;
            }
        }
        return true;
    }

    private void SpawnEnemy(GameObject enemy)
    {
        Transform spawnPosition = spawnPoints[Random.Range(zero, spawnPoints.Length)];
        Instantiate(enemy, spawnPosition.position, enemy.transform.rotation);
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        state = WaveState.Spawning;

        waveUI.text = currentWave++.ToString();

        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            SpawnEnemy(wave.enemyAI);
            yield return new WaitForSeconds(wave.spawnRate);
        }

        state = WaveState.Waiting;
        yield break;
    }

    private void WaveCompleted()
    {
        state = WaveState.Counting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = zero;
        }
        else
        {
            nextWave++;
        }

        UpdateScore(scoreValue);
    }

    private void WaveController()
    {
        if (state == WaveState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= zero && state != WaveState.Spawning)
        {
            StartCoroutine(SpawnWave(waves[nextWave]));
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    public void UpdateScore(float value)
    {
        gameScore += value;
        scoreUI.text = $"{gameScore}";
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

    public void RestartLevel()
    {
        SceneManager.LoadScene(mainGameScene);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
    #endregion
}