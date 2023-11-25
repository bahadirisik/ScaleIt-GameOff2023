using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
	public enum Scene
	{
		MainMenuScene,
		Level1,
		Level2,
		Level3,
		Level4,
		Level5,
		Level6,
		LoadingScene,
	}

	private static int targetScene;


	public static void Load(int targetScene)
	{
		Loader.targetScene = targetScene;

		SceneManager.LoadScene("LoadingScene");
	}

	public static void LoaderCallback()
	{
		SceneManager.LoadScene(targetScene);
	}
}
