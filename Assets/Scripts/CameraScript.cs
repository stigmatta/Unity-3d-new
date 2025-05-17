using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform cameraAnchor;
    private float sensitivityX = 4f;
    private float sensitivityY = 1f;
    private float minVerticalAngle = 30f;
    private float maxVerticalAngle = 60f;
    private float minVerticalAngleFPV = -5f;
    private float maxVerticalAngleFPV = 30f;
    private float minOffset = 2f;
    private float maxOffset = 13f;

    public static bool isFixed = false;
    public static bool isFixedTwo = false;
    public static Transform fixedCameraPosition = null!;
    public static Transform fixedCameraTwo = null!;


    private Vector3 offset;
    private float angleY;
    private float angleX;

    private InputAction lookAction;

    private void Start()
    {
        offset = transform.position - cameraAnchor.position;

        angleY = transform.eulerAngles.y;
        angleX = transform.eulerAngles.x;

        lookAction = InputSystem.actions.FindAction("Look");
        if (lookAction != null)
            lookAction.Enable();
        GameState.isFpv = offset.magnitude < minOffset;
    }

    private void Update()
    {
        if (isFixed && !isFixedTwo)
        {
            this.transform.position = fixedCameraPosition.position;
            this.transform.rotation = fixedCameraPosition.rotation;
        }
        else if (isFixedTwo && !isFixed)
        {
            this.transform.position = fixedCameraTwo.position;
            this.transform.rotation = fixedCameraTwo.rotation;
        }
        else
        {
            Vector2 zoomValue = Input.mouseScrollDelta * Time.timeScale;
            if (zoomValue.y > 0 && !GameState.isFpv)
            {
                offset *= 0.9f;

                if (offset.magnitude < minOffset)
                {
                    offset *= 0.01f;
                }
            }
            else if (zoomValue.y < 0)
            {
                if (GameState.isFpv)
                {
                    offset *= minOffset / offset.magnitude;
                }
                if (offset.magnitude < maxOffset)
                {
                    offset *= 1.1f;
                }
            }

            GameState.isFpv = offset.magnitude < minOffset;

            Vector2 lookValue = lookAction.ReadValue<Vector2>();

            angleY += lookValue.x * sensitivityX * Time.deltaTime;
            angleX -= lookValue.y * sensitivityY * Time.deltaTime;

            if (!GameState.isFpv)
                angleX = Mathf.Clamp(angleX, minVerticalAngle, maxVerticalAngle);
            else
                angleX = Mathf.Clamp(angleX, minVerticalAngleFPV, maxVerticalAngleFPV);

            transform.rotation = Quaternion.Euler(angleX, angleY, 0f);
            transform.position = cameraAnchor.position + Quaternion.Euler(0f, angleY, 0f) * offset;
        }
    }
}
