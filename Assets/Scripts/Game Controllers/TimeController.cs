using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeController : MonoBehaviour
{

    [SerializeField]
    private PostProcessVolume effects;

    //private bool slowedTime;
    private float slowDownFactor;
    private float slowDownDuration;

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
    }

    public static TimeController GetInstance()
    {
        return instance;
    }

    #endregion

    public void SlowTime(float slowDownFactor, float slowDownDuration)
    {

        if (slowDownFactor < 0 || slowDownFactor > 1)
        {
            throw new System.ArgumentException("slowDownFactor is not within the range [0,1]", "slowedDownFactor");
        }

        //slowedTime = true;
        this.slowDownFactor = slowDownFactor;
        this.slowDownDuration = slowDownDuration;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        effects.enabled = true;
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedUnscaledDeltaTime;
        effects.enabled = false;
    }
}
