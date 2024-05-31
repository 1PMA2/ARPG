using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ITile;

public class Road : MonoBehaviour, ITile
{
    [SerializeField] private GameObject upRoad;
    [SerializeField] private GameObject downRoad;
    [SerializeField] private GameObject leftRoad;
    [SerializeField] private GameObject RightRoad;

    private bool up, down, left, right = false;

    public void SetWall(Vector2Int dir)
    {

    }

    public void OpenWall(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
        {
            up = true;
        }
        if (dir == Vector2Int.down)
        {
            down = true;
        }
        if (dir == Vector2Int.left)
        {
            left = true;
        }
        if (dir == Vector2Int.right)
        {
            right = true;
        }



        upRoad.SetActive(up);
        downRoad.SetActive(down);
        leftRoad.SetActive(left);
        RightRoad.SetActive(right);
    }

    public void CloseDoor(Vector2Int dir)
    {

    }

    public void OpenDoor()
    {

    }

    public void SetType(TileType tileType)
    {

    }
    public void Dest()
    {

    }

}
