using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    private Recoil recoil;
    private UnitInformation unitInformation;
    // Start is called before the first frame update
    void Start()
    {
        recoil = GetComponentInParent<Recoil>();
        unitInformation = GetComponentInParent<UnitInformation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageState.SetState(other, recoil, unitInformation);     
    }
    
}
