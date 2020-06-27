using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    [Header("Exploding")]
    public bool explodeOnContact;
    public float explosionRadius;
    [SerializeField]
    //private GameObject explosionEffect;

    protected override void OnTrigger()
    {
        ParticleController.GetInstance().InitiateParticle(ParticleController.Explosion, transform.position);

        //Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, ~(1 << LayerMask.NameToLayer("Environment")));
        //foreach (Collider collider in colliders)
        //{
        //    Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();

        //    // If this RB has a destructible, it receives damage.
        //    IDestructable destructable = rb.gameObject.GetComponent<IDestructable>();
        //    if (destructable != null)
        //    {
        //        //destructable.ReceiveDamage(CalculateDamage(rb.gameObject));
        //        //continue;
        //    }

        //    // If this is a bullet, the bullet gets destroyed.
        //    Bullet bullet = rb.gameObject.GetComponent<Bullet>();
        //    if (bullet != null)
        //    {
        //        Destroy(bullet.gameObject);
        //    }
        //    //rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        //}

        Destroy(gameObject);

    }

}
