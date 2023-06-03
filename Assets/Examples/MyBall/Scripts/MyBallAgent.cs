using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Linq;

// Agent class object attached to platform
// Platform act as an agent try to keep a ball from falling by rotating itself
public class MyBallAgent : Agent
{
    [SerializeField] private GameObject ball;
    private Rigidbody ballRigidBody;
    private Vector3 ballStartPos;

    public override void Initialize()
    {
        ballRigidBody = ball.GetComponent<Rigidbody>();
        ballStartPos = ball.transform.position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.rotation.z);
        sensor.AddObservation(transform.rotation.x);
        sensor.AddObservation(ball.transform.position - transform.position);
        sensor.AddObservation(ballRigidBody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        gameObject.transform.Rotate(new Vector3(0, 0, 1), actions.ContinuousActions[0]);
        gameObject.transform.Rotate(new Vector3(1, 0, 0), actions.ContinuousActions[1]);

        SetReward(0.1f);

        if ((ball.transform.position.y - transform.position.y) < -2f ||
            Mathf.Abs(ball.transform.position.x - transform.position.x) > 3f ||
            Mathf.Abs(ball.transform.position.z - transform.position.z) > 3f )
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        ballRigidBody.velocity = new Vector3(0f, 0f, 0f);
        ball.transform.position = ballStartPos;
    }
}
