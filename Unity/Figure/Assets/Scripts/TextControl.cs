using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextControl : MonoBehaviour {

	[SerializeField]
	private Text instruction_text;

	void Start () {

	}

	void Update () {
		if (CutObject.meshState == MeshState.None)
		{
			instruction_text.text = "切断点を選択 - " + (3 - SetCutPoints.cutPoints.Count) + " -";

		}
		else if (CutObject.meshState == MeshState.Isolation)
		{
			instruction_text.text = "削除対象を選択";

		}
		else if (CutObject.meshState == MeshState.Cut)
		{
			instruction_text.text = "切断完了";

		}
		else {
			instruction_text.text = "無効な切断点です";

		}
	
	}
}
