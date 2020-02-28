using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{


    private GameObject poolingObject;
    private Queue<GameObject> objectPool;
    private int size;


    /// <summary>
    /// Constructor for ObjectPooler. Instantiates a queue of pooling Objects of size.
    /// </summary>
    /// <param name="poolingObject">GameObject to be pooled.</param>
    /// <param name="size">Size of the pool.</param>
    public ObjectPooler(GameObject poolingObject, int size){
        this.poolingObject = poolingObject;
        objectPool = new Queue<GameObject>();
        this.size = size;

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(poolingObject);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

    }

    /// <summary>
    /// Dequeue an object from the pool and enqueue it again for later use.
    /// </summary>
    /// <returns>GameObject that is dequeued.</returns>
    public GameObject TakeFromPool()
    {
        GameObject obj = null;
        obj = objectPool.Dequeue();

        //obj.SetActive(true);
        //obj.transform.position = parentPosition;
        //obj.transform.rotation = parentRotation;

        //IPooledObject pooledObject = obj.GetComponent<IPooledObject>();

        //if (pooledObject != null)
        //{
        //    pooledObject.OnObjectSpawn();
        //}

        objectPool.Enqueue(obj);
        return obj;
    }

}
