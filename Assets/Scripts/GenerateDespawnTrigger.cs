using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDespawnTrigger : MonoBehaviour
{

    public float despawnerOffset;

    // Start is called before the first frame update
    void Start()
    {

        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.LogWarning("Cannot generate a despawn trigger wall if there is no box collider on the current object.", this.gameObject);
        }

        float colliderWidth = collider.size.x;
        float colliderHeight = collider.size.y;

        // For despawner
        Transform origin = transform.Find("Despawn Trigger");
        // Add collider trigger to top of collider.
        SpawnWall(origin, new Vector2(0f, (colliderHeight / 2) + despawnerOffset), new Vector2(colliderWidth + despawnerOffset * 2f, 1f), true);
        // Add collider trigger to right of collider.
        SpawnWall(origin, new Vector2((colliderWidth / 2) + despawnerOffset, 0f), new Vector2(1f, colliderHeight + despawnerOffset * 2f), true);
        // Add collider trigger to the bottom of the collider.
        SpawnWall(origin, new Vector2(0f, (colliderHeight / -2) - despawnerOffset), new Vector2(colliderWidth + despawnerOffset * 2f, 1f), true);
        // Add collider trigger to the left of the collider.
        SpawnWall(origin, new Vector2((colliderWidth / -2) - despawnerOffset, 0f), new Vector2(1f, colliderHeight + despawnerOffset * 2f), true);

        // For player wall
        origin = transform.Find("Player Wall");
        // Add collider trigger to top of collider.
        SpawnWall(origin, new Vector2(0f, (colliderHeight / 2)), new Vector2(colliderWidth, 1f), false);
        // Add collider trigger to right of collider.
        SpawnWall(origin, new Vector2((colliderWidth / 2), 0f), new Vector2(1f, colliderHeight), false);
        // Add collider trigger to the bottom of the collider.
        SpawnWall(origin, new Vector2(0f, (colliderHeight / -2)), new Vector2(colliderWidth, 1f), false);
        // Add collider trigger to the left of the collider.
        SpawnWall(origin, new Vector2((colliderWidth / -2), 0f), new Vector2(1f, colliderHeight), false);

    }

    public void SpawnWall(Transform parent, Vector2 position, Vector2 size, bool despawner)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.parent = parent;

        // Change the position of the collider
        box.transform.position = position;

        // Change the size of the collider
        BoxCollider boxCollider = box.GetComponent<BoxCollider>();
        boxCollider.size = size;

        // Hide the Mesh
        MeshRenderer mesh = box.GetComponent<MeshRenderer>();
        mesh.enabled = false;
        
        if (despawner)
        {
            boxCollider.isTrigger = true;
            box.AddComponent<Despawner>();
        }
    }
}
