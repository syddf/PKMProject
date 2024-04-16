using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatReceiver : ParameterizedSignalReceiver
{
    public ChatAnim PlayerAnim;
    public ChatAnim EnemyAnim;

    public override void Process(SignalWithParams InSignal)
    {
        string Text = InSignal.GetParamValue("MessageText");
        string Trainer = InSignal.GetParamValue("Trainer");
        if(Trainer == "Player")
        {
            PlayerAnim.MessageText = Text;
            PlayerAnim.gameObject.SetActive(true);
            EnemyAnim.gameObject.SetActive(false);
        }
        else if(Trainer == "Enemy")
        {
            EnemyAnim.MessageText = Text;
            EnemyAnim.gameObject.SetActive(true);
            PlayerAnim.gameObject.SetActive(false);
        }
    }
}
