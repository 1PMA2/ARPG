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

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
