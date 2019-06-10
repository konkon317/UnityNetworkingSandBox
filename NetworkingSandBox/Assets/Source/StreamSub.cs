using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using StreamTest;
using Google.Protobuf;
using Grpc.Core;

using System.Threading;
using System.Threading.Tasks;

public class StreamSub: MonoBehaviour
{
    [SerializeField]
    private  int _port = 50051;

    [SerializeField]
    string _name ;

    [SerializeField]
    string _ipAddress = "localhost";

    StreamRelay.StreamRelayClient _client;

    Channel _channel = null;    
        
    Queue< byte[]> _receivedByteArrayQueue=new Queue<byte[]>();

    public byte[] CurrentArray { get { return _currentArray; } }
    byte[] _currentArray = null;

    byte _b;
    public byte B { get { return _b; } }

    private void Awake()
    {
        
    }

    private void Start()
    {
        _channel = new Channel(_ipAddress + ":" + _port.ToString(), ChannelCredentials.Insecure);

        _client = new StreamRelay.StreamRelayClient(_channel);

        Task.Run(StartSubscribeStream);
    }

    private void Update()
    {
        lock(_receivedByteArrayQueue)
        {
            if(_receivedByteArrayQueue.Count>0)
            {
                _currentArray = _receivedByteArrayQueue.Dequeue();
            }
        }
    }

    private async Task StartSubscribeStream()
    {
        using (var _responceStream
             = _client.SubscribeStreaming(new SubscribeReq { Name = _name }))
        {
            while (await _responceStream.ResponseStream.MoveNext())
            {
                var array = _responceStream.ResponseStream.Current.Chunc.ToByteArray();

                lock (_receivedByteArrayQueue)
                {
                    _receivedByteArrayQueue.Enqueue(array);
                }                                
            }
        }
    }

    private void OnDestroy()
    {
       Task.Run(()=> _channel.ShutdownAsync());
    }

}
