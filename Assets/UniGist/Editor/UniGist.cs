using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KoganeEditorLib
{
	/// <summary>
	/// Unity から Gist に投稿するクラス
	/// </summary>
	public static class UniGist
	{
		//====================================================================================
		// 関数
		//====================================================================================
		/// <summary>
		/// Gist に投稿します
		/// </summary>
		public static IEnumerator Create
		(
			string			user		,
			string			password	,
			string			description	,
			bool			isPublic	,
			string			filename	,
			string			content		,
			Action<string>	onComplete	= null,
			Action<string>	onError		= null
		)
		{
			var encoding		= Encoding.UTF8;
			var s				= string.Format( "{0}:{1}", user, password );
			var inArray			= encoding.GetBytes( s );
			var credentials		= Convert.ToBase64String( inArray );
			var authorization	= string.Format( "Basic {0}", credentials );
			var dataRaw			= CreateDataRaw( description, isPublic, filename, content );
			var data			= encoding.GetBytes( dataRaw );

			var headers = new Dictionary<string, string>
			{
				{ "Authorization"	, authorization	},
				{ "User-Agent"		, "GistSharp"	},
			};

			var www = new WWW( @"https://api.github.com/gists", data, headers );

			yield return www;

			var error = www.error;

			if ( !string.IsNullOrEmpty( error ) )
			{
				if ( onError != null )
				{
					onError( error );
				}
				yield break;
			}

			if ( onComplete != null )
			{
				onComplete( www.text );
			}
		}

		/// <summary>
		/// Gist に投稿するデータを作成して返します
		/// </summary>
		private static string CreateDataRaw
		(
			string	description	,
			bool	isPublic	,
			string	filename	,
			string	content
		)
		{
			var dataRaw = @"{""description"":""" + description + @""","
				+ @"""public"":" + isPublic.ToString().ToLower() + ","
				+ @"""files"":{""" + filename + @""":{"
				+ @"""content"":""" + Escape( content ) + @"""}}}";

			return dataRaw;
		}

		/// <summary>
		/// 指定された文字列をエスケープして返します
		/// </summary>
		private static string Escape( string value )
		{
			return value
				.Replace( @"\", @"\\" )
				.Replace( @"""", @"\""" )
				.Replace( @"
", @"\n" )
				.Replace( "\t", @"\t" )
			;
		}
	}
}