using Controller;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class Katana : MonoBehaviour
{
    private Recoil recoil;
    private UnitInformation unitInformation;
    private MultiColliderManager multiColliderManager;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        recoil = GetComponentInParent<Recoil>();
        unitInformation = GetComponentInParent<UnitInformation>();

        multiColliderManager = FindObjectOfType<MultiColliderManager>();
        if (multiColliderManager != null)
        {
            multiColliderManager.OnTriggerEnterEvent += OnMultiTriggerEnter;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        DamageState.Attack(other, recoil, unitInformation, transform);
    }

    private void OnMultiTriggerEnter(MultiColliderManager manager, Collider other)
    {
        if (other.gameObject == gameObject)
        {
            DamageState.Attack(manager, other, recoil, unitInformation, transform);
        }
    }



}
