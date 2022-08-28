using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using Random = UnityEngine.Random;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.SideChannels;
using Unity.MLAgents.Actuators;

public class RobotAgent : Agent
{
    public Rigidbody m_AgentRb;

//    RayPerceptionSensorComponent3D RayInput;
    ObsSideChannel obsSideChannel;

    public Text debugText;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        obsSideChannel = new ObsSideChannel();
        SideChannelManager.RegisterSideChannel(obsSideChannel);
    }

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();

    }

    public void OnDestroy()
    {
        SideChannelManager.UnregisterSideChannel(obsSideChannel);
    }

    public override void OnEpisodeBegin()
    {
        count = -2;

        m_AgentRb.transform.position = new Vector3(0f, 3f, 0f);
        m_AgentRb.transform.eulerAngles = new Vector3(0f, 270f, 0f);

        m_AgentRb.velocity = Vector3.zero;
        m_AgentRb.angularVelocity = Vector3.zero;

        SetResetParameters();

        DisplayObs(m_AgentRb.transform.position);
        obsSideChannel.SendObsToPython(count, m_AgentRb.transform.position[0], m_AgentRb.transform.position[1],
        m_AgentRb.transform.position[2]);
    }

    // Moves the agent according to the selected action
    public void MoveAgent(float act0, float act1)
    {
        m_AgentRb.transform.position = new Vector3(act0, -1f, act1);
        m_AgentRb.transform.eulerAngles = new Vector3(0f, 270f, 0f);

//        var dirToGo = Vector3.zero;
//        var rotateDir = Vector3.zero;
//        dirToGo = transform.forward * 0.03f + transform.up * act0 * 0.00f;
//        rotateDir = -transform.up * 0.0f;
//
//        transform.Rotate(rotateDir, Time.fixedDeltaTime * Math.Abs(0.0f));
//        m_AgentRb.AddForce(dirToGo, ForceMode.VelocityChange);

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        count += 1;

        // Move the agent using the action.
        var continuous_actions = actionBuffers.ContinuousActions;
        MoveAgent(continuous_actions[0], continuous_actions[1]);

        DisplayObs(m_AgentRb.transform.position);
        obsSideChannel.SendObsToPython(count, m_AgentRb.transform.position[0], m_AgentRb.transform.position[1],
        m_AgentRb.transform.position[2]);
    }

    public void DisplayObs(Vector3 pos_rb)
    {
        debugText.text = "Step Num: " + count + ", " +
        "\nCurrent Pos: " + pos_rb[0].ToString("0.00") + ", " + pos_rb[1].ToString("0.00") + ", "
        + pos_rb[2].ToString("0.00");
    }

    void SetResetParameters()
    {

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }
}
