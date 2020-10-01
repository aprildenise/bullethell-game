using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOverTime : MonoBehaviour, ITimerNotification
{

    public float timeToDespawn;
    private Timer timer;

    public void OnTimerFinished()
    {
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(timeToDespawn);
        timer.StartTimer();
    }
}
