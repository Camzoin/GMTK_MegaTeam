using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchManager : MonoBehaviour
{
    public Vector2 cardDims, padding, originCardPos;

    public GameObject cardPrefab;

    public Sprite[] spritePrefabs;

    public Card[,] cards = new Card[9, 4];

    private List<Card> deckBuffer = new List<Card>();

    public void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        GenerateDeck();
        ShuffleDeck();
        PopulateCards();
        PlaceCards();
    }

    public void GenerateDeck()
    {
        float pFive = 0f;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var cardObject = Instantiate(cardPrefab);
                var card = cardObject.GetComponent<Card>();
                card.cardType = (Card.CardType)(int)pFive;
                card.sprites[1] = spritePrefabs[(int)pFive];
                deckBuffer.Add(card);
                pFive += 0.5f;
            }
        }
    }

    public void ShuffleDeck()
    {
        int n = deckBuffer.Count;
        System.Random rng = new System.Random();

        while (n > 1) //Fisher-Yates Shuffle
        {
            n--;
            int r = rng.Next(n + 1);
            Card card = deckBuffer[r];
            deckBuffer[r] = deckBuffer[n];
            deckBuffer[n] = card;
        }
    }

    public void PopulateCards()
    {
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                cards[i, j] = deckBuffer[(i * 4) + j];
            }
        }
    }

    public void PlaceCards()
    {
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                cards[i, j].transform.position = new Vector2(originCardPos.x + ((cardDims.x + padding.x) * i), originCardPos.y + ((cardDims.y + padding.y) * j));
            }
        }
    }
}
