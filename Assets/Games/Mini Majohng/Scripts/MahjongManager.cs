using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class MahjongManager : MonoBehaviour
{
    public enum PieceColor { blue, green, red, yellow }

    public class MahjongPiece
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

    public Camera cam;

    public Vector3 pieceDimensions;

    public List<TextAsset> files;

    public List<MahjongPiece>[,] boardState;

    private int totalPieces;
    private int tallestStack;

    private Queue<MahjongPiece> pieceBuffer;

    private Vector3 mPos, lastMPos;

    public void Start()
    {
        NewGame();
    }

    public void Update()
    {
        mPos = cam.ScreenPointToRay(Input.mousePosition).origin;
        Vector3 v = new Vector3();

        if (lastMPos != mPos)
        {
            lastMPos = mPos;
            v = new Vector3(mPos.x, cam.transform.position.y, mPos.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            MahjongPiece piece = FindMahjongPieceOnTop(v, cam.ScreenPointToRay(Input.mousePosition).direction, 100);

            if (piece != null)
            {
                var mats = piece.piece.GetComponent<MeshRenderer>().materials;
                mats[0].SetColor("_FresnelColor", new Color(0, 1, 0, 1));
                mats[1].SetColor("_FresnelColor", new Color(0, 1, 0, 1));
            }
        }

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
                pieces[i, j] = (int)char.GetNumericValue(text[i * boardSize + j]);
            }
        }
        return pieces;
    }

    public void PopulatePieceBuffer() //Instantiates objects into pieceBuffer
    {
        var pieces = new List<MahjongPiece>();
        int i, j, k;
        i = j = k = 0;
        for (; i < totalPieces; i++)
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

    public bool SetPieces(int[,] pieces) //Call AddStack into index according to text file
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                AddStack(i, j, pieces[i, j]);
            }
        }
        return true;
    }

    public bool AddStack(int x, int y, int n) //Adds a stack of pieces to the board state from piecebuffer
    {
        if (x >= boardSize || y >= boardSize) return false;
        List<MahjongPiece> tList = new List<MahjongPiece>();
        for(int i = 0; i < n; i++)
        {
            tList.Add(pieceBuffer.Dequeue());
        }
        boardState[x, y] = tList;
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

    public MahjongPiece FindMahjongPieceOnTop(Vector3 pos, Vector3 dir, int maxIterations)
    {
        Ray ray = new Ray(pos, dir);
        List<MahjongPiece> stack = FindNearestStack(new Vector2(pos.x, pos.z));
        if (stack.Count > 0 && ray.origin.y < stack[stack.Count - 1].piece.transform.position.y)
        {
            Debug.Log("mpos: " + pos + " stack pos: " + stack[stack.Count - 1].piece.transform.position);

            return stack[stack.Count - 1];
        }

        for (int i = 0; i < maxIterations; i++)
        {
            ray.origin += dir * 1 / 3;

            stack = FindNearestStack(new Vector2(ray.origin.x, ray.origin.z));
            if (stack.Count > 0 && ray.origin.y < stack[stack.Count - 1].piece.transform.position.y)
            {
                Debug.Log("mpos: " + pos + " stack pos: " + stack[stack.Count - 1].piece.transform.position);
                return stack[stack.Count - 1];
            }
        }

        Console.WriteLine("No piece found");
        return null;
    }

    public List<MahjongPiece> FindNearestStack(Vector2 pos) //Finds the bottom bottom piece of the nearest stack
    {
        List<MahjongPiece> returnStack = boardState[0,0];

        foreach (List<MahjongPiece> l in boardState)
        {
            if(l.Count <= 0) continue;

            Vector2 currStackPos = new Vector2(l[0].piece.transform.position.x, l[0].piece.transform.position.z);

            Vector2 currStackAdjPos = new Vector2(l[0].piece.transform.position.x + pieceDimensions.x / 2, l[0].piece.transform.position.z + pieceDimensions.z / 2);
            Vector2 returnStackAdjPos = new Vector2(returnStack[0].piece.transform.position.x + pieceDimensions.x / 2, returnStack[0].piece.transform.position.z + pieceDimensions.z / 2);
            if (Vector2.Distance(pos, returnStackAdjPos) > Vector2.Distance(pos, currStackAdjPos))
            {
                returnStack = l;
            }
        }
        return returnStack;
    }
}
