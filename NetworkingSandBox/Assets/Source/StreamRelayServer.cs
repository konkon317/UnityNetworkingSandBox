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
    [SerializeField]
    private int _port = 50051;

    [SerializeField]
    string _ipAddress = "localhost";

    Grpc.Core.Server _server;

    StreamRelayImpl _streamRelayImpl;

    void Awake()
    {
        _streamRelayImpl = new StreamRelayImpl();
        _server = new Grpc.Core.Server
        {
            Services = { StreamRelay.BindService(_streamRelayImpl) },
            Ports = { new ServerPort(_ipAddress, _port, ServerCredentials.Insecure) }
        };
        
        _server.Start();
    }

    void OnDestroy()
    {
        _streamRelayImpl.ShutDown();

        Task.Run(ShutDownProcess);
    }

    async Task ShutDownProcess()
    {
        await _server.ShutdownAsync();
    }
	
}

public class StreamRelayImpl :StreamRelay.StreamRelayBase
{
    Empty emp = new Empty();

    Dictionary<string, SubStreamoInfo> _responceStreamDic = new Dictionary<string, SubStreamoInfo>();

    System.Text.StringBuilder _builder=new System.Text.StringBuilder();
    
    bool _serverShutdowned = false;

    
    class SubStreamoInfo
    {          
        public bool _active;
        public IServerStreamWriter<StreamedSubscribeResponse> _responseStream;

        public StreamedSubscribeResponse _responceMessage = new StreamedSubscribeResponse();
    }

    public override async Task<Empty> SendMasterStream(IAsyncStreamReader<MasterStream> requestStream, ServerCallContext context)
    {

        while (await requestStream.MoveNext())
        {           
            int chunkLength = requestStream.Current.Chunc.Length;
            var chunk= requestStream.Current.Chunc;
            
            foreach(var key in _responceStreamDic.Keys)
            {
                if (_responceStreamDic[key]._active == false)
                {
                    continue;
                }

                _responceStreamDic[key]._responceMessage.Chunc=ByteString.CopyFrom(chunk.ToByteArray());
                SendToSubscriber(_responceStreamDic[key]._responseStream, _responceStreamDic[key]._responceMessage) ;
            }
        }

        foreach(var subscribers in _responceStreamDic.Values)
        {
               subscribers._active=false;
               subscribers._responseStream=null;
        }

        return emp;
    }   
       

    public void SendToSubscriber(IServerStreamWriter<StreamedSubscribeResponse> responseStream,StreamedSubscribeResponse res)
    {
        Task.Run(()=>responseStream.WriteAsync(res));
    }

    public void ShutDown()
    {
        _serverShutdowned = true;
    }

    public override async Task SubscribeStreaming(SubscribeReq request, IServerStreamWriter<StreamedSubscribeResponse> responseStream, ServerCallContext context)
    {
        if (!_responceStreamDic.ContainsKey(request.Name) || _responceStreamDic[request.Name]._active == false)
        {
            _responceStreamDic[request.Name] = new SubStreamoInfo();
            _responceStreamDic[request.Name]._active = true;
            _responceStreamDic[request.Name]._responseStream = responseStream;
        }
        else
        {
            return;
        }
                
        while (_responceStreamDic[request.Name]._active && (_serverShutdowned==false))
        {
            await Task.Delay(100);
                      
        }

        _responceStreamDic[request.Name]._active = false;
        _responceStreamDic[request.Name]._responseStream=null;
    }

    
}
