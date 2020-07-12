using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum CardType { blueCircle, greenCircle, pinkCircle,
                           blueDiamond, greenDiamond, pinkDiamond,
                           blueHeart, greenHeart, pinkHeart,
                           blueSquare, greenSquare, pinkSquare,
                           blueStar, greenStar, pinkStar,
                           blueTriangle, greenTriangle, pinkTriangle
                         }

    public CardType cardType;

    public Sprite[] sprites = new Sprite[2];

    public int spriteIndex = 0;

    public Card(CardType cardType, Sprite sprite)
    {
        this.cardType = cardType;
        sprites[1] = sprite;
    }
}
