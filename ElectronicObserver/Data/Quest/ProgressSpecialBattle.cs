using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicObserver.Data.Quest
{
	[DataContract(Name = "ProgressSpecialBattle")]
	public class ProgressSpecialBattle : ProgressBattle
	{
		/// <summary>
		/// 対象となるゲージ本数（-1は任意のゲージ）
		/// </summary>
		[DataMember]
		private int GaugeIndex = -1;

		public ProgressSpecialBattle(QuestData quest, int maxCount, string lowestRank, int[] targetArea, bool isBossOnly) : base(quest, maxCount, lowestRank, targetArea, isBossOnly)
		{
		}

		public ProgressSpecialBattle(QuestData quest, int maxCount, string lowestRank, int[] targetArea, bool isBossOnly, int gaugeIndex) : base(quest, maxCount, lowestRank, targetArea, isBossOnly)
		{
			GaugeIndex = gaugeIndex;
		}


		public override void Increment(string rank, int areaID, bool isBoss)
		{
			// 邪悪
			var Empty = (ShipTypes)(-1);

			// 邪悪
			var bm = KCDatabase.Instance.Battle;

			var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

			if (fleet == null)
			{
				// 出撃中ではない - たぶん UI 操作経由のコール?
				base.Increment(rank, areaID, isBoss);
				return;
			}

			var members = fleet.MembersWithoutEscaped;
			var memberstype = members.Select(s => s?.MasterShip?.ShipType ?? Empty).ToArray();

			string[] membernames;
			bool[] fleetmember;

			bool isAccepted = false;

			switch (QuestID)
			{
				//============================ 200～299 ============================
				// |249|月|「第五戦隊」出撃せよ！|2-5ボスS勝利1|要「那智」「妙高」「羽黒」
				case 249:
					{
						bool nachi = false, myoukou = false, haguro = false;
						foreach (var ship in members)
						{
							switch (ship?.MasterShip?.NameReading)
							{
								case "なち":
									nachi = true;
									break;
								case "みょうこう":
									myoukou = true;
									break;
								case "はぐろ":
									haguro = true;
									break;
							}
						}
						isAccepted = nachi && myoukou && haguro;
					}
					break;
				// |257|月|「水雷戦隊」南西へ！|1-4ボスS勝利1|要軽巡旗艦、軽巡3隻まで、他駆逐艦　他艦種禁止
				case 257:
					isAccepted =
						memberstype[0] == ShipTypes.LightCruiser &&
						memberstype.Count(t => t == ShipTypes.LightCruiser) <= 3 &&
						memberstype.All(t => t == ShipTypes.Destroyer || t == ShipTypes.LightCruiser || t == Empty);
					break;
				// |259|月|「水上打撃部隊」南方へ！|5-1ボスS勝利1|要(大和型or長門型or伊勢型or扶桑型)3/軽巡1　巡戦禁止、戦艦追加禁止
				case 259:
					{
						int battleships = 0;
						bool hasLightCruiser = false;
						foreach (var ship in members)
						{
							switch (ship?.MasterShip?.ShipType)
							{
								case ShipTypes.Battleship:
								case ShipTypes.AviationBattleship:
									switch (ship?.MasterShip?.ShipClass)
									{
										case 2:     // 伊勢型
										case 19:    // 長門型
										case 26:    // 扶桑型
										case 37:    // 大和型
											battleships++;
											break;
										default:
											battleships = -9999;
											break;
									}
									break;

								case ShipTypes.Battlecruiser:
									//大和改二のみ例外（艦種優先でカウント対象）
									if(ship?.MasterShip?.ShipID == 911)
									{
										battleships++;
										break;
									}
									else
									{
										battleships = -9999;
										break;
									}

								case ShipTypes.LightCruiser:
									hasLightCruiser = true;
									break;
							}
						}
						isAccepted = battleships >= 3 && hasLightCruiser;
					}
					break;
				// |264|月|「空母機動部隊」西へ！|4-2ボスS勝利1|要(空母or軽母or装母)2/駆逐2
				case 264:
					isAccepted =
						memberstype.Count(t => t == ShipTypes.AircraftCarrier || t == ShipTypes.LightAircraftCarrier || t == ShipTypes.ArmoredAircraftCarrier) >= 2 &&
						memberstype.Count(t => t == ShipTypes.Destroyer) >= 2;
					break;
				// |266|月|「水上反撃部隊」突入せよ！|2-5ボスS勝利1|要駆逐旗艦、重巡1軽巡1駆逐4
				case 266:
					isAccepted =
						memberstype[0] == ShipTypes.Destroyer &&
						memberstype.Count(t => t == ShipTypes.HeavyCruiser) == 1 &&
						memberstype.Count(t => t == ShipTypes.LightCruiser) == 1 &&
						memberstype.Count(t => t == ShipTypes.Destroyer) == 4;
					break;
				case 280:   // |280|月|兵站線確保！海上警備を強化実施せよ！|1-2・1-3・1-4・2-1ボスS勝利各1|要(軽母or軽巡or雷巡or練巡)1/(駆逐or海防)3
				case 284:   // |284|季|南西諸島方面「海上警備行動」発令！|1-4・2-1・2-2・2-3ボスS勝利各1|要(軽母or軽巡or雷巡or練巡)1/(駆逐or海防)3
					isAccepted =
						memberstype.Any(t => t == ShipTypes.LightAircraftCarrier || t == ShipTypes.LightCruiser || t == ShipTypes.TorpedoCruiser || t == ShipTypes.TrainingCruiser) &&
						memberstype.Count(t => t == ShipTypes.Destroyer || t == ShipTypes.Escort) >= 3;
					break;
				//============================ 800～899 ============================
				case 840:   //|840|週|【節分任務:豆】節分作戦二〇二四|1-1・1-3・1-4ボスA勝利各1|鳳翔・阿賀野・浦波・深雪・朧・潮・清霜・風雲・朝霜・峯雲・迅鯨・長鯨から旗艦と二番艦, 期間限定
					membernames = new string[] { "ほうしょう", "あがの", "うらなみ", "みゆき", "おぼろ", "うしお", "きよしも", "かざぐも", "あさしも", "みねぐも", "じんげい", "ちょうげい" };
					fleetmember = new bool[] { false, false };
					foreach (var item in membernames)
					{
						if (fleetmember[0] == false && members[0]?.MasterShip?.NameReading == item)
						{
							fleetmember[0] = true;
						}
						if (fleetmember[1] == false && members[1]?.MasterShip?.NameReading == item)
						{
							fleetmember[1] = true;
						}
					}
					isAccepted = (fleetmember[0] == true && fleetmember[1] == true);
					break;
				case 841:   //|841|週|【節分任務:鬼】南西方面節分作戦二〇二四|2-1・2-2・7-4のボスA勝利各2|Ranger・Johnston・早霜・神鷹・大淀・明石・天霧・狭霧・瑞穂・Commandant Testeから旗艦と二番艦, 期間限定
					membernames = new string[] { "レンジャー", "ジョンストン", "はやしも", "しんよう", "おおよど", "あかし", "あまぎり", "さぎり", "みずほ", "コマンダン・テスト" };
					fleetmember = new bool[] { false, false };
					foreach (var item in membernames)
					{
						if (fleetmember[0] == false && members[0]?.MasterShip?.NameReading == item)
						{
							fleetmember[0] = true;
						}
						if (fleetmember[1] == false && members[1]?.MasterShip?.NameReading == item)
						{
							fleetmember[1] = true;
						}
					}
					isAccepted = (fleetmember[0] == true && fleetmember[1] == true);
					break;
				case 843:   //|843|月|【節分任務:柊】節分拡張作戦二〇二四 精強即応！|2-3・4-5・5-5・6-5ボスS勝利各1|要最上型2+軽母1+自由枠3, 期間限定
					int count_typemogami = members.Count(s => s?.MasterShip?.ShipClass == 9);
					int count_cvl = members.Count(s => s?.MasterShip?.ShipType == ShipTypes.LightAircraftCarrier);
					bool suzukuma_cvl = members.Any(s => s?.ShipID == 508) || members.Any(s => s?.ShipID == 509);

					if (suzukuma_cvl)
					{
						if ((count_typemogami >= 3 && count_cvl >= 1) || (count_typemogami >= 2 && count_cvl >= 2))
						{
							isAccepted = true;
						}
						else
						{
							isAccepted = false;
						}
					}
					else
					{
						if (count_typemogami >= 2 && count_cvl >= 1)
						{
							isAccepted = true;
						}
						else
						{ 
							isAccepted = false;
						}
					}
					break;
				// |854|季|戦果拡張任務！「Z作戦」前段作戦|2-4・6-1・6-3ボスA勝利各1/6-4ボスS勝利1|要第一艦隊
				case 854:
					isAccepted =
						fleet.FleetID == 1;
					break;
				// |861|季|強行輸送艦隊、抜錨！|1-6終点到達2|要(航空戦艦or補給艦)2
				case 861:
					isAccepted =
						memberstype.Count(t => t == ShipTypes.AviationBattleship || t == ShipTypes.FleetOiler) >= 2;
					break;
				// |862|季|前線の航空偵察を実施せよ！|6-3ボスA勝利2|要水母1軽巡2
				case 862:
					isAccepted =
						memberstype.Count(t => t == ShipTypes.SeaplaneTender) >= 1 &&
						memberstype.Count(t => t == ShipTypes.LightCruiser) >= 2;
					break;
				// |873|季|北方海域警備を実施せよ！|3-1・3-2・3-3ボスA勝利各1|要軽巡1, 1エリア達成で50%,2エリアで80%
				case 873:
					isAccepted =
						memberstype.Any(t => t == ShipTypes.LightCruiser);
					break;
				// |875|季|精鋭「三一駆」、鉄底海域に突入せよ！|5-4ボスS勝利2|要長波改二/(高波改or沖波改or朝霜改)
				case 875:
					isAccepted =
						members.Any(s => s?.ShipID == 543) &&
						members.Any(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "たかなみ":
								case "おきなみ":
								case "あさしも":
									return s.MasterShip.RemodelTier >= 1;
								default:
									return false;
							}
						});
					break;
				// |888|季|新編成「三川艦隊」、鉄底海峡に突入せよ！|5-1・5-3・5-4ボスS勝利各1|要(鳥海or青葉or衣笠or加古or古鷹or天龍or夕張)4
				case 888:
					isAccepted =
						members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "ちょうかい":
								case "あおば":
								case "きぬがさ":
								case "かこ":
								case "ふるたか":
								case "てんりゅう":
								case "ゆうばり":
									return true;
								default:
									return false;
							}
						}) >= 4;
					break;
				case 872:   // |872|季|戦果拡張任務！「Z作戦」後段作戦|5-5・6-2・6-5・7-2(第二)ボスS勝利各1|要第一艦隊
					isAccepted = fleet.FleetID == 1 && CheckGaugeIndex72(bm.Compass);
					break;
				case 893:   // |893|季|泊地周辺海域の安全確保を徹底せよ！|1-5・7-1・7-2(第一＆第二)ボスS勝利各3|3エリア達成時点で80%
					isAccepted = CheckGaugeIndex72(bm.Compass);
					break;
				// |894|季|空母戦力の投入による兵站線戦闘哨戒|1-3・1-4・2-1・2-2・2-3ボスS勝利各1?|要空母系
				case 894:
					isAccepted =
						memberstype.Any(t => t == ShipTypes.LightAircraftCarrier || t == ShipTypes.AircraftCarrier || t == ShipTypes.ArmoredAircraftCarrier);
					break;
				//============================ 900～999 ============================
				case 903:   // |903|季|拡張「六水戦」、最前線へ！|5-1・5-4・6-4・6-5ボスS勝利各1|要旗艦夕張改二(|特|丁), 由良改二or(睦月/如月/弥生/卯月/菊月/望月2)|進捗3/4で80%
					isAccepted = members[0]?.MasterShip?.NameReading == "ゆうばり" && members[0]?.MasterShip?.RemodelTier >= 2 &&
						(members.Any(s => s?.ShipID == 488) || members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "むつき":
								case "きさらぎ":
								case "やよい":
								case "もちづき":
								case "きくづき":
								case "うづき":
									return true;
								default:
									return false;
							}
						}) >= 2);
					break;
				case 904:   // |904|年(2月)|精鋭「十九駆」、躍り出る！|2-5・3-4・4-5・5-3ボスS勝利各1|要綾波改二/敷波改二
					isAccepted = members.Any(s => s?.ShipID == 195) && members.Any(s => s?.ShipID == 627);
					break;
				case 905:   // |905|年(2月)|「海防艦」、海を護る！|1-1・1-2・1-3・1-5ボスA勝利各1/1-6終点到達1|要海防艦3, 5隻以下の編成
					isAccepted = members.Count(s => s != null) <= 5 && memberstype.Count(t => t == ShipTypes.Escort) >= 3;
					break;

				case 912:   // |912|年(3月)|工作艦「明石」護衛任務|1-3・2-1・2-2・2-3ボスA勝利各1/1-6終点到達1|要明石旗艦, 駆逐艦3
					isAccepted = members.FirstOrDefault()?.MasterShip?.NameReading == "あかし" &&
						memberstype.Count(t => t == ShipTypes.Destroyer) >= 3;
					break;
				case 914:   // |914|３|重巡戦隊、西へ！|4-1・4-2・4-3・4-4ボスA勝利各1|要重巡3/駆逐1
					isAccepted = memberstype.Count(t => t == ShipTypes.HeavyCruiser) >= 3 &&
						memberstype.Count(t => t == ShipTypes.Destroyer) >= 1;
					break;
				case 928:   //|928|９|歴戦「第十方面艦隊」、全力出撃！|4-2・7-2(第二)・7-3(第二)ボスS勝利各2|要(羽黒/足柄/妙高/高雄/神風)2
					isAccepted = members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "はぐろ":
								case "あしがら":
								case "みょうこう":
								case "たかお":
								case "かみかぜ":
									return true;
								default:
									return false;
							}
						}) >= 2 && CheckGaugeIndex72(bm.Compass) && CheckGaugeIndex73(bm.Compass);
					break;
				case 944:   //|944|６|鎮守府近海海域の哨戒を実施せよ！|1-2、1-3、1-4ボスA勝利以上各2回|条件：旗艦に重巡洋艦(航空巡洋艦は不可)or駆逐艦、随伴に駆逐艦or海防艦3
					isAccepted =
						(members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.HeavyCruiser &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Destroyer || s?.MasterShip?.ShipType == ShipTypes.Escort) >= 3)
						 ||
						(members.FirstOrDefault().MasterShip.ShipType == ShipTypes.Destroyer &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Destroyer || s?.MasterShip?.ShipType == ShipTypes.Escort) >= 4);
					break;
				case 945:   //|945|６|南西方面の兵站航路の安全を図れ！|1-5、2-1ボスA勝利以上各2回、1-6輸送マス到達2回|条件：旗艦に練巡/軽巡/駆逐、随伴に駆逐艦or海防艦3
					isAccepted =
						((members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.LightCruiser || members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.TrainingCruiser) &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Destroyer || s?.MasterShip?.ShipType == ShipTypes.Escort) >= 3)
						 ||
						(members.FirstOrDefault().MasterShip.ShipType == ShipTypes.Destroyer &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Destroyer || s?.MasterShip?.ShipType == ShipTypes.Escort) >= 4);
					break;
				case 946:   //|946|６|空母機動部隊、出撃！敵艦隊を迎撃せよ！|2-2，2-3、2-4ボスS勝利各1回|条件：旗艦に空母系、随伴に重巡・航巡2
					isAccepted =
						(members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.LightAircraftCarrier ||
						 members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.AircraftCarrier ||
						 members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.ArmoredAircraftCarrier)
						 &&
						(members.Count(s => s?.MasterShip?.ShipType == ShipTypes.HeavyCruiser || s?.MasterShip?.ShipType == ShipTypes.AviationCruiser) >= 2);
					break;
				case 947:   //|947|６|AL作戦|3-1、3-3、3-4、3-5ボスS勝利各1回|条件：軽空母x2他自由
					isAccepted =
						members.Count(s => s?.MasterShip?.ShipType == ShipTypes.LightAircraftCarrier) >= 2;
					break;
				case 948:   //|948|６|機動部隊決戦|5-2、5-5、6-4、6-5ボスS勝利各1回|条件：旗艦に空母系
					isAccepted =
						(members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.LightAircraftCarrier ||
						 members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.AircraftCarrier ||
						 members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.ArmoredAircraftCarrier);
					break;
				case 949:   //|949|単|改装特務空母「Gambier Bay Mk.II」抜錨！|2-4, 3-5ボスS勝利各2回、6-4ボスA勝利2回|要Gambier Bay Mk.II旗艦、Fletcher級駆逐艦x1
					isAccepted =
						members.FirstOrDefault()?.MasterShip?.ShipID == 707 && (members.Count(s => s?.MasterShip?.ShipClass == 91) >= 1);
					break;
				case 950:   //|950|単|【夏季限定】「渚のマーメイド」作戦！|1-4, 2-3, 3-2ボスS勝利各2回|条件：曙/潮/漣/朧 or 白露改二/時雨改二/村雨改二/夕立改二のいずれかの組み合わせ、期間限定(2021/07/15～????/??/??)
					isAccepted = (members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "おぼろ":
								case "さざなみ":
								case "うしお":
								case "あけぼの":
									return true;
								default:
									return false;
							}
						}) >= 4) ||	(members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "しらつゆ":
								case "しぐれ":
								case "むらさめ":
								case "ゆうだち":
									return s.MasterShip.RemodelTier >= 2;
								default:
									return false;
							}
						}) >= 4);
					break;
				case 951:   //|951|単|【夏季限定】「渚のシレーナ」欧州作戦！|4-1, 4-3, 4-4ボスS勝利各2回|伊駆逐、独駆逐、米駆逐、仏艦艇、「Littorio(Italia)」「U-511(呂500)」「Houston」「Gotland」の中から5隻、期間限定(2021/07/15～????/??/??)
					isAccepted =
						(
							(members.Count(s => 
								 s?.MasterShip?.ShipClass == 61 ||										//マエストラーレ級駆逐艦
								 s?.MasterShip?.ShipClass == 48 ||										//Z1型駆逐艦
								 s?.MasterShip?.ShipClass == 87 || s?.MasterShip?.ShipClass == 91 ||	//ジョンCバトラー級駆逐艦、フレッチャー級駆逐艦
								 s?.MasterShip?.ShipClass == 70 || s?.MasterShip?.ShipClass == 79))		//リシュリュー級戦艦、コマンダンテスト級水上機母艦
							+
							(members.Count(s =>
							{
								switch (s?.MasterShip?.NameReading)
								{
									case "リットリオ・イタリア":
									case "ゆー511・ろ500":
									case "ヒューストン":
									case "ゴトランド":
										return true;
									default:
										return false;
								}
							}))
						) >= 5;
					break;
				case 952:   //|952|単|【作戦準備】第二段階任務(対地/対空整備)|1-3, 1-4, 2-1, 2-2ボスS勝利各1回|条件：駆逐3以上|
					isAccepted = memberstype.Count(t => t == ShipTypes.Destroyer) >= 3;
					break;
				case 953:   //|953|週|【梅雨限定任務】雨の南西諸島防衛戦！|2-1, 2-2, 2-3ボスA勝利各1回|条件：巡洋艦を旗艦、駆逐艦x1、海防艦x1、水上機母艦x1|期間限定任務
					isAccepted =
						(members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.HeavyCruiser ||
						 members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.LightCruiser ||
						 members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.TorpedoCruiser ||
						 members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.TrainingCruiser ||
						 members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.AviationCruiser)
						 &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Destroyer) >= 1 &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Escort) >= 1 &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.SeaplaneTender) >= 1;
					break;
				case 954:   //|954|週|【梅雨拡張任務】梅雨の海上護衛強化！|1-2, 1-3, 1-5ボスS勝利各1回+1-6到達2回|条件：駆逐艦を旗艦、海防艦x2|期間限定任務
					isAccepted =
						(members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.Destroyer)
						 &&
						(members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Escort) >= 2);
					break;
				case 955:   //|955|月|【梅雨限定月間任務】西方海域統合作戦|4-1, 4-2, 4-3, 4-4, 4-5ボスS勝利各1回|条件：空母2以上、(球磨型+大淀)1|期間限定任務
					isAccepted =
						(members.Count(s => s?.MasterShip?.ShipType == ShipTypes.LightAircraftCarrier ||
											s?.MasterShip?.ShipType == ShipTypes.AircraftCarrier ||
											s?.MasterShip?.ShipType == ShipTypes.ArmoredAircraftCarrier) >= 2) &&
						members.Count(s => s?.MasterShip?.ShipClass == 4 || s?.MasterShip?.NameReading == "おおよど") >= 1;
					break;
				case 957:	//|957|単|「山風改二」、抜錨せよ！|1-2、1-3、1-4、1-5ボス各S勝利1改|条件：山風改二旗艦および随伴に駆逐/海防3|
					isAccepted = 
						(members[0]?.MasterShip?.ShipID == 588 || members[0]?.MasterShip?.ShipID == 667) && (memberstype.Count(t => t == ShipTypes.Destroyer) + memberstype.Count(t => t == ShipTypes.Escort)) >= 4;
					break;
				case 958:   //|958|単|改白露型駆逐艦「山風改二」、奮戦す！|2-2、7-2、5-1、6-4ボスS勝利1回||条件：山風改二、江風改二、海風改二から2隻|
					isAccepted =
						members.Count(s =>
						{
							switch (s?.MasterShip?.ShipID)
							{
								case 588: //山風改二
								case 667: //山風改二丁
								case 469: //江風改二
								case 587: //海風改二 
									return true;
								default:
									return false;
							}
						}) >= 2;
					break;
				case 959:   //|959|単|「鎮守府秋刀魚祭り」発動準備！|1-1～1-5ボスを各S勝利1回ずつ|条件：軽巡、練巡、水上機母艦、特務艦のいずれかが旗艦|
					//※暫定で書いてある通り特務艦のみにした。灯台補給艦や南極観測船だとダメなのかは未検証
					isAccepted = (
						new[] {
							ShipTypes.LightCruiser,
							ShipTypes.TrainingCruiser,
							ShipTypes.SeaplaneTender,
						}.Contains(memberstype.FirstOrDefault()) || members[0]?.MasterShip?.ShipID == 699);
					break;
				case 960:   //|960|単|続：「鎮守府秋刀魚祭り」発動準備！|3-1～3-5ボスを各S勝利1回ずつ|条件：軽巡、練巡、潜水母艦、特務艦のいずれかが旗艦|
					//※暫定で書いてある通り特務艦のみにした。灯台補給艦や南極観測船だとダメなのかは未検証
					isAccepted = (
						new[] {
							ShipTypes.LightCruiser,
							ShipTypes.TrainingCruiser,
							ShipTypes.SubmarineTender,
						}.Contains(memberstype.FirstOrDefault()) || members[0]?.MasterShip?.ShipID == 699);
					break;
				case 961:   //|961|単|奮戦！精鋭「第十五駆逐隊」第一小隊|2-4、5-4、7-2-2ボスを各S勝利1回ずつ|条件：黒潮改二、親潮改二を編成に入れる|
					isAccepted =
						members.Count(s =>
						{
							switch (s?.MasterShip?.ShipID)
							{
								case 568: //黒潮改二
								case 670: //親潮改二
									return true;
								default:
									return false;
							}
						}) >= 2;
					break;
				case 973:   //|973|５|日英米合同水上艦隊、抜錨せよ！|3-1、3-3、4-3、7-3-2ボスを各A勝利以上1回ずつ|条件：米+英艦艇3隻を編成に入れる、かつ空母を含まない|
					isAccepted =
						(memberstype.Count(t => t == ShipTypes.LightAircraftCarrier) == 0) &&
						(memberstype.Count(t => t == ShipTypes.AircraftCarrier) == 0) &&
						(memberstype.Count(t => t == ShipTypes.ArmoredAircraftCarrier) == 0) &&
						members.Count(s => s?.MasterShip?.IsGBandUSA == true) >= 3;
					break;
				case 975:   //|975|５|精鋭「第十九駆逐隊」、全力出撃！|1-5、2-3、3-2、5-3ボスを各S勝利1回ずつ|条件：磯波改二、浦波改二、綾波改二、敷波改二を編成に入れる|
					isAccepted =
						members.Count(s =>
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
						}) >= 4;
					break;
				case 987:   //|987|週|【限定週間任務】10周年秋南瓜祭りおかわり！|6-1, 6-2, 6-3ボスA勝利各1回|条件：「阿武隈+伊201」「鵜来+稲木」「瑞穂+Brooklyn+Johnston」何れかの組み合わせ、南瓜イベント
					isAccepted = (members.Count(s =>
					{
						switch (s?.MasterShip?.NameReading)
						{
							case "あぶくま":
							case "い201":
								return true;
							default:
								return false;
						}
					}) >= 2) || (members.Count(s =>
					{
						switch (s?.MasterShip?.NameReading)
						{
							case "うくる":
							case "いなぎ":
								return true;
							default:
								return false;
						}
					}) >= 2) || (members.Count(s =>
					{
						switch (s?.MasterShip?.NameReading)
						{
							case "みずほ":
							case "ブルックリン":
							case "ジョンストン":
								return true;
							default:
								return false;
						}
					}) >= 3);
					break;
				//|1002|単|【期間限定任務】10周年秋南瓜祭り拡張作戦！|1-5、2-3、4-4、6-4ボスを各S勝利1回ずつ|条件：鵜来, 稲木, 朝潮, 夕立, 野分, 浜波, 早波, 阿武隈から2隻|
				case 1002:
					isAccepted =
						members.Count(s =>
						{
							switch (s?.MasterShip?.NameReading)
							{
								case "うくる":
								case "いなぎ":
								case "あさしお":
								case "ゆうだち":
								case "のわき":
								case "はまなみ":
								case "はやなみ":
								case "あぶくま":
									return true;
								default:
									return false;
							}
						}) >= 2;
					break;
				//|1005|１|精強「第七駆逐隊」緊急出動！|1-2、1-3、1-5、3-2ボスを各A勝利1回ずつ|条件：「朧改」「漣改」「曙改(二)」「潮改(二)」を編成に入れる|
				case 1005:
					isAccepted =
						members.Count(s =>
						{
							switch (s?.MasterShip?.ShipID)
							{
								case 231: //曙改
								case 665: //曙改二
								case 233: //潮改
								case 407: //潮改二
								case 230: //朧改
								case 232: //漣改
									return true;
								default:
									return false;
							}
						}) >= 4;
					break;
				case 1010:  //|1010|週|【期間限定任務】対潜掃討作戦|1-5 S勝利×3回,1-6×1回港到達|条件：(駆逐+海防)3|
					isAccepted =
						members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Destroyer || s?.MasterShip?.ShipType == ShipTypes.Escort) >= 3;
					break;
				case 1011:  //|1011|週|【期間限定任務】精強海防艦、緊急近海防衛！|1-1, 1-2, 1-3, 1-5, 2-1 A勝利以上×1回|条件：海防3(旗艦含)|
					isAccepted =
						(members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.Escort &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Escort) >= 3);
					break;
				case 1012:  //|1012|５|鵜来型海防艦、静かな海を防衛せよ！|1-1S勝利3回、1-2, 1-5 A勝利2回以上|条件：鵜来型(旗艦), 海防1-3 (旗艦込最大4隻), 海防艦のみ|
					isAccepted =
						(members.FirstOrDefault()?.MasterShip?.ShipClass == 117 &&
						 members.Count(s => s?.MasterShip?.ShipType == ShipTypes.Escort) <= 4 &&
						 members.Count(s => s?.MasterShip?.ShipType != ShipTypes.Escort) == 0);
					break;
			}

			// 第二ゲージでも第一ボスに行ける場合があるので、個別対応が必要
			//if (GaugeIndex != -1)
			//	isAccepted &= bm.Compass.MapInfo.CurrentGaugeIndex == GaugeIndex;

			if (isAccepted)
				base.Increment(rank, areaID, isBoss);
		}



		private bool CheckGaugeIndex72(CompassData compass)
		{
			if (compass.MapAreaID == 7 && compass.MapInfoID == 2)
			{
				switch (compass.Destination)
				{
					case 7:
						return GaugeIndex == 1;
					case 15:
						return GaugeIndex == 2;
					default:
						return false;
				}
			}
			return true;
		}

		private bool CheckGaugeIndex73(CompassData compass)
		{
			if (compass.MapAreaID == 7 && compass.MapInfoID == 3)
			{
				switch (compass.Destination)
				{
					case 5:
					case 8:
						return GaugeIndex == 1;
					case 18:
					case 23:
					case 24:
					case 25:
						return GaugeIndex == 2;
					default:
						return false;
				}
			}
			return true;
		}


		public override string GetClearCondition()
		{
			var sb = new StringBuilder(base.GetClearCondition());

			if (GaugeIndex != -1)
				sb.AppendFormat("(第{0}ゲージ)", GaugeIndex);

			/*
			switch (QuestID)
			{
				// |249|月|「第五戦隊」出撃せよ！|2-5ボスS勝利1|要「那智」「妙高」「羽黒」
				case 249:
					sb.Append("【要「那智」「妙高」「羽黒」】");
					break;

				// |257|月|「水雷戦隊」南西へ！|1-4ボスS勝利1|要軽巡旗艦、軽巡3隻まで、他駆逐艦　他艦種禁止
				case 257:
					sb.Append("【要軽巡旗艦、軽巡3隻まで、他駆逐艦　他艦種禁止】");
					break;

				// |259|月|「水上打撃部隊」南方へ！|5-1ボスS勝利1|要(大和型or長門型or伊勢型or扶桑型)3/軽巡1　巡戦禁止、戦艦追加禁止
				case 259:
					sb.Append("【要(大和型or長門型or伊勢型or扶桑型)3/軽巡1　巡戦禁止、戦艦追加禁止】");
					break;

				// |264|月|「空母機動部隊」西へ！|4-2ボスS勝利1|要(空母or軽母or装母)2/駆逐2
				case 264:
					sb.Append("【要空母系2/駆逐2】");
					break;

				// |266|月|「水上反撃部隊」突入せよ！|2-5ボスS勝利1|要駆逐旗艦、重巡1軽巡1駆逐4
				case 266:
					sb.Append("【要駆逐旗艦、重巡1軽巡1駆逐4】");
					break;

				// |861|季|強行輸送艦隊、抜錨！|1-6終点到達2|要(航空戦艦or補給艦)2
				case 861:
					sb.Append("【要(航空戦艦or補給艦)2】");
					break;

				// |862|季|前線の航空偵察を実施せよ！|6-3ボスA勝利2|要水母1軽巡2
				case 862:
					sb.Append("【要水母1軽巡2】");
					break;

				// |873|季|北方海域警備を実施せよ！|3-1・3-2・3-3ボスA勝利各1|要軽巡1, 1エリア達成で50%,2エリアで80%
				case 873:
					sb.Append("【要軽巡1】");
					break;

				// |875|季|精鋭「三一駆」、鉄底海域に突入せよ！|5-4ボスS勝利2|要長波改二/(高波改or沖波改or朝霜改)
				case 875:
					sb.Append("【要長波改二/(高波改or沖波改or朝霜改)】");
					break;

				// |888|季|新編成「三川艦隊」、鉄底海峡に突入せよ！|5-1・5-3・5-4ボスS勝利各1|要(鳥海or青葉or衣笠or加古or古鷹or天龍or夕張)4
				case 888:
					sb.Append("【要(鳥海or青葉or衣笠or加古or古鷹or天龍or夕張)4】");
					break;

				// |893|季|泊地周辺海域の安全確保を徹底せよ！|1-5・7-1・7-2(第一＆第二)ボスS勝利各3|3エリア達成時点で80%
				case 893:
					sb.Append("【7-2: #1/#2ゲージ両方】");
					break;

				// |894|季|空母戦力の投入による兵站線戦闘哨戒|1-3・1-4・2-1・2-2・2-3ボスS勝利各1?|要空母系
				case 894:
					sb.Append("【要空母系】");
					break;
			}
			*/

			return sb.ToString();
		}
	}
}
