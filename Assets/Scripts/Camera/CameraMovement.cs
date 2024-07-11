using Cinemachine;
using UnityEngine;

namespace Input
{
    public class CameraMovement : MonoBehaviour
    {
        public bool CameraLocked { get; set; }
        [HideInInspector] public CinemachineFreeLook mainCam;
        [HideInInspector] public float scrollSpeed = 15;
        [HideInInspector] public float globalXAxisSpeedScale = 1;
        [HideInInspector] public float globalYAxisSpeedScale = 1;
        [HideInInspector] public int xAxisInversedValue = 1;
        [HideInInspector] public int yAxisInversedValue = 1;
        [HideInInspector] public float requiredDragTime = 2;

        private float minFieldOfView = 10;
        private float maxFieldOfView = 70;
        private float yAxisSpeedScale = 0.02f;
        private float keyboardXAxisSpeedScale = 75;
        private float keyboardSpeed = 1;
        private float mouseSpeed = 1f;
        private bool movedLeft, movedRight, movedUp, movedDown;
        private bool zoomedIn, zoomedOut;
        private bool rotateCameraStepCompleted, zoomCameraStepCompleted;
        private bool draggedLeft, draggedRight, draggedUp, draggedDown;
        private bool allDirectionsDragged;
        private float dragTime;

        private void Start()
        {
            CameraLocked = true;
            mainCam.m_XAxis.m_InputAxisName = "";
            mainCam.m_YAxis.m_InputAxisName = "";

            LoadSettings();
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
            else
            {
                CheckCameraZoomTutorial();
            }
        }

        private void CheckCameraInput()
        {
            float xAxisValue = 0;
            float yAxisValue = 0;
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.A) && UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                xAxisValue = 0;
            }

            xAxisValue = GetKeyboardAxis(KeyCode.D, KeyCode.A, keyboardXAxisSpeedScale * globalXAxisSpeedScale);
            yAxisValue = GetKeyboardAxis(KeyCode.S, KeyCode.W, globalYAxisSpeedScale);

            mainCam.m_XAxis.Value += xAxisValue * xAxisInversedValue;
            mainCam.m_YAxis.Value += yAxisValue * yAxisInversedValue;

            float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            HandleZoom(scroll);

            if (UnityEngine.Input.GetMouseButton(1))
            {
                HandleMouseDrag();
            }
            else if (UnityEngine.Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private float GetKeyboardAxis(KeyCode negativeKey, KeyCode positiveKey, float scale)
        {
            float axisValue = 0;

            if (UnityEngine.Input.GetKey(negativeKey))
            {
                axisValue -= keyboardSpeed * scale * Time.deltaTime;
                movedLeft = true;
            }
            if (UnityEngine.Input.GetKey(positiveKey))
            {
                axisValue += keyboardSpeed * scale * Time.deltaTime;
                movedRight = true;
            }

            return axisValue;
        }

        private void HandleZoom(float scroll)
        {
            float zoom = 0;

            if (UnityEngine.Input.GetKey(KeyCode.Q))
            {
                zoom = scrollSpeed * 10f * Time.deltaTime;
                zoomedIn = true;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.E))
            {
                zoom = -scrollSpeed * 10f * Time.deltaTime;
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
        }

        private void HandleMouseDrag()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = UnityEngine.Input.GetAxis("Mouse X") * mouseSpeed * globalXAxisSpeedScale;
            float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * mouseSpeed * yAxisSpeedScale * globalYAxisSpeedScale;

            mainCam.m_XAxis.Value += mouseX * xAxisInversedValue;
            mainCam.m_YAxis.Value += mouseY * yAxisInversedValue;

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

        private void LoadSettings()
        {
            scrollSpeed = PlayerPrefs.GetFloat("ZoomSpeed", scrollSpeed);
            globalXAxisSpeedScale = PlayerPrefs.GetFloat("XAxisSpeed", globalXAxisSpeedScale);
            globalYAxisSpeedScale = PlayerPrefs.GetFloat("YAxisSpeed", globalYAxisSpeedScale);
            xAxisInversedValue = PlayerPrefs.GetInt("XAxisInvert", xAxisInversedValue);
            yAxisInversedValue = PlayerPrefs.GetInt("YAxisInvert", yAxisInversedValue);
        }
    }
}
