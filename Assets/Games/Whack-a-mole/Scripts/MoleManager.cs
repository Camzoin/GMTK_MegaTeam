using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleManager : MonoBehaviour
{

	public Transform molePoints;
	public GameObject molePrefab;

	public int numberOfMoles;
	//Up to 28, this is the total number of mole points
	private int molesSpawned = 0;

	private List<int> usedHoles = new List<int>();

    void Start()
    {
        while(molesSpawned < numberOfMoles)
		{
			int spawnAt = Random.Range(0, 28);

			while (usedHoles.Contains(spawnAt))
			{
				spawnAt = Random.Range(0, 28);
			}

			int count = 0;
			foreach (Transform molePoint in molePoints)
			{
				//Hide Sphere
				molePoint.gameObject.SetActive(false);

				if (count == spawnAt)
				{
					usedHoles.Add(spawnAt);

					//Spawn Mole at point
					Instantiate(molePrefab, molePoint.position, Quaternion.identity);
					molesSpawned++;
				}
				count++;
			}
		}
    }
}
