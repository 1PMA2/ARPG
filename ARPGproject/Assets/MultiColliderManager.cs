using Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiColliderManager : MonoBehaviour
{
    public class ColliderInfo
    {
        public Collider target;
        public int count;
    }

    [SerializeField] private readonly List<ColliderInfo> allNowStayTargetList = new();
    [SerializeField] private HashSet<Collider> processedCollider = new HashSet<Collider>();

    public delegate void TriggerEventHandler(MultiColliderManager manager, Collider other);
    public event TriggerEventHandler OnTriggerEnterEvent;
    public event TriggerEventHandler OnTriggerExitEvent;

    public Collider[] colliders;


    private void Start()
    {
        foreach (var collider in colliders)
        {
            RegisterCollider(collider);
        }

        OnTriggerEnterEvent += HandleTriggerEnter;
        OnTriggerExitEvent += HandleTriggerExit;
    }

    public void RegisterCollider(Collider collider)
    {
        var triggerProxy = collider.gameObject.AddComponent<TriggerHandler>();
        triggerProxy.Initialize(this);
    }

    private void HandleTriggerEnter(MultiColliderManager manager, Collider other)
    {
        if(!processedCollider.Contains(other))
        {
            Debug.Log($"{other.name} <color=green>OnTriggerEnter</color>");
            processedCollider.Add(other);
        }
    }

    private void HandleTriggerExit(MultiColliderManager manager, Collider other)
    {
        if (processedCollider.Contains(other))
        {
            Debug.Log($"{other.name} <color=red>OnTriggerOut</color>");
            processedCollider.Remove(other);
        }
    }
    
    private void FixedUpdate()
    {
        for (var i = allNowStayTargetList.Count - 1; i >= 0; i--)
        {
            var info = allNowStayTargetList[i];
            info.count--;
            if (info.count < 0)
            {
                allNowStayTargetList.Remove(info);
                
                OnTriggerExitEvent(this, info.target);
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var info = allNowStayTargetList.Find(x => x.target == other);
        if (info != null)
        {
            // TargetStay
            info.count = 1;
        }
        else
        {
            var newInfo = new ColliderInfo();
            newInfo.count = 1;
            newInfo.target = other;
            allNowStayTargetList.Add(newInfo);
            OnTriggerEnterEvent(this, other);
        }
    }



}
