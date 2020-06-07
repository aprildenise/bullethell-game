using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDestructable
{

    #region Variables

    // Other components for the Player object
    private Player thisPlayer;
    private Rigidbody rigidBody;
    public Weapon currentWeapon;
    public Weapon[] availableWeapons;
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
    
    public float attackPower;
    public bool isMoving;
    public Vector3 lookingAt;
    //private bool openedWeaponPanel;
    private int heathPoints;

    #endregion


    #region Singleton
    private static PlayerController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public static PlayerController GetInstance()
    {
        return instance;
    }

    #endregion

    /// <summary>
    /// Start is called before the first frame update. Handles setups specific to the PlayerController.
    /// </summary>
    private void Start()
    {

        moveSpeed = moveSpeed == 0 ? 10f : moveSpeed;
        weaponPanel = WeaponPanel.GetWeaponPanel();

        rigidBody = GetComponent<Rigidbody>();
        crawlSpeed = moveSpeed / 2;

        isMoving = false;
        //openedWeaponPanel = false;
    }

    /// <summary>
    /// Update is called once per frame. Listens for inputs from the player.
    /// </summary>
    private void Update()
    {
        //Debug.Log("weapon panel:" + WeaponPanel.panelEnabled);
        //Debug.Log("input:" + Input.GetAxisRaw("Horizontal"));

        // If the player is hitting space, show the weapon panel.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (WeaponPanel.panelEnabled)
            {
                weaponPanel.DeactivateWeaponPanel();
                //openedWeaponPanel = false;
            }
            else
            {
                //openedWeaponPanel = true;
                weaponPanel.ActivateWeaponPanel();
            }
        }

        if (WeaponPanel.panelEnabled) return;

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


        // Calculate the rotation of the player.
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            lookingAt = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
            this.gameObject.transform.LookAt(lookingAt);

        }

    }


    /// <summary>
    /// Update is called for every physics update. Applies physics to the attached object to move the object based on player input.
    /// </summary>
    private void FixedUpdate()
    {

        if (WeaponPanel.panelEnabled) return;

        // Move the object based on the movement input
        rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);

        // Get button input for firing.
        if (currentWeapon != null)
        {
            if (Input.GetMouseButton(0))
            {
                currentWeapon.UseWeapon(true);
            }
            else
            {
                currentWeapon.UseWeapon(false);
            }
        }
    }


    private void LoseLife()
    {
        Debug.Log("Player lost a life.");
    }

    /// <summary>
    /// Set the Player's current Shooter based on the given Tyoe and Size.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="size"></param>
    public void SetCurrentShooter(Type type, Size size)
    {
        // Find the shooter in the available shooters.
        foreach (Weapon w in availableWeapons)
        {
            //Debug.Log("s:" + s.GetGameType() + s.GetSize());
            if (TypeSizeController.Equals(w.GetGameType(), type) && TypeSizeController.Equals(w.GetSize(), size))
            {
                //TODO See if it's appropriate to simply deactivate
                // Turn off current shooter and set the new one.
                if (currentWeapon != null) currentWeapon.gameObject.SetActive(false);
                currentWeapon = w;
                currentWeapon.gameObject.SetActive(true);
                return;
            }
        }
        
    }

    public string GetCurrentWeaponName()
    {
        if (currentWeapon == null)
        {
            return "";
        }
        return currentWeapon.weaponName;
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
