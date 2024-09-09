using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("# Game Control")]
    public bool islive;
    public float GameTime;
    public float MaxGameTime = 2 * 10f;
    [Header("# player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100; 
    public int Level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    public GameObject enemyCleaner;

    private void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        playerId = id;        
        health = maxHealth;

        player.gameObject.SetActive(true);
        uiLevelUp.select(playerId % 2);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.playSfx(AudioManager.Sfx.Select);
    }

    public void GameOut()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        islive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.playSfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        islive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.playSfx(AudioManager.Sfx.win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (!islive)
        {
            return;
        }

        GameTime += Time.deltaTime;
        if (GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!islive)
        {
            return;
        }
        exp++;
        
        if(exp == nextExp[Mathf.Min(Level,nextExp.Length - 1)])
        {
            Level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        islive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }
    public void Resume()
    {
        islive = true;
        Time.timeScale = 1; //¹è¼Ó
        uiJoy.localScale = Vector3.one;
    }
}
