using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour, ITile
{

    [SerializeField] private GameObject upDoor;
    [SerializeField] private GameObject downDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject RightDoor;

    private bool up, down, left, right = false;


    public void SetWall(Vector2Int dir)
    {
        if(dir == Vector2Int.up)
        {
            up = !up;
        }
        if (dir == Vector2Int.down)
        {
            down = !down;
        }
        if (dir == Vector2Int.left)
        {
            left = !left;
        }
        if (dir == Vector2Int.right)
        {
            right = !right;
        }



        upDoor.SetActive(!up);
        downDoor.SetActive(!down);
        leftDoor.SetActive(!left);
        RightDoor.SetActive(!right);
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



        upDoor.SetActive(!up);
        downDoor.SetActive(!down);
        leftDoor.SetActive(!left);
        RightDoor.SetActive(!right);
    }

}
