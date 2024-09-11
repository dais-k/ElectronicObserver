using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
	/// <summary>
	/// 個別の艦娘データを保持します。
	/// </summary>
	public class ShipData : APIWrapper, IIdentifiable
	{
		/// <summary>
		/// 艦娘を一意に識別するID
		/// </summary>
		public int MasterID => (int)RawData.api_id;

		/// <summary>
		/// 並べ替えの順番
		/// </summary>
		public int SortID => (int)RawData.api_sortno;

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID => (int)RawData.api_ship_id;

		/// <summary>
		/// レベル
		/// </summary>
		public int Level => (int)RawData.api_lv;

		/// <summary>
		/// 累積経験値
		/// </summary>
		public long ExpTotal => (long)RawData.api_exp[0];

		/// <summary>
		/// 次のレベルに達するために必要な経験値
		/// </summary>
		public int ExpNext => (int)RawData.api_exp[1];

		/// <summary>
		/// 耐久現在値
		/// </summary>
		public int HPCurrent { get; internal set; }

		/// <summary>
		/// 耐久最大値
		/// </summary>
		public int HPMax => (int)RawData.api_maxhp;

		/// <summary>
		/// 速力
		/// </summary>
		public int Speed => RawData.api_soku() ? (int)RawData.api_soku : MasterShip.Speed;

		/// <summary>
		/// 射程
		/// </summary>
		public int Range => (int)RawData.api_leng;

		/// <summary>
		/// 装備スロット(ID)
		/// </summary>
		public ReadOnlyCollection<int> Slot { get; private set; }

		/// <summary>
		/// 装備スロット(マスターID)
		/// </summary>
		public ReadOnlyCollection<int> SlotMaster => Array.AsReadOnly(Slot.Select(id => KCDatabase.Instance.Equipments[id]?.EquipmentID ?? -1).ToArray());

		/// <summary>
		/// 装備スロット(装備データ)
		/// </summary>
		public ReadOnlyCollection<EquipmentData> SlotInstance => Array.AsReadOnly(Slot.Select(id => KCDatabase.Instance.Equipments[id]).ToArray());

		/// <summary>
		/// 装備スロット(装備マスターデータ)
		/// </summary>
		public ReadOnlyCollection<EquipmentDataMaster> SlotInstanceMaster => Array.AsReadOnly(Slot.Select(id => KCDatabase.Instance.Equipments[id]?.MasterEquipment).ToArray());

		/// <summary>
		/// 補強装備スロット(ID)
		/// 0=未開放, -1=装備なし 
		/// </summary>
		public int ExpansionSlot { get; private set; }

		/// <summary>
		/// 補強装備スロット(マスターID)
		/// </summary>
		public int ExpansionSlotMaster => ExpansionSlot == 0 ? 0 : (KCDatabase.Instance.Equipments[ExpansionSlot]?.EquipmentID ?? -1);

		/// <summary>
		/// 補強装備スロット(装備データ)
		/// </summary>
		public EquipmentData ExpansionSlotInstance => KCDatabase.Instance.Equipments[ExpansionSlot];

		/// <summary>
		/// 補強装備スロット(装備マスターデータ)
		/// </summary>
		public EquipmentDataMaster ExpansionSlotInstanceMaster => KCDatabase.Instance.Equipments[ExpansionSlot]?.MasterEquipment;

		/// <summary>
		/// 全てのスロット(ID)
		/// </summary>
		public ReadOnlyCollection<int> AllSlot => Array.AsReadOnly(Slot.Concat(new[] { ExpansionSlot }).ToArray());

		/// <summary>
		/// 全てのスロット(マスターID)
		/// </summary>
		public ReadOnlyCollection<int> AllSlotMaster => Array.AsReadOnly(AllSlot.Select(id => KCDatabase.Instance.Equipments[id]?.EquipmentID ?? -1).ToArray());

		/// <summary>
		/// 全てのスロット(装備データ)
		/// </summary>
		public ReadOnlyCollection<EquipmentData> AllSlotInstance => Array.AsReadOnly(AllSlot.Select(id => KCDatabase.Instance.Equipments[id]).ToArray());

		/// <summary>
		/// 全てのスロット(装備マスターデータ)
		/// </summary>
		public ReadOnlyCollection<EquipmentDataMaster> AllSlotInstanceMaster => Array.AsReadOnly(AllSlot.Select(id => KCDatabase.Instance.Equipments[id]?.MasterEquipment).ToArray());

		private int[] _aircraft;

		/// <summary>
		/// 各スロットの航空機搭載量
		/// </summary>
		public ReadOnlyCollection<int> Aircraft => Array.AsReadOnly(_aircraft);

		/// <summary>
		/// 現在の航空機搭載量
		/// </summary>
		public int AircraftTotal => _aircraft.Sum(a => Math.Max(a, 0));

		/// <summary>
		/// 搭載燃料
		/// </summary>
		public int Fuel { get; internal set; }

		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public int Ammo { get; internal set; }

		/// <summary>
		/// スロットのサイズ
		/// </summary>
		public int SlotSize => !RawData.api_slotnum() ? 0 : (int)RawData.api_slotnum;

		/// <summary>
		/// 入渠にかかる時間(ミリ秒)
		/// </summary>
		public int RepairTime => (int)RawData.api_ndock_time;

		/// <summary>
		/// 入渠にかかる鋼材
		/// </summary>
		public int RepairSteel => (int)RawData.api_ndock_item[1];

		/// <summary>
		/// 入渠にかかる燃料
		/// </summary>
		public int RepairFuel => (int)RawData.api_ndock_item[0];

		/// <summary>
		/// コンディション
		/// </summary>
		public int Condition { get; internal set; }

		#region Parameters

		/********************************************************
		 * 強化値：近代化改修・レベルアップによって上昇した数値
		 * 総合値：装備込みでのパラメータ
		 * 基本値：装備なしでのパラメータ(初期値+強化値)
		 ********************************************************/

		private int[] _modernized;

		/// <summary>
		/// 火力強化値
		/// </summary>
		public int FirepowerModernized => _modernized.Length >= 5 ? _modernized[0] : 0;

		/// <summary>
		/// 雷装強化値
		/// </summary>
		public int TorpedoModernized => _modernized.Length >= 5 ? _modernized[1] : 0;

		/// <summary>
		/// 対空強化値
		/// </summary>
		public int AAModernized => _modernized.Length >= 5 ? _modernized[2] : 0;

		/// <summary>
		/// 装甲強化値
		/// </summary>
		public int ArmorModernized => _modernized.Length >= 5 ? _modernized[3] : 0;

		/// <summary>
		/// 運強化値
		/// </summary>
		public int LuckModernized => _modernized.Length >= 5 ? _modernized[4] : 0;

		/// <summary>
		/// 耐久強化値
		/// </summary>
		public int HPMaxModernized => _modernized.Length >= 7 ? _modernized[5] : 0;

		/// <summary>
		/// 対潜強化値
		/// </summary>
		public int ASWModernized => _modernized.Length >= 7 ? _modernized[6] : 0;

		/// <summary>
		/// 火力改修残り
		/// </summary>
		public int FirepowerRemain => (MasterShip.FirepowerMax - MasterShip.FirepowerMin) - FirepowerModernized;

		/// <summary>
		/// 雷装改修残り
		/// </summary>
		public int TorpedoRemain => (MasterShip.TorpedoMax - MasterShip.TorpedoMin) - TorpedoModernized;

		/// <summary>
		/// 対空改修残り
		/// </summary>
		public int AARemain => (MasterShip.AAMax - MasterShip.AAMin) - AAModernized;

		/// <summary>
		/// 装甲改修残り
		/// </summary>
		public int ArmorRemain => (MasterShip.ArmorMax - MasterShip.ArmorMin) - ArmorModernized;

		/// <summary>
		/// 運改修残り
		/// </summary>
		public int LuckRemain => (MasterShip.LuckMax - MasterShip.LuckMin) - LuckModernized;

		/// <summary>
		/// 耐久改修残り
		/// </summary>
		public int HPMaxRemain => (IsMarried ? MasterShip.HPMaxMarriedModernizable : MasterShip.HPMaxModernizable) - HPMaxModernized;

		/// <summary>
		/// 対潜改修残り
		/// </summary>
		public int ASWRemain => ASWMax <= 0 ? 0 : MasterShip.ASWModernizable - ASWModernized;

		/// <summary>
		/// 火力総合値
		/// </summary>
		public int FirepowerTotal => (int)RawData.api_karyoku[0];

		/// <summary>
		/// 雷装総合値
		/// </summary>
		public int TorpedoTotal => (int)RawData.api_raisou[0];

		/// <summary>
		/// 対空総合値
		/// </summary>
		public int AATotal => (int)RawData.api_taiku[0];

		/// <summary>
		/// 装甲総合値
		/// </summary>
		public int ArmorTotal => (int)RawData.api_soukou[0];

		/// <summary>
		/// 回避総合値
		/// </summary>
		public int EvasionTotal => (int)RawData.api_kaihi[0];

		/// <summary>
		/// 対潜総合値
		/// </summary>
		public int ASWTotal => (int)RawData.api_taisen[0];

		/// <summary>
		/// 索敵総合値
		/// </summary>
		public int LOSTotal => (int)RawData.api_sakuteki[0];

		/// <summary>
		/// 運総合値
		/// </summary>
		public int LuckTotal => (int)RawData.api_lucky[0];

		/// <summary>
		/// 爆装総合値
		/// </summary>
		public int BomberTotal => AllSlotInstanceMaster.Sum(eq => eq?.Bomber ?? 0);

		/// <summary>
		/// 命中総合値
		/// </summary>
		public int AccuracyTotal => AllSlotInstanceMaster.Sum(eq => eq?.Accuracy ?? 0);

		/// <summary>
		/// 火力基本値
		/// </summary>
		public int FirepowerBase => MasterShip.FirepowerMin + FirepowerModernized;

		/// <summary>
		/// 雷装基本値
		/// </summary>
		public int TorpedoBase => MasterShip.TorpedoMin + TorpedoModernized;

		/// <summary>
		/// 対空基本値
		/// </summary>
		public int AABase => MasterShip.AAMin + AAModernized;

		/// <summary>
		/// 装甲基本値
		/// </summary>
		public int ArmorBase => MasterShip.ArmorMin + ArmorModernized;

		/// <summary>
		/// 回避基本値
		/// </summary>
		public int EvasionBase
		{
			get
			{
				if (MasterShip.Evasion?.IsDetermined ?? false)
					return MasterShip.Evasion.GetParameter(Level);

				// パラメータ上限下限が分かっていれば上ので確実に取れる
				// 不明な場合は以下で擬似計算（装備分を引くだけ）　装備シナジーによって上昇している場合誤差が発生します
				return EvasionTotal - AllSlotInstance.Sum(eq => eq?.MasterEquipment?.Evasion ?? 0);
			}
		}

		/// <summary>
		/// 対潜基本値
		/// </summary>
		public int ASWBase
		{
			get
			{
				if (MasterShip.ASW?.IsDetermined ?? false)
					return MasterShip.ASW.GetParameter(Level) + ASWModernized;

				return ASWTotal - AllSlotInstance.Sum(eq => eq?.MasterEquipment?.ASW ?? 0);
			}
		}

		/// <summary>
		/// 索敵基本値
		/// </summary>
		public int LOSBase
		{
			get
			{
				if (MasterShip.LOS?.IsDetermined ?? false)
					return MasterShip.LOS.GetParameter(Level);

				return LOSTotal - AllSlotInstance.Sum(eq => eq?.MasterEquipment?.LOS ?? 0);
			}
		}

		/// <summary>
		/// 運基本値
		/// </summary>
		public int LuckBase => MasterShip.LuckMin + LuckModernized;

		/// <summary>
		/// 回避最大値
		/// </summary>
		public int EvasionMax => (int)RawData.api_kaihi[1];

		/// <summary>
		/// 対潜最大値
		/// </summary>
		public int ASWMax => (int)RawData.api_taisen[1];

		/// <summary>
		/// 索敵最大値
		/// </summary>
		public int LOSMax => (int)RawData.api_sakuteki[1];

		#endregion

		/// <summary>
		/// 保護ロックの有無
		/// </summary>
		public bool IsLocked => (int)RawData.api_locked != 0;

		/// <summary>
		/// 装備による保護ロックの有無
		/// </summary>
		public bool IsLockedByEquipment => (int)RawData.api_locked_equip != 0;

		/// <summary>
		/// 出撃海域
		/// </summary>
		public int SallyArea => RawData.api_sally_area() ? (int)RawData.api_sally_area : -1;

		/// <summary>
		/// 艦船のマスターデータへの参照
		/// </summary>
		public ShipDataMaster MasterShip => KCDatabase.Instance.MasterShips[ShipID];

		/// <summary>
		/// 入渠中のドックID　非入渠時は-1
		/// </summary>
		public int RepairingDockID => KCDatabase.Instance.Docks.Values.FirstOrDefault(dock => dock.ShipID == MasterID)?.DockID ?? -1;

		/// <summary>
		/// 所属艦隊　-1=なし
		/// </summary>
		public int Fleet => KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.Members.Contains(MasterID))?.FleetID ?? -1;

		/// <summary>
		/// 所属艦隊及びその位置
		/// ex. 1-3 (位置も1から始まる)
		/// 所属していなければ 空文字列
		/// </summary>
		public string FleetWithIndex
		{
			get
			{
				FleetManager fm = KCDatabase.Instance.Fleet;
				foreach (var f in fm.Fleets.Values)
				{
					int index = f.Members.IndexOf(MasterID);
					if (index != -1)
					{
						return $"{f.FleetID}-{index + 1}";
					}
				}
				return "";
			}

		}

		/// <summary>
		/// ケッコン済みかどうか
		/// </summary>
		public bool IsMarried => Level > 99;

		/// <summary>
		/// 次の改装まで必要な経験値
		/// </summary>
		public long ExpNextRemodel
		{
			get
			{
				ShipDataMaster master = MasterShip;
				if (master.RemodelAfterShipID <= 0)
					return 0;
				return Math.Max(ExpTable.ShipExp[master.RemodelAfterLevel].Total - ExpTotal, 0);
			}
		}

		/// <summary>
		/// 最終改装まで必要な経験値
		/// </summary>
		public long ExpFinalRemodel
		{
			get
			{
				ShipDataMaster master = MasterShip;
				if (master.FinalRemodelShipID <= 0)
					return 0;
				return Math.Max(ExpTable.ShipExp[master.FinalRemodelLevel].Total - ExpTotal, 0);
			}
		}

		/// <summary>
		/// 艦名
		/// </summary>
		public string Name => MasterShip.Name;

		/// <summary>
		/// 艦名(レベルを含む)
		/// </summary>
		public string NameWithLevel => $"{MasterShip.Name} Lv. {Level}";

		/// <summary>
		/// HP/HPmax
		/// </summary>
		public double HPRate => HPMax > 0 ? (double)HPCurrent / HPMax : 0;

		/// <summary>
		/// 最大搭載燃料
		/// </summary>
		public int FuelMax => MasterShip.Fuel;

		/// <summary>
		/// 最大搭載弾薬
		/// </summary>
		public int AmmoMax => MasterShip.Ammo;

		/// <summary>
		/// 燃料残量割合
		/// </summary>
		public double FuelRate => (double)Fuel / Math.Max(FuelMax, 1);

		/// <summary>
		/// 弾薬残量割合
		/// </summary>
		public double AmmoRate => (double)Ammo / Math.Max(AmmoMax, 1);

		/// <summary>
		/// 補給で消費する燃料
		/// </summary>
		public int SupplyFuel => (FuelMax - Fuel) == 0 ? 0 : Math.Max((int)Math.Floor((FuelMax - Fuel) * (IsMarried ? 0.85 : 1)), 1);

		/// <summary>
		/// 補給で消費する弾薬
		/// </summary>
		public int SupplyAmmo => (AmmoMax - Ammo) == 0 ? 0 : Math.Max((int)Math.Floor((AmmoMax - Ammo) * (IsMarried ? 0.85 : 1)), 1);

		/// <summary>
		/// 搭載機残量割合
		/// </summary>
		public ReadOnlyCollection<double> AircraftRate
		{
			get
			{
				double[] airs = new double[_aircraft.Length];
				var airmax = MasterShip.Aircraft;

				for (int i = 0; i < airs.Length; i++)
				{
					airs[i] = (double)_aircraft[i] / Math.Max(airmax[i], 1);
				}

				return Array.AsReadOnly(airs);
			}
		}

		/// <summary>
		/// 搭載機残量割合
		/// </summary>
		public double AircraftTotalRate => (double)AircraftTotal / Math.Max(MasterShip.AircraftTotal, 1);

		/// <summary>
		/// 補強装備スロットが使用可能か
		/// </summary>
		public bool IsExpansionSlotAvailable => ExpansionSlot != 0;

		/// <summary>
		/// 秋刀魚装備数
		/// </summary>
		public int SanmaEquipCount => GetSanmaEquipCount();

		/// <summary>
		/// 秋刀魚装備数(爆雷分)
		/// </summary>
		public int SanmaEquipCountBomb => GetSanmaEquipCountBomb();

		/// <summary>
		/// 対潜シナジー種
		/// </summary>
		public int SynergyCount => GetSynergyCount();

		/// <summary>
		/// ステータスアップアイテム【1:海色リボン】【2:白たすき】
		/// </summary>
		public int SpItemKind { get; internal set; }

		/// <summary>
		/// ステータスアップアイテム【1:海色リボン】【2:白たすき】の火力増加分
		/// </summary>
		public int SpItemHoug { get; internal set; }

		/// <summary>
		/// ステータスアップアイテム【1:海色リボン】【2:白たすき】の雷撃増加分
		/// </summary>
		public int SpItemRaig { get; internal set; }

		/// <summary>
		/// ステータスアップアイテム【1:海色リボン】【2:白たすき】の装甲増加分
		/// </summary>
		public int SpItemSouk { get; internal set; }

		/// <summary>
		/// ステータスアップアイテム【1:海色リボン】【2:白たすき】の回避増加分
		/// </summary>
		public int SpItemKaih { get; internal set; }

		#region ダメージ威力計算

		/// <summary>
		/// 航空戦威力
		/// 本来スロットごとのものであるが、ここでは最大火力を採用する
		/// </summary>
		public int AirBattlePower => _airbattlePowers.Max();

		private int[] _airbattlePowers;
		/// <summary>
		/// 各スロットの航空戦威力
		/// </summary>
		public ReadOnlyCollection<int> AirBattlePowers => Array.AsReadOnly(_airbattlePowers);

		/// <summary>
		/// 砲撃威力(最大)
		/// </summary>
		public int ShellingPower { get; private set; }

		//todo: ShellingPower に統合予定 
		/// <summary>
		/// 空撃威力(最大)
		/// </summary>
		public int AircraftPower { get; private set; }

		/// <summary>
		/// 対潜威力
		/// </summary>
		public int AntiSubmarinePower { get; private set; }

		/// <summary>
		/// 雷撃威力
		/// </summary>
		public int TorpedoPower { get; private set; }

		/// <summary>
		/// 夜戦威力(最大)
		/// </summary>
		public int NightBattlePower { get; private set; }

		/// <summary>
		/// 装備改修補正(砲撃戦)
		/// </summary>
		private double GetDayBattleEquipmentLevelBonus()
		{
			double basepower = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;

				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.MainGunSmall:
					case EquipmentTypes.MainGunMedium:
					case EquipmentTypes.AAShell:
					case EquipmentTypes.APShell:
					case EquipmentTypes.AAGun:
					case EquipmentTypes.AADirector:
					case EquipmentTypes.LandingCraft:
					case EquipmentTypes.SpecialAmphibiousTank:
					case EquipmentTypes.Rocket:
					case EquipmentTypes.Searchlight:
					case EquipmentTypes.SearchlightLarge:
					case EquipmentTypes.AviationPersonnel:
					case EquipmentTypes.SurfaceShipPersonnel:
					case EquipmentTypes.CommandFacility:
						basepower += Math.Sqrt(slot.Level);
						break;

					case EquipmentTypes.MainGunLarge:
					case EquipmentTypes.MainGunLarge2:
						basepower += Math.Sqrt(slot.Level) * 1.5;
						break;

					case EquipmentTypes.SecondaryGun:
						switch (slot.EquipmentID)
						{
							case 10:        // 12.7cm連装高角砲
							case 66:        // 8cm高角砲
							case 220:       // 8cm高角砲改+増設機銃
							case 275:       // 10cm連装高角砲改+増設機銃
							case 464:       // 10cm連装高角砲群 集中配備
								basepower += 0.2 * slot.Level;
								break;

							case 12:        // 15.5cm三連装副砲
							case 234:       // 15.5cm三連装副砲改
							case 247:       // 15.2cm三連装砲
							case 467:       // 5inch連装砲(副砲配置) 集中配備
								basepower += 0.3 * slot.Level;
								break;

							default:
								basepower += Math.Sqrt(slot.Level);
								break;
						}
						break;

					case EquipmentTypes.Sonar:
					case EquipmentTypes.SonarLarge:
						basepower += Math.Sqrt(slot.Level) * 0.75;
						break;

					case EquipmentTypes.DepthCharge:
						if (!slot.MasterEquipment.IsDepthCharge)
							basepower += Math.Sqrt(slot.Level) * 0.75;
						break;

					case EquipmentTypes.SurfaceShipEquipment:
						switch (slot.EquipmentID)
						{
							case 500:
							case 501:
								basepower += Math.Sqrt(slot.Level);
								break;
						}
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 装備改修補正(航空要員の爆装ボーナス)
		/// </summary>
		private double GetAviationPersonnelBomberLevelBonus()
		{
			double basepower = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;

				switch (slot.MasterEquipment.CategoryType)
				{

					case EquipmentTypes.AviationPersonnel:

						if(slot.Level >= 4) basepower = 1;
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 装備改修補正(航空要員の雷撃ボーナス・航空戦で加算)
		/// </summary>
		private double GetAviationPersonnelTorpedoLevelBonus()
		{
			double basepower = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;

				switch (slot.MasterEquipment.CategoryType)
				{

					case EquipmentTypes.AviationPersonnel:

						if (slot.Level >= 5) basepower = 1;
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 装備改修補正(航空要員の火力ボーナス・夜間航空攻撃で加算)
		/// </summary>
		private double GetAviationPersonnelFirepowerLevelBonus()
		{
			double basepower = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;

				switch (slot.MasterEquipment.CategoryType)
				{

					case EquipmentTypes.AviationPersonnel:

						if (slot.Level >= 10) basepower = 3;
						else if (slot.Level >= 7) basepower = 2;
						else if (slot.Level >= 1) basepower = 1;
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 装備改修補正(雷撃戦)
		/// </summary>
		private double GetTorpedoEquipmentLevelBonus()
		{
			double basepower = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;

				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.Torpedo:
					case EquipmentTypes.AAGun:
					case EquipmentTypes.SubmarineTorpedo:
						basepower += Math.Sqrt(slot.Level) * 1.2;
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 装備改修補正(対潜)
		/// </summary>
		private double GetAntiSubmarineEquipmentLevelBonus()
		{
			double basepower = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;

				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.DepthCharge:
					case EquipmentTypes.Sonar:
						basepower += Math.Sqrt(slot.Level);
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 装備改修補正(夜戦)
		/// </summary>
		private double GetNightBattleEquipmentLevelBonus()
		{
			double basepower = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;

				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.MainGunSmall:
					case EquipmentTypes.MainGunMedium:
					case EquipmentTypes.MainGunLarge:
					case EquipmentTypes.MainGunLarge2:
					case EquipmentTypes.Torpedo:
					case EquipmentTypes.MidgetSubmarine:
					case EquipmentTypes.AAShell:
					case EquipmentTypes.APShell:
					case EquipmentTypes.AADirector:
					case EquipmentTypes.LandingCraft:
					case EquipmentTypes.SpecialAmphibiousTank:
					case EquipmentTypes.Rocket:
					case EquipmentTypes.Searchlight:
					case EquipmentTypes.SearchlightLarge:
					case EquipmentTypes.AviationPersonnel:
					case EquipmentTypes.SurfaceShipPersonnel:
					case EquipmentTypes.CommandFacility:
						basepower += Math.Sqrt(slot.Level);
						break;

					case EquipmentTypes.SubmarineTorpedo:
						basepower += Math.Sqrt(slot.Level) * 0.2;
						break;

					case EquipmentTypes.SecondaryGun:
						switch (slot.EquipmentID)
						{
							case 10:        // 12.7cm連装高角砲
							case 66:        // 8cm高角砲
							case 220:       // 8cm高角砲改+増設機銃
							case 275:       // 10cm連装高角砲改+増設機銃
							case 464:       // 10cm連装高角砲群 集中配備
								basepower += 0.2 * slot.Level;
								break;

							case 12:        // 15.5cm三連装副砲
							case 234:       // 15.5cm三連装副砲改
							case 247:       // 15.2cm三連装砲
							case 467:       // 5inch連装砲(副砲配置) 集中配備
								basepower += 0.3 * slot.Level;
								break;

							default:
								basepower += Math.Sqrt(slot.Level);
								break;
						}
						break;

					case EquipmentTypes.SurfaceShipEquipment:
						switch (slot.EquipmentID)
						{
							case 500:
							case 501:
								basepower += Math.Sqrt(slot.Level);
								break;
						}
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 耐久値による攻撃力補正
		/// </summary>
		private double GetHPDamageBonus()
		{
			if (HPRate <= 0.25)
				return 0.4;
			else if (HPRate <= 0.5)
				return 0.7;
			else
				return 1.0;
		}

		/// <summary>
		/// 耐久値による攻撃力補正(雷撃戦)
		/// </summary>
		/// <returns></returns>
		private double GetTorpedoHPDamageBonus()
		{
			if (HPRate <= 0.25)
				return 0.0;
			else if (HPRate <= 0.5)
				return 0.8;
			else
				return 1.0;
		}

		/// <summary>
		/// 交戦形態による威力補正
		/// </summary>
		private double GetEngagementFormDamageRate(int form)
		{
			switch (form)
			{
				case 1:     // 同航戦
				default:
					return 1.0;
				case 2:     // 反航戦
					return 0.8;
				case 3:     // T字有利
					return 1.2;
				case 4:     // T字不利
					return 0.6;
			}
		}

		/// <summary>
		/// 残り弾薬量による威力補正
		/// <returns></returns>
		private double GetAmmoDamageRate()
		{
			return Math.Min(Math.Floor(AmmoRate * 100) / 50.0, 1.0);
		}

		/// <summary>
		/// 連合艦隊編成における砲撃戦火力補正
		/// </summary>
		private double GetCombinedFleetShellingDamageBonus()
		{
			int fleet = Fleet;
			if (fleet == -1 || fleet > 2)
				return 0;

			switch (KCDatabase.Instance.Fleet.CombinedFlag)
			{
				case 1:     //機動部隊
					if (fleet == 1)
						return +2;
					else
						return +10;

				case 2:     //水上部隊
					if (fleet == 1)
						return +10;
					else
						return -5;

				case 3:     //輸送部隊
					if (fleet == 1)
						return -5;
					else
						return +10;

				default:
					return 0;
			}
		}

		/// <summary>
		/// 連合艦隊編成における雷撃戦火力補正
		/// </summary>
		private double GetCombinedFleetTorpedoDamageBonus()
		{
			int fleet = Fleet;
			if (fleet == -1 || fleet > 2)
				return 0;

			if (KCDatabase.Instance.Fleet.CombinedFlag == 0)
				return 0;

			return -5;
		}

		/// <summary>
		/// 軽巡軽量砲補正
		/// </summary>
		private double GetLightCruiserDamageBonus()
		{
			if (MasterShip.ShipType == ShipTypes.LightCruiser ||
				MasterShip.ShipType == ShipTypes.TorpedoCruiser ||
				MasterShip.ShipType == ShipTypes.TrainingCruiser)
			{

				int single = 0;
				int twin = 0;

				foreach (var slot in AllSlotMaster)
				{
					if (slot == -1) continue;

					switch (slot)
					{
						case 4:     // 14cm単装砲
						case 11:    // 15.2cm単装砲
							single++;
							break;
						case 65:    // 15.2cm連装砲
						case 119:   // 14cm連装砲
						case 139:   // 15.2cm連装砲改
						case 310:   // 14cm連装砲改
						case 407:   // 15.2cm連装砲改二
						case 359:   // 6inch 連装速射砲 Mk.XXI
						case 303:   // Bofors15.2cm連装砲 Model1930
						case 360:   // Bofors 15cm連装速射砲 Mk.9 Model 1938
						case 361:   // Bofors 15cm連装速射砲 Mk.9改＋単装速射砲 Mk.10改 Model 1938
						case 518:   // 14cm連装砲改二
							twin++;
							break;
					}
				}

				return Math.Sqrt(twin) * 2.0 + Math.Sqrt(single);
			}

			return 0;
		}

		/// <summary>
		/// イタリア重巡砲補正
		/// </summary>
		/// <returns></returns>
		private double GetItalianDamageBonus()
		{
			switch (ShipID)
			{
				case 448:       // Zara
				case 358:       // 改
				case 496:       // due
				case 449:       // Pola
				case 361:       // 改
					return Math.Sqrt(AllSlotMaster.Count(id => id == 162));     // √( 203mm/53 連装砲 装備数 )

				default:
					return 0;
			}
		}

		private double CapDamage(double damage, int max)
		{
			if (damage < max)
				return damage;
			else
				return max + Math.Sqrt(damage - max);
		}

		/// <summary>
		/// 航空戦での威力を求めます。
		/// </summary>
		/// <param name="slotIndex">スロットのインデックス。 0 起点です。</param>
		private int CalculateAirBattlePower(int slotIndex)
		{
			double basepower = 0;
			var slots = AllSlotInstance;

			var eq = SlotInstance[slotIndex];

			if (eq == null || _aircraft[slotIndex] == 0)
				return 0;

			switch (eq.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.CarrierBasedBomber:
				case EquipmentTypes.SeaplaneBomber:
				case EquipmentTypes.JetBomber:              // 通常航空戦においては /√2 されるが、とりあえず考えない
					basepower = (eq.MasterEquipment.Bomber + GetAviationPersonnelBomberLevelBonus()) * Math.Sqrt(_aircraft[slotIndex]) + 25;
					break;
				case EquipmentTypes.CarrierBasedTorpedo:
				case EquipmentTypes.JetTorpedo:
					// 150% 補正を引いたとする
					basepower = ((eq.MasterEquipment.Torpedo + GetAviationPersonnelTorpedoLevelBonus()) * Math.Sqrt(_aircraft[slotIndex]) + 25) * 1.5;
					break;
				default:
					return 0;
			}

			//キャップ
			basepower = Math.Floor(CapDamage(basepower, 170));

			return (int)(basepower * GetAmmoDamageRate());
		}

		/// <summary>
		/// 砲撃戦での砲撃威力を求めます。
		/// </summary>
		/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
		private int CalculateShellingPower(int engagementForm = 1)
		{
			var attackKind = Calculator.GetDayAttackKind(AllSlotMaster.ToArray(), ShipID, -1);
			if (attackKind == DayAttackKind.AirAttack || attackKind == DayAttackKind.CutinAirAttack)
				return 0;


			double basepower = FirepowerTotal + SpItemHoug + GetDayBattleEquipmentLevelBonus() + GetCombinedFleetShellingDamageBonus() + 5;

			basepower *= GetHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);

			basepower += GetLightCruiserDamageBonus() + GetItalianDamageBonus();

			// キャップ
			basepower = Math.Floor(CapDamage(basepower, 220));

			// 弾着観測射撃
			switch (attackKind)
			{
				case DayAttackKind.DoubleShelling:
				case DayAttackKind.CutinMainRadar:
					basepower *= 1.2;
					break;
				case DayAttackKind.CutinMainSub:
					basepower *= 1.1;
					break;
				case DayAttackKind.CutinMainAP:
					basepower *= 1.3;
					break;
				case DayAttackKind.CutinMainMain:
					basepower *= 1.5;
					break;
				case DayAttackKind.ZuiunMultiAngle:
					basepower *= 1.35;
					break;
				case DayAttackKind.SeaAirMultiAngle:
					basepower *= 1.3;
					break;
				
			}

			return (int)(basepower * GetAmmoDamageRate());
		}

		/// <summary>
		/// 砲撃戦での空撃威力を求めます。
		/// </summary>
		/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
		private int CalculateAircraftPower(int engagementForm = 1)
		{
			var attackKind = Calculator.GetDayAttackKind(AllSlotMaster.ToArray(), ShipID, -1);
			if (attackKind != DayAttackKind.AirAttack && attackKind != DayAttackKind.CutinAirAttack)
				return 0;

			double basepower = Math.Floor((FirepowerTotal + SpItemHoug + TorpedoTotal + SpItemRaig + Math.Floor((BomberTotal+ GetAviationPersonnelBomberLevelBonus()) * 1.3) + GetDayBattleEquipmentLevelBonus() + GetCombinedFleetShellingDamageBonus()) * 1.5) + 55;

			basepower *= GetHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);

			// キャップ
			basepower = Math.Floor(CapDamage(basepower, 220));


			// 空母カットイン
			if (attackKind == DayAttackKind.CutinAirAttack)
			{
				var kind = Calculator.GetDayAirAttackCutinKind(SlotInstanceMaster);
				switch (kind)
				{
					case DayAirAttackCutinKind.FighterBomberAttacker:
						basepower *= 1.25;
						break;

					case DayAirAttackCutinKind.BomberBomberAttacker:
						basepower *= 1.20;
						break;

					case DayAirAttackCutinKind.BomberAttacker:
						basepower *= 1.15;
						break;
				}
			}

			return (int)(basepower * GetAmmoDamageRate());
		}

		/// <summary>
		/// 砲撃戦での対潜威力を求めます。
		/// </summary>
		/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
		private int CalculateAntiSubmarinePower(int engagementForm = 1)
		{
			if (!CanAttackSubmarine)
				return 0;

			double eqpower = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;

				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.CarrierBasedBomber:
					case EquipmentTypes.CarrierBasedTorpedo:
					case EquipmentTypes.SeaplaneBomber:
					case EquipmentTypes.Sonar:
					case EquipmentTypes.DepthCharge:
					case EquipmentTypes.Autogyro:
					case EquipmentTypes.ASPatrol:
					case EquipmentTypes.SonarLarge:
						eqpower += slot.MasterEquipment.ASW;
						break;
				}
			}

			double basepower = Math.Sqrt(ASWBase) * 2 + eqpower * 1.5 + GetAntiSubmarineEquipmentLevelBonus();
			if (Calculator.GetDayAttackKind(AllSlotMaster.ToArray(), ShipID, 126, false) == DayAttackKind.AirAttack)
			{       //126=伊168; 対潜攻撃が空撃なら
				basepower += 8;
			}
			else
			{   //爆雷攻撃なら
				basepower += 13;
			}

			basepower *= GetHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);

			//対潜シナジー
			int depthChargeCount = 0;
			int depthChargeProjectorCount = 0;
			int otherDepthChargeCount = 0;
			int sonarCount = 0;         // ソナーと大型ソナーの合算
			int largeSonarCount = 0;

			foreach (var slot in AllSlotInstanceMaster)
			{
				if (slot == null)
					continue;

				switch (slot.CategoryType)
				{
					case EquipmentTypes.Sonar:
						sonarCount++;
						break;
					case EquipmentTypes.DepthCharge:
						if (slot.IsDepthCharge)
							depthChargeCount++;
						else if (slot.IsDepthChargeProjector)
							depthChargeProjectorCount++;
						else
							otherDepthChargeCount++;
						break;
					case EquipmentTypes.SonarLarge:
						largeSonarCount++;
						sonarCount++;
						break;
				}
			}

			double synergy = 1.0;
			if (sonarCount > 0 && depthChargeProjectorCount > 0 && depthChargeCount > 0)
				synergy = 1.4375;
			else if (sonarCount > 0 && (depthChargeCount + depthChargeProjectorCount + otherDepthChargeCount) > 0)
				synergy = 1.15;
			else if (depthChargeProjectorCount > 0 && depthChargeCount > 0)
				synergy = 1.1;

			basepower *= synergy;

			//キャップ
			basepower = Math.Floor(CapDamage(basepower, 170));

			return (int)(basepower * GetAmmoDamageRate());
		}

		/// <summary>
		/// 雷撃戦での威力を求めます。
		/// </summary>
		/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
		private int CalculateTorpedoPower(int engagementForm = 1)
		{
			if (TorpedoBase == 0)
				return 0;       //雷撃不能艦は除外

			double basepower = TorpedoTotal + SpItemRaig + GetTorpedoEquipmentLevelBonus() + GetCombinedFleetTorpedoDamageBonus() + 5;

			basepower *= GetTorpedoHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);

			//キャップ
			basepower = Math.Floor(CapDamage(basepower, 180));

			return (int)(basepower * GetAmmoDamageRate());
		}

		/// <summary>
		/// 夜戦での威力を求めます。
		/// TODO:処理の共通化
		/// </summary>
		private int CalculateNightBattlePower()
		{
			var kind = Calculator.GetNightAttackKind(AllSlotMaster.ToArray(), ShipID, -1);
			double basepower = 0;

			//空母カットイン
			if (kind == NightAttackKind.CutinAirAttack)
			{
				var airs = SlotInstance.Zip(Aircraft, (eq, count) => new { eq, master = eq?.MasterEquipment, count }).Where(a => a.eq != null);

				basepower = FirepowerBase + GetAviationPersonnelBomberLevelBonus()+ GetAviationPersonnelFirepowerLevelBonus() + GetAviationPersonnelTorpedoLevelBonus() +
					airs.Where(p => p.master.IsNightAircraft)
						.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +
							3 * p.count +
							0.45 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level)) +
					airs.Where(p => p.master.IsSwordfish || p.master.EquipmentID == 154 || p.master.EquipmentID == 320)   // 零戦62型(爆戦/岩井隊)、彗星一二型(三一号光電管爆弾搭載機)
						.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +
							0.3 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level));

			}
			//空撃
			else if (kind == NightAttackKind.AirAttack)
			{
				int countNightAircraft = SlotInstance.Where(eq => eq != null).Where(eq => eq?.MasterEquipment.IsNightAircraft ?? false).Count();

				// Ark Royal (改) かつ 夜間に行動可能な航空機が1つもない場合(Swordfishで夜戦が可能になっている)
				if ((ShipID == 515 || ShipID == 393) && countNightAircraft == 0)
				{
					// ソードフィッシュ系に限り装備の火力/雷装/改修値が加算される
					// 改修値はルート計算
					// ※熟練度による威力補正はクリティカル時の火力を表示していないので含まない
					basepower = FirepowerBase
						+ SlotInstanceMaster.Where(eq => eq?.IsSwordfish ?? false).Sum(eq => eq.Firepower + eq.Torpedo)
						+ SlotInstance.Where(eq => eq?.MasterEquipment.IsSwordfish ?? false).Sum(eq => Math.Sqrt(eq.Level));
				}
				//神鷹改二、大鷹改二、雲鷹改二、加賀改二護、レキシントン(改) かつ 夜間に行動可能な航空機が1つもない場合(無条件に夜戦可能・航空攻撃アニメーション)
				if ((ShipID == 529 || ShipID == 536 || ShipID == 889 || ShipID == 646 || ShipID == 735 || ShipID == 966) && countNightAircraft == 0)
				{
					// ソードフィッシュ系に限らずすべての装備の火力/雷装/改修値が加算される
					// (艦載機の改修値は暫定でルート計算)
					basepower = FirepowerBase
						+ SlotInstanceMaster.Where(eq => eq?.IsAvailable ?? false).Sum(eq => eq.Firepower + eq.Torpedo)
						+ GetNightBattleEquipmentLevelBonus()
						+ SlotInstance.Where(eq => eq?.MasterEquipment.IsAircraft ?? false).Sum(eq => Math.Sqrt(eq.Level));
				}
				//その他の場合は空母カットインと同じ計算
				else
				{
					var airs = SlotInstance.Zip(Aircraft, (eq, count) => new { eq, master = eq?.MasterEquipment, count }).Where(a => a.eq != null);

					basepower = FirepowerBase + GetAviationPersonnelBomberLevelBonus() + GetAviationPersonnelFirepowerLevelBonus() + GetAviationPersonnelTorpedoLevelBonus() +
						airs.Where(p => p.master.IsNightAircraft)
							.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +
								3 * p.count +
								0.45 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level)) +
						airs.Where(p => p.master.IsSwordfish || p.master.EquipmentID == 154 || p.master.EquipmentID == 320)   // 零戦62型(爆戦/岩井隊)、彗星一二型(三一号光電管爆弾搭載機)
							.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +
								0.3 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level));
				}
			}
			//グラーフツェッペリン(改)と未改造サラトガ固有(砲撃アニメーション)
			else if (ShipID == 353 || ShipID == 432 || ShipID == 433)
			{
				// ソードフィッシュ系に限らずすべての装備の火力/雷装/改修値が加算される
				// (艦載機の改修値は暫定でルート計算)
				basepower = FirepowerBase
					+ SlotInstanceMaster.Where(eq => eq?.IsAvailable ?? false).Sum(eq => eq.Firepower + eq.Torpedo)
					+ GetNightBattleEquipmentLevelBonus()
					+ SlotInstance.Where(eq => eq?.MasterEquipment.IsAircraft ?? false).Sum(eq => Math.Sqrt(eq.Level));
			}
			//アークロイヤル(改)の夜戦連撃
			else if (ShipID == 515 || ShipID == 393)
			{
				// ソードフィッシュ系に限り装備の火力/雷装/改修値が加算される
				// 改修値はルート計算
				// ※熟練度による威力補正はクリティカル時の火力を表示していないので含まない
				basepower = FirepowerBase
					+ SlotInstanceMaster.Where(eq => eq?.IsSwordfish ?? false).Sum(eq => eq.Firepower + eq.Torpedo)
					+ SlotInstance.Where(eq => eq?.MasterEquipment.IsSwordfish ?? false).Sum(eq => Math.Sqrt(eq.Level));
			}
			else
			{
				basepower = FirepowerTotal + TorpedoTotal + GetNightBattleEquipmentLevelBonus();
			}

			basepower *= GetHPDamageBonus();

			switch (kind)
			{
				//連撃
				case NightAttackKind.DoubleShelling:
					basepower *= 1.2;
					break;

				//主魚
				case NightAttackKind.CutinMainTorpedo:
					basepower *= 1.3;
					break;

				//魚雷
				case NightAttackKind.CutinTorpedoTorpedo:
					{
						switch (Calculator.GetNightTorpedoCutinKind(AllSlotInstanceMaster, ShipID, -1))
						{
							//潜水艦：後期艦首魚雷+潜水艦電探
							case NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment:
								basepower *= 1.75;
								break;
							//潜水艦：後期艦首魚雷*2
							case NightTorpedoCutinKind.LateModelTorpedo2:
								basepower *= 1.6;
								break;
							//水上艦
							default:
								basepower *= 1.5;
								break;
						}
					}
					break;

				//主副
				case NightAttackKind.CutinMainSub:
					basepower *= 1.75;
					break;

				//主砲
				case NightAttackKind.CutinMainMain:
					basepower *= 2.0;
					break;

				//空母夜襲カットイン
				case NightAttackKind.CutinAirAttack:
					{
						int nightFighter = SlotInstanceMaster.Count(eq => eq?.IsNightFighter ?? false);
						int nightAttacker = SlotInstanceMaster.Count(eq => eq?.IsNightAttacker ?? false);
						int nightBomber = SlotInstanceMaster.Count(eq => eq?.EquipmentID == 320);     // 彗星一二型(三一号光電管爆弾搭載機)

						if (nightFighter >= 2 && nightAttacker >= 1)
							basepower *= 1.25;
						else if (nightBomber >= 1 && nightFighter + nightAttacker >= 1)
							basepower *= 1.2;
						else if (nightFighter >= 1 && nightAttacker >= 1)
							basepower *= 1.2;
						else
							basepower *= 1.18;
					}
					break;

				//主魚電
				case NightAttackKind.CutinTorpedoRadar:
					{
						double baseModifier = 1.3;

						basepower = CalcTorpedoRaderPicket(basepower, baseModifier);
						//int typeDmod2 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 267);  // 12.7cm連装砲D型改二
						//int typeDmod3 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 366);  // 12.7cm連装砲D型改三
						//var modifierTable = new double[] { 1, 1.25, 1.4 };
						//
						//baseModifier *= modifierTable[Math.Min(typeDmod2 + typeDmod3, modifierTable.Length - 1)] * (1 + typeDmod3 * 0.05);
						//
						//basepower *= baseModifier;
					}
					
					break;

				//魚見電
				case NightAttackKind.CutinTorpedoPicket:
					{
						double baseModifier = 1.25;

						basepower = CalcTorpedoRaderPicket(basepower, baseModifier);
						//int typeDmod2 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 267);  // 12.7cm連装砲D型改二
						//int typeDmod3 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 366);  // 12.7cm連装砲D型改三
						//var modifierTable = new double[] { 1, 1.25, 1.4 };
						//
						//baseModifier *= modifierTable[Math.Min(typeDmod2 + typeDmod3, modifierTable.Length - 1)] * (1 + typeDmod3 * 0.05);
						//
						//basepower *= baseModifier;
					}
					break;

				//魚魚水
				case NightAttackKind.CutinTorpedoTorpedoMasterPicket:
					{
						basepower *= 1.5;
					}
					break;

				//ド水魚
				case NightAttackKind.CutinTorpedoDrumMasterPicket:
					{
						basepower *= 1.3;
					}
					break;

				//夜間瑞雲攻撃
				case NightAttackKind.SpecialNightZuiun:
					{
						double zuiunCountHosei = 0;
						double raderHosei = 0;

						int nightZuiunCount = AllSlotInstanceMaster.Count(eq => eq?.IsNightZuiun ?? false);
						int surfaceRaderCount = AllSlotInstanceMaster.Count(eq => eq?.IsSurfaceRadar ?? false);

						if (nightZuiunCount >= 2)
						{
							zuiunCountHosei += 0.12;
						}
						else if (nightZuiunCount >= 1)
						{
							zuiunCountHosei += 0.04;
						}
						else
						{
							//ここにきている時点で0はありえないのだが、一応…
							zuiunCountHosei = 0;
						}

						if(surfaceRaderCount >= 1)
						{
							raderHosei += 0.04;
						}
						
						basepower *= (1.2 + zuiunCountHosei + raderHosei);
					}
					break;
			}

			basepower += GetLightCruiserDamageBonus() + GetItalianDamageBonus();

			//キャップ
			basepower = Math.Floor(CapDamage(basepower, 360));

			return (int)(basepower * GetAmmoDamageRate());
		}

		/// <summary>
		/// 主魚電/魚見電カットイン火力計算サブルーチン
		/// </summary>
		/// <param name="basepower"></param>
		/// <param name="baseModifier"></param>
		/// <returns></returns>
		private double CalcTorpedoRaderPicket(double basepower, double baseModifier)
		{
			int typeDmod2 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 267);  // 12.7cm連装砲D型改二
			int typeDmod3 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 366);  // 12.7cm連装砲D型改三
			var modifierTable = new double[] { 1, 1.25, 1.4 };

			//[Note]
			//D2もD3もないならmodifierTableの0番目(すなわち1)が選ばれるように計算している
			Console.WriteLine("CalcTorpedoRaderPicket: {0}x{1}x{2}", 
				baseModifier, 
				modifierTable[Math.Min(typeDmod2 + typeDmod3, modifierTable.Length - 1)],
				1 + (typeDmod3 * 0.05)
			);

			//2023/02/28 D三2本積みでこれはどうかわるか？
			//           単純に0.05+0.05ではなさそう
			baseModifier *= modifierTable[Math.Min(typeDmod2 + typeDmod3, modifierTable.Length - 1)] * (1 + (typeDmod3 * 0.05));
			return basepower *= baseModifier;
		}

		/// <summary>
		/// 威力系の計算をまとめて行い、プロパティを更新します。
		/// </summary>
		private void CalculatePowers()
		{

			int form = Utility.Configuration.Config.Control.PowerEngagementForm;

			_airbattlePowers = Slot.Select((_, i) => CalculateAirBattlePower(i)).ToArray();
			ShellingPower = CalculateShellingPower(form);
			AircraftPower = CalculateAircraftPower(form);
			AntiSubmarinePower = CalculateAntiSubmarinePower(form);
			TorpedoPower = CalculateTorpedoPower(form);
			NightBattlePower = CalculateNightBattlePower();

		}

		#endregion

		/// <summary>
		/// 対潜攻撃可能か
		/// </summary>
		public bool CanAttackSubmarine
		{
			get
			{
				switch (MasterShip.ShipType)
				{
					case ShipTypes.Escort:
					case ShipTypes.Destroyer:
					case ShipTypes.LightCruiser:
					case ShipTypes.TorpedoCruiser:
					case ShipTypes.TrainingCruiser:
					case ShipTypes.FleetOiler:
						//対潜値がある
						return ASWBase > 0;
					case ShipTypes.AviationCruiser:
					case ShipTypes.LightAircraftCarrier:
					case ShipTypes.AviationBattleship:
					case ShipTypes.SeaplaneTender:
					case ShipTypes.AmphibiousAssaultShip:
						//対潜攻撃可能な航空機を装備している
						return AllSlotInstanceMaster.Any(eq => eq != null && eq.IsAntiSubmarineAircraft);
					case ShipTypes.AircraftCarrier:
						// 加賀改二護かつ対潜攻撃可能な航空機を装備している
						return ShipID == 646 &&	AllSlotInstanceMaster.Any(eq => eq != null && eq.IsAntiSubmarineAircraft);
					case ShipTypes.Battlecruiser:
						//大和改二かつ対潜改修済(改二重にコンバート後、対潜改修をして改二に戻したケース)
						return ShipID == 911 && ASWModernized > 1;
					default:
						return false;
				}
			}
		}

		/// <summary>
		/// 開幕対潜攻撃可能か
		/// </summary>
		public bool CanOpeningASW
		{
			get
			{
				//============================================================
				//絶対に先制対潜出来ない艦(最低条件を満たしていない)
				//============================================================
				if (!CanAttackSubmarine)
					return false;
				
				//============================================================
				//攻撃型軽空母(例外的に一切の先制対潜不可能)
				//============================================================
				switch (ShipID)
				{
					case 508:   // 鈴谷航改二
					case 509:   // 熊野航改二
						return false;
				}

				//============================================================
				//無条件先制対潜の軽巡・駆逐
				//============================================================
				switch (ShipID)
				{
					case 141:       // 五十鈴改二
					case 394:       // Jervis改
					case 478:       // 龍田改二
					case 681:       // Samuel B.Roberts改
					case 920:       // Samuel B.Roberts Mk.II
					case 562:       // Johnston
					case 689:       // Johnston改
					case 596:       // Fletcher
					case 692:       // Fletcher改
					case 628:       // Fletcher改 Mod.2
					case 629:       // Fletcher Mk.II
					case 893:       // Janus改
					case 624:       // 夕張改二丁
					case 726:       // Heywood L.E.改
					case 901:		// Javelin改
						return true;
				}

				//ここから装備を見る必要があるので装備の取得を行う
				var eqs = AllSlotInstance.Where(eq => eq != null);

				//============================================================
				//その他艦固有
				//============================================================
				switch (ShipID)
				{
					case 380:   // 大鷹改
					case 529:   // 大鷹改二
					case 381:   // 神鷹改
					case 536:   // 神鷹改二
					case 382:   // 雲鷹改
					case 889:	// 雲鷹改二
					case 646:	// 加賀改二護
						//対潜値1以上の艦攻/艦爆 or 対潜哨戒機 or 回転翼機を装備
						return eqs.Any(eq => ((eq.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedTorpedo || eq.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedBomber) && eq.MasterEquipment.ASW >= 1) ||
											   eq.MasterEquipment.CategoryType == EquipmentTypes.ASPatrol ||
											   eq.MasterEquipment.CategoryType == EquipmentTypes.Autogyro);

					case 554:   // 日向改二
						// カ号観測機, オ号観測機改, オ号観測機改二
						if (eqs.Count(eq => eq.EquipmentID == 69 || eq.EquipmentID == 324 || eq.EquipmentID == 325) >= 2)
							return true;
						// S-51J, S-51J改
						if (eqs.Any(eq => eq.EquipmentID == 326 || eq.EquipmentID == 327))
							return true;

						return false;

					case 626:	//神州丸改
					case 411:	//扶桑改二
					case 412:	//山城改二
					case 916:	//大和改二重
						//水上爆撃機 or 回転翼機 + ソナーを装備 かつ 対潜合計値100以上
						return eqs.Any(eq => (eq.MasterEquipment.CategoryType == EquipmentTypes.SeaplaneBomber || eq.MasterEquipment.CategoryType == EquipmentTypes.Autogyro)) && 
							   eqs.Any(eq => eq.MasterEquipment.IsSonar) &&
							   ASWTotal >= 100;
				}

				//============================================================
				//その他軽空母および護衛空母
				//※ガンビア・ベイMk.IIが全ソナー装備できるため、小型ソナーも判定に入れている
				//============================================================
				if (MasterShip.ShipType == ShipTypes.LightAircraftCarrier)
				{
					//対潜値7以上の艦攻 or 対潜哨戒機 or 回転翼機を装備(条件1)
					bool hasASWAircraft50_65 = eqs.Any(eq =>
						(eq.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedTorpedo && eq.MasterEquipment.ASW >= 7) ||
						eq.MasterEquipment.CategoryType == EquipmentTypes.ASPatrol ||
						eq.MasterEquipment.CategoryType == EquipmentTypes.Autogyro);
					//対潜値1以上の艦攻/艦爆を装備(条件2)
					bool hasASWAircraft100 = eqs.Any(eq =>
						((eq.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedTorpedo || eq.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedBomber) && eq.MasterEquipment.ASW >= 1));

					//対潜合計値50以上の場合は条件1かつ要ソナー
					if (hasASWAircraft50_65 && ASWTotal >= 50 && eqs.Any(eq => eq.MasterEquipment.IsSonar))
						return true;
					//対潜合計値65以上の場合は条件1かつソナー不要
					if (hasASWAircraft50_65 && ASWTotal >= 65)
						return true;
					//対潜合計値100以上の場合は条件2かつ要ソナー
					if (hasASWAircraft100 && ASWTotal >= 100 && eqs.Any(eq => eq.MasterEquipment.IsSonar))
						return true;
				}

				//============================================================
				//上記以外の艦(海防艦や駆逐等)
				//※伊勢改二もここで判定される。現状対潜値100を超えることが不可能なので必ずfalseになる
				//============================================================
				bool hasSonar = eqs.Any(eq => eq.MasterEquipment.IsSonar);
				//ソナーがいらないのは海防艦かつ装備の対潜値が合計4以上
				bool needSonar = !(
					MasterShip.ShipType == ShipTypes.Escort &&
					ASWTotal >= 75 &&
					(ASWTotal - ASWBase) >= 4);

				//要ソナーかつソナーを装備していない
				if (needSonar && !hasSonar)
					return false;

				if (MasterShip.ShipType == ShipTypes.Escort)
					//海防艦は60以上
					return ASWTotal >= 60;
				else
					//それ以外は100以上
					return ASWTotal >= 100;
			}
		}

		/// <summary>
		/// 夜戦攻撃可能か
		/// </summary>
		public bool CanAttackAtNight
		{
			get
			{
				var master = MasterShip;

				if (HPRate <= 0.25)
					return false;

				if (master.FirepowerMin + master.TorpedoMin > 0)
					return true;

				// Ark Royal(改)
				if (master.ShipID == 515 || master.ShipID == 393)
				{
					if (AllSlotInstanceMaster.Any(eq => eq != null && eq.IsSwordfish))
						return true;
				}

				if (master.IsAircraftCarrier)
				{
					// 装甲空母ではなく、中破以上の被ダメージ
					if (master.ShipType != ShipTypes.ArmoredAircraftCarrier && HPRate <= 0.5)
						return false;

					// Saratoga Mk.II/赤城改二戊/加賀改二戊/龍鳳改二戊 は不要
					bool hasNightPersonnel = master.ShipID == 545 || master.ShipID == 599 || master.ShipID == 610 || master.ShipID == 883 ||
						AllSlotInstanceMaster.Any(eq => eq != null && eq.IsNightAviationPersonnel);

					bool hasNightAircraft = AllSlotInstanceMaster.Any(eq => eq != null && eq.IsNightAircraft);

					if (hasNightPersonnel && hasNightAircraft)
						return true;
				}

				return false;
			}
		}

		/// <summary>
		/// 発動可能なダメコンのID -1=なし, 42=要員, 43=女神
		/// </summary>
		public int DamageControlID
		{
			get
			{
				if (ExpansionSlotMaster == 42 || ExpansionSlotMaster == 43)
					return ExpansionSlotMaster;

				foreach (var eq in SlotMaster)
				{
					if (eq == 42 || eq == 43)
						return eq;
				}

				return -1;
			}
		}

		/// <summary>
		/// 秋刀魚用装備カウント
		/// </summary>
		private int GetSanmaEquipCount()
		{
			int SanmaEquip = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;
				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.SeaplaneBomber:
					case EquipmentTypes.Autogyro:
					case EquipmentTypes.ASPatrol:
					case EquipmentTypes.SeaplaneRecon:
					case EquipmentTypes.FlyingBoat:
					case EquipmentTypes.Searchlight:
					case EquipmentTypes.SearchlightLarge:
					case EquipmentTypes.SurfaceShipPersonnel:
					case EquipmentTypes.Ration:
						SanmaEquip++;
						break;
					case EquipmentTypes.Sonar:
						switch (slot.EquipmentID)
						{
							case 47:        // 三式水中探信儀
							case 438:       // 三式水中探信儀改
							case 260:       // Type124 ASDIC
							case 261:       // Type144/147 ASDIC
							case 262:       // HF/DF + Type144/147 ASDIC
								SanmaEquip++;
								break;
							default:
								break;
						}
						break;
					case EquipmentTypes.AviationPersonnel:
						switch (slot.EquipmentID)
						{
							case 258:       // 夜間作戦航空要員
							case 259:       // 夜間作戦航空要員+熟練甲板員
								SanmaEquip++;
								break;
							default:
								break;
						}
						break;
					case EquipmentTypes.CarrierBasedTorpedo:
						switch (slot.EquipmentID)
						{
							case 242:       // Swordfish
							case 243:       // Swordfish Mk.II(熟練)
							case 244:       // Swordfish Mk.III(熟練)
								SanmaEquip++;
								break;
							default:
								break;
						}
						break;
					case EquipmentTypes.DepthCharge:
						SanmaEquip++;
						break;
					default:
						break;
				}
			}
			return SanmaEquip;
		}

		/// <summary>
		/// 秋刀魚用装備カウント(爆雷分)
		/// </summary>
		private int GetSanmaEquipCountBomb()
		{
			int BombEquip = 0;
			foreach (var slot in AllSlotInstance)
			{
				if (slot == null)
					continue;
				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.DepthCharge:
						BombEquip++;
						break;
					default:
						break;
				}
			}
			return BombEquip;
		}

		/// <summary>
		/// 対潜シナジー装備の種類数を求めます。
		/// </summary>
		private int GetSynergyCount()
		{
			int depthChargeCount2 = 0;
			int depthChargeProjectorCount2 = 0;
			int otherDepthChargeCount2 = 0;
			int sonarCount2 = 0;         // ソナーと大型ソナーの合算
			int largeSonarCount2 = 0;
			foreach (var slot in AllSlotInstanceMaster)
			{
				if (slot == null)
					continue;
				switch (slot.CategoryType)
				{
					case EquipmentTypes.Sonar:
						sonarCount2++;
						break;
					case EquipmentTypes.DepthCharge:
						if (slot.IsDepthCharge)
							depthChargeCount2++;
						else if (slot.IsDepthChargeProjector)
							depthChargeProjectorCount2++;
						else
							otherDepthChargeCount2++;
						break;
					case EquipmentTypes.SonarLarge:
						largeSonarCount2++;
						sonarCount2++;
						break;
				}
			}

			if (sonarCount2 > 0 && depthChargeProjectorCount2 > 0 && depthChargeCount2 > 0)
				return 3;
			else if (sonarCount2 > 0 && (depthChargeCount2 + depthChargeProjectorCount2 + otherDepthChargeCount2) > 0)
				return 2;
			else if (depthChargeProjectorCount2 > 0 && depthChargeCount2 > 0)
				return 1;
			return 0;
		}

		public int ID => MasterID;
		public override string ToString() => $"[{MasterID}] {NameWithLevel}";

		public override void LoadFromResponse(string apiname, dynamic data)
		{

			switch (apiname)
			{
				default:
					base.LoadFromResponse(apiname, (object)data);

					HPCurrent = (int)RawData.api_nowhp;
					Fuel = (int)RawData.api_fuel;
					Ammo = (int)RawData.api_bull;
					Condition = (int)RawData.api_cond;
					Slot = Array.AsReadOnly((int[])RawData.api_slot);
					ExpansionSlot = (int)RawData.api_slot_ex;
					_aircraft = (int[])RawData.api_onslot;
					_modernized = (int[])RawData.api_kyouka;

					if (data.api_sp_effect_items()) // 海色リボンか白タスキを持っているかどうか
					{
						SpItemKind = (int)RawData.api_sp_effect_items[0].api_kind;
						switch(SpItemKind)
						{
							case 1: //海色リボン
								SpItemRaig = (int)RawData.api_sp_effect_items[0].api_raig;
								SpItemSouk = (int)RawData.api_sp_effect_items[0].api_souk;
								break;
							case 2: //白タスキ
								SpItemHoug = (int)RawData.api_sp_effect_items[0].api_houg;
								SpItemKaih = (int)RawData.api_sp_effect_items[0].api_kaih;
								break;
						}

					}
					break;

				case "api_req_hokyu/charge":
					Fuel = (int)data.api_fuel;
					Ammo = (int)data.api_bull;
					_aircraft = (int[])data.api_onslot;
					break;
			}

			CalculatePowers();
		}

		public override void LoadFromRequest(string apiname, Dictionary<string, string> data)
		{
			base.LoadFromRequest(apiname, data);

			KCDatabase db = KCDatabase.Instance;

			switch (apiname)
			{
				case "api_req_kousyou/destroyship":
					{
						for (int i = 0; i < Slot.Count; i++)
						{
							if (Slot[i] == -1)
								continue;

							db.Equipments.Remove(Slot[i]);
						}
					}
					break;

				case "api_req_kaisou/open_exslot":
					ExpansionSlot = -1;
					break;
			}
		}

		/// <summary>
		/// 入渠完了時の処理を行います。
		/// </summary>
		internal void Repair()
		{
			HPCurrent = HPMax;
			Condition = Math.Max(Condition, 40);

			RawData.api_ndock_time = 0;
			RawData.api_ndock_item[0] = 0;
			RawData.api_ndock_item[1] = 0;
		}
	}
}