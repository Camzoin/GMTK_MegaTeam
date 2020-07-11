using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class MahjongManager : MonoBehaviour
{
    public enum PieceColor { blue, green, red, yellow }

    public struct MahjongPiece
    {
        public PieceColor[] pieceColor;
        public GameObject piece;

        public MahjongPiece(PieceColor[] pieceColor, GameObject piece)
        {
            this.pieceColor = new PieceColor[2];
            this.pieceColor[0] = pieceColor[0];
            this.pieceColor[1] = pieceColor[1];
            this.piece = piece;
        }
    }

    public List<GameObject> piecePrefabs;

    public const int boardSize = 5;

    public Vector3 pieceDimensions;

    public List<TextAsset> files;

    public List<MahjongPiece>[,] boardState; //TODO fix this

    private int totalPieces;
    private int tallestStack;

    private Queue<MahjongPiece> pieceBuffer;

    public void Start()
    {
        NewGame();
    }

    public void NewGame() //Starts a new game
    {
        int[,] rawGameState = LoadBoardState(files[0].text);
        PopulatePieceBuffer();
        boardState = new List<MahjongPiece>[boardSize, boardSize];
        SetPieces(rawGameState);
        PlacePieces();
    }

    public int[,] LoadBoardState(string text) //Loads a .txt for evaluation
    {
        int[,] pieces = new int[boardSize, boardSize];

        totalPieces = 0;

        if (text.Length != boardSize * boardSize)
        {
            Console.WriteLine("Incorrect number of digits in raw board state file, should be " + boardSize);
            return null;
        }

        for (int i = 0; i < text.Length; i++)
        {
            if (!char.IsDigit(text[i]))
            {
                Console.WriteLine("Incorrect character type in raw board state file, must be digit");
                return null;
            }
            int currStack = (int)char.GetNumericValue(text[i]);
            totalPieces += currStack;
            tallestStack = currStack > tallestStack ? currStack : tallestStack;
        }

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                pieces[i, j] = text[i * boardSize + j];
            }
        }
        return pieces;
    }

    public void PopulatePieceBuffer() //Instantiates objects into pieceBuffer
    {
        var pieces = new List<MahjongPiece>();
        int i, j, k;
        j = k = 0;
        for (i = 0; i < totalPieces; i++)
        {
            j += k % 4 == 4 ? 1 : 0;

            PieceColor[] pieceColors = { (PieceColor)(j % 4), (PieceColor)(k % 4) };

            MahjongPiece tPiece = new MahjongPiece(pieceColors, Instantiate(piecePrefabs[i % 10]));

            pieces.Add(tPiece);

            //TODO Instantiate asynchronsly?
        }

        int n = pieces.Count;
        System.Random rng = new System.Random();

        while (n > 1) //Fisher-Yates Shuffle
        {
            n--;
            int r = rng.Next(n + 1);
            MahjongPiece piece = pieces[r];
            pieces[r] = pieces[n];
            pieces[n] = piece;
        }

        pieceBuffer = new Queue<MahjongPiece>(pieces);

    }

    public bool SetPieces(int[,] pieces) //Call AddPiece into index according to text file
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                for (int n = pieces[i, j]; n > 0; n--)
                {
                    AddPiece(i, j);
                }
            }
        }
        return true;
    }

    public bool AddPiece(int x, int y) //Adds a piece to the board state from the pieceBuffer
    {
        if (x >= boardSize || y >= boardSize) return false;
        boardState[x, y].Add(pieceBuffer.Dequeue());
        return true;
    }

    public void PlacePieces() //Places gameobjects
    {
        for(int x = 0; x < boardSize; x++)
        {
            for(int y = 0; y < boardSize; y++)
            {
                for(int count = 0; count < boardState[x,y].Count; count++)
                {
                    boardState[x, y][count].piece.transform.position = new Vector3(x * pieceDimensions.x, count * pieceDimensions.y, y * pieceDimensions.z);
                }
            }
        }
    }

    public bool ScorePiece(int x, int y) //Removes piece and call AddScore
    {
        if (x >= boardSize || y >= boardSize || boardState[x, y].Count <= 0) return false;
        boardState[x, y].RemoveAt(boardState.Length);
        //TODO Assign points
        return true;
    }
}
