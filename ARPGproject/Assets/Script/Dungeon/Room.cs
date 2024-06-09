using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static ITile;

public class Room : MonoBehaviour, ITile
{
    

    [SerializeField] private GameObject upWall;
    [SerializeField] private GameObject downWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;

    private bool up, down, left, right = false;

    private TileType roomType;
    public TileType RoomType { get { return roomType; } } 

    [SerializeField] private GameObject upDoor;
    [SerializeField] private GameObject downDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [SerializeField] private GameObject marble_0;
    [SerializeField] private GameObject marble_1;
    [SerializeField] private GameObject marble_2;
    [SerializeField] private GameObject marble_3;


    [SerializeField] private GameObject box;

    private GameObject correctMarble;
    private bool isOpen = false;

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



        upWall.SetActive(!up);
        downWall.SetActive(!down);
        leftWall.SetActive(!left);
        rightWall.SetActive(!right);
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



        upWall.SetActive(!up);
        downWall.SetActive(!down);
        leftWall.SetActive(!left);
        rightWall.SetActive(!right);
    }

    public void CloseDoor(Vector2Int dir)
    {
        if (roomType == TileType.MARBLE)
        {
            if (dir == Vector2Int.up)
            {
                upDoor.SetActive(true);
                downDoor.SetActive(false);
                leftDoor.SetActive(false);
                rightDoor.SetActive(false);
            }
            if (dir == Vector2Int.down)
            {
                upDoor.SetActive(false);
                downDoor.SetActive(true);
                leftDoor.SetActive(false);
                rightDoor.SetActive(false);
            }
            if (dir == Vector2Int.left)
            {
                upDoor.SetActive(false);
                downDoor.SetActive(false);
                leftDoor.SetActive(true);
                rightDoor.SetActive(false);
            }
            if (dir == Vector2Int.right)
            {
                upDoor.SetActive(false);
                downDoor.SetActive(false);
                leftDoor.SetActive(false);
                rightDoor.SetActive(true);
            }
        }
    }

    public void OpenDoor()
    {
        if(upDoor.activeSelf)
            StartCoroutine(OpenDown(upDoor));
        if (downDoor.activeSelf)
            StartCoroutine(OpenDown(downDoor));
        if (leftDoor.activeSelf)
            StartCoroutine(OpenDown(leftDoor));
        if (rightDoor.activeSelf)
            StartCoroutine(OpenDown(rightDoor));
        
    }

    private IEnumerator OpenDown(GameObject door)
    {
        float targetY = -0.3f;
        float speed = 0f;
        float acceleration = 9.8f;
        float elapsedTime = 0f;

        while (door.transform.position.y > targetY)
        {
            elapsedTime += Time.deltaTime;
            speed += acceleration * Time.deltaTime; 
            float newY = door.transform.position.y - speed * Time.deltaTime;

            
            if (newY < targetY)
                newY = targetY;

            door.transform.position = new Vector3(door.transform.position.x, newY, door.transform.position.z);
            yield return null;
        }

        door.SetActive(false);
    }

    public void SetTile(TileType tileType)
    {
        
        OpenDoor();

        switch(tileType)
        {
            case TileType.MARBLE:
                marble_0.SetActive(true);
                marble_1.SetActive(true);
                marble_2.SetActive(true);
                marble_3.SetActive(true);

                SetRandomColorMarble();
                SetRandomCorrectMarble();
                isOpen = false;
                break;

            case TileType.BOX:
                box.SetActive(true);
                marble_0.SetActive(false);
                marble_1.SetActive(false);
                marble_2.SetActive(false);
                marble_3.SetActive(false);
                break;

            default:
                marble_0.SetActive(false);
                marble_1.SetActive(false);
                marble_2.SetActive(false);
                marble_3.SetActive(false);
                break;
        }

        roomType = tileType;
    }


    private void SetRandomColorMarble()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        int randomIndex = Random.Range(0, 3);

        switch (randomIndex)
        {
            case 0:
                r = 1f;
                break;
            case 1:
                g = 1f;
                break;
            case 2:
                b = 1f;
                break;
        }

        Color color = new Color(r, g, b);

        marble_0.GetComponent<GlowEffect>().SetColor(color);
        marble_1.GetComponent<GlowEffect>().SetColor(color);
        marble_2.GetComponent<GlowEffect>().SetColor(color);
        marble_3.GetComponent<GlowEffect>().SetColor(color);
    }

    private void SetRandomCorrectMarble()
    {
        GameObject[] marbles = { marble_0, marble_1, marble_2, marble_3 };
        correctMarble = marbles[Random.Range(0, marbles.Length)];
    }

    public void OnMarbleHit(MarbleHit hitMarble)
    {
        if ((hitMarble.gameObject == correctMarble) && !isOpen)
        {
            hitMarble.GetGlowEffect().SetGlowIntensity(25f);
            OpenDoor();
            isOpen = true;
        }
        else if(!isOpen)
        {
            hitMarble.GetGlowEffect().SetGlowIntensity(25f);
            hitMarble.Summons();
        }

    }

    public void Reverse()
    {
        OpenDoor();
        if (roomType == TileType.MARBLE)
        {
            marble_0.SetActive(false);
            marble_1.SetActive(false);
            marble_2.SetActive(false);
            marble_3.SetActive(false);

            roomType = TileType.NONE;
        }
    }



}
