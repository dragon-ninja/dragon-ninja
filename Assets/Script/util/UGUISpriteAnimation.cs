using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Animation))]
public class UGUISpriteAnimation : MonoBehaviour
{
    public Animation Animation { get; set; }

    public void Awake()
    {
        Animation = GetComponent<Animation>();
        IsPlaying = false;
    }

    private float _lastFrameTime;
    private float _currentTime;
    private float _progressTime;

    public bool IsPlaying { get; set; }

    public void OnEnable()
    {
        if (Animation.playAutomatically)
        {
            Play();
        }
    }

    public void Play()
    {
        _progressTime = 0f;
        _lastFrameTime = Time.realtimeSinceStartup;
        IsPlaying = true;
    }

    public void Update()
    {
        if (!IsPlaying) return;
        var clip = Animation.clip;
        var state = Animation[clip.name];
        _currentTime = Time.realtimeSinceStartup;
        var deltaTime = _currentTime - _lastFrameTime;
        _lastFrameTime = _currentTime;
        _progressTime += deltaTime * state.speed;
        state.normalizedTime = _progressTime / clip.length;
        Animation.Sample();
        if (_progressTime >= clip.length)
        {
            IsPlaying = false;
        }
    }
}
