﻿using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Data
{
    /// <summary>
    /// 個別の攻撃威力を計算します。
    /// </summary>
    public class CalcShipAttackPower
    {

		#region 昼戦威力計算
		/// <summary>
		/// 昼戦での各種砲撃・空撃威力を求めます。
		/// </summary>
		/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
		public static int[] CalculateDayAttackPowers(ShipData ship)
		{

			var shipID = ship.ShipID;
			var masterShip = ship.MasterShip;
			var fleet = ship.Fleet;

			var firepowerTotal = ship.FirepowerTotal;
			var torpedoTotal = ship.TorpedoTotal;
			var bomberTotal = ship.BomberTotal;
			var spItemHoug = ship.SpItemHoug;
			var spItemRaig = ship.SpItemRaig;
			var hpRate = ship.HPRate;
			var ammoRate = ship.AmmoRate;
			var allSlotMaster = ship.AllSlotMaster.ToArray();
			var allSlotInstance = ship.AllSlotInstance.ToArray();
			var allSlotInstanceMaster = ship.AllSlotInstanceMaster;
			var attackKind = Calculator2.GetDayAttackKindList(allSlotMaster.ToArray(), shipID);

			int engagementForm = 1;
			double basepower = 0;
			int[] returnBasepower = new int[attackKind.Count()];

			foreach (var attack in attackKind.Select((name, number) => new { name, number }))
			{
				if (attack.name == DayAttackKind.AirAttack
						|| attack.name == DayAttackKind.CutinAirAttack
						|| attack.name == DayAttackKind.CutinFighterBomberAttacker
						|| attack.name == DayAttackKind.CutinBomberBomberAttacker
						|| attack.name == DayAttackKind.CutinBomberAttacker)
				{
					//空撃の基本火力
					basepower = Math.Floor((firepowerTotal + spItemHoug + torpedoTotal + spItemRaig + Math.Floor((bomberTotal + GetAviationPersonnelBomberLevelBonus(allSlotInstance)) * 1.3) + GetDayBattleEquipmentLevelBonus(allSlotInstance) + GetCombinedFleetShellingDamageBonus(fleet)) * 1.5) + 55;

					basepower *= GetHPDamageBonus(hpRate) * GetEngagementFormDamageRate(engagementForm);

					// キャップ
					basepower = Math.Floor(CapDamage(basepower, 220));
				}
				else
				{
					//砲撃の基本火力
					basepower = firepowerTotal + spItemHoug + GetDayBattleEquipmentLevelBonus(allSlotInstance) + GetCombinedFleetShellingDamageBonus(fleet) + 5;

					basepower *= GetHPDamageBonus(hpRate) * GetEngagementFormDamageRate(engagementForm);

					basepower += GetLightCruiserDamageBonus(masterShip, allSlotMaster) + GetItalianDamageBonus(shipID, allSlotMaster);

					// キャップ
					basepower = Math.Floor(CapDamage(basepower, 220));
				}
				// カットイン攻撃
				switch (attack.name)
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

					case DayAttackKind.CutinFighterBomberAttacker:
						basepower *= 1.25;
						break;
					case DayAttackKind.CutinBomberBomberAttacker:
						basepower *= 1.20;
						break;
					case DayAttackKind.CutinBomberAttacker:
						basepower *= 1.15;
						break;

				}
				returnBasepower[attack.number] = (int)(basepower * GetAmmoDamageRate(ammoRate));
			}
			return returnBasepower;
		}
		#endregion

		#region 夜戦威力計算
		/// <summary>
		/// 夜戦での攻撃種別ごとの威力を求めます。
		/// </summary>
		public static int[] CalculateNightAttackPowers(ShipData ship)
		{
			var shipID = ship.ShipID;
			var masterShip = ship.MasterShip;
			var fleet = ship.Fleet;

			var firepowerTotal = ship.FirepowerTotal;
			var firepowerBase = ship.FirepowerBase;
			var torpedoTotal = ship.TorpedoTotal;
			var bomberTotal = ship.BomberTotal;
			var spItemHoug = ship.SpItemHoug;
			var spItemRaig = ship.SpItemRaig;
			var hpRate = ship.HPRate;
			var ammoRate = ship.AmmoRate;
			var allSlotMaster = ship.AllSlotMaster.ToArray();
			var allSlotInstance = ship.AllSlotInstance.ToArray();
			var allSlotInstanceMaster = ship.AllSlotInstanceMaster;
			var slotInstance = ship.SlotInstance;
			var slotInstanceMaster = ship.SlotInstanceMaster;
			var nightKind = Calculator2.GetNightAttackKindList(allSlotMaster.ToArray(), shipID);

			double basepower = 0;
			int[] returnBasepower = new int[nightKind.Count()];

			foreach (var kind in nightKind.Select((name, number) => new { name, number }))
			{

				basepower = firepowerTotal + torpedoTotal + spItemHoug + spItemRaig + GetNightBattleEquipmentLevelBonus(allSlotInstance);
				basepower *= GetHPDamageBonus(hpRate);

				switch (kind.name)
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
						basepower *= 1.5;
						break;
					case NightAttackKind.CutinTorpedoMasterPicketSubmarine:
						basepower *= 1.75;
						break;
					case NightAttackKind.CutinTorpedoTorpedoSubmarine:
						basepower *= 1.6;
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
					case NightAttackKind.CutinNightAirAttackFFA:
						basepower = CalculateNightAirAttackBasepower(ship) * 1.25;
						break;
					case NightAttackKind.CutinNightAirAttackFA:
					case NightAttackKind.CutinNightAirAttackFS:
					case NightAttackKind.CutinNightAirAttackAS:
						basepower = CalculateNightAirAttackBasepower(ship) * 1.2;
						break;
					case NightAttackKind.CutinNightAirAttackFOther:
						basepower = CalculateNightAirAttackBasepower(ship) * 1.18;
						break;
					case NightAttackKind.NightAirAttack:
						basepower = CalculateNightAirAttackBasepower(ship);
						break;

					case NightAttackKind.NightSwordfish:
						// ソードフィッシュ系に限り装備の火力/雷装/改修値が加算される
						// 改修値はルート計算
						// ※熟練度による威力補正はクリティカル時の火力を表示していないので含まない
						basepower = firepowerBase
							+ slotInstanceMaster.Where(eq => eq?.IsSwordfish ?? false).Sum(eq => eq.Firepower + eq.Torpedo)
							+ slotInstance.Where(eq => eq?.MasterEquipment.IsSwordfish ?? false).Sum(eq => Math.Sqrt(eq.Level));
						break;
					case NightAttackKind.AirAttack:
						// ソードフィッシュ系に限らずすべての装備の火力/雷装/改修値が加算される
						// (艦載機の改修値は暫定でルート計算)
						basepower = firepowerBase
							+ slotInstanceMaster.Where(eq => eq?.IsAvailable ?? false).Sum(eq => eq.Firepower + eq.Torpedo)
							+ GetNightBattleEquipmentLevelBonus(allSlotInstance)
							+ slotInstance.Where(eq => eq?.MasterEquipment.IsAircraft ?? false).Sum(eq => Math.Sqrt(eq.Level));
						break;

					//主魚電
					case NightAttackKind.CutinTorpedoRadar_:
						{
							double baseModifier = 1.3;

							basepower = CalcTorpedoRaderPicket(basepower, baseModifier, ship);
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
					case NightAttackKind.CutinTorpedoPicket_:
						{
							double baseModifier = 1.25;

							basepower = CalcTorpedoRaderPicket(basepower, baseModifier, ship);
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
					case NightAttackKind.CutinTorpedoTorpedoMasterPicket_:
						basepower *= 1.5;
						break;

					//ド水魚
					case NightAttackKind.CutinTorpedoDrumMasterPicket_:
						basepower *= 1.3;
						break;

					//夜間瑞雲攻撃
					case NightAttackKind.SpecialNightZuiun:
						{
							double zuiunCountHosei = 0;
							double raderHosei = 0;

							int nightZuiunCount = allSlotInstanceMaster.Count(eq => eq?.IsNightZuiun ?? false);
							int surfaceRaderCount = allSlotInstanceMaster.Count(eq => eq?.IsSurfaceRadar ?? false);

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

							if (surfaceRaderCount >= 1)
							{
								raderHosei += 0.04;
							}

							basepower *= (1.2 + zuiunCountHosei + raderHosei);
						}
						break;
				}
				basepower += GetLightCruiserDamageBonus(masterShip, allSlotMaster) + GetItalianDamageBonus(shipID, allSlotMaster);

				//キャップ
				basepower = Math.Floor(CapDamage(basepower, 360));

				returnBasepower[kind.number] = (int)(basepower * GetAmmoDamageRate(ammoRate));
			}
			return returnBasepower;
		}

		/// <summary>
		/// 夜間航空攻撃の基本攻撃力
		/// </summary>
		private static double CalculateNightAirAttackBasepower(ShipData ship)
		{
			var allSlotInstance = ship.AllSlotInstance.ToArray();
			var slotInstance = ship.SlotInstance;
			var aircraft = ship.Aircraft;
			var firepowerBase = ship.FirepowerBase;
			var airs = slotInstance.Zip(aircraft, (eq, count) => new { eq, master = eq?.MasterEquipment, count }).Where(a => a.eq != null);
			return firepowerBase + GetAviationPersonnelBomberLevelBonus(allSlotInstance) + GetAviationPersonnelFirepowerLevelBonus(allSlotInstance) + GetAviationPersonnelTorpedoLevelBonus(allSlotInstance) +
				airs.Where(p => p.master.IsNightAircraft)
					.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +
						3 * p.count +
						0.45 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level)) +
				airs.Where(p => p.master.IsSwordfish || p.master.EquipmentID == 154 || p.master.EquipmentID == 320)   // 零戦62型(爆戦/岩井隊)、彗星一二型(三一号光電管爆弾搭載機)
					.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +
						0.3 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level));

		}

		/// <summary>
		/// 主魚電/魚見電カットイン火力計算サブルーチン
		/// </summary>
		/// <param name="basepower"></param>
		/// <param name="baseModifier"></param>
		/// <returns></returns>
		private static double CalcTorpedoRaderPicket(double basepower, double baseModifier, ShipData ship)
		{
			int typeDmod2 = ship.AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 267);  // 12.7cm連装砲D型改二
			int typeDmod3 = ship.AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 366);  // 12.7cm連装砲D型改三
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
		#endregion

		#region 支援艦隊攻撃
		/// <summary>
		/// 砲撃支援での威力を求めます
		/// </summary>
		public static int CalculateSupportShellingPower(ShipData ship)
		{
			var shipID = ship.ShipID;
			var allSlotInstance = ship.AllSlotInstance;
			var firepowerTotal = ship.FirepowerTotal;
			var torpedoTotal = ship.TorpedoTotal;
			var bomberTotal = ship.BomberTotal;
			var spItemHoug = ship.SpItemHoug;
			var spItemRaig = ship.SpItemRaig;

			int aircraftcheck = 0;
			double basepower = 0;

			ShipDataMaster attacker = KCDatabase.Instance.MasterShips[shipID];
			if (attacker?.ShipType == ShipTypes.AircraftCarrier
				|| attacker?.ShipType == ShipTypes.ArmoredAircraftCarrier
				|| attacker?.ShipType == ShipTypes.LightAircraftCarrier)
			{
				foreach (var slot in allSlotInstance)
				{
					if (slot == null)
						continue;
					switch (slot.MasterEquipment.CategoryType)
					{
						case EquipmentTypes.CarrierBasedBomber:
						case EquipmentTypes.CarrierBasedTorpedo:
						case EquipmentTypes.JetBomber:
						case EquipmentTypes.JetTorpedo:
							aircraftcheck++;
							break;
					}
				}
				if (aircraftcheck > 0)
					basepower = Math.Floor((firepowerTotal + spItemHoug + torpedoTotal + spItemRaig + Math.Floor(bomberTotal + GetAviationPersonnelBomberLevelBonus(allSlotInstance.ToArray()) * 1.3) - 1) * 1.5) + 55;
				else basepower = 0;
			}
			else
			{
				basepower = firepowerTotal + spItemHoug + 4;
			}
			basepower = Math.Floor(CapDamage(basepower, 170));
			return (int)basepower;
		}

		/// <summary>
		/// 航空支援での威力を求めます。
		/// </summary>
		/// <param name="slotIndex">スロットのインデックス。 0 起点です。</param>
		public static int CalculateSupportAirclaftPower(int slotIndex, ShipData ship)
		{
			var slotInstance = ship.SlotInstance;
			var aircraft = ship.Aircraft;

			double basepower = 0;
			var eq = slotInstance[slotIndex];

			if (eq == null)
				return -1;
			if (aircraft[slotIndex] == 0)
				return 0;

			switch (eq.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.CarrierBasedBomber:
				case EquipmentTypes.SeaplaneBomber:
				case EquipmentTypes.JetBomber:              // 通常航空戦においては /√2 されるが、とりあえず考えない
					basepower = eq.MasterEquipment.Bomber * Math.Sqrt(aircraft[slotIndex]) + 3;
					break;
				case EquipmentTypes.CarrierBasedTorpedo:
				case EquipmentTypes.JetTorpedo:
					// 150% 補正を引いたとする
					basepower = (eq.MasterEquipment.Torpedo * Math.Sqrt(aircraft[slotIndex]) + 3) * 1.5;
					break;
				default:
					return 0;
			}

			//キャップ
			basepower = Math.Floor(CapDamage(basepower, 170));
			//キャップ後補正
			return (int)(basepower * 1.35);
		}

		/// <summary>
		/// 航空支援での対潜威力を求めます。
		/// </summary>
		/// <param name="slotIndex">スロットのインデックス。 0 起点です。</param>
		public static double CalculateSupportAntiSubmarinePower(int slotIndex, ShipData ship)
		{
			var slotInstance = ship.SlotInstance;
			var aircraft = ship.Aircraft;

			int basepower = 0;
			var eq = slotInstance[slotIndex];

			if (eq == null)
				return -1;
			if (aircraft[slotIndex] == 0)
				return 0;

			switch (eq.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.CarrierBasedBomber:
				case EquipmentTypes.SeaplaneBomber:
				case EquipmentTypes.CarrierBasedTorpedo:
				case EquipmentTypes.ASPatrol:
				case EquipmentTypes.Autogyro:
					basepower = (int)(Math.Floor(eq.MasterEquipment.ASW * 0.6) * Math.Sqrt(aircraft[slotIndex]) + 3);
					break;
				default:
					return 0;
			}

			//キャップ
			basepower = (int)Math.Floor(CapDamage(basepower, 170));
			//キャップ後補正
			return basepower * 1.75;
		}
		#endregion

		#region 各種補正
		/// <summary>
		/// 装備改修補正(砲撃戦)
		/// </summary>
		private static double GetDayBattleEquipmentLevelBonus(EquipmentData[] allSlotInstance)
        {
            double basepower = 0;

			foreach (var slot in allSlotInstance)
            {
                if (slot == null)
                    continue;

                switch (slot.MasterEquipment.CategoryType)
                {
                    case EquipmentTypes.MainGunSmall:
                    case EquipmentTypes.MainGunMedium:
                    case EquipmentTypes.APShell:
                    case EquipmentTypes.AADirector:
                    case EquipmentTypes.Searchlight:
                    case EquipmentTypes.SearchlightLarge:
                    case EquipmentTypes.AAGun:
                    case EquipmentTypes.LandingCraft:
                    case EquipmentTypes.SpecialAmphibiousTank:
					case EquipmentTypes.AviationPersonnel:
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
                                basepower += 0.2 * slot.Level;
                                break;

                            case 12:        // 15.5cm三連装副砲
                            case 234:       // 15.5cm三連装副砲改
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
                }
            }
            return basepower;
        }

		/// <summary>
		/// 装備改修補正(航空要員の爆装ボーナス)
		/// </summary>
		private static double GetAviationPersonnelBomberLevelBonus(EquipmentData[] allSlotInstance)
		{
			double basepower = 0;
			foreach (var slot in allSlotInstance)
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
		private static double GetAviationPersonnelTorpedoLevelBonus(EquipmentData[] allSlotInstance)
		{
			double basepower = 0;
			foreach (var slot in allSlotInstance)
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
		private static double GetAviationPersonnelFirepowerLevelBonus(EquipmentData[] allSlotInstance)
		{
			double basepower = 0;
			foreach (var slot in allSlotInstance)
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
        /// 装備改修補正(夜戦)
        /// </summary>
        private static double GetNightBattleEquipmentLevelBonus(EquipmentData[] allSlotInstance)
        {
            double basepower = 0;
			foreach (var slot in allSlotInstance)
            {
                if (slot == null)
                    continue;

                switch (slot.MasterEquipment.CategoryType)
                {
                    case EquipmentTypes.MainGunSmall:
                    case EquipmentTypes.MainGunMedium:
                    case EquipmentTypes.MainGunLarge:
                    case EquipmentTypes.Torpedo:
                    case EquipmentTypes.APShell:
                    case EquipmentTypes.LandingCraft:
                    case EquipmentTypes.Searchlight:
                    case EquipmentTypes.SubmarineTorpedo:
                    case EquipmentTypes.AADirector:
                    case EquipmentTypes.MainGunLarge2:
                    case EquipmentTypes.SearchlightLarge:
                    case EquipmentTypes.SpecialAmphibiousTank:
					case EquipmentTypes.AviationPersonnel:
						basepower += Math.Sqrt(slot.Level);
                        break;

                    case EquipmentTypes.SecondaryGun:
                        switch (slot.EquipmentID)
                        {
                            case 10:        // 12.7cm連装高角砲
                            case 66:        // 8cm高角砲
                            case 220:       // 8cm高角砲改+増設機銃
                            case 275:       // 10cm連装高角砲改+増設機銃
                                basepower += 0.2 * slot.Level;
                                break;

                            case 12:        // 15.5cm三連装副砲
                            case 234:       // 15.5cm三連装副砲改
                                basepower += 0.3 * slot.Level;
                                break;

                            default:
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
        private static double GetHPDamageBonus(double hpRate)
        {
            if (hpRate <= 0.25)
                return 0.4;
            else if (hpRate <= 0.5)
                return 0.7;
            else
                return 1.0;
        }

        /// <summary>
        /// 耐久値による攻撃力補正(雷撃戦)
        /// </summary>
        /// <returns></returns>
        private static double GetTorpedoHPDamageBonus(double hpRate)
        {
            if (hpRate <= 0.25)
                return 0.0;
            else if (hpRate <= 0.5)
                return 0.8;
            else
                return 1.0;
        }

        /// <summary>
        /// 交戦形態による威力補正
        /// </summary>
        private static double GetEngagementFormDamageRate(int form = 1)
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
        private static double GetAmmoDamageRate(double ammoRate)
        {
            return Math.Min(Math.Floor(ammoRate * 100) / 50.0, 1.0);
        }

        /// <summary>
        /// 連合艦隊編成における砲撃戦火力補正
        /// </summary>
        private static double GetCombinedFleetShellingDamageBonus(int fleet)
        {
            
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
        private static double GetCombinedFleetTorpedoDamageBonus(int fleet)
        {
            if (fleet == -1 || fleet > 2)
                return 0;

            if (KCDatabase.Instance.Fleet.CombinedFlag == 0)
                return 0;

            return -5;
        }

        /// <summary>
        /// 軽巡軽量砲補正
        /// </summary>
        private static double GetLightCruiserDamageBonus(ShipDataMaster masterShip , int[] allSlotMaster)
        {
            if (masterShip.ShipType == ShipTypes.LightCruiser ||
                masterShip.ShipType == ShipTypes.TorpedoCruiser ||
                masterShip.ShipType == ShipTypes.TrainingCruiser)
            {

                int single = 0;
                int twin = 0;

                foreach (var slot in allSlotMaster)
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
        private static double GetItalianDamageBonus(int shipID, int[] allSlotMaster)
        {
            switch (shipID)
            {
                case 448:       // Zara
                case 358:       // 改
                case 496:       // due
                case 449:       // Pola
                case 361:       // 改
                    return Math.Sqrt(allSlotMaster.Count(id => id == 162));     // √( 203mm/53 連装砲 装備数 )

                default:
                    return 0;
            }
        }

		/// <summary>
		/// キャップ処理
		/// </summary>
		/// <returns></returns>
		private static double CapDamage(double damage, int max)
        {
            if (damage < max)
                return damage;
            else
                return max + Math.Sqrt(damage - max);
        }
		#endregion

		#region 対地威力計算
		/// <summary>
		/// 対地攻撃力(昼戦)
		/// </summary>
		public static int CaliculateDayGroundAtttackPower(ShipData ship, int skin)
		{
			var shipID = ship.ShipID;
			var fleet = ship.Fleet;
			var firepowerTotal = ship.FirepowerTotal;
			var spItemHoug = ship.SpItemHoug;
			var slotInstanceMaster = ship.SlotInstanceMaster;
			var allSlotInstance = ship.AllSlotInstance.ToArray();
			var antiGroundBomber = 0;
			double[] rateDD = new double[4] { 1.4, 1.0, 1.0, 1.0 };
			double[] bonusSub = new double[4] { 30, 30, 30, 30 };
			double basepower = 0;

			ShipDataMaster attacker = KCDatabase.Instance.MasterShips[shipID];
			if (attacker?.ShipType == ShipTypes.AircraftCarrier
				|| attacker?.ShipType == ShipTypes.ArmoredAircraftCarrier
				|| attacker?.ShipType == ShipTypes.LightAircraftCarrier)
			{
				foreach (var slot in allSlotInstance)
				{
					if (slot == null)
						continue;
					switch (slot.EquipmentID)
					{

						case 319:
						case 320:
						case 391:
						case 392:
						case 148:
						case 277:
						case 233:
						case 474:
						case 420:
						case 421:
						case 64:
						case 305:
						case 306:
							antiGroundBomber++;
							break;
					}
				}

				var rateBomb = Math.Floor(slotInstanceMaster.Where(eq => eq?.IsAntiGroundBomber ?? false).Sum(eq => eq.Bomber) * 1.3);
				if (antiGroundBomber != 0)
				{
					basepower = spItemHoug + firepowerTotal + GetDayBattleEquipmentLevelBonus(allSlotInstance) + 5 + GetCombinedFleetShellingDamageBonus(fleet);
					basepower = GetGroundEnemyAttackPower(ship, skin, basepower, true);
					basepower = Math.Floor((basepower + rateBomb + 15) * 1.5) + 25;
					basepower = Math.Floor(CapDamage(basepower, 220));
				}
				else
				{
					basepower = Math.Floor(spItemHoug + firepowerTotal + GetDayBattleEquipmentLevelBonus(allSlotInstance) + rateBomb + GetCombinedFleetShellingDamageBonus(fleet)*1.5) + 55;
					basepower = Math.Floor(CapDamage(basepower, 220));
				}
			}
			else
			{
				if (attacker?.ShipType == ShipTypes.Destroyer)
				{
					basepower *= rateDD[skin];
				}
				if (attacker?.ShipType == ShipTypes.Submarine || attacker?.ShipType == ShipTypes.SubmarineAircraftCarrier)
				{
					basepower += bonusSub[skin];
				}
				basepower = spItemHoug + firepowerTotal + GetDayBattleEquipmentLevelBonus(allSlotInstance) + 5 + GetCombinedFleetShellingDamageBonus(fleet);

				basepower = GetGroundEnemyAttackPower(ship, skin, basepower, true);
				basepower = Math.Floor(CapDamage(basepower, 220));
				if (skin == 3)
					basepower = GetDayGroundAttackAfterCAP(ship, basepower);
			}

			return (int)basepower;
		}

		/// <summary>
		/// 対地攻撃力(夜戦)
		/// </summary>
		public static int CaliculateNightGroundAtttackPower(ShipData ship, int skin)
		{
			var shipID = ship.ShipID;
			var fleet = ship.Fleet;
			var firepowerBase = ship.FirepowerBase;
			var firepowerTotal = ship.FirepowerTotal;
			var spItemHoug = ship.SpItemHoug;
			var allSlotInstance = ship.AllSlotInstance.ToArray();
			var slotInstanceMaster = ship.SlotInstanceMaster;
			var slotInstance = ship.SlotInstance;
			var aircraft = ship.Aircraft;
			var airs = slotInstance.Zip(aircraft, (eq, count) => new { eq, master = eq?.MasterEquipment, count }).Where(a => a.eq != null); 
			
			double[] rateDD = new double[4] { 1.4, 1.0, 1.0, 1.0 };
			double[] bonusSub = new double[4] { 30, 30, 30, 30 };
			double basepower = 0;

			ShipDataMaster attacker = KCDatabase.Instance.MasterShips[shipID];
			if (attacker?.ShipType == ShipTypes.AircraftCarrier
				|| attacker?.ShipType == ShipTypes.ArmoredAircraftCarrier
				|| attacker?.ShipType == ShipTypes.LightAircraftCarrier)
			{
				basepower = (firepowerBase + GetAviationPersonnelBomberLevelBonus(allSlotInstance) + GetAviationPersonnelFirepowerLevelBonus(allSlotInstance) + GetAviationPersonnelTorpedoLevelBonus(allSlotInstance)) +
							airs.Where(p => p.master.IsNightAircraft).Sum(p => p.master.Firepower + p.master.Bomber +
								3 * p.count + 0.45 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level)) +
						     airs.Where(p => p.master.IsSwordfish || p.master.EquipmentID == 154 || p.master.EquipmentID == 320)   // 零戦62型(爆戦/岩井隊)、彗星一二型(三一号光電管爆弾搭載機)
								.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +	0.3 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level));
			}
			else
			{
				if (attacker?.ShipType == ShipTypes.Destroyer)
				{
					basepower *= rateDD[skin];
				}
				if (attacker?.ShipType == ShipTypes.Submarine || attacker?.ShipType == ShipTypes.SubmarineAircraftCarrier)
				{
					basepower += bonusSub[skin];
				}
				basepower = spItemHoug + firepowerTotal + GetNightBattleEquipmentLevelBonus(allSlotInstance) + 5;
				basepower = GetGroundEnemyAttackPower(ship, skin, basepower, false);
				basepower = Math.Floor(CapDamage(basepower, 220));
				if (skin == 3)
					basepower = GetDayGroundAttackAfterCAP(ship, basepower);
			}
			return (int)basepower;
		}
		#endregion

		#region 対地補正計算
		/// <summary>
		/// 対地攻撃補正(配列は砲台、ハードスキン、ソフトスキン、集積地キャップ前)
		/// </summary>
		private static double GetGroundEnemyAttackPower(ShipData ship, int skin, double basepower, bool ifday)
		{
			var shipID = ship.ShipID;
			var masterShip = ship.MasterShip;
			var allSlotMaster = ship.AllSlotMaster.ToArray();
			var allSlotInstance = ship.AllSlotInstance.ToArray();
			var hpRate = ship.HPRate;

			double landigCraftLevel = 0; double[] rate_landigCraft = new double[4] { 1.8, 1.8, 1.4, 1.4 };
			double spAmphibiousTankLevel = 0;

			var aaShell = 0; double[] rate_aaShell = new double[4] { 1.0, 1.75, 2.5, 2.5 };
			var rocketWG = 0; double[] rate_rocketWG1 = new double[4] { 1.6 ,1.4, 1.3, 1.3}; double[] rate_rocketWG2 = new double[4] { 2.72, 2.1, 1.82, 1.82 };
			var rocket20 = 0; double[] rate_rocket201 = new double[4] { 1.5, 1.3, 1.25, 1.25 }; double[] rate_rocket202 = new double[4] { 2.7, 2.145, 1.875, 1.875 };
			var rocket20S = 0;
			var depthCharge = 0; double[] rate_depthCharge1 = new double[4] { 1.3, 1.2, 1.2, 1.2 }; double[] rate_depthCharge2 = new double[4] { 1.95, 1.68, 1.56, 1.56 };
			var depthChargeS = 0; 
			var daihatsu = 0; double[] rate_daihatsu1 = new double[4] { 1.0, 1.0, 1.0, 1.0 };
			var tokudaihatsu = 0; double[] rate_tokudaihatsu1 = new double[4] { 1.15, 1.15, 1.15, 1.15 };
			var rikusen = 0; double[] rate_rikusenisshiki1 = new double[4] { 1.5, 1.2, 1.5, 1.5 }; double[] rate_rikusenisshiki2 = new double[4] { 2.1, 1.68, 1.95, 1.95 };
			var isshiki = 0; 
			var elevenReg = 0; double[] rate_elevenReg1 = new double[4] { 1.0, 1.0, 1.0, 1.0 };
			var no2Tank = 0; double[] rate_no2Tank1 = new double[4] { 1.5, 1.2, 1.5, 1.5 }; double[] rate_no2Tank2 = new double[4] { 2.1, 1.68, 1.95, 1.95 };
			var no3Tank = 0; double[] rate_no3Tank1 = new double[4] { 1.0, 1.0, 1.0, 1.0 };
			var m4A1DD = 0; double[] rate_m4A1DD = new double[4] { 2.0, 1.8, 1.1, 1.1 };
			var busouDaihatsu = 0; double[] rate_busouAB1 = new double[4] { 1.3, 1.3, 1.1, 1.1 }; double[] rate_busouAB2 = new double[4] { 1.56, 1.43, 1.21, 1.21 };
			var daihaysuAB = 0;
			var chiha = 0;
			var spAmphibiousTank = 0; double[] rate_spAmphibiousTank1 = new double[4] { 2.4, 2.4, 1.5, 1.5 }; double[] rate_spAmphibiousTank2 = new double[4] { 3.24, 3.24, 1.8, 1.8 };
			var apShell = 0; double[] rate_apShell = new double[4] { 1.85, 1.0, 1.0, 1.0 };
			var seaPlane = 0; double[] rate_seaPlane = new double[4] { 1.5, 1.0, 1.2, 1.2 };
			var Bomber = 0; double[] rate_Bomber1 = new double[4] { 1.5, 1.4, 1.0, 1.0 }; double[] rate_Bomber2 = new double[4] { 3.0, 2.45, 1.0, 1.0 };

			foreach (var slot in allSlotInstance)
			{
				if (slot == null)
					continue;
				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.AAShell:
						aaShell++;
						break;
					case EquipmentTypes.APShell:
						apShell++;
						break;
					case EquipmentTypes.LandingCraft:
						landigCraftLevel += slot.Level;
						break;
					case EquipmentTypes.SeaplaneFighter:
					case EquipmentTypes.SeaplaneBomber:
						seaPlane++;
						break;
					case EquipmentTypes.CarrierBasedBomber:
					case EquipmentTypes.JetBomber:
						Bomber++;
						break;
				}

				switch (slot.EquipmentID)
				{
					case 126:
						rocketWG++;
						if (rocketWG > 4) rocketWG = 4;
						break;
					case 348:
						rocket20++;
						if (rocket20 > 4) rocket20 = 4;
						break;
					case 349:
						rocket20S++;
						if (rocket20S > 4) rocket20S = 4;
						break;
					case 346:
						depthCharge++;
						if (depthCharge > 4) depthCharge = 4; break;
					case 347:
						depthChargeS++;
						if (depthChargeS > 4) depthChargeS = 4; break;
					case 68:
						daihatsu++;
						break;
					case 193:
						tokudaihatsu++;
						break;
					case 166:
						rikusen++;
						break;
					case 449:
						isshiki++;
						break;
					case 230:
						elevenReg++;
						break;
					case 436:
						no2Tank++;
						break;
					case 482:
						no3Tank++;
						break;
					case 355:
						m4A1DD++;
						break;
					case 408:
						daihaysuAB++;
						break;
					case 409:
						busouDaihatsu++;
						break;
					//case 494:
					//case 495:
					//chiha++;
					//break;
					case 167:
						spAmphibiousTank++;
						spAmphibiousTankLevel += slot.Level;
						break;
				}
			}	

			//一般対地乗算補正
			{ 
				if (aaShell != 0)
					basepower *= rate_aaShell[skin];
				if (rocketWG != 0)
					if (rocketWG >= 2)
						basepower *= rate_rocketWG2[skin];
					else
						basepower *= rate_rocketWG1[skin];
				if (rocket20 + rocket20S != 0)
					if (rocket20 + rocket20S >= 2)
						basepower *= rate_rocket202[skin];
					else
						basepower *= rate_rocket201[skin];
				if (depthCharge + depthChargeS != 0)
					if (depthCharge + depthChargeS >= 2)
						basepower *= rate_depthCharge2[skin];
					else
						basepower *= rate_depthCharge1[skin];
				if ((daihatsu + tokudaihatsu + rikusen + isshiki + elevenReg + no2Tank + no3Tank + m4A1DD + daihaysuAB + busouDaihatsu) != 0)
				{
					basepower *= rate_landigCraft[skin];
					if (daihatsu != 0)
						basepower *= rate_daihatsu1[skin];
					if (tokudaihatsu != 0)
						basepower *= rate_tokudaihatsu1[skin];
					if (rikusen + isshiki != 0)
						if (rikusen + isshiki >= 2)
							basepower *= rate_rikusenisshiki2[skin];
						else
							basepower *= rate_rikusenisshiki1[skin];
					if (elevenReg != 0)
						basepower *= rate_elevenReg1[skin];
					if (no3Tank != 0)
						basepower *= rate_no3Tank1[skin];
					if (no2Tank != 0)
						if (no2Tank >= 2)
							basepower *= rate_no2Tank2[skin];
						else
							basepower *= rate_no2Tank1[skin];
					if (m4A1DD != 0)
						basepower *= rate_m4A1DD[skin];
					if(ifday == true)
					{ 
						if (daihaysuAB + busouDaihatsu != 0)
							if (daihaysuAB + busouDaihatsu >= 2)
								basepower *= rate_busouAB2[skin];
							else
								basepower *= rate_busouAB1[skin];
					}
					if (landigCraftLevel != 0)
						basepower *= (landigCraftLevel / (daihatsu + tokudaihatsu + rikusen + isshiki + elevenReg + no2Tank + no3Tank + m4A1DD + daihaysuAB + busouDaihatsu) / 50) + 1;
				}
				if (spAmphibiousTank != 0)
				{
					if (spAmphibiousTank >= 2)
						basepower *= rate_spAmphibiousTank2[skin];
					else
						basepower *= rate_spAmphibiousTank1[skin];
					if (spAmphibiousTankLevel != 0)
						basepower *= (spAmphibiousTankLevel / spAmphibiousTank / 30) + 1;
				}
				if (apShell != 0)
					basepower *= rate_apShell[skin];
				if (seaPlane != 0)
					basepower *= rate_seaPlane[skin];
				if (Bomber != 0)
					if (Bomber >= 2)
						basepower *= rate_Bomber2[skin];
					else
						basepower *= rate_Bomber1[skin];
			}

			//11連隊・一式砲戦車・III号戦車補正
			{ 
				if (elevenReg + isshiki + no3Tank != 0)
				{
					basepower *= 1.8;
					basepower += 25;
				}
			}

			//M4A1 DD特殊補正
			{ 
				if (m4A1DD != 0)
				{
					basepower *= 1.4;
					basepower += 35;
				}
			}

			//一式砲戦車特殊補正
			{ 
				if (isshiki != 0)
				{
					basepower *= 1.3;
					basepower += 42;
				}
			}

			//上陸支援舟艇シナジー補正
			{ 
				var typeA = daihatsu + tokudaihatsu + rikusen + isshiki + no2Tank;
				var typeB = elevenReg + no3Tank + spAmphibiousTank;
				if (daihaysuAB != 0 && busouDaihatsu != 0)
				{
					if (typeA != 0 && typeB != 0)
					{
						basepower *= 1.56;
						basepower += 15;
					}
					else if (typeA == 0 && typeB != 0)
					{
						basepower *= 1.44;
						basepower += 13;
					}
					else if (typeA != 0 && typeB == 0)
					{
						basepower *= 1.32;
						basepower += 12;
					}
				}
				else if (daihaysuAB + busouDaihatsu != 0)
				{
					if (typeA + typeB != 0)
					{
						basepower *= 1.2;
						basepower += 10;
					}
				}
			}

			//WG迫撃砲加算補正
			{ 
				int[] wg42 = new int[5] { 0, 75, 110, 140, 160 };
				int[] taichi = new int[5] { 0, 55, 115, 160, 190 };
				int[] taichisyu = new int[5] {0, 80, 170, 230, 260 };
				int[] nisiki = new int[5] {0, 30, 55, 75, 90 };
				int[] nisikisyu = new int[5] {0, 60, 110, 150, 180 };
				basepower += wg42[rocketWG] + taichi[rocket20] + taichisyu[rocket20S] + nisiki[depthCharge] + nisikisyu[depthChargeS];
			}
			int engagementForm = 1;
			basepower *= GetHPDamageBonus(hpRate) * GetEngagementFormDamageRate(engagementForm);
			basepower += GetLightCruiserDamageBonus(masterShip, allSlotMaster) + GetItalianDamageBonus(shipID, allSlotMaster);

			return basepower;
		}

		/// <summary>
		/// 対地攻撃(集積地キャップ後)
		/// </summary>
		private static double GetDayGroundAttackAfterCAP(ShipData ship, double basepower)
		{

			var allSlotInstance = ship.AllSlotInstance.ToArray();

			double landigCraftLevel = 0; 
			double spAmphibiousTankLevel = 0;

			var rocketWG = 0; 
			var rocket20 = 0; 
			var depthCharge = 0;
			var daihatsu = 0;
			var tokudaihatsu = 0;
			var rikusen = 0;
			var isshiki = 0;
			var elevenReg = 0; 
			var no2Tank = 0;
			var no3Tank = 0;
			var m4A1DD = 0; 
			var busouDaihatsu = 0;
			var daihaysuAB = 0;
			var spAmphibiousTank = 0;

			foreach (var slot in allSlotInstance)
			{
				if (slot == null)
					continue;
				switch (slot.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.LandingCraft:
						landigCraftLevel += slot.Level;
						break;
				}

				switch (slot.EquipmentID)
				{
					case 126:
						rocketWG++;
						break;
					case 348:
					case 349:
						rocket20++;
						break;
					case 346:
					case 347:
						depthCharge++;
						break;
					case 68:
						daihatsu++;
						break;
					case 193:
						tokudaihatsu++;
						break;
					case 166:
						rikusen++;
						break;
					case 449:
						isshiki++;
						break;
					case 230:
						elevenReg++;
						break;
					case 436:
						no2Tank++;
						break;
					case 482:
						no3Tank++;
						break;
					case 355:
						m4A1DD++;
						break;
					case 408:
						daihaysuAB++;
						break;
					case 409:
						busouDaihatsu++;
						break;
					case 167:
						spAmphibiousTank++;
						spAmphibiousTankLevel += slot.Level;
						break;
				}
			}

			//一般対地乗算補正
			{
				if (rocketWG != 0)
					if (rocketWG >= 2)
						basepower *= 1.625;
					else
						basepower *= 1.25;
				if (rocket20 != 0)
					if (rocket20 >= 2)
						basepower *= 1.68;
					else
						basepower *= 1.2;
				if (depthCharge != 0)
					if (depthCharge >= 2)
						basepower *= 1.38;
					else
						basepower *= 1.15;
				if ((daihatsu + tokudaihatsu + rikusen + isshiki + elevenReg + no2Tank + no3Tank + m4A1DD + daihaysuAB + busouDaihatsu) != 0)
				{
					basepower *= 1.7;
					if (daihatsu != 0)
						basepower *= 1.0;
					if (tokudaihatsu != 0)
						basepower *= 1.2;
					if (rikusen + isshiki != 0)
						if (rikusen + isshiki >= 2)
							basepower *= 2.08;
						else
							basepower *= 1.3;
					if (elevenReg != 0)
						basepower *= 1.0;
					if (no3Tank != 0)
						basepower *= 1.0;
					if (no2Tank != 0)
						if (no2Tank >= 2)
							basepower *= 1.3;
						else
							basepower *= 1.3;
					if (m4A1DD != 0)
						basepower *= 1.2;
					if (daihaysuAB + busouDaihatsu != 0)
						if (daihaysuAB + busouDaihatsu >= 2)
							basepower *= 1.65;
						else
							basepower *= 1.5;
					if (landigCraftLevel != 0)
					{ 
						if(rikusen>=1 && no2Tank >= 1)
							basepower *= Math.Pow((landigCraftLevel / (daihatsu + tokudaihatsu + rikusen + isshiki + elevenReg + no2Tank + no3Tank + m4A1DD + daihaysuAB + busouDaihatsu) / 50) + 1,3);
						else if(rikusen + isshiki + no2Tank != 0)
							basepower *= Math.Pow((landigCraftLevel / (daihatsu + tokudaihatsu + rikusen + isshiki + elevenReg + no2Tank + no3Tank + m4A1DD + daihaysuAB + busouDaihatsu) / 50) + 1, 2);
						else
							basepower *= (landigCraftLevel / (daihatsu + tokudaihatsu + rikusen + isshiki + elevenReg + no2Tank + no3Tank + m4A1DD + daihaysuAB + busouDaihatsu) / 50) + 1;
					}
				}
				if (spAmphibiousTank != 0)
				{
					if (spAmphibiousTank >= 2)
						basepower *= 2.55;
					else
						basepower *= 1.7;
					if (spAmphibiousTankLevel != 0)
						basepower *= (spAmphibiousTankLevel / spAmphibiousTank / 30) + 1;
				}
			}
			return basepower;
		}
		#endregion
	}
}