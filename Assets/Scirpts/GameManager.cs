using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] ememyObjs;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public TextMeshProUGUI scoreText;
    public Image[] lifeImage;
    public GameObject gameOver;
    Player playerLogic;

    private void Awake()
    {
        playerLogic = player.GetComponent<Player>();
    }

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        //UI Score UPdate
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);

        GameObject enemy = Instantiate(ememyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if (ranPoint == 5 || ranPoint == 6) //����
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * 1, -1);
        }
        else if (ranPoint == 7 || ranPoint == 8) //������
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * -1);
        }
    }

    public void UpdateLifeUI(int life)
    {
        //Life UI Init Disable
        for (int index = 0; index < playerLogic.maxLife; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        //Life UI Active
        for (int index = 0; index < life; index++ )
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2.0f);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);
        playerLogic.isHit = false;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}

