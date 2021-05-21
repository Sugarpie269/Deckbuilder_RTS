using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    // PositionLockLerp CameraController Implementation.
    public class CameraController : AbstractCameraController
    {
        [SerializeField] public float LerpDuration;
        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;
        private float timecounter;

        // NOTE: Assuming hardcoded values for Z-position and cross size.
        float ZVal = 85f;
        float crossSize = 4f;

        private void Awake()
        {
            this.ManagedCamera = this.gameObject.GetComponent<Camera>();
            this.CameraLineRenderer = this.gameObject.GetComponent<LineRenderer>();

            //this.ManagedCamera.transform.position = new Vector3(0, 0, this.ManagedCamera.transform.position.z);
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            var targetPosition = this.Target.transform.position;
            var cameraPosition = this.ManagedCamera.transform.position;


            Vector3 newposition = cameraPosition;
            newposition.y = Mathf.Lerp(cameraPosition.y, targetPosition.y,  3/LerpDuration * Time.deltaTime);
            newposition.x = Mathf.Lerp(cameraPosition.x, targetPosition.x,  3/LerpDuration * Time.deltaTime);
        
            this.ManagedCamera.transform.position= newposition;

            if (this.DrawLogic)
            {
                this.CameraLineRenderer.enabled = true;
                this.DrawCameraLogic();
            }
            else
            {
                this.CameraLineRenderer.enabled = false;
            }
        }

        public override void DrawCameraLogic()
        {
            var zPos = this.ZVal;

            Vector3 TopVert = new Vector3(0, 0.5f * this.crossSize, zPos);
            Vector3 BottomVert = new Vector3(0, -0.5f * this.crossSize, zPos);
            Vector3 Center = new Vector3(0, 0f, zPos);
            Vector3 LeftHoriz = new Vector3(-0.5f * this.crossSize, 0, zPos);
            Vector3 RightHoriz = new Vector3 (0.5f * this.crossSize, 0, zPos);

            this.CameraLineRenderer.positionCount = 6;
            this.CameraLineRenderer.useWorldSpace = false;

            this.CameraLineRenderer.SetPosition(0, TopVert);
            this.CameraLineRenderer.SetPosition(1, BottomVert);
            this.CameraLineRenderer.SetPosition(2, Center);
            this.CameraLineRenderer.SetPosition(3, LeftHoriz);
            this.CameraLineRenderer.SetPosition(4, Center);
            this.CameraLineRenderer.SetPosition(5, RightHoriz);
        }
    }
}