using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{

    class StringComparer : IEqualityComparer<string>
    {
        bool IEqualityComparer<string>.Equals(string x, string y)
        {
            return x == y;
        }
        int IEqualityComparer<string>.GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }

    private static GameObject cachedCameraPrefab;
    private static GameObject cachedMinimapPrefab;

    //private List<GameObject> cameras = new List<GameObject>();

    private Dictionary<string, GameObject> cameras = new Dictionary<string, GameObject>(new StringComparer());

    public CameraManager()
    {
        
    }
    private void Start()
    {
        cachedCameraPrefab = Resources.Load("Prefeb/CameraArm") as GameObject;
        //cachedMinimapPrefab = Resources.Load("Prefeb/Minimap") as GameObject;
        CreateCamera();

    }

    private void CreateCamera()
    {
        foreach (var key in cameras.Keys.ToArray())
        {
            GameObject unitCamera = Instantiate(cachedCameraPrefab);
            unitCamera.name = key;
            UnitController unit = cameras[key].GetComponent<UnitController>();

            unit.StickCamera(unitCamera);
            cameras[key] = unitCamera; // 딕셔너리 업데이트
        }

        //Instantiate(cachedMinimapPrefab);

    }

    public void CamRegister(string name, GameObject unit)
    {
        cameras.Add(name, unit);   
    }

    public GameObject GetCamera(string name)
    {
        if(cameras.ContainsKey(name))
            return cameras[name];
        else
            return null;
    }

    public void ShakeCamera(string name, float duration, float magnitude)
    {
        StartCoroutine(Shake(GetCamera(name), duration, magnitude));
    }

    public IEnumerator Shake(GameObject camera, float duration, float magnitude)
    {
        Vector3 originalPosition = camera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude * (1f - elapsed / duration);
            float y = Random.Range(-1f, 1f) * magnitude * (1f - elapsed / duration);
            float z = Random.Range(-1f, 1f) * magnitude * (1f - elapsed / duration);

            camera.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z + z);

            elapsed += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        camera.transform.localPosition = originalPosition;
    }

}