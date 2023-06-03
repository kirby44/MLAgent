using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class BartenderAgent : Agent
{
    private float pitchChange;
    private float yawChange;
    public bool mousePressed;
    public bool back = false;
    private PlayerCam cam;

    public override void Initialize()
    {
        cam = GameObject.FindObjectOfType<PlayerCam>();
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        ///2 coutinuous: yaw, pitch
        ///2 discrete: hold or not, back or not
        pitchChange = actions.ContinuousActions[0];
        yawChange = actions.ContinuousActions[1];
        mousePressed = System.Convert.ToBoolean(actions.DiscreteActions[0]);
        back = System.Convert.ToBoolean(actions.DiscreteActions[1]);

        cam.Rotate(pitchChange, yawChange);

        SetReward(-0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        pitchChange = -Input.GetAxisRaw("Mouse Y");
        yawChange = Input.GetAxisRaw("Mouse X");
        if (Input.GetKey(KeyCode.S)) back = true;
        else back = false;

        continuousActions[0] = pitchChange;
        continuousActions[1] = yawChange;
        discreteActions[0] = System.Convert.ToInt16(Input.GetMouseButton(0));
        discreteActions[1] = System.Convert.ToInt16(back);
    }
}
