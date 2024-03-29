using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }

    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
        
        if (sprites[endIndex].position.y < viewHeight * (-1))
        {
            //Sprite ReUse
            Vector3 backSpritePos = sprites[endIndex].localPosition;
            Vector3 frontSpritePos = sprites[startIndex].localPosition;

            sprites[endIndex].transform.localPosition = frontSpritePos + Vector3.up * viewHeight;
            


            //Cursor Index Change
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = startIndexSave - 1 == -1 ? sprites.Length - 1 : startIndexSave -1;

        }
    }
}
