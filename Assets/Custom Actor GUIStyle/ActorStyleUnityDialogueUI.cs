using UnityEngine;
using System;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;

public class ActorStyleUnityDialogueUI : UnityDialogueUI {

	[Serializable]
	public class ActorStyle {
		public string actorName;
		public string guiStyleName;
	}

	public ActorStyle[] actorStyles;

	private string originalGuiStyleName = "label";

	public override void Start() {
		originalGuiStyleName = dialogue.npcSubtitle.line.guiStyleName;
		base.Start();
	}

	public override void ShowSubtitle(Subtitle subtitle) {
		dialogue.npcSubtitle.line.guiStyleName = originalGuiStyleName;
		foreach (var actorStyle in actorStyles) {
			if (string.Equals(subtitle.speakerInfo.Name, actorStyle.actorName)) {
				dialogue.npcSubtitle.line.guiStyleName = actorStyle.guiStyleName;
			}
		}
		base.ShowSubtitle(subtitle);
	}

}
