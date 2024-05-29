using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFromUnit : MonoBehaviour
{
    public GameObject unit;
    public GameObject _RenderTargetParticle;
    public GameObject _ExplosionParticle;
    public RenderTexture _splatmap;

    // Start is called before the first frame update
    void Start()
    {
       // _camera.transform.SetParent(unit.transform);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           
        }
        
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatmap, ScaleMode.ScaleToFit, false, 1);
    }
}
