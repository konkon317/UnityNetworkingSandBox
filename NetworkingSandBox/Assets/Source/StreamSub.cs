using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StreamTest;
using Google.Protobuf;
using Grpc.Core;

using System.Threading;
using System.Threading.Tasks;

public class StreamSub: MonoBehaviour
{
    private const int Port = 50051;

    [SerializeField]
    string _ipAddress = "";

    StreamRelay.StreamRelayClient _client;

    Channel _channel = null;    

    private void Awake()
    {
        
    }

    private void Start()
    {
        _channel = new Channel(_ipAddress + ":" + Port.ToString(), ChannelCredentials.Insecure);

        _client = new StreamRelay.StreamRelayClient(_channel);

        Task.Run(StartSubscribeStream);
    }

    private async Task StartSubscribeStream()
    {
        using (var _responceStream
             = _client.SubscribeStreaming(new SubscribeReq { Name = "A" }))
        {
            while (await _responceStream.ResponseStream.MoveNext())
            {
                byte [] b = _responceStream.ResponseStream.Current.Chunc.ToByteArray();

                Debug.Log(b[0]);
            }
        }
    }

    private void OnDestroy()
    {
        _channel.ShutdownAsync();
    }

}
