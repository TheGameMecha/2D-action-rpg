using UnityEngine;
using System;

/// <summary>
/// A Timer object that tracks its own state
/// </summary>
[System.Serializable]
public class Timer
{
    public float duration = 1.0f;
    public bool isActive;

    float timer = 0f;

    /// <summary>
    /// Construct a new Timer
    /// </summary>
    /// <param name="_duration">The amount of time before the timer is done</param>
    public Timer(float _duration)
    {
        duration = _duration;
        isActive = false;
    }

    /// <summary>
    /// Begin the timer from the beginning
    /// </summary>
    public void StartTimer()
    {
        isActive = true;
        timer = 0f;
    }

    /// <summary>
    /// Call this from your Update function
    /// </summary>
    /// <param name="delta">Delta time being used</param>
    public void Tick(float delta)
    {
        if (isActive)
            timer += delta;

        if (timer >= duration)
        {
            TimerCompleted();
            isActive = false;
        }
    }

    /// <summary>
    /// Pauses the timer without resetting the current time
    /// </summary>
    public void PauseTimer()
    {
        isActive = false;
    }

    /// <summary>
    /// Unpauses the timer, leaving off when it was last paused
    /// </summary>
    public void UnpauseTimer()
    {
        isActive = true;
    }

    /// <summary>
    /// Get if this timer has reached its duration
    /// </summary>
    /// <returns>Whether or not the timer has finished</returns>
    public bool IsDone()
    {
        return !isActive;
    }

    /// <summary>
    /// Get the current value of the timer
    /// </summary>
    /// <returns>Returns the current time of the timer</returns>
    public float GetCurrentTime()
    {
        return timer;
    }

    public event Action onTimerCompleted;

    public void TimerCompleted()
    {
        if (onTimerCompleted != null)
        {
            onTimerCompleted();
        }
    }
}