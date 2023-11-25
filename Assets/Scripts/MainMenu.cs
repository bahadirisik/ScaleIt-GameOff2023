using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public AudioMixer audioMixer;

	public void LoadLevelByIndex(int index)
	{
		Loader.Load(index);
	}

	public void LoadNextLevel()
	{
		Loader.Load(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("volume", volume / 2);
	}

	public void QuitButton()
	{
		Application.Quit();
	}
}
