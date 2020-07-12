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
        public PieceColor[] pieceColors;
        public GameObject piece;

        public MahjongPiece(PieceColor[] pieceColor, GameObject piece)
        {
            this.pieceColors = new PieceColor[2];
            this.pieceColors[0] = pieceColor[0];
            this.pieceColors[1] = pieceColor[1];
            this.piece = piece;
        }
    }

    public List<GameObject> piecePrefabs;

    public const int boardSize = 5;

    public Camera cam;

    public Vector3 pieceDimensions, padding;

    public List<TextAsset> files;

    public List<MahjongPiece>[,] boardState;

    private int totalPieces;
    private int tallestStack;

    private Queue<MahjongPiece> pieceBuffer;

    private Vector3 mPos;

    MahjongPiece selectedPiece, lastSelectedPiece;

    private int pc1, pc2;

    public void Start()
    {
        NewGame();
    }

    public void Update()
    {
        mPos = cam.ScreenPointToRay(Input.mousePosition).origin;
        Vector3 v = new Vector3(mPos.x, cam.transform.position.y, mPos.z);

        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(new Ray(v, cam.ScreenPointToRay(Input.mousePosition).direction), out RaycastHit hitInfo))
            {
                MahjongPiece piece = hitInfo.collider.gameObject.GetComponent<MahjongTile>().mahjongPiece;

                foreach(List<MahjongPiece> l in boardState)
                {
                    if (l.Contains(piece))
                    {
                        piece = l[l.Count - 1];
                        break;
                    }
                }



                if (piece != null)
                {
                    SelectPiece(piece);
                    if(lastSelectedPiece != null)
                    {
                        EvaluatePieces(selectedPiece, lastSelectedPiece);
                    }
                }
            }
        }

    }

    public void NewGame() //Starts a new game
    {
        int[,] rawGameState = LoadBoardState(files[UnityEngine.Random.Range(0, files.Count)].text);
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

        int rand = UnityEngine.Random.Range(0, 10);

        for (int i = 0; i < totalPieces; i++)
        {

           PieceColor[] pieceColors = GenerateNewColors(i + rand);

            MahjongPiece tPiece = new MahjongPiece(pieceColors, Instantiate(piecePrefabs[(i + rand) % 10]));
            tPiece.piece.GetComponent<MahjongTile>().mahjongPiece = tPiece;
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
                    boardState[x, y][count].piece.transform.position = new Vector3(x * pieceDimensions.x + padding.x, count * pieceDimensions.y + padding.y, y * pieceDimensions.z + padding.z);
                }
            }
        }
    }

    public bool ScorePiece(MahjongPiece piece) //Removes piece and call AddScore
    {
        foreach (List<MahjongPiece> l in boardState)
        {
            if (l.Contains(piece))
            {
                l.Remove(piece);
                Destroy(piece.piece);
                break;
            }
        }
        //TODO Assign points
        return true;
    }

    public MahjongPiece FindMahjongPiece(GameObject gameObject)
    {
        return gameObject.GetComponent<MahjongTile>().mahjongPiece;
    }

    public void SelectPiece(MahjongPiece piece)
    {
        if(selectedPiece != null) lastSelectedPiece = selectedPiece;
        selectedPiece = piece;
        if(selectedPiece == lastSelectedPiece)
        {
            ClearSelectBuffer();
            return;
        }
        Debug.Log(selectedPiece.pieceColors[0] + " " + selectedPiece.pieceColors[1]);
        var mats = piece.piece.GetComponent<MeshRenderer>().materials;
        mats[0].SetColor("_FresnelColor", new Color(0, 1, 0, 1));
        mats[1].SetColor("_FresnelColor", new Color(0, 1, 0, 1));
    }

    public bool EvaluatePieces(MahjongPiece p1, MahjongPiece p2)
    {
        if(p1.pieceColors[0] == p2.pieceColors[0] ||
           p1.pieceColors[0] == p2.pieceColors[1] ||
           p1.pieceColors[1] == p2.pieceColors[0] ||
           p1.pieceColors[1] == p2.pieceColors[1])
        {
            ScorePiece(p1);
            ScorePiece(p2);
            ClearSelectBuffer();
            return true;
        }
        ClearSelectBuffer();
        return false;
    }

    public void ClearSelectBuffer()
    {
        if(selectedPiece != null)
        {
            var mats = selectedPiece.piece.GetComponent<MeshRenderer>().materials;
            mats[0].SetColor("_FresnelColor", new Color(0, 1, 0, 0));
            mats[1].SetColor("_FresnelColor", new Color(0, 1, 0, 0));
        }
        if (lastSelectedPiece != null)
        {
            var mats = lastSelectedPiece.piece.GetComponent<MeshRenderer>().materials;
            mats[0].SetColor("_FresnelColor", new Color(0, 1, 0, 0));
            mats[1].SetColor("_FresnelColor", new Color(0, 1, 0, 0));
        }
        lastSelectedPiece = selectedPiece = null;
    }

    public PieceColor[] GenerateNewColors(int i)
    {
        PieceColor[] colors = new PieceColor[2];
        switch (i % 10)
        {
            case 0:
                colors[0] = PieceColor.blue;
                colors[1] = PieceColor.blue;
                break;
            case 1:
                colors[0] = PieceColor.green;
                colors[1] = PieceColor.blue;
                break;
            case 2:
                colors[0] = PieceColor.green;
                colors[1] = PieceColor.green;
                break;
            case 3:
                colors[0] = PieceColor.green;
                colors[1] = PieceColor.red;
                break;
            case 4:
                colors[0] = PieceColor.green;
                colors[1] = PieceColor.yellow;
                break;
            case 5:
                colors[0] = PieceColor.red;
                colors[1] = PieceColor.blue;
                break;
            case 6:
                colors[0] = PieceColor.red;
                colors[1] = PieceColor.red;
                break;
            case 7:
                colors[0] = PieceColor.red;
                colors[1] = PieceColor.yellow;
                break;
            case 8:
                colors[0] = PieceColor.yellow;
                colors[1] = PieceColor.blue;
                break;
            case 9:
                colors[0] = PieceColor.yellow;
                colors[1] = PieceColor.yellow;
                break;
        }
        
        return colors;
    }
}
