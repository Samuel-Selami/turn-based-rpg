using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OverworldMonster : MonoBehaviour
{
    enum State { Wander, Suspicious, Alerted, Chase, Engaging}
    State currentState = State.Wander;

    public MonsterData data;
    public MonsterZone zone;
    Animator animator;
    [Header("Monster moove")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float wanderRadius = 5f;
    [SerializeField] float minWaitTime = 5f;
    [SerializeField] float maxWaitTime = 15f;
    float rotationSpeed = 5f;

    Vector3 homePosition;
    Vector3 currentTarget;

    [Header("Aggro")]
    [SerializeField] float aggroRadius = 8f;
    [SerializeField] float giveUpRadius = 14f;
    [SerializeField] float suspiciousDuration = 2f;
    [SerializeField] float reactionDelay = 1.5f;

    float stateTimer;
    Coroutine WanderRoutine;
    Transform player;

    [Header("Aggro Sprite")]
    [SerializeField] float markHeight = 2f;
    [SerializeField] float popDuration = 0.25f;
    [SerializeField] AnimationCurve popCurve;
    [SerializeField] float shakeSpeed = 12f;
    [SerializeField] float shakeAngle = 8f;
    [SerializeField] float contactDistance = 1.2f;
    GameObject currentMark;

    Camera cam;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Start()
    {
        homePosition = transform.position;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            enabled = false;
            return;
        }

        player = playerObject.transform;
        WanderRoutine = StartCoroutine(Wander());
    }

    IEnumerator ShowMark(GameObject prefab)
    {
        if (currentMark != null)
            Destroy(currentMark);

        GameObject mark = Instantiate(prefab, transform);
        currentMark = mark;
        mark.transform.localPosition = new Vector3(0f, markHeight, 0f);
        mark.transform.localScale = Vector3.zero;

        float elapsed = 0f;

        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popDuration;

            float scale = popCurve.Evaluate(t);
            mark.transform.localScale = new Vector3(scale, scale, scale);

            FaceCamera(mark);

            yield return null;
        }
        while (mark != null)
        {
            FaceCamera(mark);
            yield return null;
        }
    }
    void FaceCamera(GameObject mark)
    {
        Vector3 toCamera = mark.transform.position - cam.transform.position;
        float angle = Mathf.Sin(Time.time * shakeSpeed) * shakeAngle;
        mark.transform.rotation = Quaternion.LookRotation(toCamera) * Quaternion.Euler(0f, 0f, angle);
    }

    void HideMark()
    {
        if (currentMark != null)
            Destroy(currentMark);
    }
    IEnumerator Wander()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        while (true)
        {
            currentTarget = RandomPointAroundHome();

            animator.SetBool("Moving", true);
            while (Vector3.Distance(transform.position, currentTarget) > 0.2f)
            {
                // direction
                Vector3 direction = currentTarget - transform.position;
                direction.y = 0f;
                Quaternion target = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
                // direction

                transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
                yield return null;
            }
            animator.SetBool("Moving", false);

            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }

    Vector3 RandomPointAroundHome()
    {
        float randomPointX = Random.Range(-wanderRadius, wanderRadius);
        float randomPointZ = Random.Range(-wanderRadius, wanderRadius);

        Vector3 randomPointFinal = homePosition + new Vector3(randomPointX, 0f, randomPointZ);

        return zone.SnapToGround(randomPointFinal);
    }


    void OnDrawGizmos() // DEBUG
    {
        //spawn area
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(homePosition, wanderRadius);
        Gizmos.DrawLine(homePosition, currentTarget);
        //aggro area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
    void Update()
    {
        float distanceMonsterPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceMonsterPlayer < contactDistance) // Fight
        {
            HideMark();
            StopCoroutine(WanderRoutine);
            animator.SetBool("Moving", false);
            currentState = State.Engaging;
            Debug.Log("Combat !");
            Encounter.leader = data;
            Encounter.enemyCount = 1;
            zone.gameFlow.StartBattle();
        }

        switch (currentState)
        {
            case State.Wander:
                if (distanceMonsterPlayer < aggroRadius)
                {
                    StopCoroutine(WanderRoutine);
                    animator.SetBool("Moving", false);
                    currentState = State.Suspicious;
                    StartCoroutine(ShowMark(zone.questionMarkPrefab));
                    stateTimer = 0f;
                }
                break;

            case State.Suspicious:
                stateTimer += Time.deltaTime;

                if (distanceMonsterPlayer > aggroRadius)
                {
                    HideMark();
                    WanderRoutine = StartCoroutine(Wander());
                    currentState = State.Wander;
                }
                else if (stateTimer > suspiciousDuration)
                {
                    stateTimer = 0f;
                    currentState = State.Alerted;
                    StartCoroutine(ShowMark(zone.exclamationMarkPrefab));
                }
                break;

            case State.Alerted:
                stateTimer += Time.deltaTime;

                if (distanceMonsterPlayer > aggroRadius)
                    currentState = State.Suspicious;
                else if (stateTimer > reactionDelay)
                {
                    animator.SetBool("Moving", true);
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                //direction
                Vector3 direction = player.transform.position - transform.position;
                direction.y = 0f;
                Quaternion target = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
                // direction

                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

                if (distanceMonsterPlayer > giveUpRadius)
                {
                    HideMark();
                    animator.SetBool("Moving", false);
                    WanderRoutine = StartCoroutine(Wander());
                    currentState = State.Wander;
                }
                break;

                case State.Engaging:
                break;
        }
    }




}
