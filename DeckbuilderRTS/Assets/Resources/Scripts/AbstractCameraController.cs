using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(LineRenderer))]
    public abstract class AbstractCameraController : MonoBehaviour
    {
        [SerializeField]
        protected bool DrawLogic;
        [SerializeField]
        protected GameObject Target;

        public abstract void DrawCameraLogic();

    }

}