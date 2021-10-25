using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.WebRTC;

public class UnityWebRTC : MonoBehaviour
{
    private void Awake()
    {
        // Initialize WebRTC
        WebRTC.Initialize();

        // create local peer
        var localConnection = new RTCPeerConnection();
        var sendChannel = localConnection.CreateDataChannel("sendChannel"); // binary data transmission
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
