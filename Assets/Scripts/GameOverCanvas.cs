using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameOverCanvas : MonoBehaviour
{
    public Button ContinueButton;

    public TMP_Text ScoreText;
    
    public void Awake()
    {
        PlayerBehaviour player = FindObjectOfType<PlayerBehaviour>();
        
        ScoreText.SetText("" + player.Score);
        CartBehaviour.NextCartId = -1;
        
        ContinueButton.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
            player.MainMenuCanvas.gameObject.SetActive(true);
        });
    }
}
