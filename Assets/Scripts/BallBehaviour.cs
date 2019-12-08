using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _target;

    public float MaxHeight = 0.2f;
    public float Gravity = -0.9f;
    
    private Stopwatch _sw;
    private Vector2 _mousePos;



    public void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        
        _target = GameObject.FindWithTag("basket-target").transform;
    }


    private void Launch()
    {
        Physics.gravity = Vector3.up * Gravity;
        _rb.useGravity = true;
        _rb.velocity = CalculateLaunchVelocity();

        FindObjectOfType<PlayerBehaviour>().NotifyBallShot();
        
        Debug.Log(_rb.velocity);
    }

    private Vector3 CalculateLaunchVelocity()
    {
        Vector3 targetPos = _target.position;
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
    
    
    
    
    

    private void OnMouseDown()
    {
        _mousePos = Input.mousePosition; 
        Debug.Log($"Mouse down: {_mousePos}");
        _sw = new Stopwatch();
        _sw.Start();
        
        Launch();
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
    }
}
