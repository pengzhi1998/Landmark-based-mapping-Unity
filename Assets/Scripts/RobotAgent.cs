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

    public static float randomGoalX = 0f;
    public static float randomGoalY = 0f;
    public static float randomGoalZ = 0f;

    public static float horizontal_distance = 0f;
    public static float angle_rb_2_g = 0f;

    public static bool testMode;

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
        count = 0;

        m_AgentRb.velocity = Vector3.zero;
        m_AgentRb.angularVelocity = Vector3.zero;

        SetResetParameters();
    }

    // Moves the agent according to the selected action
    public void MoveAgent(float act0, float act1)
    {
        m_AgentRb.transform.position = new Vector3(act0, 2f, act1);
        m_AgentRb.transform.eulerAngles = new Vector3(0f, 270f, 0f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        count += 1;

        // Move the agent using the action.
        var continuous_actions = actionBuffers.ContinuousActions;
        MoveAgent(continuous_actions[0], continuous_actions[1]);

        DisplayObs(m_AgentRb.transform.position);
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
