using UnityEngine;
using UnityEngine.Networking;

namespace Rebirth.Prototype
{
    public class CameraController : MonoBehaviour
    {
        // Const variables
        private const float MIN_CATCH_SPEED_DAMP = 0f;
        private const float MAX_CATCH_SPEED_DAMP = 1f;
        private const float MIN_ROTATION_SMOOTHING = 0f;
        private const float MAX_ROTATION_SMOOTHING = 30f;

        // Serializable fields
        [SerializeField]
        public Transform target = null; // The target to follow

        public bool lockCursor = true;
        public bool m_cursorIsLocked = true;
        public bool m_allowFirstPerson = false;

        public enum CameraTypes_e
        {
            FirstPerson,
            ThirdPerson
        }
        public CameraTypes_e camType;

        public Vector3 AimVector
        {
            get { return pivot.position - (transform.position - transform.forward); }
        }

        public Transform Pivot
        {
            get { return pivot; }
        }

        [SerializeField]
        [Range(MIN_CATCH_SPEED_DAMP, MAX_CATCH_SPEED_DAMP)]
        private float catchSpeedDamp = MIN_CATCH_SPEED_DAMP;

        [SerializeField]
        [Range(MIN_ROTATION_SMOOTHING, MAX_ROTATION_SMOOTHING)]
        [Tooltip("How fast the camera rotates around the pivot")]
        private float rotationSmoothing = 15.0f;

        // private fields
        private Transform rig; // The root transform of the camera rig
        private Transform pivot; // The point at which the camera pivots around
        private Quaternion pivotTargetLocalRotation; // Controls the X Rotation (Tilt Rotation)
        private Quaternion rigTargetLocalRotation; // Controlls the Y Rotation (Look Rotation)
        private Vector3 cameraVelocity; // The velocity at which the camera moves

        private CameraOcclusionProtector cop;

        protected virtual void Awake()
        {
            this.pivot = this.transform.parent;
            this.rig = this.pivot.parent;
            this.cop = this.GetComponent<CameraOcclusionProtector>();

            this.transform.localRotation = Quaternion.identity;
        }

        protected virtual void Update()
        {
            if (target == null) return;

            if (!m_cursorIsLocked)
                return;

            var controlRotation = PlayerInput.GetMouseRotationInput();
            this.UpdateRotation(controlRotation);

        }

        protected virtual void FixedUpdate()
        {
            if (target == null) return;

            target.GetComponent<RebirthPlayerController>().Controller.RotatePlayerToCameraDir(rigTargetLocalRotation);
        }

        protected virtual void LateUpdate()
        {
            if (target == null) return;

            this.FollowTarget();

            this.UpdateCursorLock();

            switch (camType)
            {
                case CameraTypes_e.FirstPerson:
                    if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                    {
                        camType = CameraTypes_e.ThirdPerson;
                        cop.distanceToTarget = 1f;
                        return;
                    }
                    break;
                case CameraTypes_e.ThirdPerson:

                    if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                    {
                        if (cop.distanceToTarget + Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) <= CameraOcclusionProtector.MAX_DISTANCE_TO_PLAYER)
                            cop.distanceToTarget += Mathf.Abs(Input.GetAxis("Mouse ScrollWheel"));
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                    {
                        cop.distanceToTarget -= Mathf.Abs(Input.GetAxis("Mouse ScrollWheel"));
                        if (cop.distanceToTarget < 1f && m_allowFirstPerson)
                        {
                            camType = CameraTypes_e.FirstPerson;
                            cop.distanceToTarget = 0f;
                        }
                        else if (cop.distanceToTarget <= 1f)
                        {
                            cop.distanceToTarget = 1f;
                        }
                    }
                    break;
            }
        }

        //public void SetDistanceToTarget(float distanceToTarget)
        //{
        //    Vector3 cameraTargetLocalPosition = Vector3.zero;
        //    cameraTargetLocalPosition.z = -distanceToTarget;
        //    this.transform.localPosition = cameraTargetLocalPosition;
        //}

        private void FollowTarget()
        {
            if (this.target == null)
            {
                return;
            }

            this.rig.position = Vector3.SmoothDamp(this.rig.position, this.target.transform.position, ref this.cameraVelocity, this.catchSpeedDamp);
        }

        private void UpdateRotation(Quaternion controlRotation)
        {
            if (this.target != null)
            {
                // Y Rotation (Look Rotation)
                this.rigTargetLocalRotation = Quaternion.Euler(0f, controlRotation.eulerAngles.y, 0f);

                // X Rotation (Tilt Rotation)
                this.pivotTargetLocalRotation = Quaternion.Euler(controlRotation.eulerAngles.x, 0f, 0f);

                if (this.rotationSmoothing > 0.0f)
                {
                    this.pivot.localRotation =
                        Quaternion.Slerp(this.pivot.localRotation, this.pivotTargetLocalRotation, this.rotationSmoothing * Time.deltaTime);

                    this.rig.localRotation =
                        Quaternion.Slerp(this.rig.localRotation, this.rigTargetLocalRotation, this.rotationSmoothing * Time.deltaTime);
                }
                else
                {
                    this.pivot.localRotation = this.pivotTargetLocalRotation;
                    this.rig.localRotation = this.rigTargetLocalRotation;
                }
            }
        }


        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            // Wenn ein Menü aktiv dann cursorLock deaktivieren
            // m_cursorIsLocked = true;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = !m_cursorIsLocked;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}