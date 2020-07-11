using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class TPdragger : MonoBehaviour

{
    public Renderer tp;

    private Vector3 mOffset;

    public float unrollAmount = 0;

    private float mZCoord;

    Vector2 oldMousePos, mousePos;

    public float unrollVisMulti = 0.1f;

    private void Update()
    {
        tp.material.mainTextureOffset = new Vector2(0, unrollAmount * unrollVisMulti);
            

        oldMousePos = mousePos;

        mousePos = Input.mousePosition;

        
    }


    void OnMouseDrag()
    {
        if (oldMousePos.y > mousePos.y)
        {
            unrollAmount = unrollAmount + (oldMousePos.y - mousePos.y);
        }



    }

}