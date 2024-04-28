using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Support;
using Org.BouncyCastle.Math.EC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogShipAttackDetail : Form
	{

		private int _shipID;
		private int loadingResourceShipID;
		private int[] equipID = new int[6];

		private ImageLabel[] Aircrafts;
		private ImageLabel[] Equipments;
		private ImageLabel[] DayAttackNames;
		private ImageLabel[] NightAttackNames;
		private ImageLabel[] DayAttackPowers;
		private ImageLabel[] NightAttackPowers;
		private ImageLabel[] MaxDayAttacksTriggerRates;
		private ImageLabel[] MaxNightAttacksTriggerRates;
		private ImageLabel[] MinDayAttacksTriggerRates;
		private ImageLabel[] MinNightAttacksTriggerRates; 
		private ImageLabel[] DayGroundAttackPowers;
		private ImageLabel[] NightGroundAttackPowers;
		private ImageLabel[] AACutinTypes;
		private ImageLabel[] AACutinNames;
		private ImageLabel[] SupportAirclaftPowers;
		private ImageLabel[] SupportAntiSubmarinePowers12;
		private ImageLabel[] SupportAntiSubmarinePowers15;
		private ImageLabel[] SupportAntiSubmarinePowers20;



		public DialogShipAttackDetail()
		{
			InitializeComponent();

			Aircrafts = new ImageLabel[] { Aircraft1, Aircraft2, Aircraft3, Aircraft4, Aircraft5 };
			Equipments = new ImageLabel[] { Equipment1, Equipment2, Equipment3, Equipment4, Equipment5 };
			DayAttackNames = new ImageLabel[] { DayAttackName1, DayAttackName2, DayAttackName3, DayAttackName4, DayAttackName5, DayAttackName6 };
			NightAttackNames = new ImageLabel[] { NightAttackName1, NightAttackName2, NightAttackName3, NightAttackName4, NightAttackName5, NightAttackName6 };
			DayAttackPowers = new ImageLabel[] { DayAttackPower1, DayAttackPower2, DayAttackPower3, DayAttackPower4, DayAttackPower5 , DayAttackPower6 };
			NightAttackPowers = new ImageLabel[] { NightAttackPower1, NightAttackPower2, NightAttackPower3, NightAttackPower4, NightAttackPower5 , NightAttackPower6 };
			MaxDayAttacksTriggerRates = new ImageLabel[] { MaxDayTriggerRate1, MaxDayTriggerRate2, MaxDayTriggerRate3, MaxDayTriggerRate4, MaxDayTriggerRate5 , MaxDayTriggerRate6 };
			MaxNightAttacksTriggerRates = new ImageLabel[] { MaxNightTriggerRate1, MaxNightTriggerRate2, MaxNightTriggerRate3, MaxNightTriggerRate4, MaxNightTriggerRate5 , MaxNightTriggerRate6 };
			MinDayAttacksTriggerRates = new ImageLabel[] { MinDayTriggerRate1, MinDayTriggerRate2, MinDayTriggerRate3, MinDayTriggerRate4, MinDayTriggerRate5 , MinDayTriggerRate6 };
			MinNightAttacksTriggerRates = new ImageLabel[] { MinNightTriggerRate1, MinNightTriggerRate2, MinNightTriggerRate3, MinNightTriggerRate4, MinNightTriggerRate5 , MinNightTriggerRate6 };
			DayGroundAttackPowers = new ImageLabel[] { DayGrdAtk1, DayGrdAtk2, DayGrdAtk3, DayGrdAtk4, DayGrdAtk5, DayGrdAtk6 };
			NightGroundAttackPowers = new ImageLabel[] { NightGrdAtk1, NightGrdAtk2, NightGrdAtk3, NightGrdAtk4, NightGrdAtk5, NightGrdAtk6 };
			AACutinTypes = new ImageLabel[] { AACutinType1, AACutinType2, AACutinType3, AACutinType4, AACutinType5, AACutinType6, AACutinType7, AACutinType8 };
			AACutinNames = new ImageLabel[] { AACutinName1, AACutinName2, AACutinName3, AACutinName4, AACutinName5, AACutinName6, AACutinName7, AACutinName8 };
			SupportAirclaftPowers = new ImageLabel[] { SupportAirclaftPower1, SupportAirclaftPower2, SupportAirclaftPower3, SupportAirclaftPower4, SupportAirclaftPower5 };
			SupportAntiSubmarinePowers12 = new ImageLabel[] { SupportAntiSubmarinePower12_1, SupportAntiSubmarinePower12_2, SupportAntiSubmarinePower12_3, SupportAntiSubmarinePower12_4, SupportAntiSubmarinePower12_5 };
			SupportAntiSubmarinePowers15 = new ImageLabel[] { SupportAntiSubmarinePower15_1, SupportAntiSubmarinePower15_2, SupportAntiSubmarinePower15_3, SupportAntiSubmarinePower15_4, SupportAntiSubmarinePower15_5 };
			SupportAntiSubmarinePowers20 = new ImageLabel[] { SupportAntiSubmarinePower20_1, SupportAntiSubmarinePower20_2, SupportAntiSubmarinePower20_3, SupportAntiSubmarinePower20_4, SupportAntiSubmarinePower20_5 };


			loadingResourceShipID = -1;

			TitleHP.ImageList =
			TitleFirepower.ImageList =
			TitleTorpedo.ImageList =
			TitleAA.ImageList =
			TitleArmor.ImageList =
			TitleASW.ImageList =
			TitleEvasion.ImageList =
			TitleLOS.ImageList =
			TitleLuck.ImageList =
			TitleSpeed.ImageList =
			TitleRange.ImageList =
			TitleBomber.ImageList =
			TitleAccuracy.ImageList =
			TitleCarry.ImageList=
			NavalFreet.ImageList =
			TP.ImageList =
				ResourceManager.Instance.Icons;

			Equipment1.ImageList =
			Equipment2.ImageList =
			Equipment3.ImageList =
			Equipment4.ImageList =
			Equipment5.ImageList =
			EquipmentEx.ImageList =
			TP.ImageList =
				ResourceManager.Instance.Equipments;

			TitleHP.ImageIndex = (int)ResourceManager.IconContent.ParameterHP;
			TitleFirepower.ImageIndex = (int)ResourceManager.IconContent.ParameterFirepower;
			TitleTorpedo.ImageIndex = (int)ResourceManager.IconContent.ParameterTorpedo;
			TitleAA.ImageIndex = (int)ResourceManager.IconContent.ParameterAA;
			TitleArmor.ImageIndex = (int)ResourceManager.IconContent.ParameterArmor;
			TitleASW.ImageIndex = (int)ResourceManager.IconContent.ParameterASW;
			TitleEvasion.ImageIndex = (int)ResourceManager.IconContent.ParameterEvasion;
			TitleLOS.ImageIndex = (int)ResourceManager.IconContent.ParameterLOS;
			TitleLuck.ImageIndex = (int)ResourceManager.IconContent.ParameterLuck;
			TitleAccuracy.ImageIndex = (int)ResourceManager.IconContent.ParameterAccuracy;
			TitleSpeed.ImageIndex = (int)ResourceManager.IconContent.ParameterSpeed;
			TitleRange.ImageIndex = (int)ResourceManager.IconContent.ParameterRange;
			TitleBomber.ImageIndex = (int)ResourceManager.IconContent.ParameterBomber;
			TitleCarry.ImageIndex = (int)ResourceManager.IconContent.ParameterAircraft;
			NavalFreet.ImageIndex = (int)ResourceManager.IconContent.FormFleet;
			TP.ImageIndex = (int)ResourceManager.EquipmentContent.Supplies;

			ControlHelper.SetDoubleBuffered(TableDayAttack);
			ControlHelper.SetDoubleBuffered(TableNightAttack);
			ControlHelper.SetDoubleBuffered(TableGroundAttackList);
			ControlHelper.SetDoubleBuffered(TableParameterMain);
			ControlHelper.SetDoubleBuffered(TableEquipment);
			ControlHelper.SetDoubleBuffered(TableAACutinList);
			ControlHelper.SetDoubleBuffered(TableAirclaftSupport);
			ControlHelper.SetDoubleBuffered(TableAARocketBarrage);
			ControlHelper.SetDoubleBuffered(TableAdjustedAA);
			ControlHelper.SetDoubleBuffered(TableAirBattle);
			ControlHelper.SetDoubleBuffered(TableAirsup);
			ControlHelper.SetDoubleBuffered(TableASWAttack);
			ControlHelper.SetDoubleBuffered(TableSupportShellingPower);
			ControlHelper.SetDoubleBuffered(TableTorpedoAttack);
			ControlHelper.SetDoubleBuffered(NavalFreet);
			ControlHelper.SetDoubleBuffered(TP);

		}

		public DialogShipAttackDetail(int shipID)
			: this()
		{
			UpdateStatusPage(shipID);
			UpdateEqipmentListPage(shipID);
			UpdateBattleListPage(shipID);

		}



		/// <summary>
		/// 艦娘パラメータ表示
		/// </summary>
		/// <param name="shipID"></param>
		private  void UpdateStatusPage(int shipID)
		{
			KCDatabase db = KCDatabase.Instance;
			ShipData shipData = db.Ships[shipID];
			_shipID = shipID;

			BasePanelShipGirl.SuspendLayout();
			TableParameterMain.SuspendLayout();

			HPCurrent.Text = shipData.HPCurrent.ToString();
			Firepower.Text = shipData.FirepowerTotal.ToString();
			Torpedo.Text = shipData.TorpedoTotal.ToString();
			AA.Text = shipData.AATotal.ToString();
			Armor.Text = shipData.ArmorTotal.ToString();
			ASW.Text = shipData.ASWTotal.ToString();
			Evasion.Text = shipData.EvasionTotal.ToString();
			LOS.Text = shipData.LOSTotal.ToString();
			Luck.Text = shipData.LuckTotal.ToString();
			Accuracy.Text = shipData.AccuracyTotal.ToString("+0;-0");
			Bomber.Text = shipData.BomberTotal.ToString("+0;-0");
			Speed.Text = Constants.GetSpeed(shipData.Speed);
			Range.Text = Constants.GetRange(shipData.Range);
			Carry.Text = shipData.AircraftTotal.ToString();

			if (shipData.Fleet != -1)
			{
				int fleetNo = int.Parse(shipData.FleetWithIndex.FirstOrDefault().ToString());
				int number = int.Parse(shipData.FleetWithIndex.LastOrDefault().ToString());
				NavalFreet.Text = number != 1 ? "第" + fleetNo + "艦隊 " + number + "番艦" : "第" + fleetNo + "艦隊 旗艦";
			}
			else
				NavalFreet.Text = "所属艦隊：所属していません";

			TableParameterMain.ResumeLayout();
			BasePanelShipGirl.ResumeLayout();

		}

		/// <summary>
		/// 装備データ表示
		/// </summary>
		/// <param name="shipID"></param>
		private void UpdateEqipmentListPage(int shipID)
		{
			KCDatabase db = KCDatabase.Instance;
			ShipData shipData = db.Ships[shipID];
			ShipDataMaster shipDataMaster = db.MasterShips[shipData.ShipID];

			BasePanelShipGirl.SuspendLayout();
			TableEquipment.SuspendLayout();

			for (int i = 0; i < Equipments.Length; i++)
			{
				equipID[i] = -1;
				if (shipData.Aircraft[i] > 0 || i < shipData.SlotSize)
					Aircrafts[i].Text = shipData.Aircraft[i].ToString();
				else
					Aircrafts[i].Text = "";


				ToolTipInfo.SetToolTip(Equipments[i], null);

				if (shipData.SlotInstance == null)
				{
					if (i < shipData.SlotSize)
					{
						Equipments[i].Text = "???";
						Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Unknown;
					}
					else
					{
						Equipments[i].Text = "";
						Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Locked;
					}

				}
				else if (shipData.SlotMaster[i] != -1)
				{
					EquipmentDataMaster eq = db.MasterEquipments[shipData.SlotMaster[i]];
					if (eq == null)
					{
						// 破損データが入っていた場合
						Equipments[i].Text = "(なし)";
						Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Nothing;
					}
					else
					{
						Equipments[i].Text = shipData.SlotInstance[i].NameWithLevel;
						equipID[i] = shipData.SlotInstance[i].EquipmentID;
						int eqicon = eq.EquipmentType[3];

						if (eqicon >= (int)ResourceManager.EquipmentContent.Locked)
							eqicon = (int)ResourceManager.EquipmentContent.Unknown;

						Equipments[i].ImageIndex = eqicon;
						StringBuilder sb = new StringBuilder();

						sb.AppendFormat("{0} {1} (ID: {2})\r\n", eq.CategoryTypeInstance.Name, shipData.SlotInstance[i].NameWithLevel, eq.EquipmentID);
						if (eq.Firepower != 0) sb.AppendFormat("火力 {0:+0;-0}\r\n", eq.Firepower);
						if (eq.Torpedo != 0) sb.AppendFormat("雷装 {0:+0;-0}\r\n", eq.Torpedo);
						if (eq.AA != 0) sb.AppendFormat("対空 {0:+0;-0}\r\n", eq.AA);
						if (eq.Armor != 0) sb.AppendFormat("装甲 {0:+0;-0}\r\n", eq.Armor);
						if (eq.ASW != 0) sb.AppendFormat("対潜 {0:+0;-0}\r\n", eq.ASW);
						if (eq.Evasion != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? "迎撃" : "回避", eq.Evasion);
						if (eq.LOS != 0) sb.AppendFormat("索敵 {0:+0;-0}\r\n", eq.LOS);
						if (eq.Accuracy != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? "対爆" : "命中", eq.Accuracy);
						if (eq.Bomber != 0) sb.AppendFormat("爆装 {0:+0;-0}\r\n", eq.Bomber);
						sb.AppendLine("(右クリックで図鑑)");

						ToolTipInfo.SetToolTip(Equipments[i], sb.ToString());
					}
				}
				else if (i < shipDataMaster.SlotSize)
				{
					Equipments[i].Text = "";
					Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Nothing;
				}
				else
				{
					Equipments[i].Text = "";
					Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Locked;
				}

				//拡張スロット
				if (shipData.ExpansionSlotInstance != null)
				{
					EquipmentEx.Text = shipData.ExpansionSlotInstance.NameWithLevel;
					equipID[5] = shipData.ExpansionSlotInstance.EquipmentID;
					int eqicon = db.MasterEquipments[shipData.ExpansionSlotInstance.EquipmentID].EquipmentType[3];

					if (eqicon >= (int)ResourceManager.EquipmentContent.Locked)
						eqicon = (int)ResourceManager.EquipmentContent.Unknown;

					EquipmentEx.ImageIndex = eqicon;
					{
						EquipmentDataMaster ex = db.MasterEquipments[shipData.ExpansionSlotMaster];

						StringBuilder sb = new StringBuilder();

						sb.AppendFormat("{0} {1} (ID: {2})\r\n", ex.CategoryTypeInstance.Name, shipData.ExpansionSlotInstance.NameWithLevel, ex.EquipmentID);
						if (ex.Firepower != 0) sb.AppendFormat("火力 {0:+0;-0}\r\n", ex.Firepower);
						if (ex.Torpedo != 0) sb.AppendFormat("雷装 {0:+0;-0}\r\n", ex.Torpedo);
						if (ex.AA != 0) sb.AppendFormat("対空 {0:+0;-0}\r\n", ex.AA);
						if (ex.Armor != 0) sb.AppendFormat("装甲 {0:+0;-0}\r\n", ex.Armor);
						if (ex.ASW != 0) sb.AppendFormat("対潜 {0:+0;-0}\r\n", ex.ASW);
						if (ex.Evasion != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", ex.CategoryType == EquipmentTypes.Interceptor ? "迎撃" : "回避", ex.Evasion);
						if (ex.LOS != 0) sb.AppendFormat("索敵 {0:+0;-0}\r\n", ex.LOS);
						if (ex.Accuracy != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", ex.CategoryType == EquipmentTypes.Interceptor ? "対爆" : "命中", ex.Accuracy);
						if (ex.Bomber != 0) sb.AppendFormat("爆装 {0:+0;-0}\r\n", ex.Bomber);
						sb.AppendLine("(右クリックで図鑑)");

						ToolTipInfo.SetToolTip(EquipmentEx, sb.ToString());
					}
				}
				else
				{
					EquipmentEx.Text = "";
					EquipmentEx.ImageIndex = (int)ResourceManager.EquipmentContent.Nothing;
					equipID[5] = -1;
				}
			}
			int tpdamage = Calculator2.GetTPDamage(shipData);
			TP.Text = "TP輸送量：S " + tpdamage + " / A " + Math.Floor(tpdamage * 0.7);
			TableEquipment.ResumeLayout();
			BasePanelShipGirl.ResumeLayout();
		}


		private async void UpdateBattleListPage(int shipID)
		{

			KCDatabase db = KCDatabase.Instance;
			ShipData shipData = db.Ships[shipID];
			ShipDataMaster ship = db.MasterShips[shipData.ShipID];
			FleetData fleet = db.Fleet[shipData.Fleet];

			if (ship == null) return;

			BasePanelShipGirl.SuspendLayout();

			//攻撃種類
			TableDayAttack.SuspendLayout();
			TableNightAttack.SuspendLayout();

			double[] tempPlaneProf = new double [5];

			var dayAttackList = Calculator2.GetDayAttackKindList(shipData.AllSlotMaster.ToArray(), shipData.ShipID);
			var dayAttackPower = CalcShipAttackPower.CalculateDayAttackPowers(shipData);
			var nightAttackList = Calculator2.GetNightAttackKindList(shipData.AllSlotMaster.ToArray(), shipData.ShipID);
			var nightAttackPower = CalcShipAttackPower.CalculateNightAttackPowers(shipData);

			
			//リスト初期化
			for (int i = 0; i < 5; i++)
			{
				SupportAirclaftPowers[i].Text = "";
				SupportAntiSubmarinePowers12[i].Text = "";
				SupportAntiSubmarinePowers15[i].Text = "";
				SupportAntiSubmarinePowers20[i].Text = "";
				tempPlaneProf[i] = 0;
			}

			for (int i = 0; i < 6; i++)
			{
				DayAttackNames[i].Text = "";
				NightAttackNames[i].Text = "";
				DayAttackPowers[i].Text = "";
				NightAttackPowers[i].Text = "";
				MaxDayAttacksTriggerRates[i].Text = "";
				MaxNightAttacksTriggerRates[i].Text = "";
				MinDayAttacksTriggerRates[i].Text = "";
				MinNightAttacksTriggerRates[i].Text = "";
			}

			for (int i = 0; i < 8; i++)
			{
				AACutinNames[i].Text = "";
				AACutinTypes[i].Text = "";
			}

			//昼戦リスト
			if (dayAttackList.Length != 0)
			{
				foreach (var atk in dayAttackList.Select((name, num) => new { name, num }))
				{
					DayAttackNames[atk.num].Text = Constants.GetDayAttackKind(atk.name);
					DayAttackPowers[atk.num].Text = dayAttackPower[atk.num].ToString();
					MaxDayAttacksTriggerRates[atk.num].Text = "-";
					MinDayAttacksTriggerRates[atk.num].Text = "-";
				}
			}
			else
				DayAttackName1.Text = "攻撃不能";

			//夜戦リスト
			if (nightAttackList.Length != 0)
			{
				foreach (var atk in nightAttackList.Select((name, num) => new { name, num }))
				{
					NightAttackNames[atk.num].Text = Constants.GetNightAttackKind(atk.name);
					NightAttackPowers[atk.num].Text = nightAttackPower[atk.num].ToString();
					MaxNightAttacksTriggerRates[atk.num].Text = "-";
					MinNightAttacksTriggerRates[atk.num].Text = "-";
				}
			}
			else
				NightAttackName1.Text = "攻撃不能";

			//対地リスト
			for (int i = 0; i < 6; i++)
			{
				DayGroundAttackPowers[i].Text = CalcShipAttackPower.CaliculateDayGroundAtttackPower(shipData, i) != 0 ? CalcShipAttackPower.CaliculateDayGroundAtttackPower(shipData, i).ToString() : "攻撃不能";
				NightGroundAttackPowers[i].Text = CalcShipAttackPower.CaliculateNightGroundAtttackPower(shipData, i, nightAttackList) != 0 ? CalcShipAttackPower.CaliculateNightGroundAtttackPower(shipData, i, nightAttackList).ToString() : "攻撃不能";
			}


			//昼戦発動率（艦隊所属時のみ）
			if (shipData.Fleet != -1)
			{
				double otc = 0; //種別係数
				double fleetSarchCorrection = 0;
				double observationtermMax = 0;  //観測項確保
				double observationtermMin = 0;  //観測項優勢
				double leftTriggerRatesMax = 100;
				double leftTriggerRatesMin = 100;
				int shipsLOSBase = 0;		//Σ(艦娘の素の索敵値)
				int equipsLOS = 0;           //Σ(攻撃艦の装備索敵値)
				double seaplaneTotal = 0;	//Σ(水偵or水爆の装備索敵値×⌊√(水偵or水爆の機数)⌋
				var flagship = fleet.MembersWithoutEscaped[0].ID == shipData.ID;

				foreach (var members_d in fleet.MembersWithoutEscaped)
				{
					if (members_d != null)
					{
						shipsLOSBase += members_d.LOSBase;
						for (int i = 0; i < members_d.Slot.Count; i++)
						{
							var eq = members_d.SlotInstance[i];
							if (eq != null)
							{
								if (eq.MasterEquipment.CategoryType == EquipmentTypes.SeaplaneBomber || eq.MasterEquipment.CategoryType == EquipmentTypes.SeaplaneRecon)
									seaplaneTotal += members_d.SlotInstance[i].MasterEquipment.LOS * Math.Floor(Math.Sqrt(members_d.Aircraft[i]));
							}
						}
					}
				}
				
				for (int i = 0; i < shipData.AllSlot.Count; i++)
				{
					equipsLOS += shipData.AllSlotInstance[i] != null ? shipData.AllSlotInstance[i].MasterEquipment.LOS : 0;
				}

				//艦隊索敵補正：⌊√(A) + 0.1×A⌋
				fleetSarchCorrection = Math.Floor((Math.Sqrt(shipsLOSBase + seaplaneTotal)) + 0.1 * (shipsLOSBase + seaplaneTotal));
				
				//観測項：⌊(⌊√(運)+10⌋ + 0.7×(艦隊索敵補正 + 1.6×(Σ(攻撃艦の装備索敵値)) +10)⌋ + 旗艦補正 （確保と優勢）
				observationtermMax = Math.Floor(Math.Floor(Math.Sqrt(shipData.LuckTotal) + 10) + 0.7 * (fleetSarchCorrection + (1.6 * equipsLOS)) + 10) + (flagship? 15 : 0);
				observationtermMin = Math.Floor(Math.Floor(Math.Sqrt(shipData.LuckTotal) + 10) + 0.6 * (fleetSarchCorrection + (1.2 * equipsLOS))) + (flagship ? 15 : 0);

				//種別係数算出
				foreach (var atk in dayAttackList.Select((name, num) => new { name, num }))
				{
					switch (atk.name.ToString())
					{
						case "CutinFighterBomberAttacker":
							otc = 125;
							break;
						case "CutinBomberBomberAttacker":
							otc = 140;
							break;
						case "CutinBomberAttacker":
							otc = 155;
							break;
						case "CutinMainMain":
							otc = 150;
							break;
						case "CutinMainAP":
							otc = 140;
							break;
						case "CutinMainRadar":
						case "DoubleShelling":
							otc = 130;
							break;
						case "CutinMainSub":
						case "ZuiunMultiAngle":
							otc = 120;
							break;
						case "SeaAirMultiAngle":
							otc = 130;
							break;
					}

					MaxDayAttacksTriggerRates[atk.num].Text = ((observationtermMax / otc) * leftTriggerRatesMax).ToString("F1") + "％";
					MinDayAttacksTriggerRates[atk.num].Text = ((observationtermMin / otc) * leftTriggerRatesMin).ToString("F1") + "％"; 

					if (atk.name.ToString() == "Shelling" || atk.name.ToString() == "AirAttack")
					{
						MaxDayAttacksTriggerRates[atk.num].Text = leftTriggerRatesMax.ToString("F1") + "％";
						MinDayAttacksTriggerRates[atk.num].Text = leftTriggerRatesMin.ToString("F1") + "％";
					}
					else
					{
						leftTriggerRatesMax -= ((observationtermMax / otc) * leftTriggerRatesMax);
						leftTriggerRatesMin -= ((observationtermMin / otc) * leftTriggerRatesMin);
					}
				}

				//夜戦発動率
				var members_n = fleet.MembersInstance.Where(s => s != null);
				var count_Searchlight = members_n.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.Searchlight || eq?.CategoryType == EquipmentTypes.SearchlightLarge));
				var count_StarShell = members_n.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.StarShell));
				var count_PicketCrew = shipData.AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 129);
				var count_MasterPicketCrew = shipData.AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 412);
				if (shipData.MasterShip?.ShipType != ShipTypes.Destroyer && shipData.MasterShip?.ShipType != ShipTypes.LightCruiser && shipData.MasterShip?.ShipType != ShipTypes.TorpedoCruiser)
					count_MasterPicketCrew = 0;

				if (nightAttackList.Length > 0 || (nightAttackList.Length > 0 && Constants.GetDamageState(shipData.HPRate) != "大破"))
				{
					double ci = 0;
					int equipState = 0;
					int fsflag = flagship ? 15 : 0;
					int dmgState = Constants.GetDamageState(shipData.HPRate) =="中破" ? 18 : 0;
					leftTriggerRatesMax = 100;
					leftTriggerRatesMin = 100;
					otc = 0;

					if (count_Searchlight.Sum() != 0)
						equipState += 7;
					else if (count_StarShell.Sum() != 0)
						equipState += 4;
					if (count_PicketCrew != 0)
						equipState += 5;
					if (count_MasterPicketCrew != 0)
						equipState += 8;

					if (shipData.LuckTotal < 50)
						ci = 15 + shipData.LuckTotal + Math.Floor(0.75 * Math.Sqrt(shipData.Level)) + fsflag + dmgState + equipState;
					else
						ci = 65 + Math.Floor(Math.Sqrt(shipData.LuckTotal - 50)) + Math.Floor(0.8 * Math.Sqrt(shipData.Level)) + fsflag + dmgState + equipState;

					foreach (var atk in nightAttackList.Select((name, num) => new { name, num }))
					{

						switch (atk.name.ToString())
						{
							case "CutinNightAirAttackFFA":
								otc = 105;
								break;
							case "CutinNightAirAttackFA":
								otc = 120;
								break;
							case "CutinNightAirAttackFS":
							case "CutinNightAirAttackAS":
								otc = 120;
								break;
							case "CutinNightAirAttackFOther":
								otc = 130;
								break;
							case "CutinTorpedoMasterPicketSubmarine":
								otc = 105;
								break;
							case "CutinTorpedoTorpedoSubmarine":
								otc = 110;
								break;
							case "SpecialNightZuiun":
							case "SpecialNightZuiunRader":
							case "SpecialNightZuiun2":
							case "SpecialNightZuiun2Rader":
								otc = 135;
								break;
							case "CutinTorpedoRadar_":
								otc = 115;
								break;
							case "CutinTorpedoPicket_":
								otc = 140;
								break;
							case "CutinTorpedoTorpedoMasterPicket_":
								otc = 126;
								break;
							case "CutinTorpedoDrumMasterPicket_":
								otc = 122;
								break;
							case "CutinMainMain":
								otc = 140;
								break;
							case "CutinMainSub":
								otc = 130;
								break;
							case "CutinTorpedoTorpedo":
								otc = 122;
								break;
							case "CutinMainTorpedo":
								otc = 115;
								break;
						}
						

						switch (atk.name.ToString())
						{
							case "DoubleShelling":
								MaxNightAttacksTriggerRates[atk.num].Text = (leftTriggerRatesMax * 0.991).ToString("F1") + "％";
								MinNightAttacksTriggerRates[atk.num].Text = (leftTriggerRatesMin * 0.991).ToString("F1") + "％";
								leftTriggerRatesMax -= 0.991 * leftTriggerRatesMax;
								leftTriggerRatesMin -= 0.991 * leftTriggerRatesMin;
								break;
							case "NormalAttack":
							case "NightAirAttack":
							case "AirAttack":
								MaxNightAttacksTriggerRates[atk.num].Text = leftTriggerRatesMax.ToString("F1") + "％";
								MinNightAttacksTriggerRates[atk.num].Text = leftTriggerRatesMin.ToString("F1") + "％";
								break;
							default:
								MaxNightAttacksTriggerRates[atk.num].Text = ((ci / otc) * leftTriggerRatesMax).ToString("F1") + "％";
								MinNightAttacksTriggerRates[atk.num].Text = ((ci / otc) * leftTriggerRatesMin).ToString("F1") + "％";
								leftTriggerRatesMax -= ((ci / otc) * leftTriggerRatesMax);
								leftTriggerRatesMin -= ((ci / otc) * leftTriggerRatesMin);
								break;

						}
					}
				}
			}
			TableDayAttack.ResumeLayout();
			TableNightAttack.ResumeLayout();


			//雷撃
			TableTorpedoAttack.SuspendLayout();
			if (shipData.TorpedoPower > 0)
			{
				TableTorpedoAttack.Visible = true;
				TorpedoAttackPower.Text = shipData.TorpedoPower.ToString();
				if (shipData.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.MidgetSubmarine) > 0 ||
					(shipData.MasterShip.ShipType == ShipTypes.Submarine && shipData.Level >= 10) ||
					(shipData.MasterShip.ShipType == ShipTypes.SubmarineAircraftCarrier && shipData.Level >= 10))
					TorpedoAttackComment.Text = " ※先制雷撃：〇";
				else
					TorpedoAttackComment.Text = "";
			}
			else
				TableTorpedoAttack.Visible = false;
			TableTorpedoAttack.ResumeLayout();

			//対潜
			TableASWAttack.SuspendLayout();
			if (shipData.AntiSubmarinePower > 0)
			{
				TableASWAttack.Visible = true;
				ASWAttackPower.Text = shipData.AntiSubmarinePower.ToString();
				switch (shipData.SynergyCount)
				{
					case 3:
						ASWAttackComment.Text = " ※3種シナジー";
						break;
					case 2:
						ASWAttackComment.Text = " ※2種シナジー";
						break;
					case 1:
						ASWAttackComment.Text = " ※2種シナジー(迫撃砲)";
						break;
					default:
						ASWAttackComment.Text = "";
						break;
				}
				ASWAttackComment.Text += ASWAttackComment?.Text != "" ? shipData.CanOpeningASW ? "・先制対潜：〇" : "先制対潜：×" : shipData.CanOpeningASW ? " ※先制対潜：〇" : " ※先制対潜：×";
			}
			else
				TableASWAttack.Visible = false;
			TableASWAttack.ResumeLayout();

			//制空
			TableAirsup.SuspendLayout();
			if (Calculator.GetAirSuperiority(shipData) > 0)
			{ 
				TableAirsup.Visible = true;
				double[] airsup = new double[5];
				AirsupPower.Text = Calculator.GetAirSuperiority(shipData).ToString();
				for(int i = 0; i < shipData.SlotInstance.Count(); i++)
				{
					if (shipData.SlotInstance[i] != null)
						airsup[i] = Calculator.GetAirSuperiority(shipData.SlotInstance[i].EquipmentID, ship.Aircraft[i], shipData.SlotInstance[i].AircraftLevel, shipData.SlotInstance[i].Level, -1, false);
					else
						airsup[i] = 0;
				}
				AirsupPowerDetail.Text = " ( " + airsup[0] + " ｜ " + airsup[1] + " ｜ " + airsup[2] + " ｜ " + airsup[3] + " ｜ " + airsup[4] + " )";
			}
			else
				TableAirsup.Visible = false;
			TableAirsup.ResumeLayout();

			//航空戦
			TableAirBattle.SuspendLayout();
			if (shipData.AirBattlePower > 0)
			{
				TableAirBattle.Visible = true;
				double[] airbattle = new double[5];
				bool attacker = false;
				AirBattlePower.Text = shipData.AirBattlePower.ToString();
				for (int i = 0; i < shipData.SlotInstance.Count(); i++)
				{
					attacker = (attacker == true || shipData.SlotInstanceMaster[i]?.CategoryType == EquipmentTypes.CarrierBasedTorpedo); 
					if (shipData.SlotInstance[i] != null)
						airbattle[i] = shipData.AirBattlePowers[i];
					else
						airbattle[i] = 0;
				}
				AirBattleDetail.Text = " ( " + airbattle[0] + " ｜ " + airbattle[1] + " ｜ " + airbattle[2] + " ｜ " + airbattle[3] + " ｜ " + airbattle[4] + " )"; 
				AirBattleDetail.Text += attacker? " ※艦攻威力×1.5": "";
			}
			else
				TableAirBattle.Visible = false;
			TableAirBattle.ResumeLayout();

			//加重対空
			TableAdjustedAA.SuspendLayout();
			double adjustedaa = Calculator.GetAdjustedAAValue(shipData);
			if (adjustedaa > 0)
			{
				TableAdjustedAA.Visible = true;
				AdjustedAA.Text = adjustedaa.ToString();
				AdjustedAADetail.Text = " ※割合撃墜：" + (Calculator.GetProportionalAirDefense(adjustedaa) * 100).ToString("F2") + "％";
			}
			else
				TableAdjustedAA.Visible = false;
			TableAdjustedAA.ResumeLayout();

			//噴進弾幕
			TableAARocketBarrage.SuspendLayout();
			double rocket = Calculator.GetAARocketBarrageProbability(shipData);
			if (rocket > 0)
			{
				TableAARocketBarrage.Visible = true;
				AARocketBarrage.Text = (rocket *100).ToString("F1") + "％";
			}
			else
				TableAARocketBarrage.Visible=false;
			TableAARocketBarrage.ResumeLayout();

			//対空カットイン
			TableAACutinList.SuspendLayout();
			var aacutintypelist = Calculator2.GetAACutinKind(shipData.ShipID, shipData.AllSlotMaster.ToArray(), shipData.ID);
			if ( aacutintypelist.Length != 0 )
			{
				TableAACutinList.Visible = true;
				foreach ( var aacut in aacutintypelist.Select((name, num) => new { name, num }))
				{
					AACutinTypes[aacut.num].Text = (aacut.name >= 4 && aacut.name <= 9)? aacut.name + "[汎用]" : aacut.name + "[固有]";
					AACutinNames[aacut.num].Text = Constants.GetAACutinKind(aacut.name);
				}
			}
			else
			{
				AACutinName1.Text = "何も発動しませんよ";
			}
			TableAACutinList.ResumeLayout();

			//砲撃支援
			var support_ShellingPower = CalcShipAttackPower.CalculateSupportShellingPower(shipData);
			//SupportShellingPower.Text = support_ShellingPower.ToString();
			SupportShellingPower.Text = support_ShellingPower > 0 ? support_ShellingPower.ToString() : "攻撃不能";
			
			//航空支援
			TableAirclaftSupport.ResumeLayout();
			var support_AircraftPowers = shipData.Slot.Select((_, i) => CalcShipAttackPower.CalculateSupportAirclaftPower(i, shipData)).ToArray();
			foreach (var sair in support_AircraftPowers.Select((name, num) => new { name, num }))
			{
				if (sair.name == -1)
				{
					if (sair.num == 0)
					{
						SupportAirclaftPowers[sair.num].Text = "攻撃不能";
					}
				}
				else
					SupportAirclaftPowers[sair.num].Text = sair.name != -1 ? sair.name.ToString() : "";
			}
			
			//対潜支援
			var support_AntiSubmarinePower = shipData.Slot.Select((_, i) => CalcShipAttackPower.CalculateSupportAntiSubmarinePower(i, shipData)).ToArray();
			foreach (var sasw in support_AntiSubmarinePower.Select((name, num) => new { name, num }))
			{
				if (sasw.name == -1)
				{
					if(sasw.num == 0)
					{
						SupportAntiSubmarinePowers12[sasw.num].Text = "攻撃不能";
						SupportAntiSubmarinePowers15[sasw.num].Text = "攻撃不能";
						SupportAntiSubmarinePowers20[sasw.num].Text = "攻撃不能";
					}
				}
				else
				{
					SupportAntiSubmarinePowers12[sasw.num].Text = sasw.name != -1 ? (sasw.name * 1.2).ToString("F0") : "";
					SupportAntiSubmarinePowers15[sasw.num].Text = sasw.name != -1 ? (sasw.name * 1.5).ToString("F0") : "";
					SupportAntiSubmarinePowers20[sasw.num].Text = sasw.name != -1 ? (sasw.name * 2.0).ToString("F0") : "";

				}
			}
			TableAirclaftSupport.SuspendLayout();



			//タイトルバー
			this.Text = "攻撃詳細 - " + shipData.NameWithLevel + "  (ID:" + shipID +")";

		}


		private void DialogShipAttackDetail_Load(object sender, EventArgs e)
		{
			APIObserver o = APIObserver.Instance;

			o["api_req_nyukyo/start"].RequestReceived += Updated;
			o["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o["api_req_hensei/change"].RequestReceived += Updated;
			o["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o["api_req_member/updatedeckname"].RequestReceived += Updated;
			o["api_req_kaisou/remodeling"].RequestReceived += Updated;
			o["api_req_map/start"].RequestReceived += Updated;
			o["api_req_hensei/combined"].RequestReceived += Updated;
			o["api_req_kaisou/open_exslot"].RequestReceived += Updated;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_get_member/ship2"].ResponseReceived += Updated;
			o["api_get_member/ndock"].ResponseReceived += Updated;
			o["api_req_kousyou/getship"].ResponseReceived += Updated;
			o["api_req_hokyu/charge"].ResponseReceived += Updated;
			o["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o["api_get_member/ship3"].ResponseReceived += Updated;
			o["api_req_kaisou/powerup"].ResponseReceived += Updated;        //requestのほうは面倒なのでこちらでまとめてやる
			o["api_get_member/deck"].ResponseReceived += Updated;
			o["api_get_member/slot_item"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_map/next"].ResponseReceived += Updated;
			o["api_get_member/ship_deck"].ResponseReceived += Updated;
			o["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
			o["api_get_member/require_info"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;
			o["api_req_kaisou/marriage"].ResponseReceived += Updated;
			o["api_req_map/anchorage_repair"].ResponseReceived += Updated;

			this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle]);

		}

		void Updated(string apiname, dynamic data)
		{
			KCDatabase db = KCDatabase.Instance;
			ShipData shipData = db.Ships[_shipID];
			if (shipData != null)
			{
				UpdateStatusPage(_shipID);
				UpdateEqipmentListPage(_shipID);
				UpdateBattleListPage(_shipID);
			}
			else
			{ 
				Errorpanel.Visible = true;
				ErrorMessage.Visible = true;
			}
		}


		private void TableParameterMain_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		private void TableEquipment_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		private void TableTitleAttack_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		private void TableOtherAttacks_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}


		private void Equipment_MouseClick(object sender, MouseEventArgs e)
		{

			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				for (int i = 0; i < Equipments.Length; i++)
				{
					if (sender == Equipments[i])
					{
						if (equipID[i] != -1)
						{
								Cursor = Cursors.AppStarting;
								new DialogAlbumMasterEquipment(KCDatabase.Instance.MasterEquipments[equipID[i]].EquipmentID).Show(Owner);
								Cursor = Cursors.Default;
						}
					}
				}
			}
		}


		private void EquipmenEX_MouseClick(object sender, MouseEventArgs e)
		{

			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if (equipID[5] != -1)
				{
					Cursor = Cursors.AppStarting;
					new DialogAlbumMasterEquipment(KCDatabase.Instance.MasterEquipments[equipID[5]].EquipmentID).Show(Owner);
					Cursor = Cursors.Default;
				}
			}
		}


		private void DialogShipAttackDetail_FormClosed(object sender, FormClosedEventArgs e)
		{

			ResourceManager.DestroyIcon(Icon);

		}
	}
}
