using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum CardType { blueCircle, blueDiamond, blueHeart, blueSquare, blueStar, blueTriangle, greenCircle, greenDiamond, greenHeart, greenSquare, greenStar, greenTriangle, redCircle, redDiamond, redHeart, redSquare, redStar, redTriangle }

    public CardType cardType;

    public Sprite[] sprites = new Sprite[2];
}
