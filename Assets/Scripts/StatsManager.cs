using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

	private const string NUMBEROFPLACECUBES = "NumberOfPlaceCubes";
	private const string NUMBEROFREMOVECUBES = "NumberOfRemoveCubes";
	private const string NUMBEROFHINTS = "NumberOfHints";

	private void Awake()
	{
		Instance = this;
	}

	public void SetNumberOfPlaceCubes()
	{
		int amount = PlayerPrefs.GetInt(NUMBEROFPLACECUBES);
		PlayerPrefs.SetInt(NUMBEROFPLACECUBES,amount + 1);
	}

	public void SetNumberOfRemoveCubes()
	{
		int amount = PlayerPrefs.GetInt(NUMBEROFREMOVECUBES);
		PlayerPrefs.SetInt(NUMBEROFREMOVECUBES, amount + 1);
	}

	public void SetNumberOfHints(bool isHintActive)
	{
		if (isHintActive)
		{
			int amount = PlayerPrefs.GetInt(NUMBEROFHINTS);
			PlayerPrefs.SetInt(NUMBEROFHINTS, amount + 1);
		}
	}
}
