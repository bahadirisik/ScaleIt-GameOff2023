using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletePanel;
	[SerializeField] private GameManager gameManager;

	private void Start()
	{
		gameManager.OnLevelComplete += GameManager_OnLevelComplete;
	}

	private void GameManager_OnLevelComplete()
	{
		levelCompletePanel.SetActive(true);
	}
}
