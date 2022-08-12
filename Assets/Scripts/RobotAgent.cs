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

    RayPerceptionSensorComponent3D RayInput;
    ObsSideChannel obsSideChannel;

    public static float randomGoalX = 0f;
    public static float randomGoalY = 0f;
    public static float randomGoalZ = 0f;

    public static float horizontal_distance = 0f;
    public static float angle_rb_2_g = 0f;

    public static bool testMode;

    public Text debugText;

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
        m_AgentRb.velocity = Vector3.zero;
        m_AgentRb.angularVelocity = Vector3.zero;

        SetResetParameters();
    }

    // Randomize the initial position
    public (Vector3, float, Vector3) GetRandomSpawnPos()
    {
        var randomSpawnPos = Vector3.zero;
        var randomGoal = Vector3.zero;

        float chance_Robot = Random.Range(0f, 1f);
        if (chance_Robot < 0.2f)
        {
            var randomPosX = Random.Range(-1f, 1f);
            var randomPosY = Random.Range(-2f, -1f);
            var randomPosZ = Random.Range(3f, 4.5f);

            float rotationAngle = Random.Range(135f, 225f);

            float chance_Goal = Random.Range(0f, 1f);
            if (chance_Goal < 0.5f)
            {
                randomGoalX = Random.Range(-4f, -3f);
                randomGoalY = Random.Range(-2f, -1f);
                randomGoalZ = Random.Range(-7.5f, -8.5f);
            }
            else
            {
                randomGoalX = Random.Range(3f, 4f);
                randomGoalY = Random.Range(-2f, -1f);
                randomGoalZ = Random.Range(-7.5f, -8.5f);
            }
            randomSpawnPos = new Vector3(randomPosX, randomPosY, randomPosZ);
            randomGoal = new Vector3(randomGoalX, randomGoalY, randomGoalZ);

            return (randomSpawnPos, rotationAngle, randomGoal);
        }

        else if (chance_Robot < 0.4f)
        {
            var randomPosX = Random.Range(-4f, -3f);
            var randomPosY = Random.Range(-2f, -1f);
            var randomPosZ = Random.Range(-7f, -8f);

            float rotationAngle = Random.Range(-45f, 135f);

            randomGoalX = Random.Range(-3.5f, 3.5f);
            randomGoalY = Random.Range(-1.9f, -1.1f);
            randomGoalZ = Random.Range(3.5f, 4f);

            randomSpawnPos = new Vector3(randomPosX, randomPosY, randomPosZ);
            randomGoal = new Vector3(randomGoalX, randomGoalY, randomGoalZ);

            return (randomSpawnPos, rotationAngle, randomGoal);
        }

        else if (chance_Robot < 0.6f)
        {
            var randomPosX = Random.Range(3f, 4f);
            var randomPosY = Random.Range(-2f, -1f);
            var randomPosZ = Random.Range(-7f, -8f);

            float rotationAngle = Random.Range(-135f, 45f);

            randomGoalX = Random.Range(-3.5f, 3.5f);
            randomGoalY = Random.Range(-1.9f, -1.1f);
            randomGoalZ = Random.Range(3.5f, 4f);

            randomSpawnPos = new Vector3(randomPosX, randomPosY, randomPosZ);
            randomGoal = new Vector3(randomGoalX, randomGoalY, randomGoalZ);

            return (randomSpawnPos, rotationAngle, randomGoal);
        }

        else if (chance_Robot < 0.8f)
        {
            var randomPosX = Random.Range(-9.5f, -8.5f);
            var randomPosY = Random.Range(-3f, -2f);
            var randomPosZ = Random.Range(3f, 4f);

            float rotationAngle = Random.Range(-45f, 45f);

            randomGoalX = Random.Range(15f, 16f);
            randomGoalY = Random.Range(-1.5f, -2f);
            randomGoalZ = Random.Range(15f, 16f);
            randomSpawnPos = new Vector3(randomPosX, randomPosY, randomPosZ);
            randomGoal = new Vector3(randomGoalX, randomGoalY, randomGoalZ);

            return (randomSpawnPos, rotationAngle, randomGoal);
        }

        else
        {
            var randomPosX = Random.Range(8.5f, 9.5f);
            var randomPosY = Random.Range(-2f, -1f);
            var randomPosZ = Random.Range(3f, 4f);

            float rotationAngle = Random.Range(-45f, 45f);

            randomGoalX = Random.Range(-16f, -15f);
            randomGoalY = Random.Range(-2.0f, -2.5f);
            randomGoalZ = Random.Range(15f, 16f);

            randomSpawnPos = new Vector3(randomPosX, randomPosY, randomPosZ);
            randomGoal = new Vector3(randomGoalX, randomGoalY, randomGoalZ);

            return (randomSpawnPos, rotationAngle, randomGoal);
        }


    }

    // Moves the agent according to the selected action
    public void MoveAgent(float act0, float act1)
    {
        m_AgentRb.transform.position = new Vector3(act0, m_AgentRb.transform.position[1], act1);
//        Cylinder.transform.position = new Vector3(act0, m_AgentRb.transform.position[1], act1);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Move the agent using the action.
        var continuous_actions = actionBuffers.ContinuousActions;
        MoveAgent(continuous_actions[0], continuous_actions[1]);

        // Penalty given each step to encourage agent to finish task quickly.
//        DisplayObs(m_AgentRb.transform.position, m_AgentRb.transform.eulerAngles[1],
//            new Vector3(randomGoalX, randomGoalY, randomGoalZ));

        AddReward(-1f / MaxStep);
    }

//    public void DisplayObs(Vector3 pos_rb, float rotation, Vector3 pos_goal,
//        float horizontal_distance, float vertical_distance, float angle_rb_2_g)
//    {
//        debugText.text = "Delta Time: " + Time.deltaTime.ToString("0.0000") + ", " +
//        "\nCurrent Time: " + Time.time.ToString("0.0000") + ", " + "\nCurrent Pos: " +
//        pos_rb[0].ToString("0.0000") + ", " + pos_rb[1].ToString("0.0000") + ", "
//        + pos_rb[2].ToString("0.0000") + "\nCurrent Rot: " + rotation.ToString("0.0000") +
//         "\nCurrent Goal: " + pos_goal + "\nhorizontal_distance: " + horizontal_distance +
//        "\nvertical_distance: " + vertical_distance + "\nangle_rb_2_g: " + angle_rb_2_g
//        + "\ntransform:" + transform.up[0].ToString("0.0000") + ", " + transform.up[1].ToString("0.0000")
//        + ", " + transform.up[2].ToString("0.0000");
//    }

    void SetResetParameters()
    {
        // set the haze, fog and attenuation here
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }
}
