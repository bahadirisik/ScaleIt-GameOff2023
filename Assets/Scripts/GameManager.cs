using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static bool isGameEnded = false;

	public event Action OnLevelComplete;

	[SerializeField] private ControlSystem controlSystem;
    [SerializeField] private MapGenerator mapGenerator;
	[SerializeField] private PlacementSystem placementSystem;


	private void Start()
	{
		isGameEnded = false;
		placementSystem.OnLevelStatusCheck += PlacementSystem_OnLevelStatusCheck;
	}

	private void PlacementSystem_OnLevelStatusCheck()
	{
		CheckLevelStatus();
	}

	private bool IsLevelComplete()
	{
		return controlSystem.GetOccupiedPositions().Count >= mapGenerator.GetLevelCells().Count;
	}

	private void CheckLevelStatus()
	{
		if (!IsLevelComplete())
		{
			Debug.Log("Level Not Completed");
		}
		else
		{
			AudioManager.instance.PlaySound("LevelCompleted");
			Debug.Log("Level Completed");
			isGameEnded = true;
			OnLevelComplete?.Invoke();
		}
	}
}
