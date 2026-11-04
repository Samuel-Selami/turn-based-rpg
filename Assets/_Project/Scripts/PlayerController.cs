using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player movement attributes")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] Transform cameraTransform;


    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        Vector3 direction = camForward.normalized * z + camRight.normalized * x;

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        if (direction != Vector3.zero)
        {
            Quaternion target = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }
    }
}
