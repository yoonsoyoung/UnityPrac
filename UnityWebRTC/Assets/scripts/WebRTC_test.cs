using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class WebRTC_test : MonoBehaviour
{
    [SerializeField] private bool isLocalGameObject;
    public class PlayerInfo
    {
        public float x = 0;
        public float y = 0;
        public float z = 0;
    }
    public static PlayerInfo localPlayer;
    public static PlayerInfo remotePlayer;

    // WebRTC
    [DllImport("__Internal")] private static extern bool IsPlayerConnected(); // �÷��̾� 1�� 2�� ����Ǿ� �ִ��� Ȯ��
    [DllImport("__Internal")] private static extern bool IsPlayerLocal(); // �÷��̾ �������� Ȯ��
    [DllImport("__Internal")] private static extern string GetLocalPlayerInfo(); // ���� ����(��) ���� 
    [DllImport("__Internal")] private static extern string GetRemotePlayerInfo(); // ��� ���� ����
    [DllImport("__Internal")] private static extern void SendLocalToRemote(string playerInfo); // �÷��̾� ������ ���÷κ��� ���
    [DllImport("__Internal")] private static extern void SendRemoteToLocal(string playerInfo); // �÷��̾� ������ ���κ��� ��

    // Start is called before the first frame update
    void Start()
    {
        localPlayer = new PlayerInfo();
        remotePlayer = new PlayerInfo();
    }

    // Update is called once per frame
    void Update()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        if(!IsPlayerConnected()) return;

        // local player sends info
        if (IsPlayerLocal() && isLocalGameObject)
        {
            localPlayer.x = transform.position.x;
            localPlayer.y = transform.position.y;
            localPlayer.z = transform.position.z;
            SendLocalToRemote(JsonUtility.ToJson(localPlayer));
        }

        // remote player sends info
        if(!IsPlayerLocal() && isLocalGameObject)
        {
            remotePlayer.x = transform.position.x;
            remotePlayer.y = transform.position.y;
            remotePlayer.z = transform.position.z;
            SendRemoteToLocal(JsonUtility.ToJson(remotePlayer));
        }

        // local player can see remote
        if(IsPlayerLocal() && !isLocalGameObject)
        {
            PlayerInfo remotePlayer = JsonUtility.FromJson<PlayerInfo>(GetRemotePlayerInfo());
            Vector3 position = new Vector3(remotePlayer.x, remotePlayer.y, remotePlayer.z);
            transform.position = position;
        }
        // remote player can see local
        if(IsPlayerLocal() && !isLocalGameObject)
        {
            PlayerInfo remotePlayer = JsonUtility.FromJson<PlayerInfo>(GetLocalPlayerInfo());
            Vector3 position = new Vector3(localPlayer.x, localPlayer.y, localPlayer.z);
            transform.position = position;
        }

        #endif
    }
    
}
