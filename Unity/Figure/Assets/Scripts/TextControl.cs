using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextControl : MonoBehaviour {

	[SerializeField]
	private Text instruction_text;

	void Start () {

	}

	void Update () {
		switch (CutObject.meshState)
		{
			case MeshState.None:
				instruction_text.text = "切断点を選択 - " + (3 - SetCutPoints.cutPoints.Count) + " -";
				break;
				
			case MeshState.Isolation:
				instruction_text.text = "削除対象を選択";
				break;
				
			case MeshState.Cut:
				instruction_text.text = "切断完了";
				break;
				
			default:
				instruction_text.text = "無効な切断点";
				break;
		}
	}
}
