using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CatAgent2D : Agent
{
    public GameObject topB;
    public GameObject bottomB;
    public GameObject leftB;
    public GameObject rightB;

    float force = 10.0f;
    Vector3 catStartPos;
    Rigidbody2D rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        catStartPos = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = new Vector3();
        transform.position = catStartPos;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(topB.transform.position.y - transform.position.y);
        sensor.AddObservation(bottomB.transform.position.y - transform.position.y);
        sensor.AddObservation(leftB.transform.position.x - transform.position.x);
        sensor.AddObservation(rightB.transform.position.x - transform.position.x);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        rb.AddForce(Vector3.up * force * actions.ContinuousActions[0]);
        rb.AddForce(Vector3.down * force * actions.ContinuousActions[1]);
        rb.AddForce(Vector3.left * force * actions.ContinuousActions[2]);
        rb.AddForce(Vector3.right * force * actions.ContinuousActions[3]);

        SetReward(0.1f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        SetReward(-1.0f);
        EndEpisode();
    }
}
