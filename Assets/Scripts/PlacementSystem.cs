using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
	public event Action OnLevelStatusCheck;

	[SerializeField] private PreviewSystem previewSystem;
	[SerializeField] private ControlSystem controlSystem;

    //[SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cellIndicator;
	//[SerializeField] private GameObject placeCubeEffect;
	//[SerializeField] private GameObject removeCubeEffect;
    [SerializeField] private InputManager inputManager;
	[SerializeField] private Grid grid;

	[SerializeField] private CubesDatabaseSO database;
	private int selectedCubeIndex = -1;

	//private Dictionary<Vector3Int, GameObject> occupiedPositions;
	//List<Vector3Int> avaliablePositions = new List<Vector3Int>();

	private Vector3Int lastDetectedPosition = Vector3Int.zero;

	private void Start()
	{
		//occupiedPositions = new Dictionary<Vector3Int, GameObject>();
		StopPlacement();
		StopRemovingCubes();
	}

	public void StartPlacement(int ID)
	{
		StopPlacement();
		StopRemovingCubes();

		selectedCubeIndex = database.cubesData.FindIndex(cube => cube.ID == ID);
		if(selectedCubeIndex < 0)
		{
			Debug.LogError($"No ID Found {ID}");
			return;
		}
		//cellIndicator.SetActive(true);
		SetCellIndicator(true, Color.white);
		previewSystem.StartShowingPlacementPreview(database.cubesData[selectedCubeIndex].CubePrefab, database.cubesData[selectedCubeIndex].Size);
		inputManager.OnClicked += PlaceStructure;
		inputManager.OnExit += StopPlacement;
	}

	public void StartRemovingCubes()
	{
		StopPlacement();
		StopRemovingCubes();

		selectedCubeIndex = 0;

		SetCellIndicator(true, Color.red);
		inputManager.OnClicked += RemoveCubeGroup;
		inputManager.OnExit += StopRemovingCubes;
	}

	private void RemoveCubeGroup()
	{
		if (inputManager.IsPointerOverUI())
		{
			return;
		}

		Vector3 mousePosition = inputManager.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		if (!controlSystem.IsLevelCell(gridPosition))
		{
			InvalidPlacementPopUp.Create("THIS IS NOT A VALID CELL!");
			return;
		}

		if (!controlSystem.IsCellOccupied(gridPosition))
		{
			InvalidPlacementPopUp.Create("THIS CELL IS ALREADY EMPTY!");
			return;
		}

		AudioManager.instance.PlaySound("CubeRemove");
		CameraShake.Instance.ShakeCamera(5f, 0.3f);

		foreach (var cubeGroupPos in controlSystem.GetPositionedCubeGroups().ToArray())
		{
			if (!cubeGroupPos.Contains(gridPosition))
			{
				continue;
			}

			foreach (var pos in cubeGroupPos)
			{
				GameObject cubeObject = controlSystem.GetOccupiedPositions()[pos];
				controlSystem.GetOccupiedPositions().Remove(pos);
				//Destroy(cubeObject);

				ObjectPoolManager.ReturnObjectToPool(cubeObject);

				ObjectPoolManager.SpawnObject(GameAssets.ins.removeCubeEffect, pos, GameAssets.ins.removeCubeEffect.transform.rotation,
					ObjectPoolManager.PoolType.ParticleSystem);

				StatsManager.Instance.SetNumberOfRemoveCubes();

				/*GameObject removeEffectGO = Instantiate(GameAssets.ins.removeCubeEffect, pos, GameAssets.ins.removeCubeEffect.transform.rotation);
				Destroy(removeEffectGO, 2f);*/
			}
			controlSystem.GetPositionedCubeGroups().Remove(cubeGroupPos);
		}
	}

	private void StopRemovingCubes()
	{
		SetCellIndicator(false, Color.white);
		inputManager.OnClicked -= RemoveCubeGroup;
		inputManager.OnExit -= StopRemovingCubes;
		lastDetectedPosition = Vector3Int.zero;
	}

	private void SetCellIndicator(bool active,Color color)
	{
		cellIndicator.SetActive(active);
		cellIndicator.GetComponentInChildren<Renderer>().material.color = color;
	}


	private void PlaceStructure()
	{
		if (inputManager.IsPointerOverUI())
		{
			return;
		}

		Vector3 mousePosition = inputManager.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		if (!controlSystem.CheckCellsToPutPosition(gridPosition))
		{
			InvalidPlacementPopUp.Create("THIS IS NOT A VALID CELL!");
			return;
		}

		if (controlSystem.IsCellOccupied(gridPosition))
		{
			InvalidPlacementPopUp.Create("THIS CELL IS NOT EMPTY!");
			return;
		}

		if (!controlSystem.CanCubeScale(gridPosition,grid,database,selectedCubeIndex))
		{
			InvalidPlacementPopUp.Create("I CAN'T SCALE");
			return;
		}

		PlaceTheCube(controlSystem.GetAvaliablePositions());
	}

	/*private bool CheckCellsToPutPosition(Vector3Int position)
	{
		List<Vector3Int> cellsToPutPositions = mapGenerator.GetCellsToPutCubes();

		return cellsToPutPositions.Contains(position);
	}*/

	/*private bool IsCellOccupied(Vector3Int position)
	{
		return occupiedPositions.ContainsKey(position);
	}*/

	/*private bool CanCubeScale(Vector3Int startPosition)
	{
		avaliablePositions.Clear();

		CubeData currentCube = database.cubesData[selectedCubeIndex];
		int i = Math.Sign(currentCube.Size.x) * 1;

		avaliablePositions.Add(startPosition);

		while(Mathf.Abs(i) < Mathf.Abs(currentCube.Size.x))
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
	}*/

	private void PlaceTheCube(List<Vector3Int> cubePositions)
	{
		List<Vector3Int> newList = new List<Vector3Int>();

		foreach (Vector3Int item in cubePositions)
		{
			newList.Add(item);
		}
		controlSystem.SetPositionedCubeGroups(newList);

		AudioManager.instance.PlaySound("CubePlacement");

		foreach (Vector3Int pos in newList)
		{
			//GameObject cubeGameObject = Instantiate(database.cubesData[selectedCubeIndex].CubePrefab);

			GameObject cubeGameObject = ObjectPoolManager.SpawnObject(database.cubesData[selectedCubeIndex].CubePrefab,Vector3.zero,
				Quaternion.identity,ObjectPoolManager.PoolType.GameObjectSystem);

			//occupiedPositions.Add(pos, cubeGameObject);
			controlSystem.SetOccupiedPositions(pos, cubeGameObject);
			cubeGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = database.cubesData[selectedCubeIndex].CubeColor;
			cubeGameObject.transform.position = grid.CellToWorld(pos);

			ObjectPoolManager.SpawnObject(GameAssets.ins.placeCubeEffect,cubeGameObject.transform.position, 
				GameAssets.ins.placeCubeEffect.transform.rotation,ObjectPoolManager.PoolType.ParticleSystem);

			StatsManager.Instance.SetNumberOfPlaceCubes();

			/*GameObject placeCubeEffectGO = Instantiate(GameAssets.ins.placeCubeEffect, cubeGameObject.transform);
			Destroy(placeCubeEffectGO, 2f);*/
		}

		OnLevelStatusCheck?.Invoke();
	}

	private void StopPlacement()
	{
		selectedCubeIndex = -1;
		//cellIndicator.SetActive(false);
		SetCellIndicator(false, Color.white);
		previewSystem.StopShowingPreview();
		inputManager.OnClicked -= PlaceStructure;
		inputManager.OnExit -= StopPlacement;
		lastDetectedPosition = Vector3Int.zero;
	}

	private void Update()
	{
		if(selectedCubeIndex < 0)
		{
			return;
		}

		Vector3 mousePosition = inputManager.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		if(lastDetectedPosition != gridPosition)
		{
			bool checkValidity = controlSystem.CheckCellsToPutPosition(gridPosition);
			checkValidity = !controlSystem.IsCellOccupied(gridPosition);

			//mouseIndicator.transform.position = mousePosition;
			cellIndicator.transform.position = grid.CellToWorld(gridPosition);

			previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), checkValidity);
			lastDetectedPosition = gridPosition;
		}
	}
}
