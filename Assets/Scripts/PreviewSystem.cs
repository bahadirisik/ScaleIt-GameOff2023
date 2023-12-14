using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;

    private GameObject previewobject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

	private void Start()
	{
		previewMaterialInstance = new Material(previewMaterialPrefab);
	}

	public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
	{
		previewobject = Instantiate(prefab);
		Preparereview(previewobject);
	}

	private void Preparereview(GameObject previewobject)
	{
		Renderer[] renderers = previewobject.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers)
		{
			Material[] materials = renderer.materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i] = previewMaterialInstance;
			}
			renderer.materials = materials;
		}
	}

	public void StopShowingPreview()
	{
		Destroy(previewobject);
	}

	public void UpdatePosition(Vector3 position,bool validity)
	{
		MovePreview(position);
		ApplyFeedback(validity);
	}

	private void ApplyFeedback(bool validity)
	{
		Color color = validity ? Color.green : Color.red;
		color.a = 0.75f;
		previewMaterialInstance.color = color;
	}

	private void MovePreview(Vector3 position)
	{
		if (previewobject == null)
			return;

		previewobject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
	}
}
