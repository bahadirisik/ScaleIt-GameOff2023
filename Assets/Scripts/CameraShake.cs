using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
	public static CameraShake Instance { get; private set; }

	private CinemachineVirtualCamera cinemachineVirtualCamera;

	private float shakeTimer;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
	}

	public void ShakeCamera(float intensity, float timer)
	{
		CinemachineBasicMultiChannelPerlin cinemachineBasicMulti =
			cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

		cinemachineBasicMulti.m_AmplitudeGain = intensity;

		shakeTimer = timer;
	}

	private void Update()
	{
		if (shakeTimer > 0f)
		{
			shakeTimer -= Time.deltaTime;
			if (shakeTimer <= 0f)
			{
				CinemachineBasicMultiChannelPerlin cinemachineBasicMulti =
					cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

				cinemachineBasicMulti.m_AmplitudeGain = 0f;
			}

		}
	}
}
