using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexBlur : MonoBehaviour
{
    private static readonly int PROPERTY_TRAIL_DIR = Shader.PropertyToID("_TrailDir");

    [SerializeField]
    private SkinnedMeshRenderer _renderer;

    private Material _material;

    private Vector3 _trailPos;

    /// <summary> ?����Ͷڭ����?����?�� </summary>
    [SerializeField]
    private float _trailRate = 10f;

    private void Awake()
    {
        // material�˫����������ơ���𲪵�쪿�ޫƫꫢ���??�������
        _material = _renderer.material;
        //_trailPos = transform.position;
    }

    private void Update()
    {
        Mesh mesh = new Mesh();

        _renderer.BakeMesh(mesh);



        GameObject afterImageObj = new GameObject("AfterImage");

        MeshFilter mf = afterImageObj.AddComponent<MeshFilter>();

        mf.mesh = mesh;



        MeshRenderer mr = afterImageObj.AddComponent<MeshRenderer>();

        mr.material = _material;



        afterImageObj.transform.position = gameObject.transform.position;

        afterImageObj.transform.rotation = gameObject.transform.rotation;



        Destroy(afterImageObj, 1f);

        ////////////////////////
        //_trailPos = Vector3.Lerp(_trailPos, transform.position, Mathf.Clamp01(Time.deltaTime * _trailRate));
        //// ���֫������Ȫ���?�����窷�ƫ�?����۰����?������
        //Vector3 dir = transform.InverseTransformDirection(_trailPos - transform.position);
        //_material.SetVector(PROPERTY_TRAIL_DIR, dir);
    }
}
