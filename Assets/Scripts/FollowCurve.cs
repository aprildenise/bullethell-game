using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurve : MonoBehaviour
{

    public BezierCurve[] curves;
    private int nextCurve;
    private float followSpeed;
    private float t;
    public bool deccelerateToEnd;
    private bool coroutineAllowed;

    private void Start()
    {
        
        nextCurve = 0;
        t = 0f;
        followSpeed = 0.25f;
        coroutineAllowed = true;
    }

    private void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(Follow(nextCurve));
        }
    }

    private IEnumerator Follow(int curve)
    {
        coroutineAllowed = false;
        Vector3 p0 = curves[curve].transform.GetChild(0).position;
        Vector3 p1 = curves[curve].transform.GetChild(1).position;
        Vector3 p2 = curves[curve].transform.GetChild(2).position;
        Vector3 p3 = curves[curve].transform.GetChild(3).position;

        while (t < 1)
        {
            t += Time.deltaTime * followSpeed;
            Vector3 nextPosition = Mathf.Pow(1 - t, 3) * p0
                + 3 * Mathf.Pow(1 - t, 2) * t * p1
                + 3 * (1 - t) * Mathf.Pow(t, 2) * p2
                + Mathf.Pow(t, 3) * p3;

            transform.position = nextPosition;
            yield return new WaitForEndOfFrame();
        }

        t = 0;
        nextCurve++;
        if (nextCurve > curves.Length - 1)
        {
            coroutineAllowed = false;
        }
        else
        {
            coroutineAllowed = true;
        }
    }

}
