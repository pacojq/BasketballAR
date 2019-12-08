using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BallBehaviour : MonoBehaviour
{
    public const string TARGET_TAG = "basket-target";
    
    
    private Rigidbody _rb;
    private Transform _target;

    public float MaxHeight = 0.25f;
    public float Gravity = -0.9f;
    
    private Stopwatch _sw;
    private Vector2 _mousePos;



    public void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        
        _target = GameObject.FindWithTag(TARGET_TAG).transform;
    }


    private void Launch(Vector3 targetPos)
    {
        Physics.gravity = Vector3.up * Gravity;
        _rb.useGravity = true;
        _rb.velocity = CalculateLaunchVelocity(targetPos);

        FindObjectOfType<PlayerBehaviour>().NotifyBallShot();
        
        Debug.Log(_rb.velocity);
    }

    private Vector3 CalculateLaunchVelocity(Vector3 targetPos)
    {
        Vector3 ballPos = this.transform.position;
        
        float dy = targetPos.y - ballPos.y;
        
        Vector3 dxz = new Vector3(
                targetPos.x - ballPos.x,
                0,
                targetPos.z - ballPos.z
            );

        Vector3 spdY = Vector3.up * Mathf.Sqrt(-2 * Gravity * MaxHeight);
        Vector3 spdXZ = dxz / (Mathf.Sqrt(-2 * MaxHeight / Gravity) + Mathf.Sqrt(2 * (dy - MaxHeight) / Gravity));

        return spdXZ + spdY;
    }


    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == TARGET_TAG)
        {
            FindObjectOfType<PlayerBehaviour>().NotifyScore();
        }
    }


    private void OnMouseDown()
    {
        _mousePos = Input.mousePosition; 
        Debug.Log($"Mouse down: {_mousePos}");
        _sw = new Stopwatch();
        _sw.Start();
    }
    
    private void OnMouseUp()
    {
        _sw.Stop();
        long elapsed = _sw.ElapsedMilliseconds;
        
        // TODO check if elapsed is too big

        Vector2 mouseEnd = Input.mousePosition;
        float dy = mouseEnd.y - _mousePos.y;

        float spd = dy / elapsed;
        
        Debug.Log($"Delta Y: {dy}");
        Debug.Log($"Elapsed millis: {elapsed} ms");
        Debug.Log($"SPEED: {spd} ms");

        this.transform.SetParent(null);

        Vector3 target = _target.position;
        Vector3 pos = this.transform.position;
        
        Vector2 delta = new Vector2(target.x, target.z) - new Vector2(pos.x, pos.z);
        delta = delta.normalized;
        
        if (spd < 4) // Too slow
        {
            spd -= 4;
            target += new Vector3(
                spd * 0.1f * delta.x,
                0,
                spd * 0.1f * delta.y
            );
        }
        else if (spd > 6) // Too fast
        {
            spd -= 6;
            target += new Vector3(
                spd * 0.02f * delta.x,
                0,
                spd * 0.02f * delta.y
            );
        }
        else // Good speed, in range [4, 6]
        {
            
        }
        
        MaxHeight += 0.05f * (spd - 5);
        MaxHeight = Mathf.Clamp(MaxHeight, 0.05f, 0.45f);
        
        // TODO wiggle due to mouse x ?
        
        Launch(target);
    }
}
