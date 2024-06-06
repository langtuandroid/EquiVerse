using Cinemachine;
using UnityEngine;

namespace Input
{
    public class CameraMovement : MonoBehaviour
    {
        private bool movedLeft = false, movedRight = false, movedUp = false, movedDown = false, zoomedIn = false, zoomedOut = false;
        public bool CameraLocked { get; set; }
        private bool rotateCameraStepCompleted = false;
        private bool zoomCameraStepCompleted = false;

        public CinemachineFreeLook mainCam;
        
        public float keyboardSpeed;
        public float mouseSpeed;
        public float scrollSpeed;
        public float minFieldOfView;
        public float maxFieldOfView;
        public float yAxisSpeedScale;
        public float keyboardXAxisSpeedScale;
        public float requiredDragTime = 2f; // Time in seconds the camera needs to be dragged

        private float dragTime = 0f;
        private bool draggedLeft = false, draggedRight = false, draggedUp = false, draggedDown = false;
        private bool allDirectionsDragged = false;

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

            // Keyboard input
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
            
            // Mouse input
            if (UnityEngine.Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                float mouseX = UnityEngine.Input.GetAxis("Mouse X");
                float mouseY = UnityEngine.Input.GetAxis("Mouse Y");
                mainCam.m_XAxis.Value += mouseX * mouseSpeed * Time.deltaTime;
                mainCam.m_YAxis.Value += mouseY * mouseSpeed * yAxisSpeedScale * Time.deltaTime;

                // Track direction of movement for mouse drag
                if (mouseX < 0) draggedLeft = true;
                if (mouseX > 0) draggedRight = true;
                if (mouseY < 0) draggedDown = true;
                if (mouseY > 0) draggedUp = true;

                // Increment drag time if the camera is being dragged in any direction
                if (mouseX != 0 || mouseY != 0)
                {
                    dragTime += Time.deltaTime;
                }

                // Check if all directions have been dragged
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

            // Apply keyboard inputs
            mainCam.m_XAxis.Value += xAxisValue * Time.deltaTime;
            mainCam.m_YAxis.Value += yAxisValue * Time.deltaTime;

            // If there was any camera movement with keyboard
            if (cameraMoved)
            {
                dragTime += Time.deltaTime;

                // Check if all directions have been moved
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
