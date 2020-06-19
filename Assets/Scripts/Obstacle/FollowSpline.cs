using Pixelplacement;
using UnityEngine;

public class FollowSpline : MonoBehaviour
{
    public Spline splineToFollow;
    private GameObject follower;
    public int followerIndex = 0;
    public float startPosition = 0;
    public float endPosition = 1;
    public float followDuration;
    public float followDelay;
    public bool destroyOnFinish;
    public bool parentHasStateMachine;

    private bool finishedFollowing;
    private Timer timer;

    // Other components
    IStateTracker stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        if (follower == null) follower = transform.parent.gameObject;
        if (parentHasStateMachine) stateMachine = transform.parent.gameObject.GetComponent<IStateTracker>();

        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(followDuration, Timer.Status.IDLE);
        Tween.Spline(splineToFollow, follower.transform, startPosition, endPosition, false, followDuration, followDelay, Tween.EaseInOut);
        timer.StartTimer();

        finishedFollowing = false;
    }

    private void Update()
    {

        if (!finishedFollowing)
        {

            if (timer.GetStatus() == Timer.Status.FINISHED) finishedFollowing = true;

        }
        else
        {
            if (parentHasStateMachine) stateMachine.ReportFinishedState();
            if (destroyOnFinish) Destroy(this);
        }

    }



}
