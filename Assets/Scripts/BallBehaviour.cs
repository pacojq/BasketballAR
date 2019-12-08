using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BallBehaviour : MonoBehaviour
{
    private Stopwatch _sw;
    private Vector2 _mousePos;


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
    }
}
