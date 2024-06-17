using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerHandler : MonoBehaviour
{
    private MultiColliderManager parentTrigger;

    public void Initialize(MultiColliderManager parent)
    {
        parentTrigger = parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        parentTrigger?.OnTriggerStay(other);
    }

    private void OnTriggerStay(Collider other)
    {
        parentTrigger?.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        parentTrigger?.OnTriggerStay(other);
    }
}
