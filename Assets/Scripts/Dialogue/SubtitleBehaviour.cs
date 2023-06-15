using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class SubtitleBehaviour : PlayableBehaviour
{
    public string subtitleText;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TMP_Text text = playerData as TMP_Text;
        text.text = subtitleText;
        text.color = new Color(1,1,1,info.weight);
    }
}
