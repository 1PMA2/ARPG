using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private Rigidbody unitRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        unitRigidbody = GetComponent<Rigidbody>();
        StartCoroutine(CoFixedUpdate());
    }

    private void OnEnable()
    {
    }

    private void FixedUpdate()
    {
        
    }

    IEnumerator CoFixedUpdate()
    {
        while(true)
        {
            float hAxis = 0f;
            float vAxis = 0f;

            // Ű���� �Է��� �޽��ϴ�.
            if (Input.GetKey(KeyCode.UpArrow)) { vAxis = 1f; }

            if (Input.GetKey(KeyCode.DownArrow)) { vAxis = -1f; }

            if (Input.GetKey(KeyCode.RightArrow)) { hAxis = 1f; }

            if (Input.GetKey(KeyCode.LeftArrow)) { hAxis = -1f; }

          

            Vector3 cameraForward = cameraTransform.forward; // ī�޶��� ���� ����
            Vector3 cameraRight = cameraTransform.right;     // ī�޶��� ������ ����

            Vector3 inputDir = cameraForward * vAxis + cameraRight * hAxis;

            inputDir.y = 0f;
            inputDir = inputDir.normalized;

            // ĳ���Ͱ� ī�޶��� ������ �ٶ󺸵��� �����մϴ�.
            if (inputDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputDir);

                // �ε巯�� ������ �����մϴ�.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20f * Time.deltaTime);
            }

            unitRigidbody.velocity = inputDir * 5;


            yield return new WaitForFixedUpdate();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
