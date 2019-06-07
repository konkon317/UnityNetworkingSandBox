using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Display : MonoBehaviour
{
    [SerializeField]
    StreamSub _sub;

    [SerializeField]
    Text _text;


    void Update()
    {
        _text.text = _sub.B.ToString();
    }

}
