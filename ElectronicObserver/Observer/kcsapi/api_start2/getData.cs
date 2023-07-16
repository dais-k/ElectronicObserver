using ElectronicObserver.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
					shipbefore.NeedBlueprint = (int)elem.api_drawing_count;
					shipbefore.NeedCatapult = (int)elem.api_catapult_count;
					shipbefore.NeedActionReport = (int)elem.api_report_count;
					shipbefore.NeedAviationMaterial = (int)elem.api_aviation_mat_count;
					shipbefore.NeedArmamentMaterial = elem.api_arms_mat_count() ? (int)elem.api_arms_mat_count : 0;
				}
			}

			
			foreach (var elem in data.api_mst_equip_ship)
			{
				int id = (int)elem.api_ship_id;
				db.MasterShips[id].specialEquippableCategory = (int[])elem.api_equip_type;
			}

			//api_mst_equip_exslot_ship (うんこJSONを変換)
			RootObject rootObject = new();
			string text_api_mst_equip_exslot_ship = data.api_mst_equip_exslot_ship.ToString(); //元データを文字列に変換
			foreach (var elem in data.api_mst_equip_exslot_ship) //元データを使ってループ処理 
			{
				//api_slotitem_idを作り直す
				JsonElement jelem_id = JsonSerializer.Deserialize<JsonElement>(text_api_mst_equip_exslot_ship); //変換した文字列をJsonElement にデシリアライズ
				JsonProperty root_id = jelem_id.EnumerateObject().First(); //最初の要素を取りだす
				rootObject.api_slotitem_id = int.Parse(root_id.Name);
				string text_equip_exslot = root_id.ToString();
				string text_shipids = root_id.Value.ToString();

				//api_ship_ids取得
				JsonElement jelem_ship_ids = JsonSerializer.Deserialize<JsonElement>(text_shipids);
				JsonProperty root_ship_ids = jelem_ship_ids.EnumerateObject().First();
				JsonElement value_ship_ids = root_ship_ids.Value; //foreach処理用のデータ
				List<int> list_ship_ids = new List<int>();
				string text_value_ship_ids = root_ship_ids.Value.ToString(); //データ変換用作業テキスト
				if (text_value_ship_ids != "")
				{
					foreach (JsonProperty jprop in value_ship_ids.EnumerateObject())
					{
						list_ship_ids.Add(int.Parse(jprop.Name));
					}
					rootObject.api_ship_ids = list_ship_ids;
				}
				string text_stypes = text_shipids.Replace(root_ship_ids.ToString() + ",", ""); 

				//api_stypes取得
				JsonElement jelem_stypes = JsonSerializer.Deserialize<JsonElement>(text_stypes);
				JsonProperty root_stypes = jelem_stypes.EnumerateObject().First();
				JsonElement value_stypes = root_stypes.Value;
				List<int> list_stypes = new List<int>();
				string text_value_stypes = root_stypes.Value.ToString();
				if (text_value_stypes != "")
				{
					foreach (JsonProperty jprop in value_stypes.EnumerateObject())
					{
						list_stypes.Add(int.Parse(jprop.Name));
					}
					rootObject.api_stypes = list_stypes;
				}
				string text_ctypes = text_stypes.Replace(root_stypes.ToString() + ",", "");

				//api_ctypes取得
				JsonElement jelem_ctypes = JsonSerializer.Deserialize<JsonElement>(text_ctypes);
				JsonProperty root_ctypes = jelem_ctypes.EnumerateObject().First();
				JsonElement value_ctypes = root_ctypes.Value;
				List<int> list_ctypes = new List<int>();
				string text_value_ctypes = root_ctypes.Value.ToString();
				if (text_value_ctypes != "")
				{
					foreach (JsonProperty jprop in value_ctypes.EnumerateObject())
					{
						list_ctypes.Add(int.Parse(jprop.Name));
					}
					rootObject.api_ctypes = list_ctypes;
				}
				
				text_api_mst_equip_exslot_ship = text_api_mst_equip_exslot_ship.Replace(text_equip_exslot + ",", ""); //大本のテキストから今作業した要素を削除

				// 一旦一装備のみでJSONデータをシリアライズで作成
				string mst_equip_exslot_ship_temp = JsonSerializer.Serialize<RootObject>(rootObject);

				//JSONデータを作成したらrootObjectのデータをnull化する
				if (rootObject.api_ship_ids != null) rootObject.api_ship_ids = null;
				if (rootObject.api_stypes != null) rootObject.api_stypes = null;
				if (rootObject.api_ctypes != null) rootObject.api_ctypes = null;

				//作ったJSONデータをJsonSerializerでDeserializeしてデータを取りだす
				Api_mst_equip_exslot_ship_decode api_mst_equip_exslot_ship_decode = JsonSerializer.Deserialize<Api_mst_equip_exslot_ship_decode>(mst_equip_exslot_ship_temp);
				//装備ID
				int id = api_mst_equip_exslot_ship_decode.api_slotitem_id;
				//搭載可能艦娘個別指定
				if (api_mst_equip_exslot_ship_decode.api_ship_ids is not null)
				{
					foreach (var cnt_ship in api_mst_equip_exslot_ship_decode.api_ship_ids)
					{
						db.MasterEquipments[id].equippableShipsAtExpansion = api_mst_equip_exslot_ship_decode.api_ship_ids;
					}
				}
				//装備可能艦種（駆逐艦、軽巡とか）
				if (api_mst_equip_exslot_ship_decode.api_stypes is not null)
				{
					foreach (var cnt_stype in api_mst_equip_exslot_ship_decode.api_stypes)
					{
						db.MasterEquipments[id].equippableStypeAtExpansion = api_mst_equip_exslot_ship_decode.api_stypes;
					}
				}
				//装備可能艦型（綾波型、秋月型とか）
				if (api_mst_equip_exslot_ship_decode.api_ctypes is not null)
				{ 
					foreach(var cnt_ctype in api_mst_equip_exslot_ship_decode.api_ctypes)
					{
						db.MasterEquipments[id].equippableCtypeAtExpansion = api_mst_equip_exslot_ship_decode.api_ctypes;
					}
				}
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



			Utility.Logger.Add(2, "提督が鎮守府に着任しました。これより艦隊の指揮を執ります。");

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_start2/getData";
	}

	public class RootObject
	{
		public int api_slotitem_id { get; set; }
		public List<int> api_ship_ids { get; set; }
		public List<int> api_stypes { get; set; }
		public List<int> api_ctypes { get; set; }
	}

	public class Api_mst_equip_exslot_ship_decode
	{
		public int api_slotitem_id { get; set; }
		public int[] api_ship_ids { get; set; }
		public int[] api_stypes { get; set; }
		public int[] api_ctypes { get; set; }

		public Api_mst_equip_exslot_ship_decode(int api_slotitem_id, int[] api_ship_ids, int[] api_stypes, int[] api_ctypes)
		{
			this.api_slotitem_id = api_slotitem_id;
			this.api_ship_ids = api_ship_ids;
			this.api_stypes = api_stypes;
			this.api_ctypes = api_ctypes;
		}
	}
}
