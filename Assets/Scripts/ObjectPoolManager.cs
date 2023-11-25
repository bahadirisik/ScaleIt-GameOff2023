using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject objectPoolEmptyHolder;

    private static GameObject particleSystemsEmpty;
    private static GameObject gameObjectsSystemEmpty;

    public enum PoolType
	{
        ParticleSystem,
        GameObjectSystem,
        None
	}

    public static PoolType PoolingType;

	private void Awake()
	{
        CreateEmptyParents();
	}

    private void CreateEmptyParents()
	{
        objectPoolEmptyHolder = new GameObject("Pooled Objects");

        particleSystemsEmpty = new GameObject("Particle Effects");
        particleSystemsEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        gameObjectsSystemEmpty = new GameObject("GameObjects");
        gameObjectsSystemEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
    }

	public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPoint, Quaternion spawnRotation, PoolType poolType = PoolType.None)
	{
        PooledObjectInfo pool = ObjectPools.Find(p=> p.Name == objectToSpawn.name);

        //Eðer havuz yoksa oluþtur
        if(pool == null)
		{
            pool = new PooledObjectInfo { Name = objectToSpawn.name };
            ObjectPools.Add(pool);
		}

        //Havuzun içinde inaktif obje var mý diye kontrol et
        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if(spawnableObject == null)
		{
            //uygun parent objeyi bul
            GameObject parentObject = SetParentObject(poolType);

            //eðer inaktif obje yoksa oluþtur
            spawnableObject = Instantiate(objectToSpawn,spawnPoint,spawnRotation);

            if(parentObject != null)
			{
                spawnableObject.transform.SetParent(parentObject.transform);
			}
		}
		else
		{
            //eðer inaktif obje varsa
            spawnableObject.transform.position = spawnPoint;
            spawnableObject.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static void ReturnObjectToPool(GameObject obj)
	{
        string objectName = obj.name.Substring(0,obj.name.Length - 7);

        PooledObjectInfo pool = ObjectPools.Find(p => p.Name == objectName);

        if(pool == null)
		{
            Debug.LogWarning("Pool is null " + obj.name);
		}
		else
		{
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
		}
	}

    private static GameObject SetParentObject(PoolType poolType)
	{
		switch (poolType)
		{
			case PoolType.ParticleSystem:
                return particleSystemsEmpty;
			case PoolType.GameObjectSystem:
                return gameObjectsSystemEmpty;
			case PoolType.None:
                return null;
			default:
                return null;
		}
	}
}

public class PooledObjectInfo
{
    public string Name;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
