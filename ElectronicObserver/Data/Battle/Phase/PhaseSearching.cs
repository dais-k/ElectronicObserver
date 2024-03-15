using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 索敵フェーズの処理を行います。
	/// </summary>
	public class PhaseSearching : PhaseBase
	{


		public PhaseSearching(BattleData data, string title)
			: base(data, title) { }


		public override bool IsAvailable => RawData.api_search() && RawData.api_formation();

		public override void EmulateBattle(int[] hps, int[] damages)
		{
		}


		/// <summary>
		/// 自軍索敵結果
		/// </summary>
		public int SearchingFriend => !RawData.api_search() ? -1 : (int)RawData.api_search[0];


		/// <summary>
		/// 敵軍索敵結果
		/// </summary>
		public int SearchingEnemy => !RawData.api_search() ? -1 : (int)RawData.api_search[1];


		/// <summary>
		/// 自軍陣形
		/// </summary>
		public int FormationFriend
		{
			get
			{
				dynamic form = RawData.api_formation[0];
				return form is string ? int.Parse((string)form) : (int)form;
			}
		}

		/// <summary>
		/// 敵軍陣形
		/// </summary>
		public int FormationEnemy => (int)RawData.api_formation[1];

		/// <summary>
		/// 交戦形態
		/// </summary>
		public int EngagementForm => (int)RawData.api_formation[2];

		/// <summary>
		/// 煙幕展開
		/// </summary>
		public int? SmokeCount => RawData.api_smoke_type() switch
		{
			true => (int)RawData.api_smoke_type,
			_ => null,
		};
		
		/// <summary>
		/// 気球展開対象セル
		/// </summary>
		public bool IsBalloonCell => RawData.api_balloon_cell() && (int)RawData.api_balloon_cell > 0;

		/// <summary>
		/// 環礁マス
		/// </summary>
		public bool IsAtollCell => RawData.api_atoll_cell() && (int)RawData.api_atoll_cell > 0;
	}
}

