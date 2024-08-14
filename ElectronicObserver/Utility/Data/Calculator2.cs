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
			int jetbomberCount = 0;
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

					case EquipmentTypes.JetBomber:
						jetbomberCount++;
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
					if (mainGunCount >= 2 && apShellCount >= 1)
					{
						dayAttackList.Add(DayAttackKind.CutinMainMain);
					}

					if (mainGunCount >= 1 && subGunCount >= 1 && apShellCount >= 1)
					{
						dayAttackList.Add(DayAttackKind.CutinMainAP);
					}
					if (mainGunCount >= 1 && subGunCount >= 1 && radarCount >= 1)
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

					if (bomberCount >= 1 && attackerCount >= 1)
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
				if (bomberCount + attackerCount + jetbomberCount > 0) dayAttackList.Add(DayAttackKind.AirAttack); //空撃
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
			int nightAirplaneCount = 0;
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
						{ 
							nightFighterCount++;
							nightAirplaneCount++;
						}
						break;

					// (夜間)爆撃機
					case EquipmentTypes.CarrierBasedBomber:
						if (eq.EquipmentID == 154)      // 零戦62型(爆戦/岩井隊)
						{
							nightCapableBomberCount++;
							nightAirplaneCount++;
						}
						else if (eq.EquipmentID == 320) // 彗星一二型(三一号光電管爆弾搭載機)
						{
							nightBomberCount++;
							nightAirplaneCount++;
						}
							break;

					// 夜間攻撃機
					case EquipmentTypes.CarrierBasedTorpedo:
						if (eq.IsNightAttacker)
						{
							nightAttackerCount++;
							nightAirplaneCount++;
						}
							if (eq.IsSwordfish) // Swordfish系
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
					if (!nightAttackList.Contains(NightAttackKind.CutinNightAirAttackFFA))
					{
						if (nightFighterCount >= 1 && (nightFighterCountsub + nightAttackerCount + nightBomberCount + swordfishCount + nightCapableBomberCount >= 2))
							nightAttackList.Add(NightAttackKind.CutinNightAirAttackFOther);
					}
					if (nightAirplaneCount != 0)
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
						nightAttackList.Add(NightAttackKind.AirAttack); //空撃
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
				if (attacker.CanNightZuiunAttack && mainGunCount >= 2 && nightZuiunCount >= 1)
				{
					if (nightZuiunCount >= 2 && surfaceRadarCount >= 1)
						nightAttackList.Add(NightAttackKind.SpecialNightZuiun2Rader);
					if (nightZuiunCount >= 2)
						nightAttackList.Add(NightAttackKind.SpecialNightZuiun2);
					if (surfaceRadarCount >=1)
						nightAttackList.Add(NightAttackKind.SpecialNightZuiunRader);
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
					nightAttackList.Add(NightAttackKind.NormalAttack);
			}
			NightAttackKind[] getNightAtkName = nightAttackList.ToArray();
			return getNightAtkName;
		}

		/// <summary>
		/// 対空カットイン種別を取得します。
		/// </summary>
		public static int[] GetAACutinKind(int shipID, int[] slot, int id)
		{
			int highangle = 0;
			int highangle_director = 0;
			int highangle_kai = 0;
			int director = 0;
			int radar = 0;
			int aaradar = 0;
			int maingunl = 0;
			int maingunl_fcr = 0;
			int maingunl_356 = 0;
			int maingun_c3h = 0;
			int aashell = 0;
			int aagun_total = 0;
			int aagun_medium3 = 0;
			int aagun_medium4 = 0;
			int aagun_high = 0;
			int aagun_concentrated = 0;
			int aagun_pompom = 0;
			int aagun_25mmz = 0;
			int aarocket_english = 0;
			int aarocket_mod = 0;
			int highangle_musashi = 0;
			int highangle_america = 0;
			int highangle_america_gfcs = 0;
			int highangle_yamato = 0;
			int radar_gfcs = 0;
			int radar_mast = 0;
			int radar_with_range_finder = 0;
			int highangle_atlanta = 0;
			int highangle_atlanta_gfcs = 0;

			var aacutinlist = new List<int>();
			var slotmaster = slot.Select(id => KCDatabase.Instance.MasterEquipments[id]).Where(eq => eq != null).ToArray();

			foreach (var eq in slotmaster)
			{
				if (eq.IsHighAngleGun)
				{
					highangle++;

					if (eq.IsHighAngleGunWithAADirector)
						highangle_director++;

					switch (eq.EquipmentID)
					{
						case 275:   // 10cm連装高角砲改+増設機銃
							highangle_musashi++;
							break;
						case 313:   // 5inch単装砲 Mk.30改
							highangle_america++;
							break;
						case 308:   // 5inch単装砲 Mk.30改+GFCS Mk.37
							highangle_america_gfcs++;
							break;
						case 362:   // 5inch連装両用砲(集中配備)
							highangle_atlanta++;
							break;
						case 363:   // GFCS Mk.37+5inch連装両用砲(集中配備)
							highangle_atlanta_gfcs++;
							break;
						case 464:   // 10cm連装高角砲群 集中配備
							highangle_yamato++;
							break;
						case 533:   // 10cm連装高角砲改+高射装置改
							highangle_kai++;
							break;
					}
				}
				else if (eq.CategoryType == EquipmentTypes.AADirector)
				{
					director++;
				}
				else if (eq.IsRadar)
				{
					radar++;

					if (eq.IsAirRadar)
						aaradar++;

					if (eq.EquipmentID == 307)   // GFCS Mk.37
						radar_gfcs++;

					if (eq.AA >= 4)   // 電探装備マスト(13号改+22号電探改四)素対空4以上
						radar_mast++;

					if (eq.IsRadarWithRangeFinder)
						radar_with_range_finder++;
				}
				else if (eq.CategoryType == EquipmentTypes.MainGunLarge || eq.CategoryType == EquipmentTypes.MainGunLarge2)
				{
					maingunl++;

					if (eq.EquipmentID == 300)       // 16inch Mk.I三連装砲改+FCR type284
						maingunl_fcr++;

					if (eq.EquipmentID == 502)       // 35.6cm連装砲改三(ダズル迷彩仕様)
						maingunl_356++;

					if (eq.EquipmentID == 503)       // 35.6cm連装砲改四
						maingunl_356++;
				}
				else if (eq.CategoryType == EquipmentTypes.AAShell)
				{
					aashell++;
				}
				else if (eq.CategoryType == EquipmentTypes.AAGun)
				{
					aagun_total++;

					if (eq.EquipmentID == 274)      // 12cm30連装噴進砲改二
						aarocket_mod++;
					if (eq.EquipmentID == 191)      // QF 2ポンド8連装ポンポン砲
						aagun_pompom++;
					if (eq.EquipmentID == 301)      // 20連装7inch UP Rocket Launchers
						aarocket_english++;
					if (eq.EquipmentID == 505)      // 25mm対空機銃増備
						aagun_25mmz++;

					if (eq.IsConcentratedAAGun)
						aagun_concentrated++;
					if (eq.AA >= 6)
						aagun_high++;
					if (eq.AA >= 4)
						aagun_medium4++;
					if (eq.AA >= 3)
						aagun_medium3++;
				}
				else if (eq.EquipmentID == 529)
				{
					maingun_c3h++;
				}
			}

			// 固有カットイン
			switch (KCDatabase.Instance.MasterShips[shipID]?.ShipClass) //艦型別
			{
				case 54:    // 秋月型
					if (KCDatabase.Instance.MasterShips[shipID]?.RemodelTier != 0 && highangle_kai >= 2 && radar_mast >= 1)
						aacutinlist.Add(48);
					if (highangle >= 2 && radar >= 1)
						aacutinlist.Add(1);
					if (highangle >= 1 && radar >= 1)
						aacutinlist.Add(2);
					if (highangle >= 2)
						aacutinlist.Add(3);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);
					
					return aacutinlist.ToArray(); 

				case 91:    // Fletcher級
					if (highangle_america_gfcs >= 2)
						aacutinlist.Add(34);
					if (highangle_america_gfcs >= 1 && highangle_america >= 1)
						aacutinlist.Add(35);
					if (highangle_america >= 2 && radar_gfcs >= 1)
						aacutinlist.Add(36);
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle_america >= 2)
						aacutinlist.Add(37);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);
					
					return aacutinlist.ToArray();

				case 99:   // Atlanta級
					if (highangle_atlanta_gfcs >= 2)
						aacutinlist.Add(38);
					if (highangle_atlanta_gfcs >= 1 && highangle_atlanta >= 1)
						aacutinlist.Add(39);
					if (highangle_atlanta_gfcs + highangle_atlanta >= 2)
					{
						if (radar_gfcs >= 1)
							aacutinlist.Add(40);
						aacutinlist.Add(41);
					}
					break;
			}

			switch (shipID) //艦娘個別
			{
				case 428:   // 摩耶改二
					if (highangle >= 1 && aagun_concentrated >= 1)
					{
						if (aaradar >= 1)
							aacutinlist.Add(10);

						aacutinlist.Add(11);
					}
					break;

				case 141:   // 五十鈴改二
					if (highangle >= 1 && aagun_total >= 1 && aaradar >= 1)
						aacutinlist.Add(14);
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13); 
					if (highangle >= 1 && aagun_total >= 1)
						aacutinlist.Add(15);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 470:   // 霞改二乙
					if (highangle >= 1 && aagun_total >= 1 && aaradar >= 1)
						aacutinlist.Add(16);
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && aagun_total >= 1)
						aacutinlist.Add(17);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 622:   // 夕張改二
					if (highangle >= 1 && aagun_total >= 1 && aaradar >= 1)
						aacutinlist.Add(16);
					break;

				case 418:   // 皐月改二
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (aagun_concentrated >= 1)
						aacutinlist.Add(18);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 487:   // 鬼怒改二
					if (aagun_concentrated >= 1 && (highangle - highangle_director >= 1))
						aacutinlist.Add(19);
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1)
						aacutinlist.Add(20);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 488:   // 由良改二
					if (highangle >= 1 && aaradar >= 1)
						aacutinlist.Add(21);
					break;

				case 548:   // 文月改二
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (aagun_concentrated >= 1)
						aacutinlist.Add(22);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 539:   // UIT-25
				case 530:   // 伊504
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);
					if (aagun_medium3 >= 1)
						aacutinlist.Add(23);

					return aacutinlist.ToArray();
					
				case 477:   // 天龍改二
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle >= 3)
						aacutinlist.Add(30);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (highangle >= 1 && aagun_medium3 >= 1)
						aacutinlist.Add(24);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 2)
						aacutinlist.Add(31);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 478:   // 龍田改二
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle >= 3)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (highangle >= 1 && aagun_medium3 >= 1)
						aacutinlist.Add(24);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 82:    // 伊勢改
				case 88:    // 日向改
				case 553:   // 伊勢改二
				case 554:   // 日向改二
					if (aarocket_mod >= 1 && aaradar >= 1 && aashell >= 1)
						aacutinlist.Add(25);
					if (maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1) //戦艦系汎用
						aacutinlist.Add(4);
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (maingunl >= 1 && aashell >= 1 && director >= 1) //戦艦系汎用
						aacutinlist.Add(6);
					if (aarocket_mod >= 1 && aaradar >= 1)
						aacutinlist.Add(28);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();
					
				case 321:   //大淀改
					if (aaradar >= 1 && highangle_musashi >= 1 && aarocket_mod >= 1)
						aacutinlist.Add(27);
					break;

				case 557:   // 磯風乙改
				case 558:   // 浜風乙改
					if (highangle >= 1 && aaradar >= 1)
						aacutinlist.Add(29);
					break;

				case 148:   // 武蔵改
					if (maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1) //戦艦系汎用
						aacutinlist.Add(4);
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (maingunl >= 1 && aashell >= 1 && director >= 1) //戦艦系汎用
						aacutinlist.Add(6); 
					if (aarocket_mod >= 1 && aaradar >= 1)
						aacutinlist.Add(28);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 546:   // 武蔵改二
					if (radar_with_range_finder >= 1 && highangle_yamato >= 2 && aagun_high >= 1)
						aacutinlist.Add(42); //42:15m二重測距儀+21号電探改二系、10cm連装高角砲群集中配備*2、対空機銃
					if (radar_with_range_finder >= 1 && highangle_yamato >= 2)
						aacutinlist.Add(43); //43:15m二重測距儀+21号電探改二系、10cm連装高角砲群集中配備*2
					if (radar_with_range_finder >= 1 && highangle_yamato >= 1 && aagun_high >= 1)
						aacutinlist.Add(44); //44:15m二重測距儀+21号電探改二系、10cm連装高角砲群集中配備、対空機銃
					if (highangle_musashi >= 1 && aaradar >= 1)
						aacutinlist.Add(26);
					if (maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1) //戦艦系汎用
						aacutinlist.Add(4);
					if (radar_with_range_finder >= 1 && highangle_yamato >= 1)
						aacutinlist.Add(45); //45:15m二重測距儀+21号電探改二系、10cm連装高角砲群集中配備
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (maingunl >= 1 && aashell >= 1 && director >= 1) //戦艦系汎用
						aacutinlist.Add(6);
					if (aarocket_mod >= 1 && aaradar >= 1)
						aacutinlist.Add(28);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 911:   // 大和改二
				case 916:   // 大和改二重
					if (radar_with_range_finder >= 1 && highangle_yamato >= 2 && aagun_high >= 1)
						aacutinlist.Add(42); //42:15m二重測距儀+21号電探改二系、10cm連装高角砲群集中配備*2、対空機銃
					if (radar_with_range_finder >= 1 && highangle_yamato >= 2)
						aacutinlist.Add(43); //43:15m二重測距儀+21号電探改二系、10cm連装高角砲群集中配備*2
					if (radar_with_range_finder >= 1 && highangle_yamato >= 1 && aagun_high >= 1)
						aacutinlist.Add(44); //44:15m二重測距儀+21号電探改二系、10cm連装高角砲群集中配備、対空機銃
					if (highangle_musashi >= 1 && aaradar >= 1)
						aacutinlist.Add(26);
					if (maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1) //戦艦系汎用
						aacutinlist.Add(4);
					if (radar_with_range_finder >= 1 && highangle_yamato >= 1)
						aacutinlist.Add(45); //45:15m二重測距儀+21号電探改二系、10cm連装高角砲群集中配備
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (maingunl >= 1 && aashell >= 1 && director >= 1) //戦艦系汎用
						aacutinlist.Add(6);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 593:   // 榛名改二乙
					if (maingunl_356 >= 1 && aagun_concentrated >= 1 && aaradar >= 1)
						aacutinlist.Add(46);
					if (maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1) //戦艦系汎用
						aacutinlist.Add(4);
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (maingunl >= 1 && aashell >= 1 && director >= 1) //戦艦系汎用
						aacutinlist.Add(6);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7); 
					if (aarocket_english >= 2)
						aacutinlist.Add(32);
					if (aagun_pompom >= 1 && (maingunl_fcr >= 1 || aarocket_english >= 1))
						aacutinlist.Add(32);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 149:   // 金剛改二 (英国艦+金剛型改二)
				case 591:   // 金剛改二丙
				case 150:   // 比叡改二
				case 592:   // 比叡改二丙
				case 151:   // 榛名改二
				case 152:   // 霧島改二
				case 519:   // Jervis
				case 394:   // Jervis改
				case 571:   // Nelson
				case 576:   // Nelson改
				case 439:   // Warspite
				case 364:   // Warspite改
				case 515:   // Ark Royal
				case 393:   // Ark Royal改
				case 520:   // Janus
				case 893:   // Janus改
				case 514:   // Sheffield
				case 705:   // Sheffield改
				case 885:   // Victorious
				case 713:   // Victorious改
					if (maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1) //戦艦系汎用
						aacutinlist.Add(4);
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (maingunl >= 1 && aashell >= 1 && director >= 1) //戦艦系汎用
						aacutinlist.Add(6);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aarocket_english >= 2)
						aacutinlist.Add(32);
					if (aagun_pompom >= 1 && (maingunl_fcr >= 1 || aarocket_english >= 1))
						aacutinlist.Add(32);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 579:   // Gotland改
				case 630:   // Gotland andra
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle >= 1 && aagun_medium4 >= 1)
						aacutinlist.Add(33);
					if (highangle >= 3)
						aacutinlist.Add(30);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 961:   // 時雨改三
				case 975:   // 春雨改二
				case 145:   // 時雨改二
				case 497:   // 白露改二
				case 498:   // 村雨改二
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle_director >= 1 && aaradar >= 1)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if ((KCDatabase.Instance.Ships[id].AABase > 70) && (maingun_c3h >= 2 || (maingun_c3h >= 1 && (aagun_25mmz + radar_mast >= 1))))
						aacutinlist.Add(47);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();

				case 979:   // 稲木改二
					if (highangle_director >= 2 && aaradar >= 1)
						aacutinlist.Add(5);
					if (highangle >= 3)
						aacutinlist.Add(8);
					if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
						aacutinlist.Add(13);
					if (highangle >= 1 && director >= 1 && aaradar >= 1)
						aacutinlist.Add(7);
					if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
						aacutinlist.Add(12);
					if (highangle >= 2)
						aacutinlist.Add(31);
					if (highangle >= 1 && aagun_total >= 1)
						aacutinlist.Add(17);
					if (aagun_concentrated >= 1)
						aacutinlist.Add(18);
					if (highangle >= 1 && director >= 1)
						aacutinlist.Add(9);

					return aacutinlist.ToArray();
			}

			//以下汎用カットイン
			if (maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1) //戦艦系汎用
				aacutinlist.Add(4);
			if (highangle_director >= 2 && aaradar >= 1)
				aacutinlist.Add(5);
			if (maingunl >= 1 && aashell >= 1 && director >= 1) //戦艦系汎用
				aacutinlist.Add(6);
			if (highangle_director >= 1 && aaradar >= 1)
				aacutinlist.Add(8);
			if (highangle_director >= 1 && aagun_concentrated >= 1 && aaradar >= 1 && shipID != 428)
				aacutinlist.Add(13);
			if (highangle >= 1 && director >= 1 && aaradar >= 1)
				aacutinlist.Add(7);
			if (aagun_concentrated >= 1 && aagun_medium3 >= 2 && aaradar >= 1)
				aacutinlist.Add(12);
			if (highangle >= 1 && director >= 1)
				aacutinlist.Add(9);

			return aacutinlist.ToArray();
		}


		/// <summary>
		/// 個別の輸送作戦成功時の輸送量(減少TP)を求めます。
		/// (S勝利時のもの。A勝利時は int( value * 0.7 ) )
		/// </summary>
		/// <param name="ship">対象の艦娘。</param>
		/// <returns>減少TP。</returns>
		public static int GetTPDamage(ShipData ship)
		{
			int tp = 0;

			// 装備ボーナス
			foreach (var eq in ship.AllSlotInstanceMaster.Where(q => q != null))
			{

				switch (eq.CategoryType)
				{

					case EquipmentTypes.LandingCraft:
						tp += 8;
						break;

					case EquipmentTypes.TransportContainer:
						tp += 5;
						break;

					case EquipmentTypes.Ration:
						tp += 1;
						break;

					case EquipmentTypes.SpecialAmphibiousTank:
						tp += 2;
						break;
				}
			}

			// 艦種ボーナス
			switch (ship.MasterShip.ShipType)
			{

				case ShipTypes.Destroyer:
					tp += 5;
					break;

				case ShipTypes.LightCruiser:
					tp += 2;
					if (ship.ShipID == 487) // 鬼怒改二
						tp += 8;
					break;

				case ShipTypes.AviationCruiser:
					tp += 4;
					break;

				case ShipTypes.AviationBattleship:
					tp += 7;
					break;

				case ShipTypes.SeaplaneTender:
					tp += 9;
					break;

				case ShipTypes.AmphibiousAssaultShip:
					tp += 12;
					break;

				case ShipTypes.SubmarineTender:
					tp += 7;
					break;

				case ShipTypes.TrainingCruiser:
					tp += 6;
					break;

				case ShipTypes.FleetOiler:
					tp += 15;
					break;

				case ShipTypes.SubmarineAircraftCarrier:
					tp += 1;
					break;
			}

			if (ship.HPRate < 0.25)
				return 0;
			return tp;
		}
	}
}

