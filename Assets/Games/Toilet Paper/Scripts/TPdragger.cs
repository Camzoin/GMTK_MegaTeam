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

    private void Update()
    {
        tp.material.mainTextureOffset = new Vector2(0, unrollAmount * unrollVisMulti);
        tpRoll.material.mainTextureOffset = new Vector2(0, unrollAmount * unrollVisMulti);
        tpCenter.materials[2].mainTextureOffset = new Vector2(unrollAmount * unrollVisMulti * 0.25f, 0);


        float oldUnrollAmount = unrollAmount;

        float newPoints = unrollAmount - oldUnrollAmount;

        oldMousePos = mousePos;

        mousePos = Input.mousePosition;

        //score = unrollAmount * unrollVisMulti

        //adding points. I think this will work

        //GameManager.instance.ScorePoints(GameManager.games.TOILET, newPoints);
    }


    void OnMouseDrag()
    {
        if (oldMousePos.y > mousePos.y)
        {
            unrollAmount = unrollAmount + (oldMousePos.y - mousePos.y);
        }
    }

}