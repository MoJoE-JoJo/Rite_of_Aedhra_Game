using UnityEngine;
using PixelCrushers.DialogueSystem;
public class SkipTypewriterForPlayer : MonoBehaviour
{
    public void OnConversationLine(Subtitle subtitle)
    {
        if (subtitle.speakerInfo.isPlayer)
        {
            subtitle.formattedText.text = @"\^" + subtitle.formattedText.text;
        }
    }
}