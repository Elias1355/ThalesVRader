﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryFollow : MonoBehaviour
{
    private TrajectoryScript trajectoryScript;
    private GameObject Trajectory;
    [SerializeField]
    private Transform TrajectoryTransform;
    [SerializeField]
    private float rotateSpeed=10f;

    // t parameter for the bezier curve
    private float tParam;
    
    private Vector3 objectPos;
    private Vector3[] cp;
    [SerializeField]
    private float speed;
    // making sure only 1 coroutine is running
    private bool coroutineAllowed;

    private void Start()
    {        
        Trajectory = GameObject.FindGameObjectWithTag("trajectory");
        TrajectoryTransform = Trajectory.GetComponent<Transform>();
        //speed = Trajectory.GetComponent<TrajectoryScaler>().rocketSpeed;
        tParam = 0f;
        speed = 0.01f;
        coroutineAllowed = true;
        /// Getting each ControlPoint
        cp = new Vector3 [TrajectoryTransform.childCount];       
    }

    private void Update()
    {
        for (int i = 0; i < TrajectoryTransform.childCount; i++)
        {
            cp[i] = TrajectoryTransform.GetChild(i).position;
        }
//speed = Trajectory.GetComponent<TrajectoryScaler>().rocketSpeed;
        if (coroutineAllowed)
        {
            StartCoroutine(FollowTrajectory());
        }
    }

    private IEnumerator FollowTrajectory()
    {
        coroutineAllowed = false;        
        while (tParam <1)
        {
            tParam += Time.deltaTime * speed;
            objectPos = Mathf.Pow(1f - tParam, 3) * cp[0] +
                       3 * Mathf.Pow(1f - tParam, 2) * tParam * cp[1] +
                       3 * (1 - tParam) * Mathf.Pow(tParam, 2) * cp[2] +
                       Mathf.Pow(tParam, 3) * cp[3];
            transform.position = objectPos;
            yield return new WaitForEndOfFrame();
        }
        if(transform.position==objectPos)
        {
            Destroy(this.gameObject);
            StopCoroutine(FollowTrajectory());
            if(this.gameObject.tag!="target")
            {
                Debug.Log("Boom");
            }            
        }
        tParam = 0f;
        coroutineAllowed = true;
    }
}
