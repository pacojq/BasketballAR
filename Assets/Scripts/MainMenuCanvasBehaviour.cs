using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvasBehaviour : MonoBehaviour
{
    public Button StartButton;

    public void Awake()
    {
        CartBehaviour.NextCartId = -1;
        StartButton.onClick.AddListener(() =>
        {
            CartBehaviour.NextCartId = 0;
            this.gameObject.SetActive(false);
            
            // TODO activate score canvas
        });
    }
}
