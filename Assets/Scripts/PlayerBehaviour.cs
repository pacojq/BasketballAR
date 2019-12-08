using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public MainMenuCanvas MainMenuCanvas;
    public ScoreCanvas ScoreCanvas;
    
    
    public Transform SpawnAnchor;

    public BallBehaviour BallPrefab;


    private BallBehaviour _current;
    private int _score;

    
    private bool _isTimerOn;
    private float _secondsLeft = 90;


    public void Reset()
    {
        _score = 0;

        _isTimerOn = false;
        _secondsLeft = 90;
    }

    public void EndGame()
    {
        Debug.LogWarning("GAME OVER");
        
        CartBehaviour.NextCartId = -1;
        _isTimerOn = false;
    }
    
    
    

    public void Update()
    {
        if (_isTimerOn)
        {
            _secondsLeft -= Time.deltaTime;
            ScoreCanvas.TextSeconds.SetText($"{Mathf.CeilToInt(_secondsLeft):00}");
            
            if (_secondsLeft <= 0)
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
        
        _score++;
        if (isMoneyBall)
            _score++;

        // TODO sound

        ScoreCanvas.TextScore.SetText($"Score: {_score}");
    }

}