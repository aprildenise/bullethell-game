using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


[RequireComponent(typeof(Timer))]
public class TimeController : MonoBehaviour, ITimerNotification
{

    [SerializeField]
    private PostProcessVolume effects;

    private bool timeSlowed = false;
    private float slowDownFactor;
    private float slowDownDuration;

    private Timer timer;

    #region Singleton

    private static TimeController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        timer = gameObject.GetComponent<Timer>();
    }

    public static TimeController GetInstance()
    {
        return instance;
    }

    #endregion

    /// <summary>
    /// Slow down time for a given factor and duration.
    /// A Timer Component is created to count for the duration and notify
    /// TimeController when the duration is finished.
    /// </summary>
    /// <param name="slowDownFactor">Slow down factor, between 0 and 1.</param>
    /// <param name="slowDownDuration">Slow down duration.</param>
    /// <param name="delay">Delay duration occuring before time is slowed down.</param>
    public void SlowTimeForDuration(float slowDownFactor, float slowDownDuration, float delay)
    {
        if (timeSlowed) return;

        this.slowDownDuration = slowDownDuration;

        // Create and start a Timer for delay.
        // If there is delay, then SlowDown() will be called from OnTimerFinished().
        if (delay > 0)
        {
            timer.SetTimer(delay);
            timer.StartTimer();
            return;
        }

        // Start the Timer for slowDown and slow down time.
        timer.SetTimer(slowDownDuration);
        timer.StartTimer();
        SlowTime(slowDownFactor);
    }

    /// <summary>
    /// Slow down time for a given factor.
    /// </summary>
    /// <param name="slowDownFactor"></param>
    public void SlowTime(float slowDownFactor)
    {
        if (timeSlowed) return;

        if (slowDownFactor < 0 || slowDownFactor > 1)
        {
            throw new System.ArgumentException("slowDownFactor is not within the range [0,1]", "slowedDownFactor");
        }

        timeSlowed = true;
        this.slowDownFactor = slowDownFactor;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        effects.enabled = true;
    }

    /// <summary>
    /// Reset the game's time back to its original pace.
    /// </summary>
    public void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedUnscaledDeltaTime;
        effects.enabled = false;
        timeSlowed = false;
    }

    /// <summary>
    /// Called from the Timer. Controls what happens when the Timer is finished.
    /// If time is slowed, then we call ResetTime().
    /// Else time is not slowed already, we slow it.
    /// </summary>
    public void OnTimerFinished()
    {
        if (timeSlowed)
        {
            ResetTime();
        }
        else
        {
            // Start the Timer for slowDown and slow down time.
            timer.SetTimer(slowDownDuration);
            timer.StartTimer();
            SlowTime(slowDownFactor);
        }
    }
}
