using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class UnitInformation : MonoBehaviour
    {

        [SerializeField] private UnitState _currentState; 
        public UnitState currentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        [SerializeField] private bool isplayer;
        public bool IsPlayer
        {
            get { return isplayer; }
            set { isplayer = value; }
        }

        [SerializeField] private float health;
        public float Health
        {
            get { return health;}
            set { health = value; }
        }

        [SerializeField] private float damage;
        public float Damage
        {
            get { return damage;}
            set { damage = value; }
        }

        [SerializeField] private float recoilPower;
        public float RecoilPower
        {
            get { return recoilPower; }
            set { recoilPower = value; }
        }

        [SerializeField] private int maxCounter;
        public int MaxCounter
        {
            get { return maxCounter; }
            set { maxCounter = value; }
        }

        [SerializeField] private int lightninig;
        public int Lightninig
        {
            get { return lightninig; }
            set { lightninig = value; }
        }

        [SerializeField] private int superArmor;
        public int SuperArmor
        {
            get { return superArmor; }
            set { superArmor = value; }
        }

        [SerializeField] private float autoGuard;
        public float AutoGuard
        {
            get { return autoGuard; }
            set { autoGuard = value; }
        }

        [SerializeField] private int drain;
        public int Drain
        {
            get { return drain; }
            set { drain = value; }
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
