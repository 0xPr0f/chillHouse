using System.Collections;
using System.Collections.Generic;
using agora_gaming_rtc;
using Photon.Pun;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    string appID = "d164d3d113a14bd291b0b5f2edfb5f83";

    public static VoiceManager instance;
    IRtcEngine rtcEngine;

    private void Start()
    {
        rtcEngine = IRtcEngine.GetEngine(appID);

        rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccess;
        rtcEngine.OnLeaveChannel += OnLeaveChannel;
        rtcEngine.OnError += OnError;
    }
   
    void OnLeaveChannel(RtcStats stats)
    {
        Debug.Log("Left channel with duration" + stats.duration);
    }

    void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("Joined Channel" + channelName);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        rtcEngine.JoinChannel(PhotonNetwork.CurrentRoom.Name,"extras",0);

    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        rtcEngine.LeaveChannel();
    }

    private void OnDestroy()
    {
        IRtcEngine.Destroy();
    }
    void OnError(int error, string msg)
    {
        Debug.LogError("Error with Agora" + msg);
    }
}
