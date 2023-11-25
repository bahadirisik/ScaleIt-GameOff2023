using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonSound : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
	[SerializeField] private Toggle[] toggles;

	private void Start()
	{
		foreach (Button button in buttons)
		{
			button.onClick.AddListener(ButtonSoundPlay);
		}
		foreach (Toggle toggle in toggles)
		{
			toggle.onValueChanged.AddListener(delegate {
				ButtonSoundPlay();
			});
		}
	}

	private void ButtonSoundPlay()
	{
		AudioManager.instance.PlaySound("Button");
	}
}
