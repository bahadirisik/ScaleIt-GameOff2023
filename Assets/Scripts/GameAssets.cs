using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;

    public static GameAssets ins
	{
		get
		{
			if(instance == null)
			{
				instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
			}
			return instance;
		}
	}

	public Transform invalidPopup;
	public GameObject placeCubeEffect;
	public GameObject removeCubeEffect;
}
