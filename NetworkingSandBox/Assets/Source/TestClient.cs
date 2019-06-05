using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TestProtoBuffer;
using Google.Protobuf;

using Grpc.Core;

using System.Text;

public class TestClient : MonoBehaviour
{
    private const int Port = 50051;

    [SerializeField]
    string _ipAddress = "";

    Channel _channel = null;

    TestProtoBufferService.TestProtoBufferServiceClient _client=null;
    TestRequest _req;

    StringBuilder _builder = new StringBuilder();

    byte[] _byteArray;

    ByteString bytestring = ByteString.Empty;

    void Awake()
    {
       

        _byteArray = new byte[256];
        for(byte i=0;i<=255;i++)
        {
            _byteArray[i] = i;

            if(i==255)
            {
                break;
            }
        }
        bytestring = ByteString.CopyFrom(_byteArray);
        _req = new TestRequest { Name = "test", Chunk = bytestring};

    }

    // Use this for initialization
    void Start()
    {
        _builder.Length = 0;
        _builder.Append(_ipAddress).Append(":").Append(Port.ToString());

        Debug.Log(_builder);

        _channel = new Channel(_builder.ToString(), ChannelCredentials.Insecure);

        _client = new TestProtoBufferService.TestProtoBufferServiceClient(_channel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           

            var name = _client.TestFunc(_req);
            Debug.Log(name);

         
        }
    }

    void OnDestory()
    {
        _channel.ShutdownAsync().Wait();
    }



}
