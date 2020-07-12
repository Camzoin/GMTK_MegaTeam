using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class TPdragger : MonoBehaviour

{
    public Renderer tp, tpRoll, tpCenter;

    private Vector3 mOffset;

    public float unrollAmount = 0;

    private float mZCoord;

    Vector2 oldMousePos, mousePos;

    public float unrollVisMulti = 0.1f;

    float newPoints;

    float oldUnrollAmount;

    float floorTPHeight;

    public GameObject floorTP;

    bool isDragging = false;

    float time, mercyTimer = 0.1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        time = time + Time.deltaTime;

        tp.material.mainTextureOffset = new Vector2(0, unrollAmount * unrollVisMulti);
        tpRoll.material.mainTextureOffset = new Vector2(0, unrollAmount * unrollVisMulti);
        tpCenter.materials[2].mainTextureOffset = new Vector2(unrollAmount * unrollVisMulti * 0.25f, 0);

        if (floorTPHeight < 0.6f)
        {
            floorTPHeight = unrollAmount * 0.00002f;
        }


        floorTP.transform.position = new Vector3(0, floorTPHeight, 0);

        newPoints = unrollAmount - oldUnrollAmount;

        oldUnrollAmount = unrollAmount;

        oldMousePos = mousePos;

        mousePos = Input.mousePosition;

        //score = unrollAmount * unrollVisMulti

        //adding points. I think this will work

        GameManager.instance.ScorePoints(GameManager.games.TOILET, newPoints * unrollVisMulti * 0.07f);
    }


    private void OnMouseDown()
    {
        isDragging = true;
        time = 0;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    void OnMouseDrag()
    {
        if (oldMousePos.y > mousePos.y && isDragging == true)
        {
            unrollAmount = unrollAmount + (oldMousePos.y - mousePos.y);
        }

        if (oldMousePos.y < mousePos.y && time > mercyTimer)
        {
            //STOP
            isDragging = false;
        }
    }

}