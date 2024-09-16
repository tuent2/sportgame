using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using NaughtyAttributes;
using Unity.MLAgents;

public class LobbyPlayerController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] NavMeshPath path;
    [SerializeField] float _speed;

    [SerializeField] Animator animator;

    
    Joystick fixedJoy;
    Vector3 movementDirection;
    bool isFindingPath = false;
    [SerializeField] LineRenderer lineRenderer;
    bool isMoving = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        agent.SetDestination(gameObject.transform.position);
        lineRenderer.enabled = false;
        fixedJoy = Lobby_UIController.instance.lobby_joystick;
    }
    private void InitMove()
    {
        agent.destination = new Vector3(5f, 0f, 5f);
        agent.CalculatePath(new Vector3(5f,0f,5f) ,path);
       
    }

    private void Update()
    {
        movementDirection = new Vector3(fixedJoy.Direction.x, 0f, fixedJoy.Direction.y);
        
        if (movementDirection.sqrMagnitude <= 0 && !isFindingPath)
        {
           
            animator.SetBool("Moving", false);
            return;
        }

        if (Mathf.Abs(movementDirection.z) > 0.01f)
        {
            Move(movementDirection);
        }
        if (agent.hasPath && isFindingPath)
        {
            DrawPath();
        }
        //if (!isFindingPath && Mathf.Abs(movementDirection.z) == 0)
        //{
        var target = Vector3.RotateTowards(gameObject.transform.forward, movementDirection,
                agent.angularSpeed * Time.deltaTime, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(target);
        //}

        //if (agent.hasPath)
        //{

        //}
        //Vector2 input = new Vector2(fixedJoy.Horizontal, fixedJoy.Vertical);

        //if (input.magnitude <= 0 && !isFindingPath)
        //{
        //    animator.SetBool("Moving", false);    
        //}   


        //if (Mathf.Abs(input.y) > 0.01f)
        //{
        //    Move(input);
        //}
        //if (!isFindingPath && Mathf.Abs(input.y) == 0)
        //{
        //    Rotation(input);
        //}

    }

    public void Move(Vector3 input)
    {
        lineRenderer.enabled = false;
        isFindingPath = false;
         animator.SetBool("Moving", true);
        Vector3 destination = transform.position +( input.x  * transform.right + input.z * transform.forward)/7;

        agent.destination = destination;
    }

    public void Rotation(Vector2 input)
    {
        agent.destination = transform.position;
        animator.SetBool("Moving", false);
        transform.Rotate(0, agent.angularSpeed * Time.deltaTime, 0);
    }


    public void Stop()
    {
        movementDirection = Vector3.zero;
        agent.destination = transform.position;
        animator.SetBool("Moving", false);
    }
    
    public void FindToTheTarget(GameObject target)
    {
        isFindingPath = true;
        animator.SetBool("Moving", true);
        Vector3 destination = target.transform.position;
        agent.destination = destination;
    }

    void DrawPath()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = agent.path.corners.Length;
        lineRenderer.SetPosition (0,transform.position );

        if (agent.path.corners.Length<2)
        {
            return;
        }

        for (int i = 1; i<agent.path.corners.Length;i++)
        {
            Vector3 pointPosition = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y, agent.path.corners[i].z);
            lineRenderer.SetPosition(i, pointPosition);
        }
    }
}
