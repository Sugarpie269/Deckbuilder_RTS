using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    // PositionLockLerp CameraController Implementation.
    public class CameraController : AbstractCameraController
    {
        //[SerializeField] public float LerpDuration;
        private Camera ManagedCamera;
        private LineRenderer CameraLineRenderer;
        //private float timecounter;
        //private Vector3 PreviousPosition;
        //private float ElapsedTime = 0.0f;
        //[SerializeField] private float ComparisonDistance = .0005f;
        [SerializeField] private float DisplacementFraction = .75f;
        [SerializeField] private float MaxDistance = 5f;

        [SerializeField] private float ShakeTime = .5f;
        [SerializeField] private float ShakeAmount = .1f;
        private float CurrentShakeTime = 0f;
        private bool Shaking = false;

        // NOTE: Assuming hardcoded values for Z-position and cross size.
        float ZVal = 85f;
        float crossSize = 4f;

        private void Awake()
        {
            this.ManagedCamera = this.gameObject.GetComponent<Camera>();
            this.CameraLineRenderer = this.gameObject.GetComponent<LineRenderer>();
            //this.PreviousPosition = this.ManagedCamera.transform.position;

            //this.ManagedCamera.transform.position = new Vector3(0, 0, this.ManagedCamera.transform.position.z);
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

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {

            //this.ManagedCamera.transform.position = new Vector3(this.Target.transform.position.x, this.Target.transform.position.y, this.ManagedCamera.transform.position.z);
            //return;

            var targetPosition = this.Target.transform.position;
            this.ManagedCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, this.ManagedCamera.transform.position.z);
            var cameraPosition = this.ManagedCamera.transform.position;

            var playerController = this.Target.GetComponent<PlayerController>();
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos = (Vector2)((worldMousePos - this.Target.transform.position));
            //var displacement = new Vector2(mousePos.x - targetPosition.x, mousePos.y - targetPosition.y);
            var displacedFrac = mousePos * this.DisplacementFraction;
            var displacedFracDir = new Vector2(displacedFrac.x, displacedFrac.y);
            displacedFracDir.Normalize();
            if (displacedFrac.magnitude >= this.MaxDistance)
            {
                displacedFrac.Normalize();
                displacedFrac = displacedFrac * this.MaxDistance;
            }
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
            

            /*this.ElapsedTime += Time.deltaTime;

            var lerpRatio = this.ElapsedTime / this.LerpDuration;

            
            
            //Debug.Log("Distance: " + this.GetDistanceBetween(targetPosition, this.PreviousPosition).ToString());
            if (this.GetDistanceBetween(targetPosition, this.PreviousPosition) >= this.ComparisonDistance)//targetPosition != this.PreviousPosition)
            {
                Debug.Log("hi " + Time.deltaTime.ToString());
                this.ElapsedTime = 0.0f;
                this.PreviousPosition = targetPosition;
            }
            else
            {
                Debug.Log("not hi");
            }

            Vector3 newposition = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z);
            newposition.y = Mathf.Lerp(cameraPosition.y, targetPosition.y,  lerpRatio);//3/LerpDuration * Time.deltaTime);
            newposition.x = Mathf.Lerp(cameraPosition.x, targetPosition.x,  lerpRatio);//3/LerpDuration * Time.deltaTime);
        
            this.ManagedCamera.transform.position= newposition;

            if (this.DrawLogic)
            {
                this.CameraLineRenderer.enabled = true;
                this.DrawCameraLogic();
            }
            else
            {
                this.CameraLineRenderer.enabled = false;
            }*/
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