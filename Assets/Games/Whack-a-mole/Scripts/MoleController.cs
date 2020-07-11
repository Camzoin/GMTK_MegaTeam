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

	public GameObject mole;
	public GameObject bomb;

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
				UnPop();
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

		gameObject.GetComponent<CapsuleCollider>().enabled = true;
		if (Random.Range(0f, 1f) > bombPercentage)
		{
			//Use the bomb
			gameObject.tag = "Bomb";
			bomb.SetActive(true);
		}
		else
		{
			//use the mole
			gameObject.tag = "Mole";
			mole.SetActive(true);
		}

	}

	private void UnPop()
	{
		//Play hit animation
		//hide mole
		isPopped = false;
		timeOfNextPop = Time.time + Random.Range(minTimeBetweenPops, maxTimeBetweenPops);
		gameObject.GetComponent<CapsuleCollider>().enabled = false;

		gameObject.tag = "Hole";
		mole.SetActive(false);
		bomb.SetActive(false);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Hammer" && isPopped)
		{
			
		}
	}
}
