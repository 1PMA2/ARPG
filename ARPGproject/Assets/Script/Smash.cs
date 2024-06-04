using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    private float smashDamage = 5f;

    private UnitController unitController;
    private UnitInformation unitInformation;
    private Recoil recoil;
    // Start is called before the first frame update
    void Start()
    {
        smashDamage = 5f;
        unitController = GetComponentInParent<UnitController>();
        unitInformation = GetComponentInParent<UnitInformation>();
        recoil = GetComponentInParent<Recoil>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        DamageState.Attack(other, recoil, unitInformation, unitController, transform, smashDamage);
    }
}
