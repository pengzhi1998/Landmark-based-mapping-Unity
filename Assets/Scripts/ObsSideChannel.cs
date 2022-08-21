using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;

public class ObsSideChannel : SideChannel
{
    //WaterSettings settings = WaterSettings.Default;

    public ObsSideChannel()
    {
        ChannelId = new Guid("621f0a70-4f87-11ea-a6bf-784f4387d1f7");
    }

    protected override void OnMessageReceived(IncomingMessage msg)
    {
        IList<float> Init = msg.ReadFloatList();
        Cylinder cylinder = GameObject.Find("Cylinder").GetComponent<Cylinder>();
        Cube cube = GameObject.Find("Cube").GetComponent<Cube>();
        Capsule capsule = GameObject.Find("Capsule").GetComponent<Capsule>();
        Sphere sphere = GameObject.Find("Sphere").GetComponent<Sphere>();
        Stone stone = GameObject.Find("Stone").GetComponent<Stone>();
        cylinder.transform.position = new Vector3(Init[0], -3.5f, Init[1]);
        cube.transform.position = new Vector3(Init[2], -3.5f, Init[3]);
        capsule.transform.position = new Vector3(Init[4], -3.5f, Init[5]);
        sphere.transform.position = new Vector3(Init[6], -3.5f, Init[7]);
        stone.transform.position = new Vector3(Init[8], -3.5f, Init[9]);
//        if (RobotAgent.testMode)
//        {
//            TestAgent.testStartPos = new Vector3(Init[0], Init[1], Init[2]);
//            TestAgent.testStartOri = new Vector3(Init[3], Init[4], Init[5]);
//
//            RobotAgent.randomGoalX = Init[6];
//            RobotAgent.randomGoalY = Init[7];
//            RobotAgent.randomGoalZ = Init[8];
//
//            WaterSettings.controlVisibility = Init[9];
//        }
//        else
//        {
//            WaterSettings.controlVisibility = Init[9];
//        }
    }

    public void SendObsToPython(float count, float pos_x, float pos_y, float pos_z)
    {
        List<float> msgToSend = new List<float>() {count, pos_x, pos_y, pos_z};
        using (var msgOut = new OutgoingMessage())
        {
            msgOut.WriteFloatList(msgToSend);
            QueueMessageToSend(msgOut);
        }
    }
}