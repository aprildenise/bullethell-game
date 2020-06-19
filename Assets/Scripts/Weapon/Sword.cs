using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Sword : Weapon
{
    [SerializeField]
    private WeaponInfo info;
    private Blade blade;
    //public Vector3 spawnOffset;
    public float swingSpeed;
    public float swingAngle;

    private Transform rotator;

    //private float transformTime;
    protected bool isSwinging;
    private Vector3 startRotation;
    private Vector3 endRotation;
    private float anglesToEnd;
    private float anglesRotated;


    protected void RunStart()
    {
        this.Start();
    }

    // Start is called before the first frame update
    private void Start()
    {

        //transformTime = 0f;
        isSwinging = false;
        canUseWeapon = true;
        blade = transform.GetChild(0).GetComponent<Blade>();
        //switchedStartEnd = false;

        // Set up the rotation.
        rotator = this.transform;
        startRotation = rotator.eulerAngles;
        endRotation = new Vector3(startRotation.x,
            startRotation.y + swingAngle, 
            startRotation.z);
        if (endRotation.y < 0) endRotation.y += 360f;
        anglesToEnd = Mathf.Abs(endRotation.y - startRotation.y);

        //Debug.Log(startRotation + " to " + endRotation);
        //Debug.Log("angles to end:" + anglesToEnd);

    }


    protected void RunFixedUpdate()
    {
        this.FixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!isSwinging) return;

        // Rotate the sword.
        //Debug.Log(rotator.eulerAngles.y + " going to " + endRotation.y);
        anglesRotated += Mathf.Abs(swingAngle * swingSpeed * Time.deltaTime);
        rotator.RotateAround(rotator.position, Vector3.up, swingAngle * swingSpeed * Time.deltaTime);

        //Debug.Log("angles rotated:" + anglesRotated);

        if (anglesRotated >= anglesToEnd)
        {
            //Debug.Log("reached end");
            isSwinging = false;
            canUseWeapon = true;

            // Reset the rotation so that it will rotate in the other direction now.
            anglesRotated = 0;
            swingAngle *= -1;
            EnableBlade(false);

        }

        
    }

    public void EnableBlade(bool enabled)
    {
        blade.gameObject.SetActive(enabled);
    }


}
