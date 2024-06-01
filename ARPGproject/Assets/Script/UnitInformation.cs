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


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
