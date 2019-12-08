using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Transform SpawnAnchor;

    public BallBehaviour BallPrefab;


    private BallBehaviour _current;


    public void Update()
    {
        if (_current == null)
            return;
        
        _current.transform.localEulerAngles = new Vector3(
                -this.transform.localEulerAngles.x,
                0,
                0
            );
    }
    
    
    public bool SpawnBall()
    {
        if (_current != null)
            return false;


        BallBehaviour ball = Instantiate(BallPrefab, SpawnAnchor);
        _current = ball;

        return true;
    }

    public void NotifyBallShot()
    {
        _current = null;
    }

}