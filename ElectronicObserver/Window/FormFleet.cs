﻿using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{
	public partial class FormFleet : DockContent
	{
		private bool IsRemodeling = false;

		private class TableFleetControl : IDisposable
		{
			public Label Name;
			public FleetState State;
			public ImageLabel AirSuperiority;
			public ImageLabel SearchingAbility;
			public ImageLabel AntiAirPower;
			public ToolTip ToolTipInfo;

			public int BranchWeight { get; private set; } = 1;

			public TableFleetControl(FormFleet parent)
			{
				#region Initialize

				Name = new Label
				{
					Text = "[" + parent.FleetID.ToString() + "]",
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					UseMnemonic = false,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true,
					//Name.Visible = false;
					Cursor = Cursors.Help
				};

				State = new FleetState
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					Padding = new Padding(),
					Margin = new Padding(),
					AutoSize = true
				};

				AirSuperiority = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};

				SearchingAbility = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedRecon,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};
				SearchingAbility.Click += (sender, e) => SearchingAbility_Click(sender, e, parent.FleetID);

				AntiAirPower = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};


				ConfigurationChanged(parent);

				ToolTipInfo = parent.ToolTipInfo;

				#endregion
			}

			public TableFleetControl(FormFleet parent, TableLayoutPanel table)
				: this(parent)
			{
				AddToTable(table);
			}

			public void AddToTable(TableLayoutPanel table)
			{
				table.SuspendLayout();
				table.Controls.Add(Name, 0, 0);
				table.Controls.Add(State, 1, 0);
				table.Controls.Add(AirSuperiority, 2, 0);
				table.Controls.Add(SearchingAbility, 3, 0);
				table.Controls.Add(AntiAirPower, 4, 0);
				table.ResumeLayout();
			}

			private void SearchingAbility_Click(object sender, EventArgs e, int fleetID)
			{
				BranchWeight--;
				if (BranchWeight <= 0)
					BranchWeight = 4;

				Update(KCDatabase.Instance.Fleet[fleetID]);
			}

			public void Update(FleetData fleet)
			{
				KCDatabase db = KCDatabase.Instance;

				if (fleet == null) return;

				Name.Text = fleet.Name;
				{
					var members = fleet.MembersInstance.Where(s => s != null);

					int levelSum = members.Sum(s => s.Level);

					int fueltotal = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * (s.IsMarried ? 0.85 : 1.00)), 1));
					int ammototal = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * (s.IsMarried ? 0.85 : 1.00)), 1));

					int fuelunit = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));
					int ammounit = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));

					int speed = members.Select(s => s.Speed).DefaultIfEmpty(20).Min();

					string supporttype;
					switch (fleet.SupportType)
					{
						case 0:
						default:
							supporttype = "発動不能"; break;
						case 1:
							supporttype = "航空支援"; break;
						case 2:
							supporttype = "支援射撃"; break;
						case 3:
							supporttype = "支援長距離雷撃"; break;
						case 4:
							supporttype = "対潜支援哨戒"; break;
					}

					double expeditionBonus = Calculator.GetExpeditionBonus(fleet);
					int tp = Calculator.GetTPDamage(fleet);

					// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
					var transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
					var landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.LandingCraft || eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank));
					
					// 煙幕発動率
					var smokeGenrate = Calculator.GetSmokeTriggerRates(fleet, null);

					ToolTipInfo.SetToolTip(Name, string.Format(
						"Lv合計: {0} / 平均: {1:0.00}\r\n" +
						"{2}艦隊\r\n" +
						"支援攻撃: {3}\r\n" +
						"合計火力 {4:0.0} / 対空 {5:0.0} / 対潜 {6:0.0} / 索敵 {7:0.0}\r\n" +
						"ドラム缶搭載: {8}個 ({9}艦)\r\n" +
						"大発動艇搭載: {10}個 ({11}艦, +{12:p1})\r\n" +
						"輸送量(TP): S {13} / A {14}\r\n" +
						"総積載: 燃 {15} / 弾 {16}\r\n" +
						"(1戦当たり 燃 {17} / 弾 {18})\r\n" +
						"煙幕発動 3重:{19:0.0} / 2重:{20:0.0} / 1重:{21:0.0} / 不発:{22:0.0}",
						levelSum,
						(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1),
						Constants.GetSpeed(speed),
						supporttype,
						(double)members.Sum(s => s.ExpeditionFire) / 10,
						(double)members.Sum(s => s.ExpeditionAA) /10,
						(double)members.Sum(s => s.ExpeditionASW) / 10,
						(double)members.Sum(s => s.ExpeditionLOS) / 10,
						transport.Sum(),
						transport.Count(i => i > 0),
						landing.Sum(),
						landing.Count(i => i > 0),
						expeditionBonus,
						tp,
						(int)(tp * 0.7),
						fueltotal,
						ammototal,
						fuelunit,
						ammounit,
						smokeGenrate[0],
						smokeGenrate[1],
						smokeGenrate[2],
						smokeGenrate[3]
						));

				}
				State.UpdateFleetState(fleet, ToolTipInfo);

				//制空戦力計算	
				{
					int airSuperiority = fleet.GetAirSuperiority();
					bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;
					AirSuperiority.Text = fleet.GetAirSuperiorityString();
					ToolTipInfo.SetToolTip(AirSuperiority,
						string.Format("確保: {0}\r\n優勢: {1}\r\n均衡: {2}\r\n劣勢: {3}\r\n({4}: {5})\r\n",
						(int)(airSuperiority / 3.0),
						(int)(airSuperiority / 1.5),
						Math.Max((int)(airSuperiority * 1.5 - 1), 0),
						Math.Max((int)(airSuperiority * 3.0 - 1), 0),
						includeLevel ? "熟練度なし" : "熟練度あり",
						includeLevel ? Calculator.GetAirSuperiorityIgnoreLevel(fleet) : Calculator.GetAirSuperiority(fleet)));
				}

				//索敵能力計算
				SearchingAbility.Text = fleet.GetSearchingAbilityString(BranchWeight);
				{
					StringBuilder sb = new StringBuilder();
					double probStart = fleet.GetContactProbability();
					var probSelect = fleet.GetContactSelectionProbability();

					sb.AppendFormat("新判定式(33) 分岐点係数: {0}\r\n　(クリックで切り替え)\r\n\r\n触接開始率: \r\n　確保 {1:p1} / 優勢 {2:p1}\r\n",
						BranchWeight,
						probStart,
						probStart * 0.6);

					if (probSelect.Count > 0)
					{
						sb.AppendLine("触接選択率: ");

						foreach (var p in probSelect.OrderBy(p => p.Key))
						{
							sb.AppendFormat("　命中{0} : {1:p1}\r\n", p.Key, p.Value);
						}
					}

					ToolTipInfo.SetToolTip(SearchingAbility, sb.ToString());
				}

				// 対空能力計算
				{
					var sb = new StringBuilder();
					double lineahead = Calculator.GetAdjustedFleetAAValue(fleet, 1);

					AntiAirPower.Text = lineahead.ToString("0.0");

					sb.AppendFormat("艦隊防空\r\n単縦陣: {0:0.0} / 複縦陣: {1:0.0} / 輪形陣: {2:0.0}\r\n",
						lineahead,
						Calculator.GetAdjustedFleetAAValue(fleet, 2),
						Calculator.GetAdjustedFleetAAValue(fleet, 3));

					ToolTipInfo.SetToolTip(AntiAirPower, sb.ToString());
				}
			}

			public void Refresh()
			{
				State.RefreshFleetState();
			}

			public void ConfigurationChanged(FormFleet parent)
			{
				Name.Font = parent.MainFont;
				State.Font = parent.MainFont;
				State.RefreshFleetState();
				AirSuperiority.Font = parent.MainFont;
				SearchingAbility.Font = parent.MainFont;
				AntiAirPower.Font = parent.MainFont;

				ControlHelper.SetTableRowStyles(parent.TableFleet, ControlHelper.GetDefaultRowStyle());
			}

			public void Dispose()
			{
				Name.Dispose();
				State.Dispose();
				AirSuperiority.Dispose();
				SearchingAbility.Dispose();
				AntiAirPower.Dispose();
			}
		}

		private class TableMemberControl : IDisposable
		{
			public ImageLabel Name;
			public ShipStatusLevel Level;
			public ShipStatusHP HP;
			public ImageLabel Condition;
			public ShipStatusResource ShipResource;
			public ShipStatusEquipment Equipments;

			private ToolTip ToolTipInfo;
			private FormFleet Parent;

			public TableMemberControl(FormFleet parent)
			{
				#region Initialize

				Name = new ImageLabel();
				Name.SuspendLayout();
				Name.Text = "*nothing*";
				Name.Anchor = AnchorStyles.Left;
				Name.TextAlign = ContentAlignment.MiddleLeft;
				Name.ImageAlign = ContentAlignment.MiddleCenter;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding(2, 1, 2, 1);
				Name.Margin = new Padding(2, 1, 2, 1);
				Name.AutoSize = true;
				//Name.AutoEllipsis = true;
				Name.Visible = false;
				Name.Cursor = Cursors.Help;
				Name.MouseDown += Name_MouseDown;
				Name.ResumeLayout();

				Level = new ShipStatusLevel();
				Level.SuspendLayout();
				Level.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
				Level.Value = 0;
				Level.MaximumValue = ExpTable.ShipMaximumLevel;
				Level.ValueNext = 0;
				Level.MainFontColor = parent.MainFontColor;
				Level.SubFontColor = parent.SubFontColor;
				//Level.TextNext = "n.";
				Level.Padding = new Padding(0, 0, 0, 0);
				Level.Margin = new Padding(2, 0, 2, 1);
				Level.AutoSize = true;
				Level.Visible = false;
				Level.Cursor = Cursors.Help;
				Level.MouseDown += Level_MouseDown;
				Level.ResumeLayout();

				HP = new ShipStatusHP();
				HP.SuspendUpdate();
				HP.Anchor = AnchorStyles.Left;
				HP.Value = 0;
				HP.MaximumValue = 0;
				HP.MaximumDigit = 999;
				HP.UsePrevValue = false;
				HP.MainFontColor = parent.MainFontColor;
				HP.SubFontColor = parent.SubFontColor;
				HP.Padding = new Padding(0, 0, 0, 0);
				HP.Margin = new Padding(2, 1, 2, 2);
				HP.AutoSize = true;
				HP.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				HP.Visible = false;
				HP.ResumeUpdate();

				Condition = new ImageLabel();
				Condition.SuspendLayout();
				Condition.Text = "*";
				Condition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				Condition.ForeColor = parent.MainFontColor;
				Condition.TextAlign = ContentAlignment.BottomRight;
				Condition.ImageAlign = ContentAlignment.MiddleLeft;
				Condition.ImageList = ResourceManager.Instance.Icons;
				Condition.Padding = new Padding(2, 1, 2, 1);
				Condition.Margin = new Padding(2, 1, 2, 1);
				Condition.Size = new Size(40, 20);
				Condition.AutoSize = true;
				Condition.Visible = false;
				Condition.ResumeLayout();

				ShipResource = new ShipStatusResource(parent.ToolTipInfo);
				ShipResource.SuspendLayout();
				ShipResource.FuelCurrent = 0;
				ShipResource.FuelMax = 0;
				ShipResource.AmmoCurrent = 0;
				ShipResource.AmmoMax = 0;
				ShipResource.Anchor = AnchorStyles.Left;
				ShipResource.Padding = new Padding(0, 2, 0, 0);
				ShipResource.Margin = new Padding(2, 0, 2, 1);
				ShipResource.Size = new Size(30, 20);
				ShipResource.AutoSize = false;
				ShipResource.Visible = false;
				ShipResource.ResumeLayout();

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendUpdate();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding(0, 1, 0, 1);
				Equipments.Margin = new Padding(2, 0, 2, 1);
				Equipments.Size = new Size(40, 20);
				Equipments.AutoSize = true;
				Equipments.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				Equipments.Visible = false;
				Equipments.MouseDown += Equipments_MouseDown;
				Equipments.ResumeUpdate();

				ConfigurationChanged(parent);

				ToolTipInfo = parent.ToolTipInfo;
				Parent = parent;
				#endregion
			}

			public TableMemberControl(FormFleet parent, TableLayoutPanel table, int row)
				: this(parent)
			{
				AddToTable(table, row);

				Equipments.Name = string.Format("{0}_{1}", parent.FleetID, row + 1);
			}

			public void AddToTable(TableLayoutPanel table, int row)
			{
				table.SuspendLayout();

				table.Controls.Add(Name, 0, row);
				table.Controls.Add(Level, 1, row);
				table.Controls.Add(HP, 2, row);
				table.Controls.Add(Condition, 3, row);
				table.Controls.Add(ShipResource, 4, row);
				table.Controls.Add(Equipments, 5, row);

				table.ResumeLayout();
			}

			public void Update(int shipMasterID)
			{
				KCDatabase db = KCDatabase.Instance;
				ShipData ship = db.Ships[shipMasterID];

				if (ship != null)
				{

					bool isEscaped = KCDatabase.Instance.Fleet[Parent.FleetID].EscapedShipList.Contains(shipMasterID);
					var equipments = ship.AllSlotInstance.Where(eq => eq != null);
					string spitem ="";
					string sphougeki = "";
					string spraigeki = "";
					string spsoukou = "";
					string spkaihi = "";
					Name.Text = ship.MasterShip.NameWithClass;
					Name.Tag = ship.ShipID;
					switch(ship.SpItemKind)
					{
						case 0:
							spitem = "";
							sphougeki = "";
							spraigeki = "";
							spsoukou = "";
							spkaihi = "";
							break;
						case 1:
							spitem = "★";
							sphougeki = "";
							spraigeki = "+" + ship.SpItemRaig;
							spsoukou = "+" + ship.SpItemSouk;
							spkaihi = "";
							break;
						case 2:
							spitem = "☆";
							sphougeki = "+" + ship.SpItemHoug; 
							spraigeki = "";
							spsoukou = "";
							spkaihi = "+" + ship.SpItemKaih;
							break;
					}
					ToolTipInfo.SetToolTip(Name,
					string.Format(
						"{0}{1} {2}{3}\r\n火力: {4}/{5}{6}\r\n雷装: {7}/{8}{9}\r\n対空: {10}/{11}\r\n装甲: {12}/{13}{14}\r\n対潜: {15}/{16}\r\n回避: {17}/{18}{19}\r\n索敵: {20}/{21}\r\n運: {22}\r\n命中: {23:+#;-#;+0}\r\n爆装: {24:+#;-#;+0}\r\n射程: {25} / 速力: {26}\r\n(右クリックで図鑑)\n",
						ship.SallyArea > 0 ? $"[{ship.SallyArea}] " : "",
						ship.MasterShip.ShipTypeName, ship.NameWithLevel,
						spitem,
						ship.FirepowerBase, ship.FirepowerTotal, sphougeki,
						ship.TorpedoBase, ship.TorpedoTotal, spraigeki,
						ship.AABase, ship.AATotal,
						ship.ArmorBase, ship.ArmorTotal, spsoukou,
						ship.ASWBase, ship.ASWTotal,
						ship.EvasionBase, ship.EvasionTotal, spkaihi,
						ship.LOSBase, ship.LOSTotal,
						ship.LuckTotal,
						equipments.Any() ? equipments.Sum(eq => eq.MasterEquipment.Accuracy) : 0,
						equipments.Any() ? equipments.Sum(eq => eq.MasterEquipment.Bomber) : 0,
						Constants.GetRange(ship.Range),
						Constants.GetSpeed(ship.Speed)
						));
					{
						var colorscheme = Utility.Configuration.Config.FormFleet.SallyAreaColorScheme;

						if (Utility.Configuration.Config.FormFleet.AppliesSallyAreaColor &&
							(colorscheme?.Count ?? 0) > 0 &&
							ship.SallyArea >= 0)
						{
							Name.BackColor = colorscheme[Math.Min(ship.SallyArea, colorscheme.Count - 1)];
						}
						else
						{
							Name.BackColor = SystemColors.Control;
						}
					}

					Level.Value = ship.Level;
					Level.ValueNext = ship.ExpNext;
					Level.Tag = ship.MasterID;

					{
						StringBuilder tip = new StringBuilder();
						tip.AppendFormat("Total: {0} exp.\r\n", ship.ExpTotal);

						if (!Utility.Configuration.Config.FormFleet.ShowNextExp)
							tip.AppendFormat("次のレベルまで: {0} exp.\r\n", ship.ExpNext);

						if (ship.MasterShip.RemodelAfterShipID != 0 && ship.MasterShip.RemodelAfterShipID != ship.MasterShip.FinalRemodelShipID && ship.Level < ship.MasterShip.RemodelAfterLevel)
						{
							tip.AppendFormat("改装まで: Lv. {0} / {1} exp.\r\n", ship.MasterShip.RemodelAfterLevel - ship.Level, ship.ExpNextRemodel);
						}
						else if (ship.MasterShip != ship.MasterShip.FinalRemodelShip && ship.Level < ship.MasterShip.FinalRemodelLevel)
						{
							if (ship.MasterShip.RemodelAfterShipID != ship.MasterShip.FinalRemodelShipID)
							{
								tip.Append("現在改装可能\r\n");
							}
							tip.AppendFormat("最終改装まで: Lv. {0} / {1} exp.\r\n", ship.MasterShip.FinalRemodelLevel - ship.Level, ship.ExpFinalRemodel);
						}
						else
						{
							if (ship.ShipID != ship.MasterShip.FinalRemodelShipID)
							{
								tip.Append("現在最終改装可能\r\n");
							}
							else if (ship.MasterShip.CanConvertRemodel)
							{
								tip.Append("現在コンバート改装可能\r\n");
							}
							if (ship.Level <= 99)
							{
								tip.AppendFormat("Lv99まで: {0} exp.\r\n", Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, 99), 0));
							}
							else
							{
								tip.AppendFormat("Lv{0}まで: {1} exp.\r\n", ExpTable.ShipMaximumLevel, Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, ExpTable.ShipMaximumLevel), 0));
							}
						}

						tip.AppendLine("(右クリックで必要Exp計算)");

						ToolTipInfo.SetToolTip(Level, tip.ToString());
					}

					HP.SuspendUpdate();
					HP.Value = HP.PrevValue = ship.HPCurrent;
					HP.MaximumValue = ship.HPMax;
					HP.UsePrevValue = false;
					HP.ShowDifference = false;
					{
						int dockID = ship.RepairingDockID;

						if (dockID != -1)
						{
							HP.RepairTime = db.Docks[dockID].CompletionTime;
							HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Visible;
						}
						else
						{
							HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Invisible;
						}
					}
					HP.Tag = (ship.RepairingDockID == -1 && 0.5 < ship.HPRate && ship.HPRate < 1.0) ? DateTimeHelper.FromAPITimeSpan(ship.RepairTime).TotalSeconds : 0.0;
					if (isEscaped)
					{
						HP.BackColor = Color.Silver;
					}
					else
					{
						HP.BackColor = SystemColors.Control;
					}
					{
						StringBuilder sb = new StringBuilder();
						double hprate = (double)ship.HPCurrent / ship.HPMax;

						sb.AppendFormat("HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState(hprate));
						if (isEscaped)
						{
							sb.AppendLine("退避中");
						}
						else if (hprate > 0.50)
						{
							sb.AppendFormat("中破まで: {0} / 大破まで: {1}\n", ship.HPCurrent - ship.HPMax / 2, ship.HPCurrent - ship.HPMax / 4);
						}
						else if (hprate > 0.25)
						{
							sb.AppendFormat("大破まで: {0}\n", ship.HPCurrent - ship.HPMax / 4);
						}
						else
						{
							sb.AppendLine("大破しています！");
						}

						if (ship.RepairTime > 0)
						{
							var span = DateTimeHelper.FromAPITimeSpan(ship.RepairTime);
							sb.AppendFormat("入渠時間: {0} @ {1}",
								DateTimeHelper.ToTimeRemainString(span),
								DateTimeHelper.ToTimeRemainString(Calculator.CalculateDockingUnitTime(ship)));
						}

						ToolTipInfo.SetToolTip(HP, sb.ToString());
					}
					HP.ResumeUpdate();

					Condition.Text = ship.Condition.ToString();
					Condition.Tag = ship.Condition;
					SetConditionDesign(Condition, ship.Condition);

					if (ship.Condition < 49)
					{
						TimeSpan ts = new TimeSpan(0, (int)Math.Ceiling((49 - ship.Condition) / 3.0) * 3, 0);
						ToolTipInfo.SetToolTip(Condition, string.Format("完全回復まで 約 {0:D2}:{1:D2}", (int)ts.TotalMinutes, (int)ts.Seconds));
					}
					else
					{
						ToolTipInfo.SetToolTip(Condition, string.Format("あと {0} 回遠征可能", (int)Math.Ceiling((ship.Condition - 49) / 3.0)));
					}

					ShipResource.SetResources(ship.Fuel, ship.FuelMax, ship.Ammo, ship.AmmoMax);

					Equipments.SetSlotList(ship);
					Equipments.Tag = ship.ID;
					ToolTipInfo.SetToolTip(Equipments, GetEquipmentString(ship));
				}
				else
				{
					Name.Tag = -1;
				}

				Name.Visible =
				Level.Visible =
				HP.Visible =
				Condition.Visible =
				ShipResource.Visible =
				Equipments.Visible = shipMasterID != -1;
			}

			void Name_MouseDown(object sender, MouseEventArgs e)
			{
				if (Name.Tag is int id && id != -1)
				{
					if ((e.Button & MouseButtons.Right) != 0)
					{
						new DialogAlbumMasterShip(id).Show(Parent);
					}
				}
			}

			private void Equipments_MouseDown(object sender, MouseEventArgs e)
			{
				if (Equipments.Tag is int id && id != -1)
				{
					if ((e.Button & MouseButtons.Right) != 0)
					{
						new DialogShipAttackDetail(id).Show(Parent);
					}
				}
			}

			private void Level_MouseDown(object sender, MouseEventArgs e)
			{
				if (Level.Tag is int id && id != -1)
				{
					if ((e.Button & MouseButtons.Right) != 0)
					{
						new DialogExpChecker(id).Show(Parent);
					}
				}
			}

			private string GetEquipmentString(ShipData ship)
			{
				StringBuilder sb = new StringBuilder();

				for (int i = 0; i < ship.Slot.Count; i++)
				{
					var eq = ship.SlotInstance[i];
					if (eq != null)
						sb.AppendFormat("[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], eq.NameWithLevel);
				}

				{
					var exslot = ship.ExpansionSlotInstance;
					if (exslot != null)
						sb.AppendFormat("[補強] {0}\r\n", exslot.NameWithLevel);
				}

				var showAntiGroundPower = Utility.Configuration.Config.FormFleet.ShowAntiGroundPower;
				var showSupportPower = Utility.Configuration.Config.FormFleet.ShowSupportPower;
				var showSanmaEquip = Utility.Configuration.Config.FormFleet.ShowSanmaEquip;
				var slotmaster = ship.AllSlotMaster.ToArray();
				var dayAttackList = Calculator2.GetDayAttackKindList(slotmaster, ship.ShipID);
				var dayAttackPower = CalcShipAttackPower.CalculateDayAttackPowers(ship);

				var dayBunker = CalcShipAttackPower.CaliculateDayGroundAtttackPower(ship, 0);
				var dayHardskin = CalcShipAttackPower.CaliculateDayGroundAtttackPower(ship, 1);
				var daySoftskin = CalcShipAttackPower.CaliculateDayGroundAtttackPower(ship, 2);
				var dayMegane = CalcShipAttackPower.CaliculateDayGroundAtttackPower(ship, 3);
				var dayNatsuki = CalcShipAttackPower.CaliculateDayGroundAtttackPower(ship, 4);
				var dayMegane3 = CalcShipAttackPower.CaliculateDayGroundAtttackPower(ship, 5);
				sb.AppendFormat("\r\n【昼戦】");
				if (dayAttackList.Length != 0)
				{
					foreach (var attack in dayAttackList.Select((name, num) => new { name, num }))
					{
						sb.AppendFormat("\r\n {0}: {1} - 威力: {2}",
							attack.num + 1, Constants.GetDayAttackKind(attack.name), dayAttackPower[attack.num]);
					}
					if (showAntiGroundPower)
					{ 
						if (dayBunker!=0)
						{
							sb.AppendFormat("\r\n 0: 対地 - 威力: {0} / {1} / {2} / {3} / {4} / {5}", dayBunker, dayHardskin, daySoftskin, dayMegane, dayNatsuki, dayMegane3);
						}
						else
						{
							sb.AppendFormat("\r\n 0: 対地 - 攻撃不能");
						}
					}
					//sb.AppendLine();
				}
				else sb.AppendFormat("\r\n 攻撃不能");

				if (ship.CanAttackAtNight)
				{
					var nightAttackList = Calculator2.GetNightAttackKindList(slotmaster, ship.ShipID);
					var nightAttackPower = CalcShipAttackPower.CalculateNightAttackPowers(ship);
					var nightBunker = CalcShipAttackPower.CaliculateNightGroundAtttackPower(ship, 0, nightAttackList);
					var nightHardskin = CalcShipAttackPower.CaliculateNightGroundAtttackPower(ship, 1, nightAttackList);
					var nightSoftskin = CalcShipAttackPower.CaliculateNightGroundAtttackPower(ship, 2, nightAttackList);
					var nightMegane = CalcShipAttackPower.CaliculateNightGroundAtttackPower(ship, 3, nightAttackList);
					var nightNatsuki = CalcShipAttackPower.CaliculateNightGroundAtttackPower(ship, 4, nightAttackList);
					var nightMegane3 = CalcShipAttackPower.CaliculateNightGroundAtttackPower(ship, 5, nightAttackList);
					sb.AppendFormat("\r\n【夜戦】");
					if (nightAttackList.Length != 0)
					{
						foreach (var nightAttack in nightAttackList.Select((name, num) => new { name, num }))
						{
							sb.AppendFormat("\r\n {0}: {1}", nightAttack.num + 1, Constants.GetNightAttackKind(nightAttack.name));
							int night = nightAttackPower[nightAttack.num];
							if (night > 0)
							{
								sb.AppendFormat(" - 威力: {0}", night);
							}
						}
						sb.AppendLine();
						if (showAntiGroundPower)
						{
							sb.AppendFormat(" 0: 対地 - 威力: {0} / {1} / {2} / {3} / {4} / {5}", nightBunker, nightHardskin, nightSoftskin, nightMegane, nightNatsuki, nightMegane3);
							sb.AppendLine();
						}
					}
					else sb.AppendLine("\r\n 攻撃不能");
				}
				else
				{
					sb.AppendFormat("\r\n【夜戦】\r\n 攻撃不能");
					sb.AppendLine();
				}

				{
					sb.AppendLine();
					int torpedo = ship.TorpedoPower;
					int asw = ship.AntiSubmarinePower;
					int syn = ship.SynergyCount;

					if (torpedo > 0)
					{
						sb.AppendFormat("雷撃: {0}", torpedo);
					}
					if (asw > 0)
					{
						if (torpedo > 0)
							sb.Append(" / ");

						switch(syn)
						{
							case 3:
								sb.AppendFormat("対潜: {0} ※3種", asw);
								break;
							case 2:
								sb.AppendFormat("対潜: {0} ※2種", asw);
								break;
							case 1:
								sb.AppendFormat("対潜: {0} ※2種弱", asw);
								break;
							default:
								sb.AppendFormat("対潜: {0}", asw);
								break;
						}

						if (ship.CanOpeningASW)
							sb.Append(" (先制可能)");
					}
					if (torpedo > 0 || asw > 0)
						sb.AppendLine();
				}

				{
					int aacutin = Calculator.GetAACutinKind(ship.ShipID, slotmaster);
					if (aacutin != 0)
					{
						sb.AppendFormat("対空: {0}\r\n", Constants.GetAACutinKind(aacutin));
					}
					double adjustedaa = Calculator.GetAdjustedAAValue(ship);
					sb.AppendFormat("加重対空: {0} (割合撃墜: {1:p2})\r\n",
						adjustedaa,
						Calculator.GetProportionalAirDefense(adjustedaa)
						);

					double rocket = Calculator.GetAARocketBarrageProbability(ship);
					if (rocket > 0)
						sb.AppendLine($"対空噴進弾幕: {rocket:p1}");
				}

				{
					int airsup_min;
					int airsup_max;
					if (Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1)
					{
						airsup_min = Calculator.GetAirSuperiority(ship, false);
						airsup_max = Calculator.GetAirSuperiority(ship, true);
					}
					else
					{
						airsup_min = airsup_max = Calculator.GetAirSuperiorityIgnoreLevel(ship);
					}

					int airbattle = ship.AirBattlePower;
					if (airsup_min > 0)
					{

						string airsup_str;
						if (Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange && airsup_min < airsup_max)
						{
							airsup_str = string.Format("{0} ～ {1}", airsup_min, airsup_max);
						}
						else
						{
							airsup_str = airsup_min.ToString();
						}

						if (airbattle > 0)
							sb.AppendFormat("制空戦力: {0} / 航空戦威力: {1}\r\n", airsup_str, airbattle);
						else
							sb.AppendFormat("制空戦力: {0}\r\n", airsup_str);
					}
					else if (airbattle > 0)
						sb.AppendFormat("航空戦威力: {0}\r\n", airbattle);
				}

				if(showSupportPower)
				{
					sb.AppendLine();
					sb.AppendFormat("砲撃支援威力: ");
					var supportShellingPower = CalcShipAttackPower.CalculateSupportShellingPower(ship);
					if (supportShellingPower == 0)
						sb.AppendFormat("攻撃不能\r\n");
					else
						sb.AppendFormat("{0}\r\n", supportShellingPower);
					sb.AppendFormat("航空支援威力: ");
					var supportAircraftPowers = ship.Slot.Select((_, i) => CalcShipAttackPower.CalculateSupportAirclaftPower(i, ship)).ToArray();
					foreach (var sair in supportAircraftPowers.Select((name, num) => new { name, num }))
					{
						if (sair.name == -1)
						{
							if(sair.num == 0)
							{
								sb.AppendFormat("攻撃不能");
							}
							break;
						}
						if (sair.num == 0)
						{
							sb.AppendFormat("{0}", sair.name);
						}
						else
						{
							sb.AppendFormat(" / {0}", sair.name);
						}
					}
					
					sb.AppendLine();
					sb.AppendFormat("対潜支援威力: ");
					var supportAntiSubmarinePower = ship.Slot.Select((_, i) => CalcShipAttackPower.CalculateSupportAntiSubmarinePower(i, ship)).ToArray();
					foreach (var sasw in supportAntiSubmarinePower.Select((name,num) => new { name, num }))
					{
						if (sasw.name == -1)
						{
							if (sasw.num == 0)
							{
								sb.AppendFormat("攻撃不能");
							}
							break;
						}
						if (sasw.num == 0)
						{
							sb.AppendFormat("{0}({1})", sasw.name * 1.2, sasw.name * 2.0);
						}
						else
						{
							sb.AppendFormat(" / {0}({1})", sasw.name * 1.2, sasw.name * 2.0);
						}
					}
					sb.AppendLine();
				}

				if (showSanmaEquip)
				{
					int sanma = ship.SanmaEquipCount;
					int sanmaB = ship.SanmaEquipCountBomb;
					sb.AppendFormat("\r\n秋刀魚漁有効装備: {0}  ※爆雷: {1}\r\n", sanma,sanmaB);
				}

				sb.AppendFormat("\r\n※攻撃威力は同航戦・制空権確保時の値");
				if(showAntiGroundPower)
					sb.AppendFormat("\r\n※対地攻撃の威力値は(砲台/離島/ソフトスキン/集積地/港湾夏姫/集積地Ⅲ)の並び");
				if(showSupportPower)
					sb.AppendFormat("\r\n※対潜支援威力の()内の値は変動倍率x2.0(発動率50%)の値\r\n　エリソ確定大破/撃沈にはそれぞれ威力73/84が必要");
				sb.AppendFormat("\r\n(右クリックで詳細ダイアログ起動)");
				return sb.ToString();
			}

			public void ConfigurationChanged(FormFleet parent)
			{
				Name.Font = parent.MainFont;
				Level.MainFont = parent.MainFont;
				Level.SubFont = parent.SubFont;
				HP.MainFont = parent.MainFont;
				HP.SubFont = parent.SubFont;
				Condition.Font = parent.MainFont;
				SetConditionDesign(Condition, (Condition.Tag as int?) ?? 49);
				Equipments.Font = parent.SubFont;
			}

			public void Dispose()
			{
				Name.Dispose();
				Level.Dispose();
				HP.Dispose();
				Condition.Dispose();
				ShipResource.Dispose();
				Equipments.Dispose();

			}
		}

		public static void SetConditionDesign(ImageLabel label, int cond)
		{

			if (label.ImageAlign == ContentAlignment.MiddleCenter)
			{
				// icon invisible
				label.ImageIndex = -1;

				label.BackColor =
					cond < 20 ? Color.LightCoral :
					cond < 30 ? Color.LightSalmon :
					cond < 40 ? Color.Moccasin :
					cond < 50 ? Color.Transparent :
					Color.LightGreen;
			}
			else
			{
				label.BackColor = Color.Transparent;

				label.ImageIndex =
					cond < 20 ? (int)ResourceManager.IconContent.ConditionVeryTired :
					cond < 30 ? (int)ResourceManager.IconContent.ConditionTired :
					cond < 40 ? (int)ResourceManager.IconContent.ConditionLittleTired :
					cond < 50 ? (int)ResourceManager.IconContent.ConditionNormal :
					(int)ResourceManager.IconContent.ConditionSparkle;

			}
		}

		public int FleetID { get; private set; }

		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }

		private TableFleetControl ControlFleet;
		private TableMemberControl[] ControlMember;

		private int AnchorageRepairBound;


		public FormFleet(FormMain parent, int fleetID)
		{
			InitializeComponent();

			FleetID = fleetID;
			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ConfigurationChanged();

			MainFontColor = Color.FromArgb(0x00, 0x00, 0x00);
			SubFontColor = Color.FromArgb(0x88, 0x88, 0x88);

			AnchorageRepairBound = 0;

			//ui init

			ControlHelper.SetDoubleBuffered(TableFleet);
			ControlHelper.SetDoubleBuffered(TableMember);

			TableFleet.Visible = false;
			TableFleet.SuspendLayout();
			TableFleet.BorderStyle = BorderStyle.FixedSingle;
			ControlFleet = new TableFleetControl(this, TableFleet);
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			ControlMember = new TableMemberControl[7];
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i] = new TableMemberControl(this, TableMember, i);
			}
			TableMember.ResumeLayout();

			ConfigurationChanged();     //fixme: 苦渋の決断

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet]);
		}

		private void FormFleet_Load(object sender, EventArgs e)
		{
			Text = string.Format("#{0}", FleetID);

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

			//追加するときは FormFleetOverview にも同様に追加してください

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		void Updated(string apiname, dynamic data)
		{
			if (IsRemodeling)
			{
				if (apiname == "api_get_member/slot_item")
					IsRemodeling = false;
				else
					return;
			}
			if (apiname == "api_req_kaisou/remodeling")
			{
				IsRemodeling = true;
				return;
			}

			KCDatabase db = KCDatabase.Instance;

			if (db.Ships.Count == 0) return;

			FleetData fleet = db.Fleet.Fleets[FleetID];
			if (fleet == null) return;

			TableFleet.SuspendLayout();
			ControlFleet.Update(fleet);
			TableFleet.Visible = true;
			TableFleet.ResumeLayout();
			TableFleet.Refresh();

			AnchorageRepairBound = fleet.CanAnchorageRepair ? 2 + fleet.MembersInstance[0].SlotInstance.Count(eq => eq != null && eq.MasterEquipment.CategoryType == EquipmentTypes.RepairFacility) : 0;

			TableMember.SuspendLayout();
			TableMember.RowCount = fleet.Members.Count(id => id > 0);
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i].Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
			}
			TableMember.ResumeLayout();
			TableMember.Refresh();


			if (Icon != null) ResourceManager.DestroyIcon(Icon);
			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[ControlFleet.State.GetIconIndex()]);
			if (Parent != null) Parent.Refresh();       //アイコンを更新するため
		}

		void UpdateTimerTick()
		{
			FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetID];

			TableFleet.SuspendLayout();
			{
				if (fleet != null)
					ControlFleet.Refresh();

			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i].HP.Refresh();
			}
			TableMember.ResumeLayout();

			// anchorage repairing
			if (fleet != null && Utility.Configuration.Config.FormFleet.ReflectAnchorageRepairHealing)
			{
				TimeSpan elapsed = DateTime.Now - KCDatabase.Instance.Fleet.AnchorageRepairingTimer;

				if (elapsed.TotalMinutes >= 20 && AnchorageRepairBound > 0)
				{

					for (int i = 0; i < AnchorageRepairBound; i++)
					{
						var hpbar = ControlMember[i].HP;

						double dockingSeconds = hpbar.Tag as double? ?? 0.0;

						if (dockingSeconds <= 0.0)
							continue;

						hpbar.SuspendUpdate();

						if (!hpbar.UsePrevValue)
						{
							hpbar.UsePrevValue = true;
							hpbar.ShowDifference = true;
						}

						int damage = hpbar.MaximumValue - hpbar.PrevValue;
						int healAmount = Math.Min(Calculator.CalculateAnchorageRepairHealAmount(damage, dockingSeconds, elapsed), damage);

						hpbar.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.MouseOver;
						hpbar.RepairTime = KCDatabase.Instance.Fleet.AnchorageRepairingTimer + Calculator.CalculateAnchorageRepairTime(damage, dockingSeconds, Math.Min(healAmount + 1, damage));
						hpbar.Value = hpbar.PrevValue + healAmount;

						hpbar.ResumeUpdate();
					}
				}
			}
		}

		//艦隊編成のコピー
		private void ContextMenuFleet_CopyFleet_Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[FleetID];
			if (fleet == null) return;

			sb.AppendFormat("{0}\t制空戦力{1} / 索敵能力 {2} / 輸送能力 {3}\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString(ControlFleet.BranchWeight), Calculator.GetTPDamage(fleet));
			for (int i = 0; i < fleet.Members.Count; i++)
			{
				if (fleet[i] == -1)
					continue;

				ShipData ship = db.Ships[fleet[i]];

				sb.AppendFormat("{0}/{1}\t", ship.MasterShip.Name, ship.Level);

				var eq = ship.AllSlotInstance;


				if (eq != null)
				{
					for (int j = 0; j < eq.Count; j++)
					{

						if (eq[j] == null) continue;

						int count = 1;
						for (int k = j + 1; k < eq.Count; k++)
						{
							if (eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level && eq[k].AircraftLevel == eq[j].AircraftLevel)
							{
								count++;
							}
							else
							{
								break;
							}
						}

						if (count == 1)
						{
							sb.AppendFormat("{0}{1}", j == 0 ? "" : ", ", eq[j].NameWithLevel);
						}
						else
						{
							sb.AppendFormat("{0}{1}x{2}", j == 0 ? "" : ", ", eq[j].NameWithLevel, count);
						}

						j += count - 1;
					}
				}

				sb.AppendLine();
			}

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}

		private void ContextMenuFleet_Opening(object sender, CancelEventArgs e)
		{
			ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;
		}

		private string CreateDeciBuilderData(int areaId, bool[] fleetExportFlags,bool withID)
		{
			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// 手書き json の悲しみ
			sb.Append(@"{""version"":4,");
			sb.Append(@"""hplv"":"+db.Admiral.Level+",");
			foreach (var fleet in db.Fleet.Fleets.Values)
			{
				if (fleet == null || fleet.MembersInstance.All(m => m == null)) continue;

				if (fleetExportFlags[fleet.FleetID-1])
				{
					sb.AppendFormat(@"""f{0}"":{{", fleet.FleetID);

					int shipcount = 1;
					foreach (var ship in fleet.MembersInstance)
					{
						if (ship == null) break;
						if (!withID)
						{ 
							sb.AppendFormat(@"""s{0}"":{{""id"":{1},""lv"":{2},""asw"":{3},""luck"":{4},""items"":{{",
								shipcount,
								ship.ShipID,
								ship.Level,
								ship.ASWBase,
								ship.LuckBase);
						}
						else
						{
							sb.AppendFormat(@"""s{0}"":{{""api_id"":{1},""id"":{2},""lv"":{3},""asw"":{4},""luck"":{5},""items"":{{",
								shipcount,
								ship.MasterID,
								ship.ShipID,
								ship.Level,
								ship.ASWBase,
								ship.LuckBase);
						}
						int eqcount = 1;
						foreach (var eq in ship.AllSlotInstance.Where(eq => eq != null))
						{
							if (eq == null) break;

							sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2},""mas"":{3}}},", eqcount >= 6 ? "x" : eqcount.ToString(), eq.EquipmentID, eq.Level, eq.AircraftLevel);

							eqcount++;
						}

						if (eqcount > 1)
							sb.Remove(sb.Length - 1, 1);        // remove ","
						sb.Append(@"}},");

						shipcount++;
					}

					if (shipcount > 0)
						sb.Remove(sb.Length - 1, 1);        // remove ","
					sb.Append(@"},");
				}
			}

			//基地航空隊
			//Note:mode(出撃/待機などの中隊ごとの状態)は、読み取る側の制御が揃っていないため対応しない。
			//　制空権シミュレータ：modeはitemの前にないといけない、modeがない場合は「待機」になる
			//　作戦室　　　　　　：modeはitemの後にないといけない、modeがない場合は「出撃」になる
			if (areaId != 0)
			{
				int corpsNumber = 1;
				int squadronNumber = 1;
				string corpsJson = "";
				string squadronJson = "";
				foreach (KeyValuePair<int, BaseAirCorpsData> corps in db.BaseAirCorps)
				{
					if (corps.Value.MapAreaID == areaId)
					{
						corpsJson += @"""a" + corpsNumber + @""":{";
						foreach (KeyValuePair<int,BaseAirCorpsSquadron> sq in corps.Value.Squadrons)
						{
							int emid = sq.Value.EquipmentMasterID;
							EquipmentData eq = db.Equipments[emid];
							if(eq != null)
							{
								squadronJson += @"""i"+squadronNumber+@""":{""id"":"+ sq.Value.EquipmentID+@",""rf"":"+eq.Level+@",""mas"":"+eq.AircraftLevel+@"},";
								//Console.WriteLine(sq.Value.SquadronID + "," + sq.Value.EquipmentID + "," + eq.Level + "," + eq.AircraftLevel);
							}
							squadronNumber++;
						}
						squadronJson = @"""items"":{" + squadronJson.Trim(',') + "}";

						corpsJson += squadronJson + "},";
						squadronJson = "";
						squadronNumber = 1;
						corpsNumber++;
					}
				}

				corpsJson = corpsJson.Trim(',');
				Console.WriteLine(corpsJson+ "}");
				if(corpsJson != "")
				{
					sb.Append(corpsJson+ "}");
				}
			}

			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append(@"}");

			Console.WriteLine(sb.ToString());
			return sb.ToString();
		}

		private bool[] GetFleetExportFlag(DialogChooseAirBase dca)
		{
			bool[] fleet = new bool[] { true, true, true, true };
			fleet[0] = dca.fleet1;
			fleet[1] = dca.fleet2;
			fleet[2] = dca.fleet3;
			fleet[3] = dca.fleet4;

			return fleet;
		}

		/// <summary>
		/// 「艦隊デッキビルダー」用編成コピー
		/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyFleetDeckBuilder_Click(object sender, EventArgs e)
		{
			int areaId = 0;
			bool[] fleet;
			DialogChooseAirBase dca = new DialogChooseAirBase();
			DialogResult dr = dca.ShowDialog();
			if (dr == DialogResult.OK)
			{
				areaId = dca.areaId;
				fleet = GetFleetExportFlag(dca);
				Clipboard.SetData(DataFormats.StringFormat, CreateDeciBuilderData(areaId, fleet, false));
			}
		}

		/// <summary>
		/// 「艦隊晒しページ」用編成コピー
		/// <see cref="http://kancolle-calc.net/kanmusu_list.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyKanmusuList_Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// version
			sb.Append(".2");

			// <たね艦娘(完全未改造時)のID, 艦娘リスト>　に分類
			Dictionary<int, List<ShipData>> shiplist = new Dictionary<int, List<ShipData>>();

			foreach (var ship in db.Ships.Values.Where(s => s.IsLocked))
			{
				var master = ship.MasterShip;
				while (master.RemodelBeforeShip != null)
				{
					master = master.RemodelBeforeShip;
				}

				if (!shiplist.ContainsKey(master.ShipID))
				{
					shiplist.Add(master.ShipID, new List<ShipData>() { ship });
				}
				else
				{
					shiplist[master.ShipID].Add(ship);
				}
			}

			//Note:宗谷に対する特別対応
			//艦隊晒しページ(http://kancolle-calc.net/kanmusu_list.html)の宗谷は主キーがID:645(灯台補給艦)であるとしてデータを読み込んでいる(IDが一番若いから？)
			//未改造時の艦IDを主キーとしている本ロジックとマッチしないため例外的に差し替えを行う
			if (shiplist.ContainsKey(699))	//699：特務艦
			{
				List<ShipData> tmpShipData = shiplist[699];

				shiplist.Remove(699);
				shiplist.Add(645, tmpShipData);
			}
			if (shiplist.ContainsKey(650))	//650：南極観測船
			{
				List<ShipData> tmpShipData = shiplist[650];

				shiplist.Remove(650);
				shiplist.Add(645, tmpShipData);
			}

			// 上で作った分類の各項を文字列化
			// Note:KancolleSnifferでの出力がID順なのでそちらに合わせるようにした
			foreach (var sl in shiplist.OrderBy(id => id.Key))
			{
				sb.Append("|").Append(sl.Key).Append(":");

				foreach (var ship in sl.Value.OrderByDescending(s => s.Level))
				{
					sb.Append(ship.Level);

					// 改造レベルに達しているのに未改造の艦は ".<たね=1, 改=2, 改二=3, ...>" を付加
					if (ship.MasterShip.RemodelAfterShipID != 0 && ship.ExpNextRemodel == 0)
					{
						sb.Append(".");
						int count = 1;
						var master = ship.MasterShip;
						while (master.RemodelBeforeShip != null)
						{
							master = master.RemodelBeforeShip;
							count++;
						}
						sb.Append(count);
					}
					sb.Append(",");
				}

				// 余った "," を削除
				sb.Remove(sb.Length - 1, 1);
			}

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}

		/// <summary>
		/// 「艦隊分析 -艦これ-」の艦隊情報反映用フォーマットでコピー
		/// メモ
		/// https://kancolle-fleetanalysis.firebaseapp.com/#/ はサービス終了閉鎖済み
		/// 制空権シミュレータ：https://noro6.github.io/kcTools/list/ で本フォーマットを使用できる
		/// </summary>
		private void ContextMenuFleet_CopyToFleetAnalysis_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();

			sb.Append("[");
			foreach (var ship in KCDatabase.Instance.Ships.Values.Where(s => s.IsLocked))
			{
				//現在の進捗計算
				int expProgress = 0;
				if (ExpTable.ShipExp.ContainsKey(ship.Level + 1) && ship.Level != 99)
				{
					double tmpExpProgress = ((double)ExpTable.ShipExp[ship.Level].Next - (double)ship.ExpNext) / (double)ExpTable.ShipExp[ship.Level].Next * 100;
					expProgress = (int)Math.Truncate(tmpExpProgress);
				}
				long[] apiExp = { ship.ExpTotal, (long)ship.ExpNext, (long)expProgress };
				sb.AppendFormat(@"{{""api_ship_id"":{0},""api_lv"":{1},""api_kyouka"":[{2}],""api_exp"":[{3}],""api_slot_ex"":{4},""api_sally_area"":{5}}},",
					ship.ShipID, ship.Level, 
					string.Join(",", (int[])ship.RawData.api_kyouka), 
					string.Join(",", apiExp), 
					ship.ExpansionSlot, 
					(ship.SallyArea >= 0 ? ship.SallyArea : 0));
			}
			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append("]");

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}

		/// <summary>
		/// 「艦隊分析 -艦これ-」の艦隊情報反映用フォーマットにapi_idを追加してコピー
		/// メモ
		/// https://kancolle-fleetanalysis.firebaseapp.com/#/ はサービス終了閉鎖済み
		/// 制空権シミュレータ：https://noro6.github.io/kcTools/list/ で本フォーマットを使用できる
		/// </summary>
		private void ContextMenuFleet_CopyToFleetAnalysisWithID_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();

			sb.Append("[");
			foreach (var ship in KCDatabase.Instance.Ships.Values.Where(s => s.IsLocked))
			{
				//現在の進捗計算
				int expProgress = 0;
				if (ExpTable.ShipExp.ContainsKey(ship.Level + 1) && ship.Level != 99)
				{
					double tmpExpProgress = ((double)ExpTable.ShipExp[ship.Level].Next - (double)ship.ExpNext) / (double)ExpTable.ShipExp[ship.Level].Next * 100;
					expProgress = (int)Math.Truncate(tmpExpProgress);
				}
				long[] apiExp = { ship.ExpTotal, (long)ship.ExpNext, (long)expProgress };
				sb.AppendFormat(@"{{""api_id"":{0},""api_ship_id"":{1},""api_lv"":{2},""api_kyouka"":[{3}],""api_exp"":[{4}],""api_slot_ex"":{5},""api_sally_area"":{6}",
					ship.MasterID,ship.ShipID, ship.Level,
					string.Join(",", (int[])ship.RawData.api_kyouka),
					string.Join(",", apiExp),
					ship.ExpansionSlot,
					(ship.SallyArea >= 0 ? ship.SallyArea : 0));
				if (ship.SpItemKind > 0)
				{
					switch (ship.SpItemKind)
					{
						case 1:
							sb.AppendFormat(@",""api_sp_effect_items"": [{{""api_kind"":{0},""api_raig"":{1},""api_souk"":{2}}}]}},", ship.SpItemKind, ship.SpItemRaig, ship.SpItemSouk);
							break;
						case 2:
							sb.AppendFormat(@",""api_sp_effect_items"": [{{""api_kind"":{0},""api_houg"":{1},""api_kaih"":{2}}}]}},", ship.SpItemKind, ship.SpItemHoug, ship.SpItemKaih);
							break;
					}
				}
				else
					sb.AppendFormat("}},");
			}
			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append("]");

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}

		/// <summary>
		/// 外部サイトをデッキビルダー形式指定で開く
		/// =>デッキビルダー形式のデータをクリップボードにコピーして外部サイトを開く
		/// </summary>
		/// <param name="baseUrl"></param>
		/// <param name="areaId"></param>
		/// <returns></returns>
		private Process OpenUrlWithDeciBuilderData(string baseUrl, int areaId, bool[] fleet)
		{
			//string data = CreateDeciBuilderData(areaId, fleet, false);
			Clipboard.SetData(DataFormats.StringFormat, CreateDeciBuilderData(areaId, fleet, false));
			//string url = $@"{baseUrl}?predeck={data.Replace("\"", "\\\"")}";
			string url = baseUrl;


			ProcessStartInfo pi = new ProcessStartInfo()
			{
				FileName = url,
				UseShellExecute = true,
			};
			return Process.Start(pi);
		}

		/// <summary>
		/// 現在の艦隊データで制空権シミュレータ(https://noro6.github.io/kc-web)を開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContextMenuFleet_OpenAirControlSimulator_Click(object sender, EventArgs e)
		{
			int areaId = 0;
			bool[] fleet;
			DialogChooseAirBase dca = new DialogChooseAirBase();
			DialogResult dr = dca.ShowDialog();
			if (dr == DialogResult.OK)
			{
				areaId = dca.areaId;
				fleet = GetFleetExportFlag(dca);
				OpenUrlWithDeciBuilderData("https://noro6.github.io/kc-web", areaId, fleet);
			}
		}

		/// <summary>
		/// 現在の艦隊データで作戦室(https://jervis.vercel.app/)を開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContextMenuFleet_OpenTacticalRoom_Click(object sender, EventArgs e)
		{
			int areaId = 0;
			bool[] fleet;
			DialogChooseAirBase dca = new DialogChooseAirBase();
			DialogResult dr = dca.ShowDialog();
			if(dr == DialogResult.OK)
			{
				areaId = dca.areaId;
				fleet = GetFleetExportFlag(dca);
				OpenUrlWithDeciBuilderData("https://jervis.vercel.app", areaId, fleet);
			}
		}

		/// <summary>
		/// 現在の艦隊データで羅針盤シミュレータ(https://x-20a.github.io/compass/)を開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContextMenuFleet_OpenCompassSimulator_Click(object sender, EventArgs e)
		{
			int areaId = 0;
			bool[] fleet;
			DialogChooseAirBase dca = new DialogChooseAirBase();
			DialogResult dr = dca.ShowDialog();
			if (dr == DialogResult.OK)
			{
				areaId = dca.areaId;
				fleet = GetFleetExportFlag(dca);
				OpenUrlWithDeciBuilderData("https://x-20a.github.io/compass/", areaId, fleet);
			}
		}

		/// <summary>
		/// 現在の艦隊データでデッキビルダー(http://kancolle-calc.net/deckbuilder.html)を開く
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContextMenuFleet_OpenDeckBuilder_Click(object sender, EventArgs e)
		{
			//全艦隊固定出力かつ基地のデータなし
			OpenUrlWithDeciBuilderData("http://kancolle-calc.net/deckbuilder.html", 0, new bool[] { true,true,true,true,} );
		}

		/// <summary>
		/// 「艦隊分析」装備情報反映用
		/// https://kancolle-fleetanalysis.firebaseapp.com/
		/// </summary>
		private void ContextMenuFleet_CopyToFleetAnalysisEquip_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(
				"svdata={\"api_result\":1,\"api_result_msg\":\"成功\",\"api_data\":[" + string.Join(",", KCDatabase.Instance.Equipments.Values.Where(eq => eq?.ID >= 0)
				.Select(eq => $"{{\"api_id\":{eq.ID},\"api_slotitem_id\":{eq.EquipmentID},\"api_level\":{eq.Level},\"api_locked\":{(eq.IsLocked ? 1 : 0)}}}")) + "]}");
		}
		
		private void ContextMenuFleet_AntiAirDetails_Click(object sender, EventArgs e)
		{
			var dialog = new DialogAntiAirDefense();

			if (KCDatabase.Instance.Fleet.CombinedFlag != 0 && (FleetID == 1 || FleetID == 2))
				dialog.SetFleetID(5);
			else
				dialog.SetFleetID(FleetID);

			dialog.Show(this);
		}

		private void ContextMenuFleet_Capture_Click(object sender, EventArgs e)
		{
			using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
			{
				this.DrawToBitmap(bitmap, this.ClientRectangle);

				Clipboard.SetData(DataFormats.Bitmap, bitmap);
			}
		}

		private void ContextMenuFleet_OutputFleetImage_Click(object sender, EventArgs e)
		{
			using (var dialog = new DialogFleetImageGenerator(FleetID))
			{
				dialog.ShowDialog(this);
			}
		}

		void ConfigurationChanged()
		{
			var c = Utility.Configuration.Config;

			MainFont = Font = c.UI.MainFont;
			SubFont = c.UI.SubFont;

			AutoScroll = c.FormFleet.IsScrollable;

			var fleet = KCDatabase.Instance.Fleet[FleetID];

			TableFleet.SuspendLayout();
			if (ControlFleet != null && fleet != null)
			{
				ControlFleet.ConfigurationChanged(this);
				ControlFleet.Update(fleet);
			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			if (ControlMember != null)
			{
				bool showAircraft = c.FormFleet.ShowAircraft;
				bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
				bool shortHPBar = c.FormFleet.ShortenHPBar;
				bool colorMorphing = c.UI.BarColorMorphing;
				Color[] colorScheme = c.UI.BarColorScheme.Select(col => col.ColorData).ToArray();
				bool showNext = c.FormFleet.ShowNextExp;
				bool showConditionIcon = c.FormFleet.ShowConditionIcon;
				var levelVisibility = c.FormFleet.EquipmentLevelVisibility;
				bool showAircraftLevelByNumber = c.FormFleet.ShowAircraftLevelByNumber;
				int fixedShipNameWidth = c.FormFleet.FixedShipNameWidth;
				bool isLayoutFixed = c.UI.IsLayoutFixed;

				for (int i = 0; i < ControlMember.Length; i++)
				{
					var member = ControlMember[i];

					member.Equipments.ShowAircraft = showAircraft;
					if (fixShipNameWidth)
					{
						member.Name.AutoSize = false;
						member.Name.Size = new Size(fixedShipNameWidth, 20);
					}
					else
					{
						member.Name.AutoSize = true;
					}

					member.HP.SuspendUpdate();
					member.HP.Text = shortHPBar ? "" : "HP:";
					member.HP.HPBar.ColorMorphing = colorMorphing;
					member.HP.HPBar.SetBarColorScheme(colorScheme);
					member.HP.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.HP.Margin.Vertical) : Size.Empty;
					member.HP.ResumeUpdate();
					member.Level.TextNext = showNext ? "next:" : null;
					member.Condition.ImageAlign = showConditionIcon ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
					member.Equipments.LevelVisibility = levelVisibility;
					member.Equipments.ShowAircraftLevelByNumber = showAircraftLevelByNumber;
					member.Equipments.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.Equipments.Margin.Vertical) : Size.Empty;
					member.ShipResource.BarFuel.ColorMorphing =
					member.ShipResource.BarAmmo.ColorMorphing = colorMorphing;
					member.ShipResource.BarFuel.SetBarColorScheme(colorScheme);
					member.ShipResource.BarAmmo.SetBarColorScheme(colorScheme);

					member.ConfigurationChanged(this);
					if (fleet != null)
						member.Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
				}
			}

			ControlHelper.SetTableRowStyles(TableMember, ControlHelper.GetDefaultRowStyle());
			TableMember.ResumeLayout();

			TableMember.Location = new Point(TableMember.Location.X, TableFleet.Bottom /*+ Math.Max( TableFleet.Margin.Bottom, TableMember.Margin.Top )*/ );

			TableMember.PerformLayout();        //fixme:サイズ変更に親パネルが追随しない
		}

		private void TableMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		protected override string GetPersistString()
		{
			return "Fleet #" + FleetID.ToString();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			ControlFleet.Dispose();
			for (int i = 0; i < ControlMember.Length; i++)
				ControlMember[i].Dispose();

			// --- auto generated ---
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
