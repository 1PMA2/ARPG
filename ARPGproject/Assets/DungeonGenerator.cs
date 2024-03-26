using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DungeonGenerator : Singleton<DungeonGenerator>
{

    enum TILE { ROAD = 0, WALL, CHECK, START, BOSS };

    private GameObject roomPrefeb;
    private GameObject roadPrefeb;
    private GameObject bossPrefeb;

    private ITile tile;
    private ITile cornerTile;

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
        roadPrefeb = Resources.Load("Prefeb/Terrain/Road") as GameObject;
        bossPrefeb = Resources.Load("Prefeb/Terrain/Boss") as GameObject;

        //RandPosSelect();
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

                if(x >= (end.x - 1) && x <= (end.x + 1) && y > end.y)
                {
                    map[x, y] = (int)TILE.BOSS;
                }
            }
        }

        pos = start;
        startPos = new Vector3(pos.x * tileSize, 1, pos.y * tileSize);
        map[pos.x, pos.y] = (int)TILE.START;
        CreateRoom(pos.x, pos.y);
        stackTile.Push(tile);
    }

    private void RandPosSelect()
    {
        width = Random.Range(5, 20);
        height = Random.Range(5, 20);

        start = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        do{
            end = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (end == start);

    }

    private void RandDirection() //무작위로 방향을 섞는 메소드
    {
        for (int i = 0; i < direction.Length; i++)
        {
            int randNum = Random.Range(0, direction.Length); //4방향 중 무작위로 방향 선택
            Vector2Int temp = direction[randNum]; //현재 인덱스에 해당하는 방향과 랜덤으로 선택한 방향을 서로 바꿈
            direction[randNum] = direction[i];
            direction[i] = temp;
        }
    }

    private IEnumerator GenerateRoad()
    {

        do
        {
            int index = -1; //-1은 갈 수 있는 길이 없음을 의미

            RandDirection(); //방향 무작위로 섞음

            for (int i = 0; i < direction.Length; i++)
            {
                if (CheckForRoad(i))
                {
                    index = i; //선택한 방향에 길이 없을 경우 방향 배열의 인덱스 저장
                    break;
                }
            }

            if (index != -1) //갈 수 있는 길이 있을 경우
            {
                stackDir.Push(direction[index]); //스택에 방향 저장
                stackTile.Push(tile); //스택에 현재 타일 저장
                tile.OpenWall(direction[index]); // 현재 타일의 진행방향 문을 열음

                pos += direction[index]; //위치 변수 수정
                map[pos.x, pos.y] = (int)TILE.CHECK; 
                CreateRoom(pos.x, pos.y); //다음 타일 생성
                tile.OpenWall(direction[index] * -1); // 다음 타일의 지나온 방향의 문을 열음
            }
            else //갈 수 있는 길이 없을 경우
            {
                Vector2Int reverseDir = stackDir.Pop() * -1;
                map[pos.x, pos.y] = (int)TILE.ROAD; //완성된 길 의미
                pos += reverseDir; //방향을 저장하는 스택에서 데이터를 꺼낸뒤 -1을 곱해 방향 반전
                tile = stackTile.Pop();
                if (map[pos.x, pos.y] == (int)TILE.ROAD)
                {
                    tile.OpenWall(reverseDir); //되돌아 온길의 문을 열음
                }
            }

            yield return null;
        }
        while (pos != end); //현재 위치가 끝 지점일때 종료

        tile.OpenWall(Vector2Int.up);
        Instantiate(bossPrefeb, new Vector3(end.x * tileSize, 0, (end.y + 1) * tileSize + 10), Quaternion.identity).transform.SetParent(transform);

        stackDir.Clear();
        stackTile.Clear();
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
        int rand = Random.Range(0, 2);

        if(x == 0 && z == 0)
            rand = 0;

        if (rand == 0)
        {
            GameObject room = Instantiate(roomPrefeb, pos, Quaternion.identity);
            room.transform.SetParent(transform);
            tile = room.GetComponent<Room>();
        }
        else
        {
            GameObject road = Instantiate(roadPrefeb, pos, Quaternion.identity);
            road.transform.SetParent(transform);
            tile = road.GetComponent<Road>();
        }

        
        
    }
}
