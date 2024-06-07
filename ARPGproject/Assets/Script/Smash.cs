using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    private UnitController unitController;
    private UnitInformation unitInformation;
    private Recoil recoil;
    [SerializeField] private ItemData data;
    // Start is called before the first frame update
    void Start()
    {
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
        DamageState.SmashAttack(other, recoil, unitInformation, unitController, transform, data.baseDamage);
    }
}
