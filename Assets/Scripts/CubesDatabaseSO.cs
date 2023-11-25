using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CubesDatabaseSO : ScriptableObject
{
    public List<CubeData> cubesData;
}

[Serializable]
public class CubeData
{
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public int ID { get; private set; }
	public Color CubeColor;
	[field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
	[field: SerializeField] public GameObject CubePrefab { get; private set; }
}