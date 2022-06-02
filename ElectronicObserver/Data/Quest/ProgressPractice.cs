using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest
{

	/// <summary>
	/// 演習任務の進捗を管理します。
	/// </summary>
	[DataContract(Name = "ProgressPractice")]
	public class ProgressPractice : ProgressData
	{

		/// <summary>
		/// 勝利のみカウントする
		/// </summary>
		[DataMember]
		private bool WinOnly { get; set; }

		/// <summary>
		/// 条件を満たす最低ランク
		/// </summary>
		[DataMember]
		private int LowestRank { get; set; }

		/// <summary>
		/// 編成制限があるか
		/// </summary>
		[DataMember]
		private bool IsSpecial { get; set; }

		public ProgressPractice(QuestData quest, int maxCount, bool winOnly)
			: base(quest, maxCount)
		{
			LowestRank = winOnly ? Constants.GetWinRank("B") : Constants.GetWinRank("");
			WinOnly = winOnly;
			IsSpecial = false;
		}

		public ProgressPractice(QuestData quest, int maxCount, string lowestRank, bool isSpecial)
			: base(quest, maxCount)
		{
			LowestRank = Constants.GetWinRank(lowestRank);
			WinOnly = false;
			IsSpecial = isSpecial;
		}

		public void Increment(string rank)
		{
			if (Constants.GetWinRank(rank) < LowestRank) return;

			if (IsSpecial)
			{
				if (!MeetsSpecialRequirements(QuestID)) return;
			}

			//if (WinOnly && Constants.GetWinRank(rank) < Constants.GetWinRank("B"))
			//	return;

			Increment();
		}

		private bool MeetsSpecialRequirements(int questId)
		{
			bool ret = false;
			FleetData fleet = KCDatabase.Instance.Fleet.Fleets.Values
				.FirstOrDefault(f => f.IsInPractice);

			if (fleet == null) return false;

			List<ShipData> ships = fleet.MembersInstance.Where(s => s != null).ToList();

			switch (questId)
			{
				//################################
				//任務IDごとに個別記述(地獄)
				//################################
				case 318:   //|318|月|給糧艦「伊良湖」の支援|編成条件を満たした状態で演習に3回勝利後、達成後旗艦におにぎり2つ装備|編成条件：軽巡2隻|マンスリーだが1日で進捗リセット|
					if(ships.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser) >= 2)
					{
						ret = true;
					}
					break;
				case 330:   //|330|Ｑ|空母機動部隊、演習始め！|演習B勝利以上4|条件：航空母艦旗艦他1隻計2隻以上及び駆逐艦2隻を含む|クォータリーだが1日で進捗リセット
					if ((ships.FirstOrDefault().MasterShip.IsAircraftCarrier) &&
						(ships.Count(s => s.MasterShip.IsAircraftCarrier) >= 2) &&
						(ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2))
					{
						ret = true;
					}
					break;
				case 337:   //|337|Ｑ|「十八駆」演習！|演習S勝利以上3|条件：霞、霰、陽炎、不知火 |クォータリーだが1日で進捗リセット
					if (ships.Count(s =>
					{
						switch (s?.MasterShip?.NameReading)
						{
							case "かすみ":
							case "あられ":
							case "かげろう":
							case "しらぬい":
								return true;
							default:
								return false;
						}
					}) >= 4)
					{
						ret = true;
					}
					break;
				case 339:   //|339|Ｑ|「十九駆」演習！|演習S勝利以上3|条件：磯波、浦波、綾波、敷波|クォータリーだが1日で進捗リセット
					if (ships.Count(s =>
					{
						switch (s?.MasterShip?.NameReading)
						{
							case "いそなみ":
							case "うらなみ":
							case "あやなみ":
							case "しきなみ":
								return true;
							default:
								return false;
						}
					}) >= 4)
					{
						ret = true;
					}
					break;
				case 342:   //|342|Ｑ|小艦艇群演習強化任務|演習A勝利以上4|(駆逐艦/海防艦)3隻+(駆逐艦/海防艦/軽巡級)1隻|クォータリーだが1日で進捗リセット
					if ((ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer || s.MasterShip.ShipType == ShipTypes.Escort) >= 4)
							||
						((ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer || s.MasterShip.ShipType == ShipTypes.Escort)) >= 3
								&&
						 (ships.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser || ships.FirstOrDefault().MasterShip.ShipType == ShipTypes.TrainingCruiser || ships.FirstOrDefault().MasterShip.ShipType == ShipTypes.TorpedoCruiser)) >= 1)
					)
					{
						ret = true;
					}
					break;
				case 345:   //|345|10|演習ティータイム！|演習勝利A以上4|条件：Warspite、Ark Royal、金剛、Nelson、J級駆逐艦から4隻|イヤーリーだが1日で進捗リセット|
					if (ships.Count(s => s.MasterShip.ShipClass == 82) + //J級駆逐艦
						(ships.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "こんごう":
								case "ネルソン":
								case "アークロイヤル":
								case "ウォースパイト":
									return true;
								default:
									return false;
							}
						})) >= 4
					)
					{
						ret = true;
					}
					break;
				case 346:   //|346|10|最精鋭！主力オブ主力、演習開始！|演習A勝利以上4|夕雲改二、巻雲改二、風雲改二、秋雲改二の4隻|イヤーリーだが1日で進捗リセット|
					if (ships.Count(s =>
					{
						switch (s?.MasterShip?.ShipID)
						{
							case 542: //夕雲改二
							case 563: //巻雲改二
							case 564: //風雲改二
							case 648: //秋雲改二
								return true;
							default:
								return false;
						}
					}) >= 4)
					{
						ret = true;
					}
					break;
				case 348:   //|348|２|「精鋭軽巡」演習！|演習A勝利以上4|条件：軽巡級(雷巡を除く)旗艦、旗艦含む軽巡3隻以上、随伴に駆逐艦2隻以上|イヤーリーだが1日で進捗リセット|
					if ((ships.FirstOrDefault().MasterShip.ShipType == ShipTypes.LightCruiser || ships.FirstOrDefault().MasterShip.ShipType == ShipTypes.TrainingCruiser) &&
						(ships.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser || s.MasterShip.ShipType == ShipTypes.TrainingCruiser) >= 3) &&
					    (ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2))
					{
						ret = true;
					}
					break;
				case 350:   //|350|３|精鋭「第七駆逐隊」演習開始！|演習A勝利3|条件：朧、曙、漣、潮|イヤーリーだが1日で進捗リセット
					if (ships.Count(s =>
					{
						switch (s?.MasterShip?.NameReading)
						{
							case "おぼろ":
							case "あけぼの":
							case "さざなみ":
							case "うしお":
								return true;
							default:
								return false;
						}
					}) >= 4)
					{
						ret = true;
					}
					break;
				case 353:   //|353|６|「巡洋艦戦隊」演習！|演習B勝利以上5|条件：重巡or航巡4(旗艦含む)、駆逐2|イヤーリーだが1日で進捗リセット|
					if (
						(ships.FirstOrDefault().MasterShip.ShipType == ShipTypes.HeavyCruiser || ships.FirstOrDefault().MasterShip.ShipType == ShipTypes.AviationCruiser) &&
						(ships.Count(s => s.MasterShip.ShipType == ShipTypes.HeavyCruiser || s.MasterShip.ShipType == ShipTypes.AviationCruiser) >= 4) &&
						(ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2)
					)
					{
						ret = true;
					}
					break;
				case 354:   //|354|７|「改装特設空母」任務部隊演習！|演習S勝利以上4|条件：旗艦がガンビアベイMK2かつフレッチャー級orジョンCバトラー級2隻以上を含む|イヤーリーだが1日で進捗リセット|
					if (ships.FirstOrDefault()?.MasterShip?.ShipID == 707 && 
						ships.Count(s => s?.MasterShip?.ShipClass == 87 || s?.MasterShip?.ShipClass == 91) >= 2
					)
					{
						ret = true;
					}
					break;
				case 355:   //|355|10|精鋭「第十五駆逐隊」第一小隊演習！|演習S勝利以上4|条件：親潮改二、黒潮改二を1番艦、2番艦に配置|イヤーリーだが1日で進捗リセット|
					if ((ships[0].MasterShip.ShipID == 568 && ships[1].MasterShip.ShipID == 670) ||
						(ships[0].MasterShip.ShipID == 670 && ships[1].MasterShip.ShipID == 568)
					)
					{
						ret = true;
					}
					break;
				case 356:   //|356|５|精鋭「第十九駆逐隊」演習！|演習S勝利以上3|条件：磯波改二、浦波改二、綾波改二、敷波改二|イヤーリーだが1日で進捗リセット|
					if (ships.Count(s =>
					{
						switch (s?.MasterShip?.ShipID)
						{
							case 666:
							case 195:
							case 647:
							case 627:
								return true;
							default:
								return false;
						}
					}) >= 4)
					{
						ret = true;
					}
					break;
				default:
					//ここに来たらバグ
					ret = false;
					break;
			}
			return ret;
		}

		public override string GetClearCondition()
		{
			string retStr = "";
			if (LowestRank == 0)
			{
				//勝利条件がない場合
				retStr = "演習" + (WinOnly ? "勝利" : "") + ProgressMax;
			}
			else
			{
				//勝利条件がある場合
				retStr = "演習" + (Constants.GetWinRank(LowestRank)) + "勝利以上" + ProgressMax;
			}

			return retStr;
		}
	}
}
