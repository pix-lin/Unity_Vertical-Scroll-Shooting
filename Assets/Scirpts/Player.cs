using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;
    public bool isHit;
    public bool isBoomTime;
    public bool isRespawnTime;

    public bool[] joyControl; //어떤 버튼을 눌렀나
    public bool isControl; //버튼을 눌렀나

    public int life;
    public int maxLife;
    public int score;
    public int power;
    public int maxPower;
    public int maxBoom;
    public int curBoom;

    public float speed;
    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject boomEffect;
    public GameManager gameManager;
    public ObjectManager objectManager;

    public GameObject[] followers;

    Animator anime;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(UnbetableWithDelay(3.0f));
    }

    IEnumerator UnbetableWithDelay(float delay)
    {
        Unbetable();
        yield return new WaitForSeconds(delay);
        Unbetable(); 
    }

    void Unbetable()
    {
        isRespawnTime = !isRespawnTime;
        if (isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            for (int index = 0; index < followers.Length; index++)
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }

        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);

            for (int index = 0; index < followers.Length; index++)
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1:
                //Power One
                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            default:
                GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
                GameObject bulletCC = objectManager.MakeObj("BulletPlayerB");
                GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.3f;
                bulletCC.transform.position = transform.position;
                bulletLL.transform.position = transform.position + Vector3.left * 0.3f;
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

        }
        

        curShotDelay = 0;
    }

    void Boom()
    {
        if (!Input.GetButton("Fire2"))
            return;

        if (isBoomTime)
            return;

        if (curBoom == 0)
            return;

        curBoom--;
        isBoomTime = true;
        gameManager.UpdateBoomUI(curBoom);

        //Effect visible
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 3.0f);
        Debug.Log("Hello!");

        //Remove Enemy
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");

        for (int index = 0; index < enemiesL.Length; index++)
        {
            if (enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
           
        }

        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        //Remove Enemy Bullet
        //GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        GameObject[] bulletsA = objectManager.GetPool("BulletEnemyA");
        GameObject[] bulletsB = objectManager.GetPool("BulletEnemyB");

        for (int index = 0; index < bulletsA.Length; index++)
        {
            bulletsA[index].SetActive(false);
        }

        for (int index = 0; index < bulletsB.Length; index++)
        {
            bulletsB[index].SetActive(false);
        }

    }
    void Reload()
    {
        curShotDelay += Time.deltaTime;

    }

    public void JoyPanel(int type)
    {
        for (int index = 0; index < 9; index++)
        {
            joyControl[index] = index == type;
        }
    }

    public void JoyDown()
    {
        isControl = true;
    }

    public void JoyUp()
    {
        isControl= false;
    }
    
    void Move()
    {
        //Keyboard Control Value
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //Joy Control Value
        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1) || !isControl)
            h = 0;
        
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1) || !isControl)
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
            anime.SetInteger("Input", (int)h);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
        }

        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isRespawnTime)
                return;

            if (isHit)
                return;

            isHit = true;
            life--;
            gameManager.UpdateLifeUI(life);
            gameManager.CallExplosion(transform.position, "P"); ;

            if(life == 0) {
                gameManager.GameOver();
            }

            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();

            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == maxPower)
                        score += 500;
                    else
                    {
                        power++;
                        AddFollower();
                    }
                    break;
                    
                case "Boom":
                    if (curBoom == maxBoom)
                        score += 500;
                    else
                    {
                        curBoom++;
                        gameManager.UpdateBoomUI(curBoom);
                    }
                        
                    break;
            }
            collision.gameObject.SetActive(false);
        }

    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false; 
    }

    void AddFollower()
    {
        if (power == 4)
            followers[0].SetActive(true);
        else if (power == 5)
            followers[1].SetActive(true);
        else if (power == 6)
            followers[2].SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
            }
        }

    }
}
