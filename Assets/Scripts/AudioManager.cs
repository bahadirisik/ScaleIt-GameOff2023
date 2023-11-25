using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
	private Sound currentTheme;
	private int currentThemeIndex;
	string[] themes = { "Theme1","Theme2","Theme3","Theme4" };

	public static AudioManager instance;

	private bool isFocus = false;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.outputAudioMixerGroup = s.group;
		}
	}

	private void Start()
	{
		currentThemeIndex = 0;
		PlayTheme(themes[currentThemeIndex]);

		//PlaySound("Theme1");
	}

	public void PlaySound(string name)
	{
		Sound s = Array.Find(sounds,s => s.name == name);
		if(s == null)
		{
			return;
		}
		s.source.Play();
	}

	public void PlayTheme(string name)
	{
		currentTheme = Array.Find(sounds, s => s.name == name);
		if (currentTheme == null)
		{
			return;
		}
		currentTheme.source.PlayDelayed(3f);
	}

	private void FixedUpdate()
	{
		if (!currentTheme.source.isPlaying && isFocus)
		{
			currentTheme.source.Stop();
			currentThemeIndex += 1;
			if(currentThemeIndex >= themes.Length)
			{
				currentThemeIndex = 0;
			}

			PlayTheme(themes[currentThemeIndex]);
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		isFocus = focus;
	}

}
