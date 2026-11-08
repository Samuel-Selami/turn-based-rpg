using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OverworldMonster : MonoBehaviour
{
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        homePosition = transform.position;
        StartCoroutine(Wander());
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(homePosition, wanderRadius);
        Gizmos.DrawLine(homePosition, currentTarget);
    }
    void Update()
    {
        
    }
}
