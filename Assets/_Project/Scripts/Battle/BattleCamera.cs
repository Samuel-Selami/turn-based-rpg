using Unity.Cinemachine;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    [SerializeField] CinemachineOrbitalFollow orbit;
    [SerializeField] float swingSpeed = 0.2f;
    [SerializeField] float swingAngle = 20f;
    [SerializeField] bool drift;
    [SerializeField] float driftSpeed = 3f;
    [SerializeField] Transform orbitPivot;

    void Update()
    {
        if (orbit == null)
        {
            transform.RotateAround(orbitPivot.position, Vector3.up, driftSpeed * Time.deltaTime);
            return;
        }

        if (drift)
            orbit.HorizontalAxis.Value += driftSpeed * Time.deltaTime;
        else
            orbit.HorizontalAxis.Value = Mathf.Sin(Time.time * swingSpeed) * swingAngle;


    }
}
