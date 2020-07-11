using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
	public float minPopTime;
	public float maxPopTime;
	public float minTimeBetweenPops;
	public float maxTimeBetweenPops;

	public float bombPercentage;

	private float timeOfNextPop;
	private float timeOfUnPop;

	private bool isPopped = false;

    // Start is called before the first frame update
    void Start()
    {
		timeOfNextPop = Time.time + Random.Range(minTimeBetweenPops, maxTimeBetweenPops);
    }

    // Update is called once per frame
    void Update()
    {
		if (isPopped)
		{
			if (Time.time >= timeOfUnPop)
			{
				isPopped = false;
				//hide the mole/bomb
				timeOfNextPop = Time.time + Random.Range(minTimeBetweenPops, maxTimeBetweenPops);
			}
		}
		else
		{
			if (Time.time >= timeOfNextPop)
			{
				Pop();
			}
		}
    }

	private void Pop()
	{
		isPopped = true;
		timeOfUnPop = Time.time + Random.Range(minPopTime, maxPopTime);

		if (Random.Range(0f, 1f) > bombPercentage)
		{
			//Use the bomb
			gameObject.tag = "Bomb";
		}
		else
		{
			//use the mole
			gameObject.tag = "Mole";
		}

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Hammer" && isPopped)
		{
			//Play hit animation
			//hide mole
			isPopped = false;
			timeOfNextPop = Time.time + Random.Range(minTimeBetweenPops, maxTimeBetweenPops);
		}
	}
}
