using ElectronicObserver.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ElectronicObserver.Observer.kcsapi.api_start2
{
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
			RootObject rootObject = new RootObject();
			string amees = "";
			string damees = data.api_mst_equip_exslot_ship.ToString(); //元データを文字列に変換
			foreach (var elem in data.api_mst_equip_exslot_ship)
			{
				//api_slotitem_idを作り直す
				JsonElement jelem0 = JsonSerializer.Deserialize<JsonElement>(damees); 
				JsonProperty root0 = jelem0.EnumerateObject().First();
				JsonElement value = root0.Value;
				rootObject.api_slotitem_id = int.Parse(root0.Name);
				string jsons = root0.ToString();
				string shipids = root0.Value.ToString();

				//api_ship_ids取得
				JsonElement jelem1 = JsonSerializer.Deserialize<JsonElement>(shipids);
				JsonProperty root1 = jelem1.EnumerateObject().First();
				JsonElement value1 = root1.Value;
				List<int> list1 = new List<int>();
				string apishipids = root1.Value.ToString();
				if (apishipids != "")
				{
					foreach (JsonProperty jprop in value1.EnumerateObject())
					{
						list1.Add(int.Parse(jprop.Name));
					}
					rootObject.api_ship_ids = list1;
				}
				string stypes = shipids.Replace(root1.ToString() + ",", "");

				//api_stypes取得
				JsonElement jelem2 = JsonSerializer.Deserialize<JsonElement>(stypes);
				JsonProperty root2 = jelem2.EnumerateObject().First();
				JsonElement value2 = root2.Value;
				List<int> list2 = new List<int>();
				string apistypes = root2.Value.ToString();
				if (apistypes != "")
				{
					foreach (JsonProperty jprop in value2.EnumerateObject())
					{
						list2.Add(int.Parse(jprop.Name));
					}
					rootObject.api_stypes = list2;
				}
				string ctypes = stypes.Replace(root2.ToString() + ",", "");

				//api_ctypes取得
				JsonElement jelem3 = JsonSerializer.Deserialize<JsonElement>(ctypes);
				JsonProperty root3 = jelem3.EnumerateObject().First();
				JsonElement value3 = root3.Value;
				List<int> list3 = new List<int>();
				string apictypes = root3.Value.ToString();
				if (apictypes != "")
				{
					foreach (JsonProperty jprop in value3.EnumerateObject())
					{
						list3.Add(int.Parse(jprop.Name));
					}
					rootObject.api_ctypes = list3;
				}
				damees = damees.Replace(jsons + ",", "");

				// 一旦一装備のみでJSONデータをシリアライズで作成
				amees = JsonSerializer.Serialize<RootObject>(rootObject);

				//JSONデータを作成したらrootObjectのデータをnull化する
				if(rootObject.api_ship_ids != null) rootObject.api_ship_ids = null;
				if(rootObject.api_stypes != null) rootObject.api_stypes = null;
				if(rootObject.api_ctypes != null) rootObject.api_ctypes = null;

				//作ったJSONデータをJsonSerializerでDeserializeしてデータを取りだす
				Api_mst_equip_exslot_ship_decode api_mst_equip_exslot_ship_decode = JsonSerializer.Deserialize<Api_mst_equip_exslot_ship_decode>(amees);
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


}
