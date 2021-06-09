using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    // CameraController Implementation.
    public class CameraController : AbstractCameraController
    {
        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;
        [SerializeField] private float DisplacementFraction = .75f;
        [SerializeField] private float MaxDistance = 5f;

        [SerializeField] private float ShakeTime = .5f;
        [SerializeField] private float ShakeAmount = .1f;
        private float CurrentShakeTime = 0f;
        private bool Shaking = false;

        private bool ControlCamera = false;
        private Vector2 DisplacementPos;

        // NOTE: Assuming hardcoded values for Z-position and cross size.
        float ZVal = 85f;
        float crossSize = 4f;

        private void Awake()
        {
            this.ManagedCamera = this.gameObject.GetComponent<Camera>();
            this.CameraLineRenderer = this.gameObject.GetComponent<LineRenderer>();
            this.DisplacementPos = new Vector2(0f, 0f);
        }

        private float GetDistanceBetween(Vector3 pos1, Vector3 pos2)
        {
            var x = Mathf.Abs(pos1.x - pos2.x);
            var y = Mathf.Abs(pos1.y - pos2.y);
            return Mathf.Sqrt(x * x + y * y);
        }

        public void SetShaking()
        {
            this.Shaking = true;
        }

        public void SetCameraControl()
        {
            this.ControlCamera = true;
        }

        public void ClearCameraControl()
        {
            this.ControlCamera = false;
        }

        public void ToggleCameraControl()
        {
            this.ControlCamera = !this.ControlCamera;
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            if (!this.ControlCamera)
            {
                var newPos = new Vector3(this.Target.transform.position.x + this.DisplacementPos.x, this.Target.transform.position.y + this.DisplacementPos.y, this.ManagedCamera.transform.position.z);
                if (this.Shaking)
                {
                    this.CurrentShakeTime += Time.deltaTime;
                    if (this.CurrentShakeTime >= this.ShakeTime)
                    {
                        this.CurrentShakeTime = 0f;
                        this.Shaking = false;
                    }
                    float shakeAmount = this.ShakeAmount;
                    var xShake = Random.Range(-shakeAmount, shakeAmount);
                    var yShake = Random.Range(-shakeAmount, shakeAmount);
                    newPos.x += xShake;
                    newPos.y += yShake;
                }
                this.ManagedCamera.transform.position = newPos;
                return;
            }
            else
            {
                var targetPosition = this.Target.transform.position;
                this.ManagedCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, this.ManagedCamera.transform.position.z);
                var cameraPosition = this.ManagedCamera.transform.position;

                var playerController = this.Target.GetComponent<PlayerController>();
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos = (Vector2)((worldMousePos - this.Target.transform.position));
                var displacedFrac = mousePos * this.DisplacementFraction;
                var displacedFracDir = new Vector2(displacedFrac.x, displacedFrac.y);
                displacedFracDir.Normalize();
                if (displacedFrac.magnitude >= this.MaxDistance)
                {
                    displacedFrac.Normalize();
                    displacedFrac = displacedFrac * this.MaxDistance;
                }
                this.DisplacementPos = displacedFrac;
                var newPos = new Vector3(targetPosition.x + displacedFrac.x, targetPosition.y + displacedFrac.y, cameraPosition.z);
                if (this.Shaking)
                {
                    this.CurrentShakeTime += Time.deltaTime;
                    if (this.CurrentShakeTime >= this.ShakeTime)
                    {
                        this.CurrentShakeTime = 0f;
                        this.Shaking = false;
                    }
                    float shakeAmount = this.ShakeAmount;
                    var xShake = Random.Range(-shakeAmount, shakeAmount);
                    var yShake = Random.Range(-shakeAmount, shakeAmount);
                    newPos.x += xShake;
                    newPos.y += yShake;
                }


                this.ManagedCamera.transform.position = newPos;
                return;
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