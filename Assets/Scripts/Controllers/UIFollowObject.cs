using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowObject : MonoBehaviour
{

    public GameObject target;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        //Vector3 position = Camera.main.WorldToScreenPoint(target.transform.position);
        //Debug.Log("position:" + position);
        //this.transform.position = position;
        Vector3 position = target.transform.position + offset;
        this.transform.position = position;
    }
}
