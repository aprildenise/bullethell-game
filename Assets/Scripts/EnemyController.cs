﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float healthPoints;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("HealthPoints of " + gameObject.name + ":" + healthPoints);
    }


    /// <summary>
    /// Deal damage and subtrack from this Enemy's healthPoints. When healthPoints reaches zero,
    /// this Enemy object is destroyed.
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(float damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Bullet")
        {
            Damage(CalculateDamageTaken(other));
        }
    }

    /// <summary>
    /// Calculate the damage dealt to an enemy based on the enemy's distance from the player.
    /// Only used if isPlayerShooter is true.
    /// </summary>
    /// <param name="hit">RaycastHit of the object the laser/bullet is hitting.</param>
    /// <returns>Calculated damage.</returns>
    private float CalculateDamageTaken(Collider hit)
    {
        // Damage dealt is based on the distance from the raycast origin to the center of the hit collider.
        // The smaller the distance means the more damage dealt.
        float damage = 0f;
        float distance = Mathf.Abs(Vector3.Distance(hit.transform.position, PlayerController.instance.transform.position)) / 100;
        damage = PlayerController.instance.attackPower - distance;
        if (damage <= 0)
        {
            damage = PlayerController.instance.attackPower;
        }
        return damage;
    }
}
