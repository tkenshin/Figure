/// <summary>
/// Mesh の状態(カットされたか、されてないか)
/// </summary>

public enum MeshState
{
	/// <summary>
	/// カットされていない。
	/// </summary>
	None,

	/// <summary>
	/// 無効なカットラインだった場合。
	/// </summary>
	Invalid,

	/// <summary>
	/// 分離を行った状態。(切断は行ってわけたが、一方を消していない状態)
	/// </summary>
	Isolation,

	/// <summary>
	/// カットされている。(分離した一方を切断した状態)
	/// </summary>
	Cutted

}