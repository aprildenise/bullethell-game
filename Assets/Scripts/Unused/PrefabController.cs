using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    private static PrefabController instance;
    private int counter;
    public List<GameObject> prefabs;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        counter = 0;
    }

    public static PrefabController GetInstance()
    {
        return instance;
    }

    public void InitPrefab(string prefabName, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject obj = Instantiate(prefabs[0], position, rotation, parent);
        obj.name = obj.name + " " + counter;
        counter++;
        obj.GetComponent<DiscreteShooter>().enabled = true;
        Debug.Log(obj.name + ":" + obj.GetComponent<DiscreteShooter>().enabled);
    }

    public void RemovePrefab(GameObject obj)
    {
        prefabs.Remove(obj);
    }

    public GameObject GetPrefabByName(string name)
    {
        foreach(GameObject obj in prefabs)
        {
            if (obj.name.Equals(name)) return obj;
        }
        throw new System.ArgumentNullException("Prefab");
    }
}
