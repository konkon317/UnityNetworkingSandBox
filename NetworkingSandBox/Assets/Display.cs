using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using System.Text;

public class Display : MonoBehaviour
{
    [SerializeField]
    StreamSub _sub;

    [SerializeField]
    Text _text;


    StringBuilder _builder=new StringBuilder();

    void Update()
    {
        if (_sub.CurrentArray != null)
        {
            var array = _sub.CurrentArray;
            _builder.Length = 0;
            foreach (var b in array)
            {
                _builder.Append(b).Append("-");
            }

            _text.text = _builder.ToString();

        }
        else
        {
            _text.text = "no array received";
        }

       
    }

}
