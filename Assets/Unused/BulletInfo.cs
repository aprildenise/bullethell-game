using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Bullet", menuName = "Bullet Info")]
public class BulletInfo : ScriptableObject
{

    [Header("Homing")]
    public float homingRate;

    [Header("Acceleration/Decceleration")]
    public bool accelerating;
    public bool decelerating;
    public float timeToMax;
    public float timeToMin;
    public float maxSpeed;
    public float minSpeed;

    [Header("Deccelerate, then accelerate")]
    public bool deaccelerating;
    public float timeInDecceleration;

    [Header("Exploding")]
    public bool explodeOnContact;
    public float explosionRadius;
    public float explosionForce;
    public GameObject explosionEffect;

    [Header("Spawn More Bullets")]
    public bool spawnMoreBullets;
    public float timeToSpawn;
    public GameObject spawnerPrefab;

    [Header("Despawn Over Time")]
    public bool despawnOverTime;
    public float timeToDespawn;


}
