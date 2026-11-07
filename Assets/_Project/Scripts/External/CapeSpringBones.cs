
using UnityEngine;

[DefaultExecutionOrder(10000)]
public class CapeSpringBones : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform characterRoot;
    [SerializeField] private Transform[] capeBones;

    [Header("Movement")]
    [SerializeField] private float maxRunSpeed = 5f;
    [SerializeField] private float runLift = 16f;
    [SerializeField] private float turnInfluence = 0.025f;

    [Header("Wind")]
    [SerializeField] private float idleWind = 2f;
    [SerializeField] private float movingWind = 5f;
    [SerializeField] private float windFrequency = 1.4f;
    [SerializeField] private float waveDelay = 0.85f;

    [Header("Spring")]
    [SerializeField] private float springSmoothTime = 0.18f;
    [SerializeField] private float maxJointAngle = 12f;

    [Header("Direction")]
    [SerializeField] private float bendDirection = 1f;

    private Quaternion[] restRotations;
    private float[] angles;
    private float[] angularVelocities;

    private Vector3 previousPosition;
    private float previousYaw;

    private float smoothedSpeed;
    private float smoothedTurn;

    private bool initialized;

    private void Start()
    {
        if (characterRoot == null)
            characterRoot = transform;

        if (capeBones == null || capeBones.Length == 0)
        {
            Debug.LogError("No cape bones assigned!", this);
            enabled = false;
            return;
        }

        int count = capeBones.Length;

        restRotations = new Quaternion[count];
        angles = new float[count];
        angularVelocities = new float[count];

        for (int i = 0; i < count; i++)
        {
            if (capeBones[i] == null)
            {
                Debug.LogError("Missing cape bone!", this);
                enabled = false;
                return;
            }

            restRotations[i] = capeBones[i].localRotation;
        }

        previousPosition = characterRoot.position;
        previousYaw = characterRoot.eulerAngles.y;

        initialized = true;
    }

    private void LateUpdate()
    {
        if (!initialized)
            return;

        float dt = Time.deltaTime;

        if (dt <= 0f)
            return;

        // Avoid large jumps after pauses or teleports.
        if (dt > 0.1f)
        {
            ResetMotion();
            return;
        }

        // Movement speed.
        Vector3 delta =
            characterRoot.position - previousPosition;

        previousPosition = characterRoot.position;

        if (delta.magnitude > 2f)
        {
            ResetMotion();
            return;
        }

        float speed = delta.magnitude / dt;

        float speedRatio = Mathf.Clamp01(
            speed / Mathf.Max(0.01f, maxRunSpeed)
        );

        // Character turning.
        float currentYaw = characterRoot.eulerAngles.y;

        float turnSpeed =
            Mathf.DeltaAngle(previousYaw, currentYaw) / dt;

        previousYaw = currentYaw;

        turnSpeed = Mathf.Clamp(turnSpeed, -360f, 360f);

        // Smooth movement inputs.
        float inputBlend = 1f - Mathf.Exp(-8f * dt);

        smoothedSpeed = Mathf.Lerp(
            smoothedSpeed,
            speedRatio,
            inputBlend
        );

        smoothedTurn = Mathf.Lerp(
            smoothedTurn,
            turnSpeed,
            inputBlend
        );

        float time = Time.time;

        int count = capeBones.Length;

        for (int i = 0; i < count; i++)
        {
            float weight = (i + 1f) / count;

            // More movement toward the cape tip.
            float tipWeight = weight * weight;

            // Distribute the overall lift
            // across the entire chain.
            float lift =
                runLift * smoothedSpeed / count;

            // Traveling wave across the bones.
            float phase =
                time * windFrequency * Mathf.PI * 2f
                - i * waveDelay;

            float amplitude = Mathf.Lerp(
                idleWind,
                movingWind,
                smoothedSpeed
            );

            float wave =
                Mathf.Sin(phase) *
                amplitude *
                tipWeight;

            // Secondary wave for organic variation.
            float secondaryWave =
                Mathf.Sin(phase * 0.53f + 1.7f) *
                amplitude * 0.25f * tipWeight;

            // Small reaction to character turning.
            float turn =
                smoothedTurn *
                turnInfluence *
                tipWeight;

            float targetAngle =
                (lift + wave + secondaryWave + turn)
                * bendDirection;

            targetAngle = Mathf.Clamp(
                targetAngle,
                -maxJointAngle,
                maxJointAngle
            );

            // Spring-like smooth response.
            angles[i] = Mathf.SmoothDampAngle(
                angles[i],
                targetAngle,
                ref angularVelocities[i],
                Mathf.Max(0.01f, springSmoothTime),
                Mathf.Infinity,
                dt
            );

            angles[i] = Mathf.Clamp(
                angles[i],
                -maxJointAngle,
                maxJointAngle
            );

            // The cape's verified bending axis is Z.
            capeBones[i].localRotation =
                restRotations[i] *
                Quaternion.Euler(0f, 0f, angles[i]);
        }
    }

    private void ResetMotion()
    {
        previousPosition = characterRoot.position;
        previousYaw = characterRoot.eulerAngles.y;

        smoothedSpeed = 0f;
        smoothedTurn = 0f;

        for (int i = 0; i < angles.Length; i++)
        {
            angles[i] = 0f;
            angularVelocities[i] = 0f;

            capeBones[i].localRotation = restRotations[i];
        }
    }

    private void OnDisable()
    {
        if (restRotations == null)
            return;

        for (int i = 0; i < capeBones.Length; i++)
        {
            if (capeBones[i] != null)
                capeBones[i].localRotation = restRotations[i];
        }

        initialized = false;
    }
}
