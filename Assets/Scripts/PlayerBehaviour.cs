using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public MainMenuCanvas MainMenuCanvas;
    public ScoreCanvas ScoreCanvas;
    public GameOverCanvas GameOverCanvas;

    public AudioClip Coin;
    public AudioClip Buzzer;
    
    public Transform SpawnAnchor;

    public BallBehaviour BallPrefab;

    
    public int Score { get; private set; }
    public float SecondsLeft { get; private set; }
    
    private AudioSource _audio;

    private BallBehaviour _current;

    
    private bool _isTimerOn;


    public void Awake()
    {
        _audio = transform.GetComponent<AudioSource>();
    }
    
    public void Reset()
    {
        Score = 0;

        _isTimerOn = false;
        SecondsLeft = 90;
    }

    public void EndGame()
    {
        Debug.LogWarning("GAME OVER");
        
        _audio.clip = Buzzer;
        _audio.Play();
        
        CartBehaviour.NextCartId = -1;
        _isTimerOn = false;
        ScoreCanvas.gameObject.SetActive(false);
        GameOverCanvas.gameObject.SetActive(true);
    }
    
    
    

    public void Update()
    {
        if (_isTimerOn)
        {
            SecondsLeft -= Time.deltaTime;
            ScoreCanvas.TextSeconds.SetText($"{Mathf.CeilToInt(SecondsLeft):00}");
            
            if (SecondsLeft <= 0)
            {
                EndGame();
                return;
            }
        }
        
        if (_current == null)
            return;
        
        _current.transform.localEulerAngles = new Vector3(
                -this.transform.localEulerAngles.x,
                0,
                0
            );
    }
    
    
    public bool SpawnBall(bool isMoneyBall)
    {
        if (_current != null)
            return false;

        _isTimerOn = true;

        BallBehaviour ball = Instantiate(BallPrefab, SpawnAnchor);
        ball.IsMoneyBall = isMoneyBall;
        _current = ball;

        return true;
    }

    public void NotifyBallShot()
    {
        _current = null;
    }


    public void NotifyScore(bool isMoneyBall)
    {
        Debug.Log("SCORE!!!!");
        
        Score++;
        if (isMoneyBall)
            Score++;

        _audio.clip = Coin;
        _audio.Play();

        ScoreCanvas.TextScore.SetText($"Score: {Score}");
    }

}