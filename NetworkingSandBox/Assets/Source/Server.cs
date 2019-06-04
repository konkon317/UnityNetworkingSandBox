using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

using System.Text;

public class Server: MonoBehaviour
{

    const short MessageTypeTest= MsgType.Highest+1;
	
	// Update is called once per frame

    class Message:MessageBase
    {
        public byte[] message;
    }

    Message _m = new Message();

    StringBuilder _builder=new StringBuilder();

    byte[] _message;

    [SerializeField]
    string ipaddress="127.0.0.1";
 
    private void Awake()
    {
        NetworkServer.RegisterHandler(MessageTypeTest, networkmessage =>
        {
            var reader = networkmessage.reader;
            int length = reader.ReadByte() + (256 * reader.ReadByte());
            var mes = reader.ReadBytes(reader.Length - (int)reader.Position);

            _builder.Length = 0;
            _builder.Append("Host received : ");
            _builder.Append(length).Append("  : ");
            foreach (var b in mes)
            {
                _builder.Append(b.ToString()).Append(",");
            }
            Debug.Log(_builder);

            for (int i = 0; i < mes.Length; i++)
            {
                mes[i]++;
            }

            _m.message = mes;
            networkmessage.conn.Send(MessageTypeTest, _m);
        }
        );

        NetworkServer.Listen(ipaddress, 7000);

        
    }
   
}
