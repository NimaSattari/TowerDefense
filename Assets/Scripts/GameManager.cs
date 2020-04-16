using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next,play,gameover,win
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int TotalWaves = 10;
    [SerializeField] Text totalMoneytext;
    [SerializeField] Text CurrentWavetext;
    [SerializeField] Text totalEscapedtext;
    [SerializeField] Text PlayBtntext;
    [SerializeField] Button PlayBtn;
    [SerializeField] GameObject YouWin;
    int waveNumber = 0;
    int totalEscaped = 0;
    int totalMoney = 10;
    int roundEscaped = 0;
    int totalKilled = 0;
    int whichEnemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;


    [SerializeField] GameObject spawnPoint;
    [SerializeField] Enemy[] Enemies;
    [SerializeField] int totalEnemies = 3;
    [SerializeField] int EnemiesPerSpawn;
    [SerializeField] float SpawnDelay = 1f;
    public List<Enemy> EnemyList = new List<Enemy>();
    AudioSource audioSource;
    int EnemiesToSpawn = 0;

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneytext.text = totalMoney.ToString();
        }
    }
    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }
    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }
    private void Start()
    {
        PlayBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        showMenu();
    }

    private void Update()
    {
        HandleEscape();
    }

    IEnumerator Spawn()
    {
        if (EnemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < EnemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(Enemies[Random.Range(0, EnemiesToSpawn)]);
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }
            yield return new WaitForSeconds(SpawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnRegisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }
    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }
    public void isWaveOver()
    {
        totalEscapedtext.text = "Excaped: " + TotalEscaped + " /10";
        if((roundEscaped + TotalKilled) == totalEnemies)
        {
            if(waveNumber <= Enemies.Length)
            {
                EnemiesToSpawn = waveNumber;
            }
            setCurrentGameState();
            showMenu();
        }
    }
    public void setCurrentGameState()
    {
        if (TotalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber == 0&&(TotalKilled+RoundEscaped) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= TotalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }
    public void showMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
                PlayBtntext.text = "Play Again!";
                break;
            case gameStatus.next:
                PlayBtntext.text = "Next Wave";
                break;
            case gameStatus.play:
                PlayBtntext.text = "Play";
                break;
            case gameStatus.win:
                PlayBtntext.text = "Play Again!";
                YouWin.SetActive(true);
                break;
        }
        PlayBtn.gameObject.SetActive(true);
    }
    public void PlayBtnPressed()
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber++;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 3;
                TotalEscaped = 0;
                TotalMoney = 10;
                EnemiesToSpawn = 0;
                YouWin.SetActive(false);
                TowerManager.Instance.DestroyAllTower();
                TowerManager.Instance.RenameTagsBuildSite();
                totalMoneytext.text = TotalMoney.ToString();
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                totalEscapedtext.text = "Excaped: " + TotalEscaped + " /10";
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        CurrentWavetext.text = "Wave: " + (waveNumber + 1);
        StartCoroutine(Spawn());
        PlayBtn.gameObject.SetActive(false);
    }

    private void HandleEscape()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TowerManager.Instance.disableDragSprite();
            TowerManager.Instance.TowerBtnPressed = null;
        }
    }
}
