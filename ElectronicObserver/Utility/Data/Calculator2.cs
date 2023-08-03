using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Data
{
	/// <summary>
	/// FormFleetの個別攻撃表示処理用
	/// </summary>
	public static class Calculator2
	{
		/// <summary>
		/// 昼戦における攻撃種別を取得してリスト化します(FormFreet用)
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenderShipID">防御艦の艦船ID。なければ-1</param>
		/// <param name="includeSpecialAttack">弾着観測砲撃を含むか。falseなら除外して計算</param>
		public static DayAttackKind[] GetDayAttackKindList(int[] slot, int attackerShipID, bool includeSpecialAttack = true)
		{
			int reconCount = 0;
			int mainGunCount = 0;
			int subGunCount = 0;
			int apShellCount = 0;
			int radarCount = 0;
			int rocketCount = 0;
			int attackerCount = 0;
			int bomberCount = 0;
			int suisei634Count = 0;
			int zuiunCount = 0;
			int fighterCount = 0;
			var dayAttackList = new List<DayAttackKind>();

			if (slot == null)
				dayAttackList.Add(DayAttackKind.Unknown);

			var slotmaster = slot.Select(s => KCDatabase.Instance.MasterEquipments[s]).Where(eq => eq != null).ToArray();

			foreach (var eq in slotmaster)
			{
				switch (eq.CategoryType)
				{
					case EquipmentTypes.MainGunSmall:
					case EquipmentTypes.MainGunMedium:
					case EquipmentTypes.MainGunLarge:
						mainGunCount++;
						break;

					case EquipmentTypes.SecondaryGun:
						subGunCount++;
						break;

					case EquipmentTypes.CarrierBasedBomber:
						bomberCount++;
						if (eq.Name.Contains("六三四空"))
							suisei634Count++;
						break;

					case EquipmentTypes.CarrierBasedTorpedo:
						attackerCount++;
						break;

					case EquipmentTypes.CarrierBasedFighter:
						fighterCount++;
						break;

					case EquipmentTypes.SeaplaneRecon:
					case EquipmentTypes.SeaplaneBomber:
						reconCount++;
						if (eq.Name.Contains("瑞雲"))
							zuiunCount++;
						break;

					case EquipmentTypes.RadarSmall:
					case EquipmentTypes.RadarLarge:
						radarCount++;
						break;

					case EquipmentTypes.APShell:
						apShellCount++;
						break;

					case EquipmentTypes.Rocket:
						rocketCount++;
						break;

				}
			}

			ShipDataMaster attacker = KCDatabase.Instance.MasterShips[attackerShipID];

			if (includeSpecialAttack) //瑞雲特殊攻撃判定
			{
				if (attackerShipID == 553 || attackerShipID == 554) // 伊勢改二・日向改二
				{
					if (mainGunCount >= 1 && zuiunCount >= 2)
					{
						dayAttackList.Add(DayAttackKind.ZuiunMultiAngle);
					}
					if (mainGunCount >= 1 && suisei634Count >= 2)
						dayAttackList.Add(DayAttackKind.SeaAirMultiAngle);
				}

				if (reconCount > 0) //砲撃判定
				{
					if (mainGunCount == 2 && apShellCount == 1)
					{
						dayAttackList.Add(DayAttackKind.CutinMainMain);
					}

					if (mainGunCount == 1 && subGunCount == 1 && apShellCount == 1)
					{
						dayAttackList.Add(DayAttackKind.CutinMainAP);
					}
					if (mainGunCount == 1 && subGunCount == 1 && radarCount == 1)
					{
						dayAttackList.Add(DayAttackKind.CutinMainRadar);
					}
					if (mainGunCount >= 1 && subGunCount >= 1)
					{
						dayAttackList.Add(DayAttackKind.CutinMainSub);
					}
					if (mainGunCount >= 2)
					{
						dayAttackList.Add(DayAttackKind.DoubleShelling);
					}
				}

				if (bomberCount > 0 && attackerCount > 0) //空母攻撃判定
				{
					if (fighterCount >= 1 && bomberCount >= 1 && attackerCount >= 1)
						dayAttackList.Add(DayAttackKind.CutinFighterBomberAttacker);

					if (bomberCount >= 2 && attackerCount >= 1)
						dayAttackList.Add(DayAttackKind.CutinBomberBomberAttacker);

					if (bomberCount == 1 && attackerCount >= 1)
						dayAttackList.Add(DayAttackKind.CutinBomberAttacker);
				}

			}

			if (attackerShipID == 352) //速吸改
			{
				if (slotmaster.Any(eq => eq.CategoryType == EquipmentTypes.CarrierBasedTorpedo))
					dayAttackList.Add(DayAttackKind.AirAttack);
				else
					dayAttackList.Add(DayAttackKind.Shelling);
			}
				
			if (attackerShipID == 717) //山汐丸改
			{
				if (slotmaster.Any(eq => eq.CategoryType == EquipmentTypes.CarrierBasedBomber))
					dayAttackList.Add(DayAttackKind.AirAttack);
				else
					dayAttackList.Add(DayAttackKind.Shelling);
			}

			if (!attacker.IsAircraftCarrier && !attacker.IsSubmarine)
				dayAttackList.Add(DayAttackKind.Shelling); //砲撃

			if (attacker.IsAircraftCarrier)
			{
				if (bomberCount + attackerCount > 0) dayAttackList.Add(DayAttackKind.AirAttack); //空撃
			}

			if (attacker.IsSubmarine) 
			{
				if (slotmaster.Any(eq => eq.CategoryType == EquipmentTypes.SpecialAmphibiousTank)) dayAttackList.Add(DayAttackKind.Shelling); //内火艇装備の場合砲撃
			}

			DayAttackKind[] getDayAtkName = dayAttackList.ToArray();
			return getDayAtkName;

		}


		/// <summary>
		/// 夜戦における攻撃種別を取得します(FormFreet用)
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenderShipID">防御艦の艦船ID。なければ-1</param>
		/// <param name="includeSpecialAttack">カットイン/連撃の判定を含むか。falseなら除外して計算</param>
		/// <param name="nightAirAttackFlag">夜戦空母攻撃フラグ</param>
		public static NightAttackKind[] GetNightAttackKindList(int[] slot, int attackerShipID, bool includeSpecialAttack = true)
		{
			int mainGunCount = 0;
			int subGunCount = 0;
			int torpedoCount = 0;
			int rocketCount = 0;
			int lateModelTorpedoCount = 0;
			int submarineEquipmentCount = 0;
			int nightFighterCount = 0;
			int nightFighterCountsub = 0;
			int nightAttackerCount = 0;
			int swordfishCount = 0;
			int nightCapableBomberCount = 0;
			int nightBomberCount = 0;
			int nightPersonnelCount = 0;
			int surfaceRadarCount = 0;
			int picketCrewCount = 0;
			int masterPicketCrewCount = 0;
			int drumCount = 0;
			int nightZuiunCount = 0;
			int typeD2 = 0;
			int typeD3 = 0;
			var nightAttackList = new List<NightAttackKind>();

			if (slot == null)
				nightAttackList.Add(NightAttackKind.Unknown);

			ShipDataMaster attacker = KCDatabase.Instance.MasterShips[attackerShipID];

			var slotmaster = slot.Select(id => KCDatabase.Instance.MasterEquipments[id]).Where(eq => eq != null).ToArray();

			foreach (var eq in slotmaster)
			{
				switch (eq.CategoryType)
				{
					// 主砲系
					case EquipmentTypes.MainGunMedium:
					case EquipmentTypes.MainGunLarge:
					case EquipmentTypes.MainGunLarge2:
						mainGunCount++;
						break;

					//小口径主砲(D2・D3)
					case EquipmentTypes.MainGunSmall:
						mainGunCount++;
						if (eq.EquipmentID == 267)
							typeD2++;
						else if (eq.EquipmentID == 366)
							typeD3++;
						break;

					// 副砲
					case EquipmentTypes.SecondaryGun:
					case EquipmentTypes.SecondaryGun2:
						subGunCount++;
						break;

					// 魚雷系
					case EquipmentTypes.Torpedo:
					case EquipmentTypes.SubmarineTorpedo:
						torpedoCount++;

						if (eq.IsLateModelTorpedo)
							lateModelTorpedoCount++;
						break;

					// 夜間瑞雲
					case EquipmentTypes.SeaplaneBomber:
						if (eq.IsNightZuiun)
							nightZuiunCount++;
						break;

					// 夜間戦闘機
					case EquipmentTypes.CarrierBasedFighter:
						if (eq.IsNightFighter)
							nightFighterCount++;
						break;

					// (夜間)爆撃機
					case EquipmentTypes.CarrierBasedBomber:
						if (eq.EquipmentID == 154)      // 零戦62型(爆戦/岩井隊)
							nightCapableBomberCount++;
						else if (eq.EquipmentID == 320) // 彗星一二型(三一号光電管爆弾搭載機)
							nightBomberCount++;
						break;

					// 夜間攻撃機
					case EquipmentTypes.CarrierBasedTorpedo:
						if (eq.IsNightAttacker)
							nightAttackerCount++;

						if (eq.IsSwordfish)
							swordfishCount++;
						break;

					// 電探
					case EquipmentTypes.RadarSmall:
					case EquipmentTypes.RadarLarge:
						if (eq.IsSurfaceRadar)
							surfaceRadarCount++;
						break;

					// 見張員
					case EquipmentTypes.SurfaceShipPersonnel:
						if (eq.EquipmentID == 412)
						{
							masterPicketCrewCount++; //水雷戦隊 熟練見張員(水)
						}
						else
						{
							picketCrewCount++; //熟練見張員(見)
						}
						break;

					// 夜間作戦航空要員
					case EquipmentTypes.AviationPersonnel:
						if (eq.IsNightAviationPersonnel)
							nightPersonnelCount++;
						break;

					// 対地装備
					case EquipmentTypes.Rocket:
						rocketCount++;
						break;

					// 潜水艦装備
					case EquipmentTypes.SubmarineEquipment:
						submarineEquipmentCount++;
						break;

					//ドラム缶
					case EquipmentTypes.TransportContainer:
						if (eq.EquipmentID == 75)
						{
							drumCount++;
						}
						break;
				}
			}

			if (attackerShipID == 545
				|| attackerShipID == 599 || attackerShipID == 610 || attackerShipID == 883)      // 無条件夜間航空攻撃可能な娘 Saratoga Mk.II/赤城改二戊/加賀改二戊/龍鳳改二戊
				nightPersonnelCount++;

			if (includeSpecialAttack)
			{
				// 空母カットイン
				if (nightPersonnelCount > 0)
				{
					if (nightFighterCount >= 2 && nightAttackerCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinNightAirAttackFFA);
					if (nightFighterCount >= 1 && nightAttackerCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinNightAirAttackFA);
					if (nightFighterCount >= 1 && nightBomberCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinNightAirAttackFS);
					if (nightFighterCount == 0 && nightAttackerCount >= 1 && nightBomberCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinNightAirAttackAS);
					nightFighterCountsub = nightFighterCount - 1;
					nightFighterCount = nightFighterCount - nightFighterCountsub;
					if (nightFighterCount == 1 && (nightFighterCountsub + nightAttackerCount + nightBomberCount + swordfishCount + nightCapableBomberCount >= 2))
						nightAttackList.Add(NightAttackKind.CutinNightAirAttackFOther);
					nightAttackList.Add(NightAttackKind.NightAirAttack);
				}

				//空母夜間特殊攻撃
				if (nightPersonnelCount == 0)
				{
					if (attackerShipID == 515 || attackerShipID == 393)     // Ark Royal(改)
						if (swordfishCount > 0)
							nightAttackList.Add(NightAttackKind.NightSwordfish);
					if (attackerShipID == 432 || attackerShipID == 353
						|| attackerShipID == 433 || attackerShipID == 646
						|| attackerShipID == 889 || attackerShipID == 536
						|| attackerShipID == 529)        // Graf Zeppelin(改), Saratoga,加賀改二護,大鷹型改二
						nightAttackList.Add(NightAttackKind.AirAttack);
				}

				// 潜水艦カットイン
				if (attacker?.ShipType == ShipTypes.Submarine || attacker.ShipType == ShipTypes.SubmarineAircraftCarrier)
				{
					if (lateModelTorpedoCount >= 1 && submarineEquipmentCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinTorpedoMasterPicketSubmarine);
					else if (lateModelTorpedoCount >= 2)
						nightAttackList.Add(NightAttackKind.CutinTorpedoTorpedoSubmarine);
				}

				//夜間瑞雲攻撃
				if (attacker.CanNightZuiunAttack && nightZuiunCount >= 1 && mainGunCount >= 2)
				{
					nightAttackList.Add(NightAttackKind.SpecialNightZuiun);
				}

				// 駆逐艦カットイン
				if (attacker?.ShipType == ShipTypes.Destroyer)
				{
					//note:発動優先度が高い順にしておく
					//     例えば魚雷/魚雷/ドラム缶/水雷見張員だと魚魚水が表示される

					//主魚電
					if (mainGunCount >= 1 && torpedoCount >= 1 && surfaceRadarCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinTorpedoRadar_);
					//魚見電、魚水電
					if (torpedoCount >= 1 && surfaceRadarCount >= 1 && (picketCrewCount >= 1 || masterPicketCrewCount >= 1))
						nightAttackList.Add(NightAttackKind.CutinTorpedoPicket_);
					//魚魚水
					if (torpedoCount >= 2 && masterPicketCrewCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinTorpedoTorpedoMasterPicket_);
					//ド水魚
					if (torpedoCount >= 1 && masterPicketCrewCount >= 1 && drumCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinTorpedoDrumMasterPicket_);
				}

				// 汎用カットイン
				if (mainGunCount >= 3)
					nightAttackList.Add(NightAttackKind.CutinMainMain);

				if (!nightAttackList.Contains(NightAttackKind.CutinMainMain))
				{
					if (mainGunCount == 2 && subGunCount > 0)
						nightAttackList.Add(NightAttackKind.CutinMainSub);
				}

				if ((!nightAttackList.Contains(NightAttackKind.CutinMainMain))
					&& (!nightAttackList.Contains(NightAttackKind.CutinMainSub))
					&& (!nightAttackList.Contains(NightAttackKind.CutinTorpedoMasterPicketSubmarine))
					&& (!nightAttackList.Contains(NightAttackKind.CutinTorpedoTorpedoSubmarine)))
				{
					if (torpedoCount >= 2)
						nightAttackList.Add(NightAttackKind.CutinTorpedoTorpedo);
				}

				if ((!nightAttackList.Contains(NightAttackKind.CutinMainMain)) && (!nightAttackList.Contains(NightAttackKind.CutinMainSub))
					&& (!nightAttackList.Contains(NightAttackKind.CutinTorpedoTorpedo)))
				{
					if (mainGunCount >= 1 && torpedoCount >= 1)
						nightAttackList.Add(NightAttackKind.CutinMainTorpedo);
				}

			}
			//連撃判定
			if ((mainGunCount == 2 && subGunCount == 0 & torpedoCount == 0) ||
				(mainGunCount == 1 && subGunCount > 0) ||
				(subGunCount >= 2 && torpedoCount <= 1))
				nightAttackList.Add(NightAttackKind.DoubleShelling);

			// 基本夜戦しない判定
			if (attacker?.ShipType != ShipTypes.AircraftCarrier
				&& attacker?.ShipType != ShipTypes.ArmoredAircraftCarrier
				&& attacker?.ShipType != ShipTypes.LightAircraftCarrier
				&& attackerShipID != 605
				&& attackerShipID != 645
				&& attackerShipID != 650)
			{
				if (mainGunCount + subGunCount + torpedoCount >= 1)
					nightAttackList.Add(NightAttackKind.NormalAttack);
			}
			NightAttackKind[] getNightAtkName = nightAttackList.ToArray();
			return getNightAtkName;
		}
	}
}

