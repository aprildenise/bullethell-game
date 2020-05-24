using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDestructable
{

    #region Fields

    private Player thisPlayer;
    private Rigidbody rigidBody;

    /// <summary>
    /// Given multiplier to determine how fast the object will move.
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// Given multiplier to make the object move slower than the moveSpeed.
    /// </summary>
    /// 
    private float crawlSpeed;
    /// <summary>
    /// Force applied to object to make it move.
    /// </summary>
    private Vector3 moveVelocity;
    public ManualShooter shooter;
    public float attackPower;
    public static bool isMoving;

    private int heathPoints;

    #endregion


    #region Singleton
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
    }

    #endregion

    /// <summary>
    /// Start is called before the first frame update. Handles setups specific to the PlayerController.
    /// </summary>
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogWarning("PlayerController cannot find a RigidBody2D component on the object it is attached to.", this);
            return;
        }
        crawlSpeed = moveSpeed / 2;

        if (shooter == null)
        {
            Debug.LogWarning("PlayerController does not have a shooter component.", this);
        }

        isMoving = false;
    }

    /// <summary>
    /// Update is called once per frame. Listens for inputs from the player.
    /// </summary>
    void Update()
    {

        // Get axis input for movement.
        //Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (moveInput != Vector3.zero)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.LeftShift)) // If player is hitting leftShift, calculate movement with crawlSpeed.
        {
            moveVelocity = moveInput.normalized * crawlSpeed;
        }
        else
        {
            moveVelocity = moveInput.normalized * moveSpeed;
        }

    }

    /// <summary>
    /// Update is called for every physics update. Applies physics to the attached object to move the object based on player input.
    /// </summary>
    private void FixedUpdate()
    {
        // Move the object based on the movement input
        rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);

        // Get button input for firing.
        if (shooter != null)
        {
            if (Input.GetKey(KeyCode.X))
            {
                shooter.AllowShooting(true);
            }
            else
            {
                shooter.AllowShooting(false);
            }
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Enemy Bullet")
    //    {
    //        LoseLife();
    //        Destroy(other.gameObject);
    //    }
    //}

    private void LoseLife()
    {
        Debug.Log("Player lost a life.");
    }

    public void Damage(float damageReceived)
    {
        LoseLife();
    }
}
