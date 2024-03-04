using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anime;

    void Start()
    {
        anime = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (gameObject.activeSelf)
            StartCoroutine(DisableWithDelay(2.0f));
    }

    IEnumerator DisableWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }

    public void StartExplosion(string target)
    {
        anime.SetTrigger("OnExplosion");

        switch (target)
        {
            case "S":
                transform.localScale = Vector3.one * 0.7f;
                break;
            case "P":
            case "M":
                transform.localScale = Vector3.one * 1f;
                break;
            case "L":
                transform.localScale = Vector3.one * 2f;
                break;
            case "B":
                transform.localScale = Vector3.one * 3f;
                break;
            default:
                Debug.Log("크기변화 없음");
                break;
        }
    }
}
