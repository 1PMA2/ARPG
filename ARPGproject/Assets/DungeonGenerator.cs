using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : Singleton<DungeonGenerator>
{

    enum TILE { ROAD = 0, WALL, CHECK, START };

    private GameObject roomPrefeb;
    private GameObject cornerPrefeb;
    private GameObject walkWayPrefeb;

    private ITile tile;
    private ITile cornerTile;
    public bool up, down, left, right;

    int tileSize = 20;

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    private int[,] map;

    private Vector2Int[] direction = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private Vector2Int pos = Vector2Int.zero;
    private Stack<Vector2Int> stackDir = new Stack<Vector2Int>();
    private Stack<ITile> stackTile = new Stack<ITile>();

    [SerializeField] private Vector2Int start;
    [SerializeField] private Vector2Int end;

    private Vector3 startPos;
    public Vector3 StartPos
    {
        get
        {
            return startPos;
        }
    }


    private new void Awake()
    {
        roomPrefeb = Resources.Load("Prefeb/Terrain/Room") as GameObject;
        cornerPrefeb = Resources.Load("Prefeb/Terrain/Corner") as GameObject;
        walkWayPrefeb = Resources.Load("Prefeb/Terrain/WalkWay") as GameObject;

        InitGrid();
        StartCoroutine(GenerateRoad());
    }
    private void Start()
    {
        
    }

    private void Update()
    {
    }

    void InitGrid()
    {
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (int)TILE.WALL;
            }
        }

        map[pos.x, pos.y] = (int)TILE.START; //RandPosSelect �Լ����� �������� ������ ������ ���� �������� ����
        CreateRoom(pos.x, pos.y);
        stackTile.Push(tile);
    }

    private void RandPosSelect()
    {

        pos = new Vector2Int(Random.Range(0, width), Random.Range(0, height)); //�̷� ���� ������ ������ ����

    }

    private void RandDirection() //�������� ������ ���� �޼ҵ�
    {
        for (int i = 0; i < direction.Length; i++)
        {
            int randNum = Random.Range(0, direction.Length); //4���� �� �������� ���� ����
            Vector2Int temp = direction[randNum]; //���� �ε����� �ش��ϴ� ����� �������� ������ ������ ���� �ٲ�
            direction[randNum] = direction[i];
            direction[i] = temp;
        }
    }

    private IEnumerator GenerateRoad()
    {
       
        do
        {
            int index = -1; //-1�� �� �� �ִ� ���� ������ �ǹ�

            RandDirection(); //���� �������� ����

            for (int i = 0; i < direction.Length; i++)
            {
                if (CheckForRoad(i))
                {
                    index = i; //������ ���⿡ ���� ���� ��� ���� �迭�� �ε��� ����
                    break;
                }
            }

            if (index != -1) //�� �� �ִ� ���� ���� ���
            {
                stackDir.Push(direction[index]); //���ÿ� ���� ����
                stackTile.Push(tile);
                pos += direction[index]; //��ġ ���� ����
                tile.OpenWall(direction[index]);
                map[pos.x, pos.y] = (int)TILE.CHECK; //Ÿ�� ����
                CreateRoom(pos.x, pos.y); //Ÿ�� ���� ����
                tile.OpenWall(direction[index] * -1);
            }
            else //�� �� �ִ� ���� ���� ���
            {
                Vector2Int reverseDir = stackDir.Pop() * -1;
                map[pos.x, pos.y] = (int)TILE.ROAD; //�ϼ��� �� �ǹ�
                pos += reverseDir; //������ �����ϴ� ���ÿ��� �����͸� ������ -1�� ���� ���� ����
                tile = stackTile.Pop();
                if (map[pos.x, pos.y] == (int)TILE.ROAD)
                {
                    tile.OpenWall(reverseDir);
                }
            }

            yield return null;
        }
        while (pos != end);

    }

    private bool CheckForRoad(int index)
    {
        if ((pos + direction[index]).x >= width) return false; 
        if ((pos + direction[index]).x < 0) return false;
        if ((pos + direction[index]).y >= height) return false;
        if ((pos + direction[index]).y < 0) return false;
        if (map[(pos + direction[index]).x, (pos + direction[index]).y] != (int)TILE.WALL) return false;
        return true;
    }



    void CreateRoom(int x, int z)
    {
        Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize); 
        GameObject room = Instantiate(roomPrefeb, pos, Quaternion.identity);
        tile = room.GetComponent<Room>();
    }
}
