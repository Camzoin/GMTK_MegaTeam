using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
	public float hammerMoveSpeed;
	private float yPos;

	public AudioSource hammerSwish;

	private bool wasDownLastFrame = false;

	private float maxHammerDownTime = 0.25f;
	private float hammerDownAt;

	// Start is called before the first frame update
	void Start()
    {
		yPos = transform.position.y;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;

	}

	// Update is called once per frame
	void Update()
    {
		Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Camera.main.transform.LookAt(transform.position);

		if (mousePoint.x > 20f)
		{
			mousePoint.x = 20f;
		}
		else if (mousePoint.x < -20f)
		{
			mousePoint.x = -20f;
		}

		gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, mousePoint, hammerMoveSpeed);

		transform.position = new Vector3(transform.position.x, yPos, (transform.position.z * 1.1f) - 4.75f);

		if (transform.position.z > 18f)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 18f);
		}
		else if (transform.position.z < -16f)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, -16f);
		}

		if (Input.GetMouseButton(0))
		{
			if (!wasDownLastFrame)
			{
				hammerSwish.Play();
				hammerDownAt = Time.time;
			}

			if (Time.time < hammerDownAt + maxHammerDownTime)
			{
				wasDownLastFrame = true;
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(-105f, transform.rotation.y, transform.rotation.z)), 0.5f);
			}
			else
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, transform.rotation.y, transform.rotation.z)), 0.2f);
			}
			
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