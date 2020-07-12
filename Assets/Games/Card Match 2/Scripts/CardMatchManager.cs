using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardMatchManager : MonoBehaviour
{
    public Vector2 cardDims, padding, originCardPos;

    public Camera cam;

    public GameObject cardPrefab;

    public Sprite[] spritePrefabs;

    public Card[,] cards = new Card[9, 4];

    private List<Card> deckBuffer = new List<Card>();

    private Vector3 mPos;

    private Card selectedCard, lastSelectedCard;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        NewGame();
    }
    public void Update()
    {
        mPos = cam.ScreenPointToRay(Input.mousePosition).origin;
        Vector3 v = new Vector3(mPos.x, mPos.y, mPos.z);

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(new Ray(v, cam.ScreenPointToRay(Input.mousePosition).direction * 10), out RaycastHit hitInfo))
            {
                Card card = hitInfo.collider.GetComponentInParent<Card>();
                if (card)
                {
                    SelectCard(card);
                    StartCoroutine(FlipCard(card, 0f, 0.15f));
                    if (lastSelectedCard != null) 
                    {
                        if (EvaluateCards(selectedCard, lastSelectedCard))
                        {
                            ScorePoints(1f);
                            PopCard(selectedCard);
                            PopCard(lastSelectedCard);
                            DeselectCards();
                        }
                        else
                        {
                            StartCoroutine(FlipCard(selectedCard, 1f, 0.15f));
                            StartCoroutine(FlipCard(lastSelectedCard, 1f, 0.15f));
                            DeselectCards();
                        }
                    }
                }
            }
        }
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

    public IEnumerator FlipCard(Card card, float warmup, float flipDuration)
    {
        for (float i = 0; i < warmup; i += Time.deltaTime) yield return null;

        GameObject g = card.gameObject;
        card.spriteIndex += card.spriteIndex == 0 ? 1 : -1;
        g.GetComponent<BoxCollider>().enabled = false;
        g.GetComponent<SpriteRenderer>().sprite = card.sprites[card.spriteIndex];

        for(float i = 0; i < flipDuration; i += Time.deltaTime)
        {
            g.transform.localScale = new Vector3(i / flipDuration, 1, 1);
            yield return null;
        }
        g.GetComponent<BoxCollider>().enabled = true;
    }

    public void SelectCard(Card card)
    {
        lastSelectedCard = selectedCard != null ? selectedCard : null;
        selectedCard = card;
    }

    public bool EvaluateCards(Card c0, Card c1)
    {
        if (c0 == c1) return false;
        return c0.cardType == c1.cardType ? true : false;
    }

    public void ScorePoints(float points)
    {
        GameManager.instance.ScorePoints(GameManager.games.CARDFLIP, points);
    }

    public void PopCard(Card card)
    {
        card.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(ShrinkCard(card, 0.25f));
    }

    public IEnumerator ShrinkCard(Card card, float duration)
    {
        for(float i = duration; i > 0; i -= Time.deltaTime)
        {
            card.transform.localScale = Vector3.one * (i / duration);
            yield return null;
        }
        Destroy(card.gameObject);
    }

    public void DeselectCards()
    {
        lastSelectedCard = selectedCard = null;
    }
}
