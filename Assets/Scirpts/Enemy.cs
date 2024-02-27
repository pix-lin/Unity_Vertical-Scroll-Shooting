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
                Invoke("Stop", 2.0f);
                break;
            case "L":
                health = 40;
                break;
            case "M":
                health = 10;
                break;
            case "S":
                health = 5;
                break;
        }
    }

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

    void FireFoward()
    {
        //Fire 4 Bullet Forward
        GameObject bulletL1 = objectManager.MakeObj("BulletEnemyBossC");
        GameObject bulletL2 = objectManager.MakeObj("BulletEnemyBossC");
        GameObject bulletR1 = objectManager.MakeObj("BulletEnemyBossC");
        GameObject bulletR2 = objectManager.MakeObj("BulletEnemyBossC");
        bulletL1.transform.position = transform.position + Vector3.down * 1.0f + Vector3.left * 0.5f;
        bulletL2.transform.position = transform.position + Vector3.down * 1.0f + Vector3.left * 0.75f;
        bulletR1.transform.position = transform.position + Vector3.down * 1.0f + Vector3.right * 0.5f;
        bulletR2.transform.position = transform.position + Vector3.down * 1.0f + Vector3.right * 0.75f;
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
            Invoke("FireFoward", 2);
        else
            Invoke("Think", 2);
    }

    void FireShot()
    {
        Debug.Log("플레이어 방향으로 샷건");
        
        //Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 2);
        else
            Invoke("Think", 2);
    }

    void FireArc()
    {
        Debug.Log("부채 모양으로 발사");

        //Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 2);
        else
            Invoke("Think", 2);
    }

    void FireAround()
    {
        Debug.Log("원 형태로 전체 공격");

        //Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 2);
        else
            Invoke("Think", 2);
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
            Invoke("ReturnSprite", 0.1f);
        }
        

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            //Random Ratio Item Drop
            int ran = enemyName == "B" ? 0 : Random.Range(0, 10);
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

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        
    }

    void ReturnSprite()
    {
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
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            //collision.gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
            
    }
}
