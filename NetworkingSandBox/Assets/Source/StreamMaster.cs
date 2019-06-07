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

    private const int Port = 50051;
    byte[] array=new byte [1024];

    [SerializeField]
    string _ipaddress = "";

    StreamRelay.StreamRelayClient _client = null;

    Channel _channel = null;
    MasterStream masterStream = new MasterStream();

  
    AsyncClientStreamingCall<MasterStream,Empty> _call;


    void Awake()
    {
        for(int i=0;i<1024;i++)
        {
            array[i] = 255;
        }
        array[0] = 0;
        
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
        _call.RequestStream.CompleteAsync();
        _channel.ShutdownAsync().Wait();
    }

}
