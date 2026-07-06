using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ElectronicObserver.Utility.Data
{
	/// <summary>
	/// 各種Webツール用フォーマット作成
	/// </summary>
	public static class GenerateDeckBuilderFormat
	{

		/// <summary>
		/// デッキビルダー形式の艦隊データ作成
		/// </summary>
		public static string CreateDeciBuilderData(int areaId, bool[] fleetExportFlags)
		{
			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// 手書き json の悲しみ
			sb.Append(@"{""version"":4,");
			sb.Append(@"""hplv"":" + db.Admiral.Level + ",");
			foreach (var fleet in db.Fleet.Fleets.Values)
			{
				if (fleet == null || fleet.MembersInstance.All(m => m == null)) continue;

				if (fleetExportFlags[fleet.FleetID - 1])
				{
					sb.AppendFormat(@"""f{0}"":{{", fleet.FleetID);

					int shipcount = 1;
					foreach (var ship in fleet.MembersInstance)
					{
						if (ship == null) break;
							sb.AppendFormat(@"""s{0}"":{{""api_id"":{1},""id"":{2},""lv"":{3},""asw"":{4},""luck"":{5},""exa"":{6},""items"":{{",
								shipcount,
								ship.MasterID,
								ship.ShipID,
								ship.Level,
								ship.ASWBase,
								ship.LuckBase,
								ship.IsExpansionSlotAvailable.ToString().ToLower());
						int eqcount = 1;
						foreach (var eq in ship.SlotInstance.Where(eq => eq != null))
						{
							if (eq != null)
								sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2},""mas"":{3}}},", eqcount.ToString(), eq.EquipmentID, eq.Level, eq.AircraftLevel);
							eqcount++;
						}
						if (ship.IsExpansionSlotAvailable && ship.ExpansionSlotInstance != null)
						{
							sb.AppendFormat(@"""ix"":{{""id"":{0},""rf"":{1},""mas"":{2}}}", ship.ExpansionSlotInstance.EquipmentID, ship.ExpansionSlotInstance.Level, ship.ExpansionSlotInstance.AircraftLevel);
						}
						else if (eqcount > 1)
							sb.Remove(sb.Length - 1, 1);        // remove ","
						sb.Append(@"}},");
						shipcount++;
					}

					if (shipcount > 0)
						sb.Remove(sb.Length - 1, 1);        // remove ","
					sb.Append(@"},");
				}
			}

			//基地航空隊
			//Note:mode(出撃/待機などの中隊ごとの状態)は、読み取る側の制御が揃っていないため対応しない。
			//　制空権シミュレータ：modeはitemの前にないといけない、modeがない場合は「待機」になる
			//　作戦室　　　　　　：modeはitemの後にないといけない、modeがない場合は「出撃」になる
			if (areaId != 0)
			{
				int corpsNumber = 1;
				int squadronNumber = 1;
				string corpsJson = "";
				string squadronJson = "";
				foreach (KeyValuePair<int, BaseAirCorpsData> corps in db.BaseAirCorps)
				{
					if (corps.Value.MapAreaID == areaId)
					{
						corpsJson += @"""a" + corpsNumber + @""":{";
						foreach (KeyValuePair<int, BaseAirCorpsSquadron> sq in corps.Value.Squadrons)
						{
							int emid = sq.Value.EquipmentMasterID;
							EquipmentData eq = db.Equipments[emid];
							if (eq != null)
							{
								squadronJson += @"""i" + squadronNumber + @""":{""id"":" + sq.Value.EquipmentID + @",""rf"":" + eq.Level + @",""mas"":" + eq.AircraftLevel + @"},";
								//Console.WriteLine(sq.Value.SquadronID + "," + sq.Value.EquipmentID + "," + eq.Level + "," + eq.AircraftLevel);
							}
							squadronNumber++;
						}
						squadronJson = @"""items"":{" + squadronJson.Trim(',') + "}";

						corpsJson += squadronJson + "},";
						squadronJson = "";
						squadronNumber = 1;
						corpsNumber++;
					}
				}

				corpsJson = corpsJson.Trim(',');
				Console.WriteLine(corpsJson + "}");
				if (corpsJson != "")
				{
					sb.Append(corpsJson + "}");
				}
			}

			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append(@"}");

			Console.WriteLine(sb.ToString());
			return sb.ToString();
		}

		/// <summary>
		/// 全装備リスト（旧艦隊分析ページフォーマット）
		/// <para>https://kancolle-fleetanalysis.firebaseapp.com/はサービス終了閉鎖済み</para> 
		/// <para>以下のサイトで本フォーマットを使用できる</para>
		/// <para>制空権シミュレータ：https://noro6.github.io/kc-web/ </para>
		/// <para>らくらく支援艦隊改：https://kancolle-support-kai.netlify.app/</para>
		/// </summary>
		public static string CreateEquipmentList()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(
				string.Join(",", KCDatabase.Instance.Equipments.Values.Where(eq => eq?.ID >= 0)
				.Select(eq => $"{{\"api_slotitem_id\":{eq.EquipmentID},\"api_level\":{eq.Level}}}")));
			return sb.ToString();
		}


		/// <summary>
		/// 全艦娘リスト（旧艦隊分析ページフォーマット）
		/// <para>https://kancolle-fleetanalysis.firebaseapp.com/ はサービス終了閉鎖済み</para>
		/// <para>制空権シミュレータ：https://noro6.github.io/kc-web/ で本フォーマットを使用できる</para>
		/// </summary>
		public static string CreateAllFleetListWithID()
		{
			var sb = new StringBuilder();

			sb.Append("[");
			foreach (var ship in KCDatabase.Instance.Ships.Values.Where(s => s.IsLocked))
			{
				//現在の進捗計算
				int expProgress = 0;
				if (ExpTable.ShipExp.ContainsKey(ship.Level + 1) && ship.Level != 99)
				{
					double tmpExpProgress = ((double)ExpTable.ShipExp[ship.Level].Next - (double)ship.ExpNext) / (double)ExpTable.ShipExp[ship.Level].Next * 100;
					expProgress = (int)Math.Truncate(tmpExpProgress);
				}
				long[] apiExp = { ship.ExpTotal, (long)ship.ExpNext, (long)expProgress };
				sb.AppendFormat(@"{{""api_id"":{0},""api_ship_id"":{1},""api_lv"":{2},""api_kyouka"":[{3}],""api_exp"":[{4}],""api_slot_ex"":{5},""api_sally_area"":{6}",
					ship.MasterID, ship.ShipID, ship.Level,
					string.Join(",", (int[])ship.RawData.api_kyouka),
					string.Join(",", apiExp),
					ship.ExpansionSlot,
					(ship.SallyArea >= 0 ? ship.SallyArea : 0));
				if (ship.Isonslotmax)
				{
					var maxs = ship.AircraftMax;
					var values = string.Join(",", Enumerable.Range(0, 5).Select(i => i < maxs.Count ? maxs[i].ToString() : "0"));
					sb.AppendFormat(@",""api_onslot_max"":[{0}]", values);
				}
				if (ship.SpItemKind > 0)
				{
					switch (ship.SpItemKind)
					{
						case 1:
							sb.AppendFormat(@",""api_sp_effect_items"": [{{""api_kind"":{0},""api_raig"":{1},""api_souk"":{2}}}]}},", ship.SpItemKind, ship.SpItemRaig, ship.SpItemSouk);
							break;
						case 2:
							sb.AppendFormat(@",""api_sp_effect_items"": [{{""api_kind"":{0},""api_houg"":{1},""api_kaih"":{2}}}]}},", ship.SpItemKind, ship.SpItemHoug, ship.SpItemKaih);
							break;
					}
				}
				else
					sb.AppendFormat("}},");
			}
			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append("]");

			return sb.ToString();
		}

	}
}
