// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Test.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace TestProtoBuffer
{
    public static partial class TestProtoBufferService
    {
        static readonly string __ServiceName = "testProtoBuffer.TestProtoBufferService";

        static readonly grpc::Marshaller<global::TestProtoBuffer.TestRequest> __Marshaller_testProtoBuffer_TestRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TestProtoBuffer.TestRequest.Parser.ParseFrom);
        static readonly grpc::Marshaller<global::TestProtoBuffer.TestReply> __Marshaller_testProtoBuffer_TestReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::TestProtoBuffer.TestReply.Parser.ParseFrom);

        static readonly grpc::Method<global::TestProtoBuffer.TestRequest, global::TestProtoBuffer.TestReply> __Method_TestFunc = new grpc::Method<global::TestProtoBuffer.TestRequest, global::TestProtoBuffer.TestReply>(
            grpc::MethodType.Unary,
            __ServiceName,
            "TestFunc",
            __Marshaller_testProtoBuffer_TestRequest,
            __Marshaller_testProtoBuffer_TestReply);

        /// <summary>Service descriptor</summary>
        public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
        {
            get { return global::TestProtoBuffer.TestReflection.Descriptor.Services[0]; }
        }

        /// <summary>Base class for server-side implementations of TestProtoBufferService</summary>
        [grpc::BindServiceMethod(typeof(TestProtoBufferService), "BindService")]
        public abstract partial class TestProtoBufferServiceBase
        {
            public virtual global::System.Threading.Tasks.Task<global::TestProtoBuffer.TestReply> TestFunc(global::TestProtoBuffer.TestRequest request, grpc::ServerCallContext context)
            {
                throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
            }

        }

        /// <summary>Client for TestProtoBufferService</summary>
        public partial class TestProtoBufferServiceClient : grpc::ClientBase<TestProtoBufferServiceClient>
        {
            /// <summary>Creates a new client for TestProtoBufferService</summary>
            /// <param name="channel">The channel to use to make remote calls.</param>
            public TestProtoBufferServiceClient(grpc::Channel channel) : base(channel)
            {
            }
            /// <summary>Creates a new client for TestProtoBufferService that uses a custom <c>CallInvoker</c>.</summary>
            /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
            public TestProtoBufferServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
            {
            }
            /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
            protected TestProtoBufferServiceClient() : base()
            {
            }
            /// <summary>Protected constructor to allow creation of configured clients.</summary>
            /// <param name="configuration">The client configuration.</param>
            protected TestProtoBufferServiceClient(ClientBaseConfiguration configuration) : base(configuration)
            {
            }

            public virtual global::TestProtoBuffer.TestReply TestFunc(global::TestProtoBuffer.TestRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
            {
                return TestFunc(request, new grpc::CallOptions(headers, deadline, cancellationToken));
            }
            public virtual global::TestProtoBuffer.TestReply TestFunc(global::TestProtoBuffer.TestRequest request, grpc::CallOptions options)
            {
                return CallInvoker.BlockingUnaryCall(__Method_TestFunc, null, options, request);
            }
            public virtual grpc::AsyncUnaryCall<global::TestProtoBuffer.TestReply> TestFuncAsync(global::TestProtoBuffer.TestRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
            {
                return TestFuncAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
            }
            public virtual grpc::AsyncUnaryCall<global::TestProtoBuffer.TestReply> TestFuncAsync(global::TestProtoBuffer.TestRequest request, grpc::CallOptions options)
            {
                return CallInvoker.AsyncUnaryCall(__Method_TestFunc, null, options, request);
            }
            /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
            protected override TestProtoBufferServiceClient NewInstance(ClientBaseConfiguration configuration)
            {
                return new TestProtoBufferServiceClient(configuration);
            }
        }

        /// <summary>Creates service definition that can be registered with a server</summary>
        /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
        public static grpc::ServerServiceDefinition BindService(TestProtoBufferServiceBase serviceImpl)
        {
            return grpc::ServerServiceDefinition.CreateBuilder()
                .AddMethod(__Method_TestFunc, serviceImpl.TestFunc).Build();
        }

        /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
        /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
        /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
        /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
        public static void BindService(grpc::ServiceBinderBase serviceBinder, TestProtoBufferServiceBase serviceImpl)
        {
            serviceBinder.AddMethod(__Method_TestFunc, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::TestProtoBuffer.TestRequest, global::TestProtoBuffer.TestReply>(serviceImpl.TestFunc));
        }

    }
}
#endregion