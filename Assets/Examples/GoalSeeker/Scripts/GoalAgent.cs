using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GoalAgent : Agent
{
    public GameObject goal;
    public GameObject area;
    private float force = 40.0f;
    Rigidbody rb;
    Vector3 startPosition;
    Vector3 goalPosition;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        goalPosition = goal.transform.position;
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        transform.position = startPosition + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        goal.transform.position = goalPosition + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(area.transform.position.x - transform.position.x);
        sensor.AddObservation(area.transform.position.y - transform.position.y);
        sensor.AddObservation(goal.transform.position.x - transform.position.x);
        sensor.AddObservation(goal.transform.position.y - transform.position.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 dir = Vector3.zero;
        dir.x = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        dir.z = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        //rb.AddForce(new Vector3(dir.x * force, 0, dir.z * force));
        rb.AddForce(dir * force);
        SetReward(-0.005f);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("goal"))
        {
            SetReward(1f);
            EndEpisode();
        }
        else if (collision.collider.CompareTag("dead"))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}
