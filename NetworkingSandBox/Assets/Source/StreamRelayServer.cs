using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StreamTest;
using Google.Protobuf;
using Grpc.Core;

using System.Threading;
using System.Threading.Tasks;

using System.Linq;

public class StreamRelayServer: MonoBehaviour
{
    private const int Port = 50051;

    [SerializeField]
    string _ipAddress;

    Grpc.Core.Server server;

    void Awake()
    {
        server = new Grpc.Core.Server
        {
            Services = { StreamRelay.BindService(new StreamRelayImpl()) },
            Ports = { new ServerPort(_ipAddress, Port, ServerCredentials.Insecure) }
        };
        
        server.Start();
    }

    void OnDestroy()
    {
        server.ShutdownAsync().Wait();
    }
	
}

public class StreamRelayImpl :StreamRelay.StreamRelayBase
{
    Empty emp = new Empty();

    Dictionary<string, SubStreamoInfo> _responceStreamDic = new Dictionary<string, SubStreamoInfo>();

    System.Text.StringBuilder _builder=new System.Text.StringBuilder();

    class SubStreamoInfo
    {
        public int sendedChunks;
        public bool active;
        public IServerStreamWriter<StreamedSubscribeResponse> responseStream;

        public StreamedSubscribeResponse responceMessage = new StreamedSubscribeResponse();
    }

    public override async Task<Empty> SendMasterStream(IAsyncStreamReader<MasterStream> requestStream, ServerCallContext context)
    {

        while (await requestStream.MoveNext())
        {           
            int chunkLength = requestStream.Current.Chunc.Length;
            var chunk= requestStream.Current.Chunc;
            
            foreach(var key in _responceStreamDic.Keys)
            {
                _responceStreamDic[key].responceMessage.Chunc=ByteString.CopyFrom(chunk.ToByteArray());
                SendToSubscriber(_responceStreamDic[key].responseStream, _responceStreamDic[key].responceMessage) ;

            }
        }

        foreach(var subscribers in _responceStreamDic.Values)
        {
               subscribers.active=false;
               subscribers.responseStream=null;
        }

        return emp;
    }

    public void SendToSubscriber(IServerStreamWriter<StreamedSubscribeResponse> responseStream,StreamedSubscribeResponse res)
    {
        Task.Run(()=>responseStream.WriteAsync(res));
    }

    public override async Task SubscribeStreaming(SubscribeReq request, IServerStreamWriter<StreamedSubscribeResponse> responseStream, ServerCallContext context)
    {
        if (!_responceStreamDic.ContainsKey(request.Name) || _responceStreamDic[request.Name].active == false)
        {
            _responceStreamDic[request.Name] = new SubStreamoInfo();
            _responceStreamDic[request.Name].active = true;
            _responceStreamDic[request.Name].responseStream = responseStream;
        }
        else
        {
            return;
        }
                
        while (_responceStreamDic[request.Name].active)
        {
            await Task.Delay(100);
                      
        }        

        _responceStreamDic[request.Name].responseStream=null;
    }

    
}
