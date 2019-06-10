using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;
using System.Threading;

using StreamTest;
using Google.Protobuf;
using Grpc.Core;

public class StreamMaster : MonoBehaviour
{

    [SerializeField]
    private int Port = 50051;

    byte[] array=new byte [256];

    [SerializeField]
    string _ipaddress = "localhost";

    StreamRelay.StreamRelayClient _client = null;

    Channel _channel = null;
    MasterStream masterStream = new MasterStream();

  
    AsyncClientStreamingCall<MasterStream,Empty> _call;


    void Awake()
    {
        for(int i=0;i<256;i++)
        {
            array[i] = 255;
        }
    }

    void Start()
    {
        _channel = new Channel(_ipaddress + ":" + Port.ToString(),ChannelCredentials.Insecure);

        _client = new StreamRelay.StreamRelayClient(_channel);
        _call = _client.SendMasterStream();
    }

    void Update()
    {
        array[0]++;

        masterStream.Chunc = ByteString.CopyFrom(array);
        
        Task.Run(() =>
        {
            _call.RequestStream.WriteAsync(masterStream);
        }
        );
    }

    private void OnDestroy()
    {
        Task.Run(ShutDownProcess);
    }

    async Task ShutDownProcess()
    {
        await _call.RequestStream.CompleteAsync();
        await _channel.ShutdownAsync();
    }

}
