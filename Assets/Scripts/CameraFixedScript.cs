using UnityEngine;

public class CameraFixedScript : MonoBehaviour
{
    void Start()
    {
        CameraScript.fixedCameraPosition = this.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CameraScript.isFixed = !CameraScript.isFixed;
            if (CameraScript.isFixed)
                CameraScript.isFixedTwo = false;
        }

    }
}
