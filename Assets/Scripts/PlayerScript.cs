using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    InputAction moveAction;
    private static PlayerScript prevInstance=null;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (prevInstance!= null)
        {
            //this.rb.linearVelocity = prevInstance.rb.linearVelocity;
            //this.rb.angularVelocity = prevInstance.rb.angularVelocity;
            Destroy(prevInstance.gameObject);
        }
        else
            prevInstance = this;
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        if(camForward == Vector3.zero)
            camForward = Camera.main.transform.up;
        else
            camForward.Normalize();

        Vector3 force = camForward * moveValue.y + camRight * moveValue.x;
        rb.AddForce(force * Time.timeScale);
    }
}
