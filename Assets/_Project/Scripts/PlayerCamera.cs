using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] float cameraMoveSpeed = 5f;
    [SerializeField] Vector3 cameraOffset ;
    [SerializeField] Vector3 cameraRotation;
    [SerializeField] float orbitSpeed = 120f;
    float yaw;


    void LateUpdate()
    {
        float turn = 0f;
        if (Gamepad.current != null)
            turn = Gamepad.current.rightStick.ReadValue().x;

        yaw += turn * orbitSpeed * Time.deltaTime;

        Vector3 wantedPosition = player.transform.position + Quaternion.Euler(0f, yaw, 0f) * cameraOffset;

        transform.position = Vector3.Lerp(transform.position, wantedPosition, cameraMoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y + yaw, cameraRotation.z);
    }
}
