using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
	public float hammerMoveSpeed;
	private float yPos;

	public AudioSource hammerSwish;

	private bool wasDownLastFrame = false;

	// Start is called before the first frame update
	void Start()
    {
		yPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Vector3 mousePoint = Input.mousePosition;

		gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, mousePoint, hammerMoveSpeed);
		transform.position = new Vector3(transform.position.x, yPos, (transform.position.z * 1.072f) - 4.75f);

		if (Input.GetMouseButton(0))
		{
			if (!wasDownLastFrame)
			{
				hammerSwish.Play();
			}
			wasDownLastFrame = true;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(-105f, transform.rotation.y, transform.rotation.z)), 0.5f);
		}
		else
		{
			wasDownLastFrame = false;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, transform.rotation.y, transform.rotation.z)), 0.2f);
		}
    }

	public void HitAMole()
	{
		GameManager.instance.ScorePoints(GameManager.games.WHACKAMOLE, 1f);
	}

	public void HitABomb()
	{
		GameManager.instance.ScorePoints(GameManager.games.WHACKAMOLE, -1f);
	}
}