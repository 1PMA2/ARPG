using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitCamera : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform target;

    [SerializeField] private float Distance = 5;

    [SerializeField] private float xmove = 0;
    [SerializeField] private float ymove = 0;

    void Start()
    {
        transform.LookAt(target.position);
        transform.parent = target;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {

        if(Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X") * 2.57f;
            ymove -= Input.GetAxis("Mouse Y") * 2.25f;
        }

        transform.rotation = Quaternion.Euler(ymove, xmove, 0);

        Distance -= Input.GetAxis("Mouse ScrollWheel") * 10;
        if (Distance < 5f) Distance = 5f;
        if (Distance > 20f) Distance = 20f;

        transform.position = target.position - transform.rotation * new Vector3(0, 0, Distance);
    }
}
