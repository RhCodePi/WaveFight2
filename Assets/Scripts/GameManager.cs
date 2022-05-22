using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyTypeList;
    [SerializeField] private int levelEnemyCount = 3;
    [SerializeField] private GameObject gameOver, win, pause, nextStage;
    [SerializeField] private bool isLastStage;
    [SerializeField] private GameObject _HealthBarUI;
    enum SpawnState {spawning, NotSpawning}
    private int enemyCount, currentScene;
    private bool isGameActive;
    private Vector3 spawnPosition;
    private SpawnState _state;
    private GameObject _BarSlide;

    void Start()
    {
        isGameActive = true;
        Time.timeScale = 1;

        currentScene = SceneManager.GetActiveScene().buildIndex;
        _state = SpawnState.spawning;
    }

    void Update()
    {
        if(_state == SpawnState.spawning && isGameActive)
        {
            //EnemySpawn;
            SpawningEnemy();
            // enemycount =>>>> 1
            _state = SpawnState.NotSpawning;
        }
        else {
            //StopSpawn;
            // enemycount =>>>>> 0
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if(enemyCount == 0) _state = SpawnState.spawning;;
        }
        if(levelEnemyCount == -1) NextStage();

        Pause();
    }

    public void SlideBar(float rateScale)
    {
        _BarSlide.transform.localScale = new Vector3(rateScale, _BarSlide.transform.localScale.y, _BarSlide.transform.localScale.z);
    }

    void SpawningEnemy()
    {
        spawnPosition = new Vector3(10, -4.14f);
        int randomIndex = Random.Range(0, enemyTypeList.Count);
        _BarSlide = Instantiate(enemyTypeList[randomIndex], spawnPosition, Quaternion.identity)
                                .transform.GetChild(1).transform.GetChild(1).gameObject;
        levelEnemyCount--;
    }
    #region UI Management
    public void NextStage()
    {
        if(isLastStage) WinGame();
        else
        {
            nextStage.SetActive(true);
            Time.timeScale = 0;
            isGameActive = false;
        }
    }
    public void GameOver()
    {
        gameOver.SetActive(true);
        isGameActive = false;
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        if(win != null)
        {
            win.SetActive(true);
            isGameActive = false;
            Time.timeScale = 0;
        }
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Continue()
    {
        SceneManager.LoadScene(currentScene+1);
    }

    public void Resume()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
    }
    #endregion UI Management
    public HealthBar getHealthBar{get { return _HealthBarUI.GetComponent<HealthBar>();}}
    public bool getIsGameActive { get{return isGameActive;}}
}
