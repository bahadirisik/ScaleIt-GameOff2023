using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Grid grid;
	[SerializeField] private List<Transform> levelCells;
	[SerializeField] private List<Transform> cellsToPutCubes;
	[SerializeField] private GameObject levelCellsIndicator;
	[SerializeField] private GameObject cellsToPutCubesIndicator;

	private void Start()
	{
		GenerateMapVisualize();
	}

	private void GenerateMapVisualize()
	{
		foreach (var cell in levelCells)
		{
			Vector3Int cellIndicatorPosition = grid.WorldToCell(cell.position);
			Instantiate(levelCellsIndicator,new Vector3(cellIndicatorPosition.x,0f,cellIndicatorPosition.z),Quaternion.identity);
		}

		foreach (var cell in cellsToPutCubes)
		{
			Vector3Int cellIndicatorPosition = grid.WorldToCell(cell.position);
			Instantiate(cellsToPutCubesIndicator, new Vector3(cellIndicatorPosition.x, 0f, cellIndicatorPosition.z), Quaternion.identity);
		}
	}

	public List<Vector3Int> GetLevelCells()
	{
		List<Vector3Int> levelCellsPositions = new List<Vector3Int>();
		foreach (var cellTransform in levelCells)
		{
			levelCellsPositions.Add(grid.WorldToCell(cellTransform.position));
		}

		return levelCellsPositions;
	}

	public List<Vector3Int> GetCellsToPutCubes()
	{
		List<Vector3Int> cellsToPutPositions = new List<Vector3Int>();
		foreach (var cellTransform in cellsToPutCubes)
		{
			cellsToPutPositions.Add(grid.WorldToCell(cellTransform.position));
		}

		return cellsToPutPositions;
	}
}
