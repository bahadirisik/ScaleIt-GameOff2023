using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
	[SerializeField] private MapGenerator mapGenerator;

	private Dictionary<Vector3Int, GameObject> occupiedPositions = new Dictionary<Vector3Int, GameObject>();
	private List<Vector3Int> avaliablePositions = new List<Vector3Int>();
	private List<List<Vector3Int>> positionedCubeGroups = new List<List<Vector3Int>>();

	public bool CheckCellsToPutPosition(Vector3Int position)
	{
		List<Vector3Int> cellsToPutPositions = mapGenerator.GetCellsToPutCubes();

		return cellsToPutPositions.Contains(position);
	}

	public bool IsCellOccupied(Vector3Int position)
	{
		return occupiedPositions.ContainsKey(position);
	}

	public bool IsLevelCell(Vector3Int position)
	{
		List<Vector3Int> levelCellPositions = mapGenerator.GetLevelCells();

		return levelCellPositions.Contains(position);
	}

	public bool CanCubeScale(Vector3Int startPosition, Grid grid,CubesDatabaseSO database, int selectedCubeIndex)
	{
		avaliablePositions.Clear();

		CubeData currentCube = database.cubesData[selectedCubeIndex];
		int i = Math.Sign(currentCube.Size.x) * 1;

		avaliablePositions.Add(startPosition);

		while (Mathf.Abs(i) < Mathf.Abs(currentCube.Size.x))
		{
			Vector3Int newPosition = grid.WorldToCell(startPosition + new Vector3Int(i, 0, 0));
			if (!mapGenerator.GetLevelCells().Contains(newPosition) || IsCellOccupied(newPosition))
			{
				avaliablePositions.Clear();
				return false;
			}

			avaliablePositions.Add(newPosition);
			i += Math.Sign(i) * 1;
		}

		i = Math.Sign(currentCube.Size.y) * 1;
		while (Mathf.Abs(i) < Mathf.Abs(currentCube.Size.y))
		{
			Vector3Int newPosition = grid.WorldToCell(startPosition + new Vector3Int(0, 0, i));
			if (!mapGenerator.GetLevelCells().Contains(newPosition) || IsCellOccupied(newPosition))
			{
				avaliablePositions.Clear();
				return false;
			}

			avaliablePositions.Add(newPosition);
			i += Math.Sign(i) * 1;
		}

		return true;
	}

	public List<Vector3Int> GetAvaliablePositions()
	{
		return avaliablePositions;
	}

	public Dictionary<Vector3Int, GameObject> GetOccupiedPositions()
	{
		return occupiedPositions;
	}

	public List<List<Vector3Int>> GetPositionedCubeGroups()
	{
		return positionedCubeGroups;
	}

	public void SetOccupiedPositions(Vector3Int position, GameObject cubeObject)
	{
		occupiedPositions.Add(position, cubeObject);
	}

	public void SetPositionedCubeGroups(List<Vector3Int> cubeGroup)
	{
		positionedCubeGroups.Add(cubeGroup);
	}

}
