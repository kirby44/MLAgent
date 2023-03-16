using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform failureTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-6f, 0), 0, Random.Range(0, 6f));
        targetTransform.localPosition = new Vector3(Random.Range(-6f, 0), 0, Random.Range(0, 6f));
        failureTransform.localPosition = new Vector3(Random.Range(-6f, 0), 0, Random.Range(0, 6f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(targetTransform.localPosition);
        //sensor.AddObservation(failureTransform.localPosition);

        ///try relative position from agent to see if agent can avoid failure
        sensor.AddObservation(targetTransform.localPosition - transform.localPosition);
        sensor.AddObservation(failureTransform.localPosition - transform.localPosition);
        ///might need one of absolute position to avoid walls
        sensor.AddObservation(transform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            SetReward(+1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (collision.gameObject.tag == "wall" || collision.gameObject.tag == "failure")
        {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("collide!!!");
    //    if (other.TryGetComponent<Goal>(out Goal goal))
    //    {
    //        SetReward(+1f);
    //        EndEpisode();
    //    }
    //    if (other.TryGetComponent<Wall>(out Wall wall))
    //    {
    //        SetReward(-1f);
    //        EndEpisode();
    //    }
    //}
}
