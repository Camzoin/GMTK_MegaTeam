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

	private string state = "Hole";

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

		if (Random.Range(0f, 1f) > bombPercentage)
		{
			//Use the bomb
			state = "Bomb";
			bomb.SetActive(true);
		}
		else
		{
			//use the mole
			state = "Mole";
			mole.SetActive(true);
		}
	}

	private void UnPop()
	{
		//Play hit animation
		//hide mole
		isPopped = false;
		timeOfNextPop = Time.time + Random.Range(minTimeBetweenPops, maxTimeBetweenPops);

		state = "Hole";
		mole.SetActive(false);
		bomb.SetActive(false);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Hammer" && isPopped)
		{
			if (state == "Bomb")
			{
				//explosion
				UnPop();
				collision.gameObject.GetComponent<HammerController>().HitABomb();
			}
			else if(state == "Mole")
			{
				//Mole oof
				UnPop();
				collision.gameObject.GetComponent<HammerController>().HitAMole();
			}
		}
	}
}
