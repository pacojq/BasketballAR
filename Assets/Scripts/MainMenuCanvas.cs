using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public Button StartButton;

    public void Awake()
    {
        CartBehaviour.NextCartId = -1;
        StartButton.onClick.AddListener(() =>
        {
            PlayerBehaviour player = FindObjectOfType<PlayerBehaviour>();
            
            this.gameObject.SetActive(false);
            
            player.Reset();
            player.ScoreCanvas.gameObject.SetActive(true);

            CartBehaviour.NextCartId = 0;
        });
    }
}
