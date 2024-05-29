using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Room : MonoBehaviour, ITile
{

    [SerializeField] private GameObject upDoor;
    [SerializeField] private GameObject downDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject RightDoor;

    //[SerializeField] private GameObject unit;
    //[SerializeField] private GameObject _rtcamera;
    //private GameObject r_rtcamera;
    //[SerializeField] private GameObject _particle;
    //[SerializeField] private RenderTexture _splatmap;

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

    //private void OnGUI()
    //{
    //    if (IsInUnit())
    //        GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatmap, ScaleMode.ScaleToFit, false, 1);
    //}

}
