using UnityEngine;

namespace KoganeEditorLib
{
	/// <summary>
	/// Unity から Gist に投稿するエディタ拡張のデフォルト設定を管理するクラス
	/// </summary>
	public sealed class UniGistSettings : ScriptableObject
	{
		//====================================================================================
		// 変数
		//====================================================================================
		public string	m_user		;
		public string	m_password	;
		public bool		m_isPublic	;
	}
}