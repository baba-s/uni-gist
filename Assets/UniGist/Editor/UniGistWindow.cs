using System.IO;
using UnityEditor;
using UnityEngine;
using UnityExtensions;

namespace KoganeEditorLib
{
	/// <summary>
	/// Unity から Gist に投稿するエディタ拡張
	/// </summary>
	public sealed class UniGistWindow : EditorWindow
	{
		//====================================================================================
		// クラス
		//====================================================================================
		public sealed class ResultData
		{
			public string html_url = string.Empty;
		}

		//====================================================================================
		// 定数
		//====================================================================================
		private const string	ITEM_NAME	= "Assets/New gist";
		private const int		PRIORITY	= 9999;

		private static Vector2 SIZE = new Vector2( 380, 182 );

		//====================================================================================
		// 変数
		//====================================================================================
		private string	m_path			;
		private string	m_user			;
		private string	m_password		;
		private string	m_description	;
		private bool	m_isPublic		;
		private string	m_filename		;
		private bool	m_isStart		;
		private bool	m_isComplete	;
		private bool	m_isError		;
		private string	m_error			;
		private string	m_url			;
		private string	m_dir			;

		//====================================================================================
		// 関数
		//====================================================================================
		/// <summary>
		/// Gist に投稿できるかどうかを確認します
		/// </summary>
		[MenuItem( ITEM_NAME, true )]
		private static bool CanOpen()
		{
			var objects	= Selection.objects;
			var length	= objects.Length;

			if ( length <= 0 || 1 < length ) return false;

			var activeObject	= Selection.activeObject;
			var isTextAsset		= activeObject is TextAsset;
			var isMonoScript	= activeObject is MonoScript;

			return isTextAsset || isMonoScript;
		}

		/// <summary>
		/// Gist に投稿するためのダイアログを表示します
		/// </summary>
		[MenuItem( ITEM_NAME, priority = PRIORITY )]
		private static void Open()
		{
			var win = GetWindow<UniGistWindow>( true, "UniGist" );
			win.Init( Selection.activeObject );
		}

		/// <summary>
		/// 初期化します
		/// </summary>
		private void Init( Object target )
		{
			m_path		= AssetDatabase.GetAssetPath( target );
			m_filename	= Path.GetFileName( m_path );
			minSize		= SIZE;
			maxSize		= SIZE;

			position.Set( position.x, position.y, SIZE.x, SIZE.y );

			var settingPath	= m_dir + "/UniGistSettings.asset";
			var settings	= AssetDatabase.LoadAssetAtPath<UniGistSettings>( settingPath );

			if ( settings == null ) return;

			m_user			= settings.m_user		;
			m_password		= settings.m_password	;
			m_isPublic		= settings.m_isPublic	;
			m_description	= null	;
			m_isStart		= false	;
			m_isComplete	= false	;
			m_isError		= false	;
			m_error			= null	;
			m_url			= null	;
		}

		/// <summary>
		/// GUI を表示します
		/// </summary>
		private void OnGUI()
		{
			m_user			= EditorGUILayout.TextField( "User", m_user );
			m_password		= EditorGUILayout.TextField( "Password", m_password );
			m_description	= EditorGUILayout.TextField( "Description", m_description );
			m_isPublic		= EditorGUILayout.Toggle( "Public", m_isPublic );
			m_filename		= EditorGUILayout.TextField( "Filename", m_filename );

			var canPost =
				!string.IsNullOrEmpty( m_user		) &&
				!string.IsNullOrEmpty( m_password	) &&
				!string.IsNullOrEmpty( m_filename	)
			;

			GUI.enabled = canPost;

			if ( GUILayout.Button( "New gist" ) )
			{
				Post();
			}

			GUI.enabled = true;

			EditorGUILayout.Space();

			if ( m_isComplete )
			{
				EditorGUILayout.LabelField( "Done!" );
				EditorGUILayout.LabelField( m_url );

				EditorGUILayout.BeginHorizontal();

				if ( GUILayout.Button( "Copy URL" ) )
				{
					EditorGUIUtility.systemCopyBuffer = m_url;
				}
				if ( GUILayout.Button( "Open URL" ) )
				{
					Application.OpenURL( m_url );
				}

				EditorGUILayout.EndHorizontal();
			}
			else if ( m_isError )
			{
				EditorGUILayout.LabelField( "Error", m_error );
			}
			else if ( m_isStart )
			{
				EditorGUILayout.LabelField( "Posting..." );
			}
		}

		/// <summary>
		/// Gist に投稿します
		/// </summary>
		private void Post()
		{
			m_isStart		= true	;
			m_isComplete	= false	;
			m_isError		= false	;
			m_error			= null	;
			m_url			= null	;

			if ( !File.Exists( m_path ) )
			{
				m_isError	= true;
				m_error		= "File Not Found";
				return;
			}

			var sr = new StreamReader( m_path );
			var content = sr.ReadToEnd();
			sr.Close();

			var coroutine = UniGist.Create
			(
				user		: m_user		,
				password	: m_password	,
				description	: m_description	,
				isPublic	: m_isPublic	,
				filename	: m_filename	,
				content		: content		,
				onComplete	: json =>
				{
					var resultData =
						JsonUtility.FromJson<ResultData>( json ) ??
						new ResultData()
					;

					m_isComplete	= true;
					m_url			= resultData.html_url;

					Repaint();
				},
				onError		: error =>
				{
					m_isError	= true;
					m_error		= error;

					Repaint();
				}
			);

			EditorCoroutine.Start( coroutine );
		}

		/// <summary>
		/// 有効になった時に呼び出されます
		/// </summary>
		private void OnEnable()
		{
			var mono = MonoScript.FromScriptableObject( this );
			var path = AssetDatabase.GetAssetPath( mono );
			m_dir = Path.GetDirectoryName( path );
		}
	}
}