using Cinemachine;
using UnityEngine;

namespace Input
{
    public class CameraMovement : MonoBehaviour
    {
        public bool CameraLocked { get; set; }
        public CinemachineFreeLook mainCam;
        public float keyboardSpeed;
        public float mouseSpeed;
        public float scrollSpeed;
        public float minFieldOfView;
        public float maxFieldOfView;
        public float yAxisSpeedScale;
        public float keyboardXAxisSpeedScale;
        public float requiredDragTime;

        private bool movedLeft = false, movedRight = false, movedUp = false, movedDown = false;
        private bool zoomedIn = false, zoomedOut = false;
        private bool rotateCameraStepCompleted = false, zoomCameraStepCompleted = false;
        private bool draggedLeft = false, draggedRight = false, draggedUp = false, draggedDown = false;
        private bool allDirectionsDragged = false;
        private float dragTime = 0f;

        private void Start()
        {
            CameraLocked = true;
            mainCam.m_XAxis.m_InputAxisName = "";
            mainCam.m_YAxis.m_InputAxisName = "";
        }

        private void Update()
        {
            if (!CameraLocked)
            {
                CheckCameraInput();
            }

            if (!rotateCameraStepCompleted)
            {
                CheckCameraRotateTutorial();
            }

            if (rotateCameraStepCompleted)
            {
                CheckCameraZoomTutorial();
            }
        }

        private void CheckCameraInput()
        {
            float xAxisValue = 0;
            float yAxisValue = 0;
            bool cameraMoved = false;

            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                xAxisValue = keyboardSpeed * keyboardXAxisSpeedScale;
                movedLeft = true;
                cameraMoved = true;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                xAxisValue = -keyboardSpeed * keyboardXAxisSpeedScale;
                movedRight = true;
                cameraMoved = true;
            }

            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                yAxisValue = -keyboardSpeed;
                movedUp = true;
                cameraMoved = true;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                yAxisValue = keyboardSpeed;
                movedDown = true;
                cameraMoved = true;
            }

            float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            float zoom = 0;

            if (UnityEngine.Input.GetKey(KeyCode.Q))
            {
                zoom = scrollSpeed * Time.deltaTime * 10f;
                zoomedIn = true;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.E))
            {
                zoom = -scrollSpeed * Time.deltaTime * 10f;
                zoomedOut = true;
            }

            mainCam.m_Lens.FieldOfView -= (scroll * scrollSpeed) + zoom;
            mainCam.m_Lens.FieldOfView = Mathf.Clamp(mainCam.m_Lens.FieldOfView, minFieldOfView, maxFieldOfView);

            if (scroll > 0)
            {
                zoomedIn = true;
            }
            else if (scroll < 0)
            {
                zoomedOut = true;
            }

            if (UnityEngine.Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                float mouseX = UnityEngine.Input.GetAxis("Mouse X");
                float mouseY = UnityEngine.Input.GetAxis("Mouse Y");
                mainCam.m_XAxis.Value += mouseX * mouseSpeed * Time.deltaTime;
                mainCam.m_YAxis.Value += mouseY * mouseSpeed * yAxisSpeedScale * Time.deltaTime;

                if (mouseX < 0) draggedLeft = true;
                if (mouseX > 0) draggedRight = true;
                if (mouseY < 0) draggedDown = true;
                if (mouseY > 0) draggedUp = true;

                if (mouseX != 0 || mouseY != 0)
                {
                    dragTime += Time.deltaTime;
                }

                if (draggedLeft && draggedRight && draggedUp && draggedDown && dragTime >= requiredDragTime)
                {
                    allDirectionsDragged = true;
                }
            }
            else if (UnityEngine.Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            mainCam.m_XAxis.Value += xAxisValue * Time.deltaTime;
            mainCam.m_YAxis.Value += yAxisValue * Time.deltaTime;

            if (cameraMoved)
            {
                dragTime += Time.deltaTime;

                if (movedLeft && movedRight && movedUp && movedDown && dragTime >= requiredDragTime)
                {
                    allDirectionsDragged = true;
                }
            }
        }

        private void CheckCameraRotateTutorial()
        {
            if ((movedLeft && movedRight && movedUp && movedDown) || allDirectionsDragged)
            {
                TutorialManager.CompleteStepAndContinueToNextStep("Step_CameraRotate");
                rotateCameraStepCompleted = true;
            }
        }

        private void CheckCameraZoomTutorial()
        {
            if (zoomedIn && zoomedOut)
            {
                TutorialManager.CompleteStepAndContinueToNextStep("Step_CameraZoom");
                zoomCameraStepCompleted = true;
            }
        }
    }
}
