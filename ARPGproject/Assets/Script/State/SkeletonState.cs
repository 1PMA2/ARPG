using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public abstract class SkeletonState : BaseState
    {
        protected Skeleton skeletonController;

        protected SkeletonState(Skeleton controller) : base(controller)
        {
            this.skeletonController = controller;
        }
    }
}
