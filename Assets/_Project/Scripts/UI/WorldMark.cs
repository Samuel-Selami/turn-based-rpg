using UnityEngine;
using UnityEngine.UI;

public class WorldMark : MonoBehaviour
{

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 toCamera = transform.position - cam.transform.position;
        transform.rotation = Quaternion.LookRotation(toCamera);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
