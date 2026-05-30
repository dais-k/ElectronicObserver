using ElectronicObserver.Data;
using ElectronicObserver.Utility;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Observer.kcsapi.api_start2
{
	public class getData : APIBase
	{


		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;


			//特別置換処理
			data.api_mst_stype[7].api_name = "巡洋戦艦";


			//api_mst_ship
			foreach (var elem in data.api_mst_ship)
			{

				int id = (int)elem.api_id;
				if (db.MasterShips[id] == null)
				{
					var ship = new ShipDataMaster();
					ship.LoadFromResponse(APIName, elem);
					db.MasterShips.Add(ship);
				}
				else
				{
					db.MasterShips[id].LoadFromResponse(APIName, elem);
				}
			}

			//改装関連のデータ設定
			foreach (var ship in db.MasterShips)
			{
				int remodelID = ship.Value.RemodelAfterShipID;
				if (remodelID != 0)
				{
					db.MasterShips[remodelID].RemodelBeforeShipID = ship.Key;
				}
			}


			//api_mst_slotitem_equiptype
			foreach (var elem in data.api_mst_slotitem_equiptype)
			{

				int id = (int)elem.api_id;
				if (db.EquipmentTypes[id] == null)
				{
					var eqt = new EquipmentType();
					eqt.LoadFromResponse(APIName, elem);
					db.EquipmentTypes.Add(eqt);
				}
				else
				{
					db.EquipmentTypes[id].LoadFromResponse(APIName, elem);
				}
			}


			//api_mst_stype
			foreach (var elem in data.api_mst_stype)
			{

				int id = (int)elem.api_id;
				if (db.ShipTypes[id] == null)
				{
					var spt = new ShipType();
					spt.LoadFromResponse(APIName, elem);
					db.ShipTypes.Add(spt);
				}
				else
				{
					db.ShipTypes[id].LoadFromResponse(APIName, elem);
				}
			}


			//api_mst_slotitem
			foreach (var elem in data.api_mst_slotitem)
			{

				int id = (int)elem.api_id;
				if (db.MasterEquipments[id] == null)
				{
					var eq = new EquipmentDataMaster();
					eq.LoadFromResponse(APIName, elem);
					db.MasterEquipments.Add(eq);
				}
				else
				{
					db.MasterEquipments[id].LoadFromResponse(APIName, elem);
				}
			}


			//api_mst_useitem
			foreach (var elem in data.api_mst_useitem)
			{

				int id = (int)elem.api_id;
				if (db.MasterUseItems[id] == null)
				{
					var item = new UseItemMaster();
					item.LoadFromResponse(APIName, elem);
					db.MasterUseItems.Add(item);
				}
				else
				{
					db.MasterUseItems[id].LoadFromResponse(APIName, elem);
				}
			}

			//api_mst_maparea
			foreach (var elem in data.api_mst_maparea)
			{
				int id = (int)elem.api_id;
				if (db.MapArea[id] == null)
				{
					var item = new MapAreaData();
					item.LoadFromResponse(APIName, elem);
					db.MapArea.Add(item);
				}
				else
				{
					db.MapArea[id].LoadFromResponse(APIName, elem);
				}
			}

			//api_mst_mapinfo
			foreach (var elem in data.api_mst_mapinfo)
			{

				int id = (int)elem.api_id;
				if (db.MapInfo[id] == null)
				{
					var item = new MapInfoData();
					item.LoadFromResponse(APIName, elem);
					db.MapInfo.Add(item);
				}
				else
				{
					db.MapInfo[id].LoadFromResponse(APIName, elem);
				}
			}


			//api_mst_mission
			foreach (var elem in data.api_mst_mission)
			{

				int id = (int)elem.api_id;
				if (db.Mission[id] == null)
				{
					var item = new MissionData();
					item.LoadFromResponse(APIName, elem);
					db.Mission.Add(item);
				}
				else
				{
					db.Mission[id].LoadFromResponse(APIName, elem);
				}

			}


			//api_mst_shipupgrade
			Dictionary<int, int> upgradeLevels = new Dictionary<int, int>();
			foreach (var elem in data.api_mst_shipupgrade)
			{
				int idbefore = (int)elem.api_current_ship_id;
				int idafter = (int)elem.api_id;
				var shipbefore = db.MasterShips[idbefore];
				var shipafter = db.MasterShips[idafter];
				int level = (int)elem.api_upgrade_level;

				if (upgradeLevels.ContainsKey(idafter))
				{
					if (level < upgradeLevels[idafter])
					{
						shipafter.RemodelBeforeShipID = idbefore;
						upgradeLevels[idafter] = level;
					}
				}
				else
				{
					shipafter.RemodelBeforeShipID = idbefore;
					upgradeLevels.Add(idafter, level);
				}

				if (shipbefore != null)
				{
					shipbefore.NeedBlueprint = (int)elem.api_drawing_count; // 改装設計図
					shipbefore.NeedCatapult = (int)elem.api_catapult_count; // 試製甲板カタパルト
					shipbefore.NeedActionReport = (int)elem.api_report_count; // 戦闘詳報
					shipbefore.NeedAviationMaterial = (int)elem.api_aviation_mat_count; // 新型航空兵装資材
					shipbefore.NeedArmamentMaterial = elem.api_arms_mat_count() ? (int)elem.api_arms_mat_count : 0; // 新型兵装資材
					shipbefore.NeedLatestTechnology = elem.api_tech_count() ? (int)elem.api_tech_count : 0; // 海外艦最新技術

				}
			}


			//api_mst_equip_ship (うんこJSON2を変換)
			var api_mst_equip_ship_Text = data.api_mst_equip_ship.ToString(); // JSON文字列を取得
			using JsonDocument doc = JsonDocument.Parse(api_mst_equip_ship_Text);
			var equipShips = doc.RootElement;

			foreach (var shipEntry in equipShips.EnumerateObject())
			{
				int shipId = int.Parse(shipEntry.Name);
				var equipTypes = shipEntry.Value.GetProperty("api_equip_type");

				var categoryList = new List<int>();
				var equipIdList = new List<int>();

				foreach (var category in equipTypes.EnumerateObject())
				{
					int categoryId = int.Parse(category.Name);
					categoryList.Add(categoryId);

					// [ID]の配列の場合のみ、装備IDを追加
					if (category.Value.ValueKind == JsonValueKind.Array)
					{
						foreach (var equipId in category.Value.EnumerateArray())
						{
							equipIdList.Add(equipId.GetInt32());
						}
					}
				}

				db.MasterShips[shipId].specialEquippableCategory = categoryList.ToArray();
				db.MasterShips[shipId].specialEquippableId = equipIdList.ToArray();
			}


			//api_mst_equip_exslot_ship (うんこJSONを変換)
			string api_mst_equip_exslot_ship_Text = data.api_mst_equip_exslot_ship.ToString();
			var equipDataDict = JsonSerializer.Deserialize<Dictionary<int, EquipExslotData>>(api_mst_equip_exslot_ship_Text);
			foreach (var kvp in equipDataDict)
			{
				int slotitemId = kvp.Key;
				var equipData = kvp.Value;

				var masterEq = db.MasterEquipments[slotitemId];

				// 個別艦娘ID
				if (equipData.Api_ship_ids != null)
				{
					masterEq.equippableShipsAtExpansion = equipData.Api_ship_ids.Keys.ToArray();
				}

				// 艦種
				if (equipData.Api_stypes != null)
				{
					masterEq.equippableStypeAtExpansion = equipData.Api_stypes.Keys.ToArray();
				}

				// 艦型
				if (equipData.Api_ctypes != null)
				{
					// 精鋭水雷戦隊 司令部の夕雲型特別対応
					IEnumerable<int> ctypes = equipData.Api_ctypes.Keys;
					if (slotitemId == 413 && equipData.Api_ctypes.ContainsKey(38))
					{
						ctypes = ctypes.Where(id => id != 38);
						var specialShips = new[] { 542, 543, 649, 743, 982, 1033 };
						masterEq.equippableShipsAtExpansion = masterEq.equippableShipsAtExpansion.Concat(specialShips).Distinct().ToArray();
					}

					masterEq.equippableCtypeAtExpansion = ctypes.ToArray();
				}

				// 改修レベル
				masterEq.equippableRequestLevel = equipData.Api_req_level;
			}


			//api_mst_shipgraph
			foreach (var elem in data.api_mst_shipgraph)
			{

				int id = (int)elem.api_id;
				if (db.ShipGraphics[id] == null)
				{
					var sgd = new ShipGraphicData();
					sgd.LoadFromResponse(APIName, elem);
					db.ShipGraphics.Add(sgd);
				}
				else
				{
					db.ShipGraphics[id].LoadFromResponse(APIName, elem);
				}
			}


			// Items.nedb.json の読み込み
			try
			{
				ImprovementDataStore.Load();
			}
			catch
			{
				// 読み込み失敗でも既存の挙動に影響させない
			}

			Utility.Logger.Add(2, "提督が鎮守府に着任しました。これより艦隊の指揮を執ります。");

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_start2/getData";
	}

#nullable enable
	public class EquipExslotData
	{
		[JsonPropertyName("api_ship_ids")] 
		public Dictionary<int, int>? Api_ship_ids { get; set; }
		
		[JsonPropertyName("api_stypes")]
		public Dictionary<int, int>? Api_stypes { get; set; }
		
		[JsonPropertyName("api_ctypes")] 
		public Dictionary<int, int>? Api_ctypes { get; set; }

		[JsonPropertyName("api_req_level")]
		public int Api_req_level { get; set; }
	}
#nullable disable

}
