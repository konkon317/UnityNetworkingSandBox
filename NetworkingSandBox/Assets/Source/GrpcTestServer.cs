using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grpc.Core;
using System.Text;

using TestProtoBuffer;
using System.Threading.Tasks;

public class GrpcTestServer : MonoBehaviour
{
    private const int Port = 50051;
    [SerializeField]
    string _ipaddress = "localhost";

    Grpc.Core.Server server;
    
    void Awake()
    {
        server = new Grpc.Core.Server
        {
            Services = { TestProtoBufferService.BindService(new TestProtoBufferImpl("testServer")) },
            Ports = {new ServerPort(_ipaddress,Port,ServerCredentials.Insecure)}
        };
        server.Start();
    }

    void OnDestroy()
    {
        server.ShutdownAsync().Wait();
    }


}

public class TestProtoBufferImpl :TestProtoBufferService.TestProtoBufferServiceBase
{
    TestReply _rep=null;
    public TestProtoBufferImpl(string name)
    {
        _rep = new TestReply { Name = name };
    }

    StringBuilder _builder = new StringBuilder();

    public override Task<TestReply> TestFunc(TestRequest request, ServerCallContext context)
    {
        _builder.Length = 0;
        foreach(var b in request.Chunk)
        {
            _builder.Append(b).Append("-");
        }

        Debug.Log(_builder);

        return Task.FromResult(_rep);
    }
}
