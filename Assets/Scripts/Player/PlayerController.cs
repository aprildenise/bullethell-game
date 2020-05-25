using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDestructable
{

    #region Variables

    private Player thisPlayer;
    private Rigidbody rigidBody;
    private WeaponPanel weaponPanel;

    /// <summary>
    /// Given multiplier to determine how fast the object will move.
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// Given multiplier to make the object move slower than the moveSpeed.
    /// </summary>
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

    public static PlayerController GetPlayerController()
    {
        return instance;
    }

    #endregion

    /// <summary>
    /// Start is called before the first frame update. Handles setups specific to the PlayerController.
    /// </summary>
    void Start()
    {

        moveSpeed = moveSpeed == 0 ? 10f : moveSpeed;
        weaponPanel = WeaponPanel.GetWeaponPanel();

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

        // If player is hitting leftShift, calculate movement with crawlSpeed.
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            moveVelocity = moveInput.normalized * crawlSpeed;
        }
        else
        {
            moveVelocity = moveInput.normalized * moveSpeed;
        }

        // If the player is hitting space, show the weapon panel.
        if (Input.GetKey(KeyCode.Space))
        {
            weaponPanel.ActivateWeaponPanel();
        }
        else
        {
            weaponPanel.DeactivateWeaponPanel();
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


    private void LoseLife()
    {
        Debug.Log("Player lost a life.");
    }

    #region Destructible

    public void ReceiveDamage(float damageReceived)
    {
        LoseLife();
    }

    public void OnZeroHealth()
    {
        // game over
    }

    public bool HasHealth()
    {
        return true;
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    #endregion
}
