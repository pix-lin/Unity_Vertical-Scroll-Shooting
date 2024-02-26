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
