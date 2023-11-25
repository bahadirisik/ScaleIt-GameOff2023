using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI numberOfCubesPlaced; 
	[SerializeField] private TextMeshProUGUI numberOfCubesRemoved; 
	[SerializeField] private TextMeshProUGUI numberOfHintsTaken; 

	private void OnEnable()
	{
		numberOfCubesPlaced.text = PlayerPrefs.GetInt("NumberOfPlaceCubes").ToString();
		numberOfCubesRemoved.text = PlayerPrefs.GetInt("NumberOfRemoveCubes").ToString();
		numberOfHintsTaken.text = PlayerPrefs.GetInt("NumberOfHints").ToString();
	}
}
