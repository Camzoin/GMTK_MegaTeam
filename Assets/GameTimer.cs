using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text gameTime, gameCount;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //set game count
        gameCount.SetText(GameManager.instance.GetRemainingGames().ToString());
        GameManager.instance.GetRemainingTime();

        int myBlubb = (int)GameManager.instance.GetRemainingTime();

        //set game time
        gameTime.SetText(myBlubb.ToString());
    }
}
