using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchManager : MonoBehaviour
{
    public Vector2 cardDims, padding;

    public GameObject cardPrefab;

    public Card[,] cards = new Card[9,4];

    public void NewGame()
    {

    }

    public void GenerateDeck()
    {
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 3; j++)
            {

            }
        }
    }
}
