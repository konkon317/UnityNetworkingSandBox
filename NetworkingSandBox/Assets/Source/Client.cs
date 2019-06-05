using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using System.Text;

public class Client : MonoBehaviour
{

    const short _messageTypeTest = MsgType.Highest + 1;

    // Update is called once per frame

    class Message : MessageBase
    {
        public byte[] message;
    }

    Message _m = new Message();

    StringBuilder _builder = new StringBuilder();

    byte[] _message;
    NetworkClient _client = new NetworkClient();
    bool _clientConnected = false;

    [SerializeField]
    string _ipaddress= "127.0.0.1";

    // Use this for initialization
    void Start ()
    {
        _message = new byte[16];
        for (byte i = 0; i < 16; i++)
        {
            _message[i] = (byte)(15 + i);
        }

        _client.RegisterHandler(_messageTypeTest, networkMessage =>
        {
            var reader = networkMessage.reader;
            int length = reader.ReadByte() + (256 * reader.ReadByte());
            var mes = networkMessage.reader.ReadBytes(reader.Length - (int)reader.Position);

            _builder.Length = 0;
            _builder.Append("client received : ");
            _builder.Append(length).Append("  : ");
            foreach (var b in mes)
            {
                _builder.Append(b.ToString()).Append(",");
            }
            Debug.Log(_builder);

            _message = mes;

        });

        _client.RegisterHandler(MsgType.Connect, _ => { _clientConnected = true; });
        _client.RegisterHandler(MsgType.Disconnect, _ => { _clientConnected = false; });
        
        _client.Connect(_ipaddress, 7000);
    }

    void Update()
    {
        if (_clientConnected)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _m.message = _message;
                _client.Send(_messageTypeTest, _m);
            }
        }

    }
}
