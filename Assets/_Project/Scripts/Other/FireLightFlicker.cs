using UnityEngine;

public class FireLightFlicker : MonoBehaviour
{
    public Light fireLight;
    public float minIntensity = 2.2f;
    public float maxIntensity = 3.2f;
    public float speed = 8f;

    private void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * speed, 0f);
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
