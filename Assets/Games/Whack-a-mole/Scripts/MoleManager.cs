﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleManager : MonoBehaviour
{

	public Transform molePoints;
	public GameObject molePrefab;

	public int numberOfMoles;
	//Up to 28
	private int molesSpawned = 0;

    void Start()
    {
        while(molesSpawned < numberOfMoles)
		{
			int spawnAt = Random.Range(0, 28);

			int count = 0;
			foreach (Transform molePoint in molePoints)
			{
				if (count == spawnAt)
				{
					//Spawn Mole at point
					Instantiate(molePrefab, molePoint.position, Quaternion.identity);
					molesSpawned++;
				}
				count++;
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
