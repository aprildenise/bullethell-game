using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOverTime : MonoBehaviour
{

    public float timeToDespawn;
    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(timeToDespawn);
        timer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.GetStatus() == Timer.Status.FINISHED)
        {
            Destroy(this.gameObject);
        }
    }
}
