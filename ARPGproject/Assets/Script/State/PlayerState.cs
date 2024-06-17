using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public abstract class PlayerState : BaseState
    {
        protected Player playerController;

        protected PlayerState(Player controller) : base(controller)
        {
            this.playerController = controller;
        }
    }
}
