using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
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
            cameras[key] = unitCamera; // µñ¼Å³Ê¸® ¾÷µ¥ÀÌÆ®
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
}