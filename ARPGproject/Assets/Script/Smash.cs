using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    private UnitController unitController;
    private UnitInformation unitInformation;
    private Recoil recoil;
    [SerializeField] private ItemData data;

    private MultiColliderManager multiColliderManager;
    // Start is called before the first frame update
    void Start()
    {
        unitController = GetComponentInParent<UnitController>();
        unitInformation = GetComponentInParent<UnitInformation>();
        recoil = GetComponentInParent<Recoil>();

        multiColliderManager = FindObjectOfType<MultiColliderManager>();

        multiColliderManager = FindObjectOfType<MultiColliderManager>();
        if (multiColliderManager != null)
        {
            multiColliderManager.OnTriggerEnterEvent += OnMultiTriggerEnter;
        }//보스룸에서 연결
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

    private void OnMultiTriggerEnter(MultiColliderManager manager, Collider other)
    {
        if (other.gameObject == gameObject)
        {
            DamageState.SmashAttack(manager, other, recoil, unitInformation, unitController, transform, data.baseDamage);
        }
    }
}
