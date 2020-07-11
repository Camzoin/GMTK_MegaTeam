using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
	public float hammerMoveSpeed;
	private float yPos;

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
		transform.position = new Vector3(transform.position.x, yPos, (transform.position.z * 1.07f) - 4.75f);
    }
}
