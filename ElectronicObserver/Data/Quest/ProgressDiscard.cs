using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest
{

	/// <summary>
	/// 装備廃棄任務の進捗を管理します。
	/// </summary>
	[DataContract(Name = "ProgressDiscard")]
	public class ProgressDiscard : ProgressData
	{

		/// <summary>
		/// 廃棄した個数をベースにカウントするか
		/// false = 回数、true = 個数
		/// </summary>
		[DataMember]
		private bool CountsAmount { get; set; }

		/// <summary>
		/// 対象となる装備カテゴリ
		/// null ならすべての装備を対象とする
		/// </summary>
		[DataMember]
		private HashSet<int> Categories { get; set; }

		/// <summary>
		/// Categories の扱い
		/// -1=装備ID, 1=図鑑分類, 2=通常の装備カテゴリ, 3=アイコン
		/// </summary>
		[DataMember]
		protected int CategoryIndex { get; set; }

		public ProgressDiscard(QuestData quest, int maxCount, bool countsAmount, int[] categories)
			: this(quest, maxCount, countsAmount, categories, 2) { }

		public ProgressDiscard(QuestData quest, int maxCount, bool countsAmount, int[] categories, int categoryIndex)
			: base(quest, maxCount)
		{
			CountsAmount = countsAmount;
			Categories = categories == null ? null : new HashSet<int>(categories);
			CategoryIndex = categoryIndex;
		}

		public void Increment(IEnumerable<int> equipments)
		{
			if (!CountsAmount)
			{
				Increment();
				return;
			}

			if (Categories == null)
			{
				foreach (var i in equipments)
					Increment();
				return;
			}

			foreach (var i in equipments)
			{
				var eq = KCDatabase.Instance.Equipments[i];

				switch (CategoryIndex)
				{
					case -1:
						if (Categories.Contains(eq.EquipmentID))
							Increment();
						break;
					case 1:
						if (Categories.Contains(eq.MasterEquipment.CardType))
							Increment();
						break;
					case 2:
						if (Categories.Contains((int)eq.MasterEquipment.CategoryType))
							Increment();
						break;
					case 3:
						if (Categories.Contains(eq.MasterEquipment.IconType))
							Increment();
						break;
				}

			}
		}

		public override void Increment()
		{
			var Empty = (ShipTypes)(-1);
			var q = KCDatabase.Instance.Quest[QuestID];
			var f = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(); //第一艦隊
			var members = f.MembersWithoutEscaped;
			var memberstype = members.Select(s => s?.MasterShip?.ShipType ?? Empty).ToArray();

			int isAccepted = -1; //-1:チェック不要、0:条件未達、1:条件達成 

			if (q == null)
			{
				TemporaryProgress++;
				return;
			}

			if (q.State != 2)
				return;

			//=============================================================
			// 任務IDごとの個別チェック
			//=============================================================
			//MEMO:装備スロットの空きはnullになっているので、要nullチェック
			switch (QuestID)
			{
				case 626:   //|626|月|精鋭「艦戦」隊の新編成|熟練搭乗員, 零式艦戦21型>>装備の鳳翔旗艦, (零式艦戦21型x2,九六式艦戦x1)廃棄
					isAccepted = 0;
					//秘書艦が鳳翔か？
					if (members.FirstOrDefault()?.MasterShip?.NameReading == "ほうしょう")
					{
						//装備スロット内を調べて練度MAXの零戦21型があれば条件達成
						foreach (var slot in members.FirstOrDefault().SlotInstance)
						{
							if(slot != null && slot.EquipmentID == 20 && slot.AircraftLevel == 7)
							{
								isAccepted = 1;
							}
						}
					}
					break;
				case 628:   //|628|月|機種転換|零式艦戦21型(熟練)>>装備の空母旗艦, 零式艦戦52型x2廃棄
					isAccepted = 0;
					//秘書艦が空母系か？
					if (members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.AircraftCarrier ||
					    members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.LightAircraftCarrier ||
					    members.FirstOrDefault()?.MasterShip?.ShipType == ShipTypes.ArmoredAircraftCarrier)
					{
						//装備スロット内を調べて練度MAXの零戦21型(熟練)があれば条件達成
						foreach(var slot in members.FirstOrDefault().SlotInstance)
						{
							if(slot != null && slot.EquipmentID == 96 && slot.AircraftLevel == 7)
							{
								isAccepted = 1;
							}
						}
					}
					break;
				case 654:   //|654|10|精鋭複葉機飛行隊の編成|(Swordfishx1, Fulmarx2)廃棄, 秘書艦Ark Royalの第一スロットにSwordfish★10装備, (熟練搭乗員x1, 弾薬x1500, ボーキx1500)保有
					isAccepted = (members.FirstOrDefault()?.MasterShip?.NameReading == "アークロイヤル" && 
								  members.FirstOrDefault().SlotInstance[0] != null &&
								  members.FirstOrDefault().SlotInstance[0].EquipmentID == 242 &&
								  members.FirstOrDefault().SlotInstance[0].Level == 10) ? 1 : 0;
					break;
				case 686:	//|686|季|戦時改修A型高角砲の量産|12.7cm連装砲A型改二★10を第一スロ装備の特型駆逐艦旗艦, (10cm連装高角砲x4, 94式高射装置x1)廃棄, (開発資材30, 鋼材900, 新型砲熕兵装資材1)保有
					isAccepted = ((members.FirstOrDefault()?.MasterShip?.ShipClass == 1 ||
								   members.FirstOrDefault()?.MasterShip?.ShipClass == 5 ||
								   members.FirstOrDefault()?.MasterShip?.ShipClass == 12) &&
								  members.FirstOrDefault().SlotInstance[0] != null &&
								  members.FirstOrDefault().SlotInstance[0].EquipmentID == 294 &&
								  members.FirstOrDefault().SlotInstance[0].Level == 10) ? 1 : 0;
					break;
				case 1106:  //|1106|精鋭三座水上偵察機隊の前線投入|秘書艦「由良改二」の第一スロットに零式水上偵察機11型乙★10を装備した状態で瑞雲x3＆零式水上偵察機x3破棄、燃料1300＆ボーキ1700＆新型航空兵装資材x1＆戦闘詳報x1＆熟練搭乗員x3を保有
					isAccepted = (members.FirstOrDefault()?.MasterShip?.ShipID == 488 &&
								  members.FirstOrDefault().SlotInstance[0] != null &&
								  members.FirstOrDefault().SlotInstance[0].EquipmentID == 238 &&
							      members.FirstOrDefault().SlotInstance[0].Level == 10) ? 1 : 0;
					break;
				case 1108:  //|1108|調整改良型「水中探信儀」の増産|秘書艦「山風改二(丁)」もしくは「時雨改二」の第一スロットに三式水中探信儀★10を装備した状態で九三式水中聴音機x2破棄、三式水中探信儀x2破棄、新型兵装資材x2＆開発資材x30＆ボーキ1300を保有
					isAccepted = (((members.FirstOrDefault()?.MasterShip?.ShipID == 588) ||  //山風改二
						           (members.FirstOrDefault()?.MasterShip?.ShipID == 667) ||  //山風改二丁
								   (members.FirstOrDefault()?.MasterShip?.ShipID == 145)) && //時雨改二
								  members.FirstOrDefault().SlotInstance[0] != null &&
								  members.FirstOrDefault().SlotInstance[0].EquipmentID == 47 &&
								  members.FirstOrDefault().SlotInstance[0].Level == 10) ? 1 : 0;
					break;
				case 1109:  //|1109|上陸作戦支援用装備の配備|秘書艦「神州丸」の第一スロットに大発戦車★10を装備した状態で7.7mm機銃x2、大発動艇x2を破棄、高速建造材x8、開発資材x10、鋼材800を保有
					isAccepted = (members.FirstOrDefault()?.MasterShip?.NameReading == "しんしゅうまる" &&
								  members.FirstOrDefault().SlotInstance[0] != null &&
								  members.FirstOrDefault().SlotInstance[0].EquipmentID == 166 &&
								  members.FirstOrDefault().SlotInstance[0].Level == 10) ? 1 : 0;
					break;
				default:
					//任務IDが当てはまらないなら-1のまま、つまりチェックの必要なし
					break;
			}
			if(isAccepted == 0)
			{
				return;
			}

			if (!IgnoreCheckProgress)
				CheckProgress(q);

			Progress = Math.Min(Progress + 1, ProgressMax);
		}

		public override string GetClearCondition()
		{
			return (Categories == null ? "" : string.Join("・", Categories.OrderBy(s => s).Select(s =>
			{
				switch (CategoryIndex)
				{
					case -1:
						return KCDatabase.Instance.MasterEquipments[s].Name;
					case 1:
						return $"図鑑[{s}]";
					case 2:
						return KCDatabase.Instance.EquipmentTypes[s].Name;
					case 3:
						return $"アイコン[{s}]";
					default:
						return $"???[{s}]";
				}
			}))) + "廃棄" + ProgressMax + (CountsAmount ? "個" : "回");
		}

		/// <summary>
		/// 互換性維持：デフォルト値の設定
		/// </summary>
		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			CategoryIndex = 2;
		}
	}
}
