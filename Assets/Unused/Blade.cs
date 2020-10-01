using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour, ITypeSize
{

    private Sword sword;

    private void Start()
    {
        sword = transform.parent.GetComponent<Sword>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //TypeSizeController.Interact(this.gameObject, other.gameObject);
    }

    public Type GetGameType()
    {
        return sword.GetGameType();
    }

    public Size GetSize()
    {
        return sword.GetSize();
    }

    public void OnAdvantage(GameObject collider, GameObject other)
    {
        Debug.Log("SWORD ADVANTAGE");
    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        Debug.Log("SWORD DISADVANTAGE");
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {
        Debug.Log("SWORD NEUTRAL");
    }

    public void SetSize(Size size)
    {
        throw new System.NotImplementedException();
    }

    public void SetType(Type type)
    {
        throw new System.NotImplementedException();
    }
}
