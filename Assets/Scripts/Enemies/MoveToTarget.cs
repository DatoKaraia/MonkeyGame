using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    private NavMeshAgent _agent;
    [SerializeField]
    private List<GameObject> TargetWaypoints = new List<GameObject>();
    private TriggerEvent _trigger;
    private bool PlayerIsFound = false;

    
    public GameObject thePlayer;
    [Header("EndPatrolAtLastPoint makes the ai not loop to the first patrol point when they reach the end")]
    [SerializeField]
    private bool EndPatrolAtLastPoint = false;
    private bool StopPatrolling = false;
    public EnemyHealth EnemyHealth;

    private int _targetIndex = 0;
    void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        EnemyHealth = GetComponentInChildren<EnemyHealth>();
        _agent = GetComponent<NavMeshAgent>();
        _trigger = GetComponentInChildren<TriggerEvent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        ChooseTarget();

    }

    //The searchCone child holds a trigger against player - swaps between chasing and patrolling behaviours.
    private void ChooseTarget()
    {
        PlayerIsFound = _trigger.EventIsTriggered;
        if (PlayerIsFound == true || EnemyHealth.TakenDamage == true)
        {
            Chasing();
        }
        else if (TargetWaypoints.Count != 0 && StopPatrolling == false)
        {
            Patrolling();
        }

    }

    private void Patrolling()
    {

        _agent.destination = TargetWaypoints[_targetIndex].transform.position;



        //if we are not currently pathfind calculating
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            //We have arrived
            Debug.Log("We have arrived", this);

            if (_targetIndex < TargetWaypoints.Count)
            {
                _targetIndex++;
            }
            if (_targetIndex == TargetWaypoints.Count && EndPatrolAtLastPoint == false)
            {
                _targetIndex = 0;
            }
            if (_targetIndex == TargetWaypoints.Count && EndPatrolAtLastPoint == true)
            {
                _targetIndex = _targetIndex - 1;
                StopPatrolling = true; //Stop patrolling at the last waypoint
            }
        }
    }

    private void Chasing()
    {
        _agent.destination = thePlayer.transform.position;

        //This terminates Chasing() if the player exceeds a range treshold. I don't plan to use this
        //if (EnemyHealth.TakenDamage == false)
        //{
        //    var MaxChaseDistance = 10f;
        //    if (Vector3.Distance(_agent.transform.position, thePlayer.transform.position) > MaxChaseDistance)
        //    {
        //        Debug.Log("ChaseEnds");
        //        _trigger.EventIsTriggered = false;
        //        PlayerIsFound = false;
        //    }
        //}

    }
}