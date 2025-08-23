using System;
using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float angle = 100;
    public float radius = 7;

    public GameObject playerRef;
    public bool canSeePlayer; 

    // create 2 layer: 
    // Target        assign to player
    // Obstructions  assign to walls and etc.

    [Tooltip("Assign the Target layer.")]
    public LayerMask targetMask;
    [Tooltip("Assign the Obstructions layer.")]
    public LayerMask obstructionMask;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
