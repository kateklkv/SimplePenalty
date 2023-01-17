using System;
using UnityEngine;

public class InputService : MonoBehaviour
{
    public event Action<Vector2, float> LongTouch;
    
    private Vector2 _startPos, _endPos, _direction;
    private float _touchTimeStart, _touchTimeFinish, _timeInterval;

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _touchTimeStart = Time.time;
            _startPos = Input.GetTouch(0).position;
        } else if (Input.GetMouseButtonDown(0))
        {
            _touchTimeStart = Time.time;
            _startPos = Input.mousePosition;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            _touchTimeFinish = Time.time;
            _timeInterval = _touchTimeFinish - _touchTimeStart;
            
            _endPos = Input.GetTouch(0).position;
            _direction = _startPos - _endPos;
            
            LongTouch?.Invoke(_direction, _timeInterval);
        } else if (Input.GetMouseButtonUp(0))
        {
            _touchTimeFinish = Time.time;
            _timeInterval = _touchTimeFinish - _touchTimeStart;
            
            _endPos = Input.mousePosition;
            _direction = _startPos - _endPos;
            
            LongTouch?.Invoke(_direction, _timeInterval);
        }
    }
}
