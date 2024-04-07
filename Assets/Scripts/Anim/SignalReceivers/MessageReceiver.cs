using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageReceiver : ParameterizedSignalReceiver
{

    public override void Process(SignalWithParams InSignal)
    {
        string Text = InSignal.GetParamValue("MessageText");
        MessageAnim MessageScript = this.gameObject.GetComponent<MessageAnim>();
        MessageScript.MessageText = Text;
    }
}
