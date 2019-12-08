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

    public float MaxHeight = 0.25f;
    public float Gravity = -0.9f;

    public bool IsMoneyBall = false;
    
    private Stopwatch _sw;
    private Vector2 _mousePos;



    public void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
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
        Vector3 spdXZ = dxz / (Mathf.Sqrt((-2 * MaxHeight) / Gravity) + Mathf.Sqrt((2f * (dy - MaxHeight)) / Gravity));

        if (float.IsNaN(spdXZ.x) || float.IsNaN(spdXZ.z))
            return Vector3.zero;

        return spdXZ + spdY;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TARGET_TAG))
        {
            FindObjectOfType<PlayerBehaviour>().NotifyScore(this.IsMoneyBall);
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
        
        // TODO check if elapsed is too big ?

        Vector2 mouseEnd = Input.mousePosition;
        float dy = mouseEnd.y - _mousePos.y;

        float spd = dy / elapsed;
        
        Debug.Log($"Delta Y: {dy}");
        Debug.Log($"Elapsed millis: {elapsed} ms");
        Debug.Log($"SPEED: {spd} ms");

        
        
        Transform parent = null;
        GameObject vuforia = GameObject.FindWithTag("vuforia-image");
        if (vuforia != null)
            parent = vuforia.transform;
        
        this.transform.SetParent(parent);
        
        
        

        Vector3 target = GameObject.FindWithTag(TARGET_TAG).transform.position;
        Vector3 pos = this.transform.position;
        
        Vector2 delta = new Vector2(target.x, target.z) - new Vector2(pos.x, pos.z);
        delta = delta.normalized;
        
        if (spd < 4) // Too slow
        {
            spd -= 4;
            target += new Vector3(
                spd * 0.05f * delta.x,
                0,
                spd * 0.05f * delta.y
            );
        }
        else if (spd > 6) // Too fast
        {
            spd -= 6;
            target += new Vector3(
                spd * 0.05f * delta.x,
                0,
                spd * 0.05f * delta.y
            );
        }

        MaxHeight = (target.y - this.transform.position.y) + 0.25f;// 0.5f;
        MaxHeight += 0.005f * (spd - 5);
        MaxHeight = Mathf.Max(MaxHeight, 0.05f);
        
        // TODO wiggle due to mouse x ?
        
        Launch(target);
    }
}
