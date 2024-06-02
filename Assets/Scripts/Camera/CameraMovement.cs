using Cinemachine;
using UnityEngine;

namespace Input
{
    public class CameraMovement : MonoBehaviour
    {
        private bool movedLeft = false, movedRight = false, movedUp = false, movedDown = false;
        public bool CameraLocked { get; set; }
        private bool cameraStepCompleted = false;

        public CinemachineFreeLook mainCam;
        
        [SerializeField]
        private float keyboardSpeed = 0.75f;
        [SerializeField]
        private float mouseSpeed = 100f;

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

            if (!cameraStepCompleted)
            {
                CheckCameraTutorial();
            }
        }

        private void CheckCameraInput()
        {
            // Reset input values
            mainCam.m_XAxis.m_InputAxisValue = 0;
            mainCam.m_YAxis.m_InputAxisValue = 0;

            // Keyboard input for horizontal movement
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                mainCam.m_XAxis.m_InputAxisValue = -keyboardSpeed;
                movedLeft = true;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                mainCam.m_XAxis.m_InputAxisValue = keyboardSpeed;
                movedRight = true;
            }

            // Keyboard input for vertical movement
            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                mainCam.m_YAxis.m_InputAxisValue = keyboardSpeed;
                movedUp = true;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                mainCam.m_YAxis.m_InputAxisValue = -keyboardSpeed;
                movedDown = true;
            }

            // Mouse input for vertical movement (scroll wheel)
            float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                mainCam.m_YAxis.m_InputAxisValue = scroll * mouseSpeed; // Adjust multiplier as needed
                if (scroll > 0)
                {
                    movedUp = true;
                }
                else if (scroll < 0)
                {
                    movedDown = true;
                }
            }

            if (UnityEngine.Input.GetMouseButton(2))
            {
                float mouseX = UnityEngine.Input.GetAxis("Mouse X");
                mainCam.m_XAxis.m_InputAxisValue = -mouseX * mouseSpeed;
                if (mouseX < 0) movedRight = true; 
                if (mouseX > 0) movedLeft = true;
            }
        }

        private void CheckCameraTutorial()
        {
            if (movedLeft && movedRight && movedUp && movedDown)
            {
                TutorialManager.CompleteStepAndContinueToNextStep("Step_Camera");
                cameraStepCompleted = true;
            }
        }
    }
}
