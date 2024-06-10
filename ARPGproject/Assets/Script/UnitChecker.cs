using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitChecker : MonoBehaviour
{
    public event Action OnDisabled;

    [Header("Cast Property")]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;

    [Header("Debug")]
    [SerializeField] private bool drawGizmo;

    private List<Transform> detectedTransform = new List<Transform>();

    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, radius);
    }

    public Transform InNearUnitTransform()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        Transform nearestTransform = null;
        float nearestDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            // �� �ݶ��̴��� ��ġ�� ��������
            Transform detectedObjectTranform = collider.transform;

            // �Ÿ� ���
            float distance = Vector3.Distance(transform.position, detectedObjectTranform.position);

            // ��������� �ִ� �Ÿ����� ���� ��� ������Ʈ
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTransform = detectedObjectTranform;
            }
        }

        return nearestTransform;
    }

    private void OnDisable()
    {
        OnDisabled?.Invoke();
    }
}
