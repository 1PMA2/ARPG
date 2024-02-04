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

            // 키보드 입력을 받습니다.
            if (Input.GetKey(KeyCode.UpArrow)) { vAxis = 1f; }

            if (Input.GetKey(KeyCode.DownArrow)) { vAxis = -1f; }

            if (Input.GetKey(KeyCode.RightArrow)) { hAxis = 1f; }

            if (Input.GetKey(KeyCode.LeftArrow)) { hAxis = -1f; }

          

            Vector3 cameraForward = cameraTransform.forward; // 카메라의 전방 벡터
            Vector3 cameraRight = cameraTransform.right;     // 카메라의 오른쪽 벡터

            Vector3 inputDir = cameraForward * vAxis + cameraRight * hAxis;

            inputDir.y = 0f;
            inputDir = inputDir.normalized;

            // 캐릭터가 카메라의 방향을 바라보도록 설정합니다.
            if (inputDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputDir);

                // 부드러운 보간을 수행합니다.
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
