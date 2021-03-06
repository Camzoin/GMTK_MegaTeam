﻿using UnityEngine;

namespace UnityTemplateProjects
{
    public class FPSPlayerController : MonoBehaviour
    {
        class CameraState
        {
            public float yaw;
            public float pitch;
            public float roll;

            public void SetFromTransform(Transform t)
            {
                pitch = t.eulerAngles.x;
                yaw = t.eulerAngles.y;
                roll = t.eulerAngles.z;
            }

            public void Translate(Vector3 translation)
            {
                Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);
            }

            public void UpdateTransform(Transform t)
            {
                t.eulerAngles = new Vector3(pitch, yaw, roll);
            }
        }

        CameraState m_TargetCameraState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        [Header("Rotation Settings")]
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool invertY = false;

        [Header("Gun Settings")]
        public float fireRate = 0.1f;
        public float accuracy = 1f;

        private float cooldownCD = 0f;

        public Camera mainCamera;
        public GameObject bulletHole;
        public Animator animator;
        public ParticleSystem fireLense;
        public GameObject fireSound;

        void OnEnable()
        {
            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
        }

        void Update()
        {
            // Hide and lock cursor when right mouse button pressed
            //if (Input.GetMouseButtonDown(1))
            //{
            Cursor.lockState = CursorLockMode.Locked;
            //}

            // Unlock and show cursor when right mouse button released
            //if (Input.GetMouseButtonUp(1))
            //{
            // Cursor.visible = true;
            //   Cursor.lockState = CursorLockMode.None;
            //}

            cooldownCD -= Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && cooldownCD <= 0f )
            {
                animator.SetTrigger("FireGun");
                fireLense.Play();
                Instantiate(fireSound, transform.position, transform.rotation);
                cooldownCD = 0.25f;

                RaycastHit hitData = new RaycastHit();

                if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitData))
                {
                    if(hitData.transform.root != null)
                    {
                        if (hitData.transform.root.gameObject.TryGetComponent(out Dummy a))
                        {
                            if (a.Up)
                            {
                                accuracy *= 1.1f;
                                a.Up = false;
                                a.shotCD = 1.5f;
                                GameObject bulletHoleObject = Instantiate(bulletHole, hitData.point, Quaternion.Euler(new Vector3(-90, 0, 0)));
                                bulletHoleObject.transform.parent = a.transform;
                                bulletHoleObject.transform.localPosition -= new Vector3(0, 0, 0.11f);

                                if (GameManager.instance != null)
                                    GameManager.instance.ScorePoints(GameManager.games.AIMTRAIN, 1);
                            }
                            else Miss();
                        }
                    }
                    else if (hitData.transform.gameObject.TryGetComponent(out Dummy a))
                    {
                        if (a.Up)
                        {
                            accuracy *= 1.1f;
                            a.Up = false;
                            a.shotCD = 1.5f;
                            GameObject bulletHoleObject = Instantiate(bulletHole, hitData.point, Quaternion.Euler(new Vector3(90, 0, 0)));
                            bulletHoleObject.transform.parent = a.transform;
                            
                            if (GameManager.instance != null)
                                GameManager.instance.ScorePoints(GameManager.games.AIMTRAIN, 1);
                        }
                        else Miss();
                    }
                    else
                    {
                        Miss();
                    }
                }
                else
                {
                    Miss();
                }
            }

            if (Input.GetMouseButton(1))
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 30, 0.1f);
            else
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, 0.1f);

            // Rotation
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
            m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;

            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f)) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            m_InterpolatingCameraState.UpdateTransform(transform);
        }

        public void Miss()
        {
            accuracy *= 0.9f;
            if (GameManager.instance != null)
                GameManager.instance.ScorePoints(GameManager.games.AIMTRAIN, -1);
        }
    }
}