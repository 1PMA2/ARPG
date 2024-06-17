using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public abstract class BearState : BaseState
    {
        protected Bear bearController;

        protected BearState(Bear controller) : base(controller)
        {
            this.bearController = controller;
        }
    }
}
