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

    [SerializeField] private GameObject upDoor;
    [SerializeField] private GameObject downDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [SerializeField] private GameObject marble_0;
    [SerializeField] private GameObject marble_1;
    [SerializeField] private GameObject marble_2;
    [SerializeField] private GameObject marble_3;

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


    public void SetType(TileType tileType)
    {
        roomType = tileType;

        marble_0.SetActive(false);
        marble_1.SetActive(false);
        marble_2.SetActive(false);
        marble_3.SetActive(false);

        OpenDoor();

        if (tileType == TileType.MARBLE)
        {
            marble_0.SetActive(true);
            marble_1.SetActive(true);
            marble_2.SetActive(true);
            marble_3.SetActive(true);

            SetRandomColorMarble();
            SetRandomCorrectMarble();

            isOpen = false;
        }
        else
        {
            
            
        }
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
            UIManager.Instance.ActiveItemUI(true);
            isOpen = true;
        }
        else if(!isOpen)
        {
            hitMarble.GetGlowEffect().SetGlowIntensity(25f);
            hitMarble.Summons();
        }

    }

    public void Dest()
    {
        if(roomType == TileType.MARBLE)
            Destroy(gameObject);
    }

    //[Header("Boxcast Property")]
    //[SerializeField] private Vector3 boxSize;
    //[SerializeField] private float maxDistance;
    //[SerializeField] private LayerMask layer;

    //[Header("Debug")]
    //[SerializeField] private bool drawGizmo;

    //private List<Vector3> detectedPositions = new List<Vector3>();

    //private void OnDrawGizmos()
    //{
    //    if (!drawGizmo) return;

    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    //}

    //public bool IsInUnit()
    //{
    //    Collider[] colliders = Physics.OverlapBox(transform.position, boxSize / 2, transform.rotation, layer);

    //    detectedPositions.Clear();

    //    foreach (var hitCollider in colliders)
    //    {
    //        // 감지된 각 콜라이더의 위치를 가져오기
    //        Vector3 detectedObjectPosition = hitCollider.transform.position;
    //        detectedPositions.Add(detectedObjectPosition);
    //    }

    //    return colliders.Length > 0;
    //}

    //void Start()
    //{
    //    r_rtcamera = Instantiate(_rtcamera, new Vector3(transform.position.x, 20, transform.position.z), _rtcamera.transform.rotation);
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        if (IsInUnit())
    //        {
    //            if (r_rtcamera != null && !r_rtcamera.activeSelf)
    //                r_rtcamera.SetActive(true);

    //            if (_particle != null)
    //            {
    //                foreach(var hitCollider in detectedPositions)
    //                {
    //                    GameObject renderTextureParticle = Instantiate(_particle, hitCollider, Quaternion.identity);

    //                    Destroy(renderTextureParticle, 2f);
    //                }

    //            }
    //            else
    //            {
    //                Debug.LogError("Particle prefab is not assigned!");
    //            }

    //        }
    //        else
    //        {
    //            if (r_rtcamera != null && r_rtcamera.activeSelf)
    //                r_rtcamera.SetActive(false);

    //        }
    //    }
    //}


}
