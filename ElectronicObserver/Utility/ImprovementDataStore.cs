using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ElectronicObserver.Utility
{
	public static class ImprovementDataStore
	{
		private const string RemoteUrl = "https://raw.githubusercontent.com/dais-k/Improvement-itemlist/main/Items.nedb.json";
		private const string LocalPath = "Record/Items.nedb.json";

		public static List<EquipmentDataMaster> Load()
		{
			string jsonLines;

			// まずリモートを試す
			try
			{
				using (var client = new WebClient())
				{
					client.Encoding = Encoding.UTF8;
					jsonLines = client.DownloadString(RemoteUrl);
				}
			}
			catch
			{
				// 失敗したらローカルを参照
				if (!File.Exists(LocalPath))
					throw new FileNotFoundException("Items.nedb.json が見つかりません。");

				jsonLines = File.ReadAllText(LocalPath, Encoding.UTF8);
			}

			var equipments = new List<EquipmentDataMaster>();

			using (var reader = new StringReader(jsonLines))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(line)) continue;

					var doc = JsonDocument.Parse(line);
					var root = doc.RootElement;

					// JSON の id を取得（MasterEquipments と照合するため）
					int id = root.GetProperty("id").GetInt32();

					var eq = new EquipmentDataMaster
					{
						Improvable = root.GetProperty("improvable").GetBoolean()
					};

					foreach (var impElement in root.GetProperty("improvement").EnumerateArray())
					{
						var imp = new EquipmentDataMaster.Improvement
						{
							Upgrade = ParseUpgrade(impElement.GetProperty("upgrade")),
							Req = ParseReq(impElement.GetProperty("req")),
							Resource = ParseResourceBlock(impElement.GetProperty("resource"))
						};
						eq.Improvements.Add(imp);
					}

					// MasterEquipments に存在すれば解析結果を反映する
					try
					{
						var master = KCDatabase.Instance.MasterEquipments[id];
						if (master != null)
						{
							master.Improvable = eq.Improvable;
							master.Improvements = eq.Improvements;
						}
					}
					catch
					{
						// マスターが未ロードの場合などは無視して続行（既存の挙動を壊さない）
					}

					equipments.Add(eq);
				}
			}

			return equipments;
		}

		// --- 以下はパーサ群 ---

		private static List<string> ParseUpgrade(JsonElement element)
		{
			if (element.ValueKind == JsonValueKind.False)
				return new List<string> { "false" };

			if (element.ValueKind == JsonValueKind.Array && element.GetArrayLength() > 0)
			{
				var first = element[0];
				string name;

				if (first.ValueKind == JsonValueKind.Number)
				{
					int equipId = first.GetInt32();
					try
					{
						var master = KCDatabase.Instance.MasterEquipments[equipId];
						if (master != null && !string.IsNullOrEmpty(master.Name))
							name = master.Name;
						else
							name = equipId.ToString();
					}
					catch
					{
						name = equipId.ToString();
					}
				}
				else if (first.ValueKind == JsonValueKind.String)
				{
					name = first.GetString() ?? "invalid";
				}
				else
				{
					name = "invalid";
				}

				return new List<string> { name };
			}

			if (element.ValueKind == JsonValueKind.Number)
			{
				int equipId = element.GetInt32();
				try
				{
					var master = KCDatabase.Instance.MasterEquipments[equipId];
					if (master != null && !string.IsNullOrEmpty(master.Name))
						return new List<string> { master.Name };
				}
				catch { }

				return new List<string> { equipId.ToString() };
			}

			return new List<string> { "invalid" };
		}

		private static EquipmentDataMaster.ReqCondition ParseReq(JsonElement reqArray)
		{
			var result = new EquipmentDataMaster.ReqCondition();
			for (int i = 0; i < 7; i++)
				result.WeekConditions.Add(new List<int> { -1 });

			foreach (var condition in reqArray.EnumerateArray())
			{
				var days = condition[0].EnumerateArray().Select(x => x.GetBoolean()).ToList();

				List<int> ids;
				if (condition[1].ValueKind == JsonValueKind.False)
					ids = new List<int> { 0 };
				else
					ids = condition[1].EnumerateArray().Select(x => x.GetInt32()).ToList();

				for (int i = 0; i < 7; i++)
				{
					if (days[i])
					{
						if (result.WeekConditions[i].Count == 1 && result.WeekConditions[i][0] == -1)
							result.WeekConditions[i].Clear();

						result.WeekConditions[i].AddRange(ids);
					}
				}
			}
			return result;
		}

		private static EquipmentDataMaster.ResourceBlock ParseResourceBlock(JsonElement resourceArray)
		{
			var block = new EquipmentDataMaster.ResourceBlock();
			block.BaseResource = ParseResource(resourceArray[0]);

			for (int i = 1; i < resourceArray.GetArrayLength(); i++)
				block.ExtraResources.Add(ParseResource(resourceArray[i]));

			return block;
		}

		private static EquipmentDataMaster.ResourceEntry ParseResource(JsonElement element)
		{
			var entry = new EquipmentDataMaster.ResourceEntry
			{
				Mat1 = element[0].GetInt32(),
				Mat2 = element[1].GetInt32(),
				Mat3 = element[2].GetInt32(),
				Mat4 = element[3].GetInt32()
			};

			if (element.GetArrayLength() > 4)
			{
				foreach (var itemArray in element[4].EnumerateArray())
				{
					string id;
					if (itemArray[0].ValueKind == JsonValueKind.Null)
						id = "-1";
					else if (itemArray[0].ValueKind == JsonValueKind.Number)
						id = itemArray[0].GetInt32().ToString();
					else if (itemArray[0].ValueKind == JsonValueKind.String)
						id = itemArray[0].GetString();
					else
						id = "invalid";

					int count = itemArray[1].GetInt32();
					entry.Items.Add(new EquipmentDataMaster.ResourceItem { Id = id, NeedCount = count });
				}
			}

			return entry;
		}
	}
}
