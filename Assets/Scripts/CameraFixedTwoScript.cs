using UnityEngine;

public class CameraFixedTwoScript : MonoBehaviour
{
    void Start()
    {
        CameraScript.fixedCameraTwo = this.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CameraScript.isFixedTwo = !CameraScript.isFixedTwo;
            if (CameraScript.isFixedTwo)
                CameraScript.isFixed = false;
        }
    }
}
