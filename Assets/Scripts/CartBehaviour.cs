using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBehaviour : MonoBehaviour
{

    // Next cart we have to shoot from
    public static int NextCartId = 0;

    public int CartId;
    
    private int _ballCount;
    
    // Start is called before the first frame update
    void Awake()
    {
        _ballCount = 5;
    }


    public void OnMouseDown()
    {
        if (_ballCount <= 0)
        {
            Debug.LogWarning("The cart has no balls left.");
            return;
        }
        if (NextCartId != this.CartId)
        {
            Debug.LogWarning("Cannot pick ball yet.");
            return;
        }
        
        PlayerBehaviour player = FindObjectOfType<PlayerBehaviour>();
        if (!player.SpawnBall())
        {
            Debug.LogWarning("The player cannot spawn any ball.");
            return;
        }

        Transform ballTransform = transform.GetChild(1).GetChild(_ballCount - 1);
        ballTransform.gameObject.SetActive(false);

        _ballCount--;
        if (_ballCount <= 0)
            NextCartId++;
        
        Debug.Log("Ball picked!");
    }
}
