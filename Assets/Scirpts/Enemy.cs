using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed;
    public int health;
    public int enemyScore;
    public Sprite[] sprites;
    
    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject player;
    public ObjectManager objectManager;
    public GameManager gameManager;

    SpriteRenderer spriteRenderer;
    Animator anime;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyName == "B")
            anime = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        switch (enemyName)
        {
            case "B":
                health = 3000;
                //Invoke("Stop", 2.0f);
                StartCoroutine(StopAfterDelay(2.0f));
                break;
            case "L":
                health = 40;
                spriteRenderer.sprite = sprites[0];
                break;
            case "M":
                health = 10;
                spriteRenderer.sprite = sprites[0];
                break;
            case "S":
                health = 5;
                spriteRenderer.sprite = sprites[0];
                break;
        }
    }
    /*
    private void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }
    */

    IEnumerator StopAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameObject.activeSelf)
        {
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = Vector2.zero;

            StartCoroutine(ThinkAfterDelay(2.0f));
        }
    }

    IEnumerator ThinkAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()
    {
        if (health <= 0)
            return;

        //Fire 4 Bullets Forward
        GameObject bulletL1 = objectManager.MakeObj("BulletEnemyBossC");
        GameObject bulletL2 = objectManager.MakeObj("BulletEnemyBossC");
        GameObject bulletR1 = objectManager.MakeObj("BulletEnemyBossC");
        GameObject bulletR2 = objectManager.MakeObj("BulletEnemyBossC");
        bulletL1.transform.position = transform.position + Vector3.down * 1.0f + Vector3.left * 0.5f;
        bulletL2.transform.position = transform.position + Vector3.down * 1.0f + Vector3.left * 0.75f;
        bulletR1.transform.position = transform.position + Vector3.down * 1.0f + Vector3.right * 0.5f;
        bulletR2.transform.position = transform.position + Vector3.down * 1.0f + Vector3.right * 0.75f;
        bulletL1.transform.rotation = Quaternion.identity;
        bulletL2.transform.rotation = Quaternion.identity;
        bulletR1.transform.rotation = Quaternion.identity;
        bulletR2.transform.rotation = Quaternion.identity;
        Rigidbody2D rigidL1 = bulletL1.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL2 = bulletL2.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidR1 = bulletR1.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidR2 = bulletR2.GetComponent<Rigidbody2D>();

        Vector3 dirVecL1 = player.transform.position - transform.position + Vector3.down * 1.0f + Vector3.left * 0.5f;
        Vector3 dirVecL2 = player.transform.position - transform.position + Vector3.down * 1.0f + Vector3.left * 0.75f;
        Vector3 dirVecR1 = player.transform.position - transform.position + Vector3.down * 1.0f + Vector3.right * 0.5f;
        Vector3 dirVecR2 = player.transform.position - transform.position + Vector3.down * 1.0f + Vector3.right * 0.75f;
        rigidL1.AddForce(dirVecL1.normalized * 3.5f, ForceMode2D.Impulse);
        rigidL2.AddForce(dirVecL2.normalized * 3.5f, ForceMode2D.Impulse);
        rigidR1.AddForce(dirVecR1.normalized * 3.5f, ForceMode2D.Impulse);
        rigidR2.AddForce(dirVecR2.normalized * 3.5f, ForceMode2D.Impulse);
        
        //Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            StartCoroutine(FireFowardAfterDelay(1.5f));
        }
        else
        {
            StartCoroutine(ThinkAfterDelay(2.5f));
        }
            
    }

    IEnumerator FireFowardAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject.activeSelf && curPatternCount < maxPatternCount[patternIndex])
            FireFoward();
    }

    void FireShot()
    {
        
        if (health <= 0)
            return;

        //Random 5 Bullets
        for (int index = 0; index < 6; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyBossD");
            bullet.transform.position = transform.position;
            //bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 4f, ForceMode2D.Impulse);
        }

        //Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            StartCoroutine(FireShotAfterDelay(1.25f));
        }
        else
        {
            StartCoroutine(ThinkAfterDelay(2.5f));
        }
    }

    IEnumerator FireShotAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject.activeSelf && curPatternCount < maxPatternCount[patternIndex])
            FireShot();
    }

    void FireArc()
    {
        if (health <= 0)
            return;

        //Fire Arc Continue Fan Attack
        GameObject bullet = objectManager.MakeObj("BulletEnemyBossC");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec1 = new Vector2(Mathf.Cos(Mathf.PI * 4 * curPatternCount / maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec1.normalized * 4f, ForceMode2D.Impulse);

        //Pattern Counting
        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            StartCoroutine(FireArcAfterDelay(0.6f));
        }
        else
        {
            StartCoroutine(ThinkAfterDelay(2.5f));
        }
    }

    IEnumerator FireArcAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject.activeSelf && curPatternCount < maxPatternCount[patternIndex])
            FireArc();
    }

    void FireAround()
    {
        
        if (health <= 0)
            return;

        //Fire Around
        int roundNumA = 40;
        int roundNumB = 50;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;
        for(int index = 0; index < roundNum; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyBossD");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec1 = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum), Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid.AddForce(dirVec1.normalized * 2f, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }
        

        //Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            StartCoroutine(FireAroundAfterDelay(1.5f));
        }
        else
        {
            StartCoroutine(ThinkAfterDelay(2.5f));
        }
    }

    IEnumerator FireAroundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject.activeSelf && curPatternCount < maxPatternCount[patternIndex])
            FireAround();
    }

    void Update()
    {
        if (enemyName == "B")
            return;

        Fire();
        Reload();
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        if (enemyName == "L")
        {
            GameObject bulletL = objectManager.MakeObj("BulletEnemyA");
            GameObject bulletR = objectManager.MakeObj("BulletEnemyA");
            bulletL.transform.position = transform.position + Vector3.left * 0.25f;
            bulletR.transform.position = transform.position + Vector3.right * 0.25f;
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();

            Vector3 dirVecL = player.transform.position - transform.position + Vector3.left * 0.25f;
            Vector3 dirVecR = player.transform.position - transform.position + Vector3.right * 0.25f;
            rigidL.AddForce(dirVecL.normalized * 3.5f, ForceMode2D.Impulse);
            rigidR.AddForce(dirVecR.normalized * 3.5f, ForceMode2D.Impulse);
        }

        else if (enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 4f, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;

    }

    public void OnHit(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;

        if (enemyName == "B")
        {
            anime.SetTrigger("OnHit");
        }

        else
        {
            spriteRenderer.sprite = sprites[1];
            StartCoroutine(ReturnSpriteAfterDelay(0.1f));
        }
        

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            //Random Ratio Item Drop
            int ran = enemyName == "B" ? 0 : UnityEngine.Random.Range(0, 10);
            if(ran < 5) //Nothing 0.5
            {
                Debug.Log("Not Item");
            }
            else if (ran < 8) //Coin 0.3
            {
                GameObject itemCoin = objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
            }

            else if (ran < 9) //Power 0.1
            {
                GameObject itemPower = objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
            }

            else if (ran < 10) //Boom 0.1
            {
                GameObject itemBoom = objectManager.MakeObj("ItemBoom");
                itemBoom.transform.position = transform.position;
            }

            transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
            gameManager.CallExplosion(transform.position, enemyName);

            //Boss Kill
            if(enemyName == "B")
            {
                gameManager.StageEnd();
            }
        }
        
    }

    IEnumerator ReturnSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
        {
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
        }
            
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Debug.Log("Bullet!!");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            collision.gameObject.SetActive(false);
        }
    }

}