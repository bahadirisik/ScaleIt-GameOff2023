using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvalidPlacementPopUp : MonoBehaviour
{
	private TextMeshPro textMesh;
	private Color textColor;

	private float disappearTimer;

	public static InvalidPlacementPopUp Create(string warning)
	{
		Transform invalidPlacementTextTransform = Instantiate(GameAssets.ins.invalidPopup);
		InvalidPlacementPopUp invalidText = invalidPlacementTextTransform.GetComponent<InvalidPlacementPopUp>();
		invalidText.Setup(warning);

		return invalidText;
	}

	private void Awake()
	{
		textMesh = transform.GetComponent<TextMeshPro>();
	}

	public void Setup(string warning)
	{
		textMesh.SetText(warning);
		textColor = textMesh.color;
		disappearTimer = 1f;
	}

	private void Update()
	{
		float moveYSpeed = 2.5f;
		transform.position += new Vector3(0f,moveYSpeed,0f) * Time.deltaTime;

		disappearTimer -= Time.deltaTime;
		if(disappearTimer < 0)
		{
			float disappearSpeed = 3f;
			textColor.a -= disappearSpeed * Time.deltaTime;
			textMesh.color = textColor;
			if(textColor.a < 0)
			{
				Destroy(gameObject);
			}
		}
	}
}
