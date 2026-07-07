using System;
using UnityEngine;
using System.Collections;
public class SquashAndStretch : MonoBehaviour
{
    public bool CanPlay { get; private set; } = true;
    [SerializeField] private float FallSpeed;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private Vector2 maxScale;
    [SerializeField] private Vector2 minScale;
    [SerializeField] private Transform sprite;
    [SerializeField] private Rigidbody2D rigi;
    [SerializeField] private AnimationCurve stretchCurve;

    private void Update()
    {
        if(!CanPlay) return;
        float t = 1f - Mathf.InverseLerp(-FallSpeed, MaxSpeed, rigi.linearVelocityY);
       // Debug.Log(t);
        float scaleY = Mathf.Clamp(stretchCurve.Evaluate(t),minScale.y,maxScale.y);
        float scaleX = Mathf.Clamp(1f / scaleY,minScale.x,maxScale.x);
        sprite.localScale = new Vector3(scaleX, scaleY, 1);
    }

    public void TurnOnOrOff(bool condition) => CanPlay = condition;
}
