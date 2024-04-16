using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributeObjects : MonoBehaviour
{
    public GameObject[] objectsToDistribute; // 要分布的对象数组

    public void Distribute(GameObject parentObject)
    {
        if (parentObject != null && objectsToDistribute != null && objectsToDistribute.Length > 0)
        {
            Bounds combinedBounds = new Bounds();
            bool boundsInitialized = false;

            SkinnedMeshRenderer[] renderers = parentObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                if (!boundsInitialized)
                {
                    combinedBounds = new Bounds(renderer.bounds.center, renderer.bounds.size);
                    boundsInitialized = true;
                }
                else
                {
                    combinedBounds.Encapsulate(renderer.bounds);
                }
            }

            // 转换bounds到世界坐标
            Vector3 worldSize = combinedBounds.size;
            Vector3 worldCenter = combinedBounds.center;

            for (int i = 0; i < objectsToDistribute.Length; i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(worldCenter.x - worldSize.x / 3, worldCenter.x + worldSize.x / 3),
                    Random.Range(worldCenter.y - worldSize.y / 3, worldCenter.y + worldSize.y / 3),
                    Random.Range(worldCenter.z - worldSize.z / 3, worldCenter.z + worldSize.z / 3)
                );

                objectsToDistribute[i].transform.position = randomPosition;
            }
        }
    }
}
