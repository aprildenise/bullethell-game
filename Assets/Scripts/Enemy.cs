using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IActivator
{

    public Shooter shooter;
    private bool isActive;

    private void Start()
    {
        isActive = false;
        SetInvisible(true);
    }

    public void activate()
    {
        if (!isActive)
        {
            Debug.Log("Triggered.");
            SetInvisible(false);
            isActive = true;
        }
    }

    private void SetInvisible(bool active)
    {
        GetComponent<MeshRenderer>().enabled = !active;
        GetComponent<BoxCollider>().enabled = !active;
        shooter.enabled = !active;
    }

}
