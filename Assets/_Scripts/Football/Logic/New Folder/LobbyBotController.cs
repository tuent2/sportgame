using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LobbyBotController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] NavMeshPath path;
    [SerializeField] float _speed;

    [SerializeField] Animator animator;


    Joystick fixedJoy;
    Vector3 movementDirection;
    bool isFindingPath = false;


    private void Start()
    {
        // Kiểm tra null trước khi sử dụng các thành phần
        if (agent == null || animator == null)
        {
            Debug.LogError("NavMeshAgent hoặc Animator chưa được gán!");
            return;
        }

        agent.SetDestination(transform.position);
        StartCoroutine(ShowMessagePeriodically());
    }

    private void Update()
    {
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            // Di chuyển và xoay nhân vật nếu nó đang di chuyển đến điểm đích
            animator.SetBool("Moving", true);
            RotateTowards(agent.steeringTarget);
        }
        else
        {
            // Ngừng di chuyển nếu đã đến điểm đích
            animator.SetBool("Moving", false);
            if (!isFindingPath)
            {
                StartCoroutine(ShowMessagePeriodically());
            }
        }
    }

    public void FindToTheTarget(Vector3 target)
    {
        StopCoroutine(ShowMessagePeriodically());
        isFindingPath = true;
        animator.SetBool("Moving", true);
        agent.destination = target;
    }

    IEnumerator ShowMessagePeriodically()
    {
        yield return new WaitForSeconds(1f);
        isFindingPath = false;
        FindToTheTarget(GetRandomNavMeshPosition(100f));
       
    }

    Vector3 GetRandomNavMeshPosition(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
    }
}
