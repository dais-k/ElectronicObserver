using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogEquipmentList : Form
	{


		private DataGridViewCellStyle CSDefaultLeft, CSDefaultRight,
			CSUnselectableLeft, CSUnselectableRight;


		public DialogEquipmentList()
		{

			InitializeComponent();

			ControlHelper.SetDoubleBuffered(EquipmentView);

			Font = Utility.Configuration.Config.UI.MainFont;

			foreach (DataGridViewColumn column in EquipmentView.Columns)
			{
				column.MinimumWidth = 2;
			}



			#region CellStyle

			CSDefaultLeft = new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft,
				BackColor = SystemColors.Control,
				Font = Font,
				ForeColor = SystemColors.ControlText,
				SelectionBackColor = Color.FromArgb(0xFF, 0xFF, 0xCC),
				SelectionForeColor = SystemColors.ControlText,
				WrapMode = DataGridViewTriState.False
			};

			CSDefaultRight = new DataGridViewCellStyle(CSDefaultLeft)
			{
				Alignment = DataGridViewContentAlignment.MiddleRight
			};

			CSUnselectableLeft = new DataGridViewCellStyle(CSDefaultLeft);
			CSUnselectableLeft.SelectionForeColor = CSUnselectableLeft.ForeColor;
			CSUnselectableLeft.SelectionBackColor = CSUnselectableLeft.BackColor;

			CSUnselectableRight = new DataGridViewCellStyle(CSDefaultRight);
			CSUnselectableRight.SelectionForeColor = CSUnselectableRight.ForeColor;
			CSUnselectableRight.SelectionBackColor = CSUnselectableRight.BackColor;


			EquipmentView.DefaultCellStyle = CSDefaultRight;
			EquipmentView_Name.DefaultCellStyle = CSDefaultLeft;
			EquipmentView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

			DetailView.DefaultCellStyle = CSUnselectableRight;
			DetailView_EquippedShip.DefaultCellStyle = CSUnselectableLeft;
			DetailView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

			#endregion

		}

		private void DialogEquipmentList_Load(object sender, EventArgs e)
		{

			UpdateView();

			this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList]);

		}


		private void EquipmentView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{

			if (e.Column.Index == EquipmentView_Name.Index)
			{

				int id1 = (int)EquipmentView[EquipmentView_ID.Index, e.RowIndex1].Value;
				int id2 = (int)EquipmentView[EquipmentView_ID.Index, e.RowIndex2].Value;

				e.SortResult =
					KCDatabase.Instance.MasterEquipments[id1].CategoryType -
					KCDatabase.Instance.MasterEquipments[id2].CategoryType;

				if (e.SortResult == 0)
				{
					e.SortResult = id1 - id2;
				}

			}
			else
			{

				e.SortResult = ((IComparable)e.CellValue1).CompareTo(e.CellValue2);

			}


			if (e.SortResult == 0)
			{
				e.SortResult = (EquipmentView.Rows[e.RowIndex1].Tag as int? ?? 0) - (EquipmentView.Rows[e.RowIndex2].Tag as int? ?? 0);
			}

			e.Handled = true;

		}

		private void EquipmentView_Sorted(object sender, EventArgs e)
		{

			for (int i = 0; i < EquipmentView.Rows.Count; i++)
				EquipmentView.Rows[i].Tag = i;

		}

		private void EquipmentView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			if (e.ColumnIndex == EquipmentView_Icon.Index)
			{
				e.Value = ResourceManager.GetEquipmentImage((int)e.Value);
				e.FormattingApplied = true;
			}

		}

		private void EquipmentView_SelectionChanged(object sender, EventArgs e)
		{

			if (EquipmentView.Enabled && EquipmentView.SelectedRows != null && EquipmentView.SelectedRows.Count > 0)
				UpdateDetailView((int)EquipmentView[EquipmentView_ID.Index, EquipmentView.SelectedRows[0].Index].Value);

		}


		private void DetailView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			if (e.ColumnIndex != DetailView_EquippedShip.Index)
			{
				if (AreEqual(e.RowIndex, e.RowIndex - 1))
				{

					e.Value = "";
					e.FormattingApplied = true;
					return;
				}
			}

			if (e.ColumnIndex == DetailView_Level.Index)
			{

				e.Value = "+" + e.Value;
				e.FormattingApplied = true;

			}

		}

		private void DetailView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{

			e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

			if (AreEqual(e.RowIndex, e.RowIndex + 1))
			{
				e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
			}
			else
			{
				e.AdvancedBorderStyle.Bottom = DetailView.AdvancedCellBorderStyle.Bottom;
			}

		}

		/// <summary>
		/// DetailView 内の行が同じレベルであるかを判定します。
		/// </summary>
		private bool AreEqual(int rowIndex1, int rowIndex2)
		{

			if (rowIndex1 < 0 ||
				rowIndex1 >= DetailView.Rows.Count ||
				rowIndex2 < 0 ||
				rowIndex2 >= DetailView.Rows.Count)
				return false;

			return ((IComparable)DetailView[DetailView_Level.Index, rowIndex1].Value)
				.CompareTo(DetailView[DetailView_Level.Index, rowIndex2].Value) == 0 &&
				((IComparable)DetailView[DetailView_AircraftLevel.Index, rowIndex1].Value)
				.CompareTo(DetailView[DetailView_AircraftLevel.Index, rowIndex2].Value) == 0;
		}


		/// <summary>
		/// 一覧ビューを更新します。
		/// </summary>
		private void UpdateView()
		{

			var ships = KCDatabase.Instance.Ships.Values;
			var equipments = KCDatabase.Instance.Equipments.Values;
			var masterEquipments = KCDatabase.Instance.MasterEquipments;
			int masterCount = masterEquipments.Values.Count(eq => !eq.IsAbyssalEquipment);

			var allCount = equipments.GroupBy(eq => eq.EquipmentID).ToDictionary(group => group.Key, group => group.Count());
			var unlockedCount = equipments.Where(eq => !eq.IsLocked).GroupBy(eq => eq.EquipmentID).ToDictionary(group => group.Key, group => group.Count());
			var remainCount = new Dictionary<int, int>(allCount);


			//剰余数計算
			foreach (var eq in ships
				.SelectMany(s => s.AllSlotInstanceMaster)
				.Where(eq => eq != null))
			{

				remainCount[eq.EquipmentID]--;
			}

			foreach (var eq in KCDatabase.Instance.BaseAirCorps.Values
				.SelectMany(corps => corps.Squadrons.Values.Select(sq => sq.EquipmentInstance))
				.Where(eq => eq != null))
			{

				remainCount[eq.EquipmentID]--;
			}

			foreach (var eq in KCDatabase.Instance.RelocatedEquipments.Values
				.Where(eq => eq.EquipmentInstance != null))
			{

				remainCount[eq.EquipmentInstance.EquipmentID]--;
			}



			//表示処理
			EquipmentView.SuspendLayout();

			EquipmentView.Enabled = false;
			EquipmentView.Rows.Clear();


			var rows = new List<DataGridViewRow>(allCount.Count);
			var ids = allCount.Keys;

			foreach (int id in ids)
			{
				//表示しようとしているデータにフィルタでチェックが付いているかを確認して、
				//ついていなければ次の行へ
				//---- 主砲 ----
				if (masterEquipments[id].CategoryTypeInstance.Name == "小口径主砲" && !GunSmall_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "中口径主砲" && !GunMidium_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "大口径主砲" && !GunLarge_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "副砲" && !Secondary_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//---- 艦載機 ----
				if (masterEquipments[id].CategoryTypeInstance.Name == "艦上戦闘機" && !Fighter_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "艦上爆撃機" && !Bomber_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "艦上攻撃機" && !Attacker_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "艦上偵察機" && !Recon_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "噴式戦闘爆撃機" && !Jet_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "オートジャイロ" && !AutoGyro_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "対潜哨戒機" && !MPA_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//水上機
				if (masterEquipments[id].CategoryTypeInstance.Name == "水上偵察機" && !PlaneRecon_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "水上爆撃機" && !PlaneBomber_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "水上戦闘機" && !PlaneFighter_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "大型飛行艇" && !FlyingBoat_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//魚雷
				if (masterEquipments[id].CategoryTypeInstance.Name == "魚雷" && !Torpedo_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "潜水艦魚雷" && !SubmarineTorpedo_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "特殊潜航艇" && !MidgetSubmarine_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//電探
				if (masterEquipments[id].CategoryTypeInstance.Name == "小型電探" && !RadarSmall_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "大型電探" && !RadarLarge_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//ソナー/爆雷
				if (masterEquipments[id].CategoryTypeInstance.Name == "ソナー" && !SonarNormal_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "大型ソナー" && !SonarLarge_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "爆雷" && !DepthCharge_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//機銃/高射装置
				if (masterEquipments[id].CategoryTypeInstance.Name == "対空機銃" && !AAGun_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "高射装置" && !AADirector_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//大発系/ドラム缶
				if (masterEquipments[id].CategoryTypeInstance.Name == "簡易輸送部材" && !Drum_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "上陸用舟艇" && !LandingCraft_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "特型内火艇" && !AmphibiousVehicle_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//砲弾
				if (masterEquipments[id].CategoryTypeInstance.Name == "対艦強化弾" && !APShell_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "対空強化弾" && !AAShell_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//バルジ
				if (masterEquipments[id].CategoryTypeInstance.Name == "追加装甲(中型)" && !BulgeMid_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "追加装甲(大型)" && !BulgeLarge_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//照明弾/探照灯
				if (masterEquipments[id].CategoryTypeInstance.Name == "照明弾" && !Flare_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "探照灯" && !Searchlight_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "大型探照灯" && !SearchlightLarge_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//その他
				if (masterEquipments[id].CategoryTypeInstance.Name == "機関部強化" && !Engine_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "対地装備" && !Rocket_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "水上艦要員" && !PicketCrew_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "航空要員" && !MaintenanceTeam_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "潜水艦装備" && !SubmarineEquipment_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "応急修理要員" && !DamageControl_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "司令部施設" && !CommandFacility_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "戦闘糧食" && !Ration_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "艦艇修理施設" && !RepairFacility_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "補給物資" && !Supplies_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "輸送機材" && !TransportMaterials_ToolStripMenuItem.Checked)
				{
					continue;
				}
				//陸上機
				if (masterEquipments[id].CategoryTypeInstance.Name == "陸上攻撃機" && !LandAttacker_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "大型陸上機" && !HeavyBomber_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "局地戦闘機" && !Interceptor_ToolStripMenuItem.Checked)
				{
					continue;
				}
				if (masterEquipments[id].CategoryTypeInstance.Name == "陸上偵察機" && !LandPatrol_ToolStripMenuItem.Checked)
				{
					continue;
				}

				//Console.WriteLine(id + "," + masterEquipments[id].IconType +","+ masterEquipments[id].CategoryTypeInstance.TypeID);

				var row = new DataGridViewRow();
				row.CreateCells(EquipmentView);
				row.SetValues(
					id,
					masterEquipments[id].IconType,
					masterEquipments[id].Name,
					allCount[id],
					remainCount[id],
					unlockedCount.ContainsKey(id) ? unlockedCount[id] : 0
					);

				{
					StringBuilder sb = new StringBuilder();
					var eq = masterEquipments[id];

					sb.AppendFormat("{0} {1}\r\n", eq.CategoryTypeInstance.Name, eq.Name, eq.EquipmentID);
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

					row.Cells[2].ToolTipText = sb.ToString();
				}
				rows.Add(row);
			}

			for (int i = 0; i < rows.Count; i++)
				rows[i].Tag = i;

			EquipmentView.Rows.AddRange(rows.ToArray());

			EquipmentView.Sort(EquipmentView_Name, ListSortDirection.Ascending);


			EquipmentView.Enabled = true;
			EquipmentView.ResumeLayout();

			if (EquipmentView.Rows.Count > 0)
				EquipmentView.CurrentCell = EquipmentView[0, 0];

		}

		private void EquipmentView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (0 <= e.RowIndex && e.RowIndex < EquipmentView.RowCount)
			{
				int equipmentID = (int)EquipmentView.Rows[e.RowIndex].Cells[0].Value;

				if ((e.Button & System.Windows.Forms.MouseButtons.Right) != 0)
				{
					Cursor = Cursors.AppStarting;
					new DialogAlbumMasterEquipment(equipmentID).Show(Owner);
					Cursor = Cursors.Default;
				}
			}
		}


		private class DetailCounter : IIdentifiable
		{

			public int level;
			public int aircraftLevel;
			public int countAll;
			public int countRemain;
			public int countRemainPrev;

			public List<string> equippedShips;

			public DetailCounter(int lv, int aircraftLv)
			{
				level = lv;
				aircraftLevel = aircraftLv;
				countAll = 0;
				countRemainPrev = 0;
				countRemain = 0;
				equippedShips = new List<string>();
			}

			public static int CalculateID(int level, int aircraftLevel)
			{
				return level + aircraftLevel * 100;
			}

			public static int CalculateID(EquipmentData eq)
			{
				return CalculateID(eq.Level, eq.AircraftLevel);
			}

			public int ID => CalculateID(level, aircraftLevel);
		}


		/// <summary>
		/// 詳細ビューを更新します。
		/// </summary>
		private void UpdateDetailView(int equipmentID)
		{

			DetailView.SuspendLayout();

			DetailView.Rows.Clear();

			//装備数カウント
			var eqs = KCDatabase.Instance.Equipments.Values.Where(eq => eq.EquipmentID == equipmentID);
			var countlist = new IDDictionary<DetailCounter>();

			foreach (var eq in eqs)
			{
				var c = countlist[DetailCounter.CalculateID(eq)];
				if (c == null)
				{
					countlist.Add(new DetailCounter(eq.Level, eq.AircraftLevel));
					c = countlist[DetailCounter.CalculateID(eq)];
				}
				c.countAll++;
				c.countRemain++;
				c.countRemainPrev++;
			}

			//装備艦集計
			foreach (var ship in KCDatabase.Instance.Ships.Values)
			{

				foreach (var eq in ship.AllSlotInstance.Where(s => s != null && s.EquipmentID == equipmentID))
				{

					countlist[DetailCounter.CalculateID(eq)].countRemain--;
				}

				foreach (var c in countlist.Values)
				{
					if (c.countRemain != c.countRemainPrev)
					{

						int diff = c.countRemainPrev - c.countRemain;

						c.equippedShips.Add(ship.NameWithLevel + (diff > 1 ? (" x" + diff) : ""));

						c.countRemainPrev = c.countRemain;
					}
				}

			}

			// 基地航空隊 - 配備中の装備を集計
			foreach (var corps in KCDatabase.Instance.BaseAirCorps.Values)
			{

				foreach (var sq in corps.Squadrons.Values.Where(sq => sq != null && sq.EquipmentID == equipmentID))
				{

					countlist[DetailCounter.CalculateID(sq.EquipmentInstance)].countRemain--;
				}

				foreach (var c in countlist.Values)
				{
					if (c.countRemain != c.countRemainPrev)
					{
						int diff = c.countRemainPrev - c.countRemain;

						c.equippedShips.Add(string.Format("#{0} {1}{2}", corps.MapAreaID, corps.Name, diff > 1 ? (" x" + diff) : ""));

						c.countRemainPrev = c.countRemain;
					}
				}

			}

			// 基地航空隊 - 配置転換中の装備を集計
			foreach (var eq in KCDatabase.Instance.RelocatedEquipments.Values
				.Select(v => v.EquipmentInstance)
				.Where(eq => eq != null && eq.EquipmentID == equipmentID))
			{

				countlist[DetailCounter.CalculateID(eq)].countRemain--;
			}

			foreach (var c in countlist.Values)
			{
				if (c.countRemain != c.countRemainPrev)
				{

					int diff = c.countRemainPrev - c.countRemain;

					c.equippedShips.Add("配置転換中" + (diff > 1 ? (" x" + diff) : ""));

					c.countRemainPrev = c.countRemain;
				}
			}


			//行に反映
			var rows = new List<DataGridViewRow>(eqs.Count());

			foreach (var c in countlist.Values)
			{

				if (c.equippedShips.Count() == 0)
				{
					c.equippedShips.Add("");
				}

				foreach (var s in c.equippedShips)
				{

					var row = new DataGridViewRow();
					row.CreateCells(DetailView);
					row.SetValues(c.level, c.aircraftLevel, c.countAll, c.countRemain, s);
					rows.Add(row);
				}

			}

			DetailView.Rows.AddRange(rows.ToArray());
			DetailView.Sort(DetailView_AircraftLevel, ListSortDirection.Ascending);
			DetailView.Sort(DetailView_Level, ListSortDirection.Ascending);

			DetailView.ResumeLayout();

			Text = "装備一覧 - " + KCDatabase.Instance.MasterEquipments[equipmentID].Name;
		}


		private void DetailView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{

			e.SortResult = ((IComparable)e.CellValue1).CompareTo(e.CellValue2);

			if (e.SortResult == 0)
			{
				e.SortResult = (DetailView.Rows[e.RowIndex1].Tag as int? ?? 0) - (DetailView.Rows[e.RowIndex2].Tag as int? ?? 0);

				if (DetailView.SortOrder == SortOrder.Descending)
					e.SortResult = -e.SortResult;
			}

			e.Handled = true;
		}

		private void DetailView_Sorted(object sender, EventArgs e)
		{

			for (int i = 0; i < DetailView.Rows.Count; i++)
				DetailView.Rows[i].Tag = i;

		}



		private void Menu_File_CSVOutput_Click(object sender, EventArgs e)
		{

			if (SaveCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				try
				{

					using (StreamWriter sw = new StreamWriter(SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding))
					{

						sw.WriteLine("固有ID,装備ID,装備名,改修Lv,艦載機Lv,ロック,装備艦ID,装備艦");
						string arg = string.Format("{{{0}}}", string.Join("},{", Enumerable.Range(0, 8)));

						foreach (var eq in KCDatabase.Instance.Equipments.Values)
						{

							if (eq.Name == "なし") continue;

							ShipData equippedShip = KCDatabase.Instance.Ships.Values.FirstOrDefault(s => s.AllSlot.Contains(eq.MasterID));


							sw.WriteLine(arg,
								eq.MasterID,
								eq.EquipmentID,
								eq.Name,
								eq.Level,
								eq.AircraftLevel,
								eq.IsLocked ? 1 : 0,
								equippedShip?.MasterID ?? -1,
								equippedShip?.NameWithLevel ?? ""
								);

						}

					}

				}
				catch (Exception ex)
				{

					Utility.ErrorReporter.SendErrorReport(ex, "装備一覧 CSVの出力に失敗しました。");
					MessageBox.Show("装備一覧 CSVの出力に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				}

			}


		}

		//フィルタ共通処理
		private void FilterFinish()
		{
			UpdateView();
			DetailView.SuspendLayout();
			DetailView.Rows.Clear();
			DetailView.ResumeLayout();
			EquipmentView.ClearSelection();
		}

		/// フィルタ用クリックイベント群
		//すべてチェック
		private void AllOn_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GunSmall_ToolStripMenuItem.Checked = true;
			GunSmall_ToolStripMenuItem.CheckState = CheckState.Checked;

			GunMidium_ToolStripMenuItem.Checked = true;
			GunMidium_ToolStripMenuItem.CheckState = CheckState.Checked;

			GunLarge_ToolStripMenuItem.Checked = true;
			GunLarge_ToolStripMenuItem.CheckState = CheckState.Checked;

			Secondary_ToolStripMenuItem.Checked = true;
			Secondary_ToolStripMenuItem.CheckState = CheckState.Checked;

			Fighter_ToolStripMenuItem.Checked = true;
			Fighter_ToolStripMenuItem.CheckState = CheckState.Checked;

			Bomber_ToolStripMenuItem.Checked = true;
			Bomber_ToolStripMenuItem.CheckState = CheckState.Checked;

			Attacker_ToolStripMenuItem.Checked = true;
			Attacker_ToolStripMenuItem.CheckState = CheckState.Checked;

			Recon_ToolStripMenuItem.Checked = true;
			Recon_ToolStripMenuItem.CheckState = CheckState.Checked;

			Jet_ToolStripMenuItem.Checked = true;
			Jet_ToolStripMenuItem.CheckState = CheckState.Checked;

			AutoGyro_ToolStripMenuItem.Checked = true;
			AutoGyro_ToolStripMenuItem.CheckState = CheckState.Checked;

			MPA_ToolStripMenuItem.Checked = true;
			MPA_ToolStripMenuItem.CheckState = CheckState.Checked;

			PlaneRecon_ToolStripMenuItem.Checked = true;
			PlaneRecon_ToolStripMenuItem.CheckState = CheckState.Checked;

			PlaneBomber_ToolStripMenuItem.Checked = true;
			PlaneBomber_ToolStripMenuItem.CheckState = CheckState.Checked;

			PlaneFighter_ToolStripMenuItem.Checked = true;
			PlaneFighter_ToolStripMenuItem.CheckState = CheckState.Checked;

			FlyingBoat_ToolStripMenuItem.Checked = true;
			FlyingBoat_ToolStripMenuItem.CheckState = CheckState.Checked;

			Torpedo_ToolStripMenuItem.Checked = true;
			Torpedo_ToolStripMenuItem.CheckState = CheckState.Checked;

			SubmarineTorpedo_ToolStripMenuItem.Checked = true;
			SubmarineTorpedo_ToolStripMenuItem.CheckState = CheckState.Checked;

			MidgetSubmarine_ToolStripMenuItem.Checked = true;
			MidgetSubmarine_ToolStripMenuItem.CheckState = CheckState.Checked;

			RadarSmall_ToolStripMenuItem.Checked = true;
			RadarSmall_ToolStripMenuItem.CheckState = CheckState.Checked;

			RadarLarge_ToolStripMenuItem.Checked = true;
			RadarLarge_ToolStripMenuItem.CheckState = CheckState.Checked;

			SonarNormal_ToolStripMenuItem.Checked = true;
			SonarNormal_ToolStripMenuItem.CheckState = CheckState.Checked;

			SonarLarge_ToolStripMenuItem.Checked = true;
			SonarLarge_ToolStripMenuItem.CheckState = CheckState.Checked;

			DepthCharge_ToolStripMenuItem.Checked = true;
			DepthCharge_ToolStripMenuItem.CheckState = CheckState.Checked;

			AAGun_ToolStripMenuItem.Checked = true;
			AAGun_ToolStripMenuItem.CheckState = CheckState.Checked;

			AADirector_ToolStripMenuItem.Checked = true;
			AADirector_ToolStripMenuItem.CheckState = CheckState.Checked;

			Drum_ToolStripMenuItem.Checked = true;
			Drum_ToolStripMenuItem.CheckState = CheckState.Checked;

			LandingCraft_ToolStripMenuItem.Checked = true;
			LandingCraft_ToolStripMenuItem.CheckState = CheckState.Checked;

			AmphibiousVehicle_ToolStripMenuItem.Checked = true;
			AmphibiousVehicle_ToolStripMenuItem.CheckState = CheckState.Checked;

			APShell_ToolStripMenuItem.Checked = true;
			APShell_ToolStripMenuItem.CheckState = CheckState.Checked;

			AAShell_ToolStripMenuItem.Checked = true;
			AAShell_ToolStripMenuItem.CheckState = CheckState.Checked;

			BulgeMid_ToolStripMenuItem.Checked = true;
			BulgeMid_ToolStripMenuItem.CheckState = CheckState.Checked;

			BulgeLarge_ToolStripMenuItem.Checked = true;
			BulgeLarge_ToolStripMenuItem.CheckState = CheckState.Checked;

			Flare_ToolStripMenuItem.Checked = true;
			Flare_ToolStripMenuItem.CheckState = CheckState.Checked;

			Searchlight_ToolStripMenuItem.Checked = true;
			Searchlight_ToolStripMenuItem.CheckState = CheckState.Checked;

			SearchlightLarge_ToolStripMenuItem.Checked = true;
			SearchlightLarge_ToolStripMenuItem.CheckState = CheckState.Checked;

			Engine_ToolStripMenuItem.Checked = true;
			Engine_ToolStripMenuItem.CheckState = CheckState.Checked;

			Rocket_ToolStripMenuItem.Checked = true;
			Rocket_ToolStripMenuItem.CheckState = CheckState.Checked;

			PicketCrew_ToolStripMenuItem.Checked = true;
			PicketCrew_ToolStripMenuItem.CheckState = CheckState.Checked;

			MaintenanceTeam_ToolStripMenuItem.Checked = true;
			MaintenanceTeam_ToolStripMenuItem.CheckState = CheckState.Checked;

			SubmarineEquipment_ToolStripMenuItem.Checked = true;
			SubmarineEquipment_ToolStripMenuItem.CheckState = CheckState.Checked;

			DamageControl_ToolStripMenuItem.Checked = true;
			DamageControl_ToolStripMenuItem.CheckState = CheckState.Checked;

			CommandFacility_ToolStripMenuItem.Checked = true;
			CommandFacility_ToolStripMenuItem.CheckState = CheckState.Checked;

			Ration_ToolStripMenuItem.Checked = true;
			Ration_ToolStripMenuItem.CheckState = CheckState.Checked;

			RepairFacility_ToolStripMenuItem.Checked = true;
			RepairFacility_ToolStripMenuItem.CheckState = CheckState.Checked;

			Supplies_ToolStripMenuItem.Checked = true;
			Supplies_ToolStripMenuItem.CheckState = CheckState.Checked;

			TransportMaterials_ToolStripMenuItem.Checked = true;
			TransportMaterials_ToolStripMenuItem.CheckState = CheckState.Checked;

			LandAttacker_ToolStripMenuItem.Checked = true;
			LandAttacker_ToolStripMenuItem.CheckState = CheckState.Checked;

			HeavyBomber_ToolStripMenuItem.Checked = true;
			HeavyBomber_ToolStripMenuItem.CheckState = CheckState.Checked;

			Interceptor_ToolStripMenuItem.Checked = true;
			Interceptor_ToolStripMenuItem.CheckState = CheckState.Checked;

			LandPatrol_ToolStripMenuItem.Checked = true;
			LandPatrol_ToolStripMenuItem.CheckState = CheckState.Checked;

			FilterFinish();
		}
		//すべてオフ
		private void AllOff_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GunSmall_ToolStripMenuItem.Checked = false;
			GunSmall_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			GunMidium_ToolStripMenuItem.Checked = false;
			GunMidium_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			GunLarge_ToolStripMenuItem.Checked = false;
			GunLarge_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Secondary_ToolStripMenuItem.Checked = false;
			Secondary_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Fighter_ToolStripMenuItem.Checked = false;
			Fighter_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Bomber_ToolStripMenuItem.Checked = false;
			Bomber_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Attacker_ToolStripMenuItem.Checked = false;
			Attacker_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Recon_ToolStripMenuItem.Checked = false;
			Recon_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Jet_ToolStripMenuItem.Checked = false;
			Jet_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			AutoGyro_ToolStripMenuItem.Checked = false;
			AutoGyro_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			MPA_ToolStripMenuItem.Checked = false;
			MPA_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			PlaneRecon_ToolStripMenuItem.Checked = false;
			PlaneRecon_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			PlaneBomber_ToolStripMenuItem.Checked = false;
			PlaneBomber_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			PlaneFighter_ToolStripMenuItem.Checked = false;
			PlaneFighter_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			FlyingBoat_ToolStripMenuItem.Checked = false;
			FlyingBoat_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Torpedo_ToolStripMenuItem.Checked = false;
			Torpedo_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			SubmarineTorpedo_ToolStripMenuItem.Checked = false;
			SubmarineTorpedo_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			MidgetSubmarine_ToolStripMenuItem.Checked = false;
			MidgetSubmarine_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			RadarSmall_ToolStripMenuItem.Checked = false;
			RadarSmall_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			RadarLarge_ToolStripMenuItem.Checked = false;
			RadarLarge_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			SonarNormal_ToolStripMenuItem.Checked = false;
			SonarNormal_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			SonarLarge_ToolStripMenuItem.Checked = false;
			SonarLarge_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			DepthCharge_ToolStripMenuItem.Checked = false;
			DepthCharge_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			AAGun_ToolStripMenuItem.Checked = false;
			AAGun_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			AADirector_ToolStripMenuItem.Checked = false;
			AADirector_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Drum_ToolStripMenuItem.Checked = false;
			Drum_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			LandingCraft_ToolStripMenuItem.Checked = false;
			LandingCraft_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			AmphibiousVehicle_ToolStripMenuItem.Checked = false;
			AmphibiousVehicle_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			APShell_ToolStripMenuItem.Checked = false;
			APShell_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			AAShell_ToolStripMenuItem.Checked = false;
			AAShell_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			BulgeMid_ToolStripMenuItem.Checked = false;
			BulgeMid_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			BulgeLarge_ToolStripMenuItem.Checked = false;
			BulgeLarge_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Flare_ToolStripMenuItem.Checked = false;
			Flare_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Searchlight_ToolStripMenuItem.Checked = false;
			Searchlight_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			SearchlightLarge_ToolStripMenuItem.Checked = false;
			SearchlightLarge_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Engine_ToolStripMenuItem.Checked = false;
			Engine_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Rocket_ToolStripMenuItem.Checked = false;
			Rocket_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			PicketCrew_ToolStripMenuItem.Checked = false;
			PicketCrew_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			MaintenanceTeam_ToolStripMenuItem.Checked = false;
			MaintenanceTeam_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			SubmarineEquipment_ToolStripMenuItem.Checked = false;
			SubmarineEquipment_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			DamageControl_ToolStripMenuItem.Checked = false;
			DamageControl_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			CommandFacility_ToolStripMenuItem.Checked = false;
			CommandFacility_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Ration_ToolStripMenuItem.Checked = false;
			Ration_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			RepairFacility_ToolStripMenuItem.Checked = false;
			RepairFacility_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Supplies_ToolStripMenuItem.Checked = false;
			Supplies_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			TransportMaterials_ToolStripMenuItem.Checked = false;
			TransportMaterials_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			LandAttacker_ToolStripMenuItem.Checked = false;
			LandAttacker_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			HeavyBomber_ToolStripMenuItem.Checked = false;
			HeavyBomber_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			Interceptor_ToolStripMenuItem.Checked = false;
			Interceptor_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			LandPatrol_ToolStripMenuItem.Checked = false;
			LandPatrol_ToolStripMenuItem.CheckState = CheckState.Unchecked;

			FilterFinish();
		}
		//艦載砲
		private void GunSmall_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void GunMidium_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void GunLarge_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Secondary_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//艦載機
		private void Fighter_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Bomber_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Attacker_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Recon_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Jet_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void AutoGyro_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void MPA_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//水上機
		private void PlaneRecon_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void PlaneBomber_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void PlaneFighter_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void FlyingBoat_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//魚雷
		private void Torpedo_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void SubmarineTorpedo_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void MidgetSubmarine_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//電探
		private void RadarSmall_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void RadarLarge_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//ソナー/爆雷
		private void SonarNormal_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void SonarLarge_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void DepthCharge_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//高射装置/対空機銃
		private void AAGun_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void AADirector_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//大発系/ドラム缶
		private void Drum_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void LandingCraft_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void AmphibiousVehicle_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//砲弾
		private void APShell_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void AAShell_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//バルジ
		private void BulgeMid_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void BulgeLarge_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//探照灯/照明弾
		private void Flare_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Searchlight_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void SearchlightLarge_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//その他
		private void Engine_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Rocket_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void PicketCrew_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void MaintenanceTeam_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void SubmarineEquipment_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void DamageControl_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void CommandFacility_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Ration_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void RepairFacility_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Supplies_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void TransportMaterials_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		//陸上機
		private void LandAttacker_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void HeavyBomber_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void Interceptor_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}
		private void LandPatrol_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterFinish();
		}

		private void VisibleAllCountColumn_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EquipmentView_CountAll.Visible = VisibleAllCountColumn_ToolStripMenuItem.Checked;
		}
		private void VisibleRemainCountColumn_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EquipmentView_CountRemain.Visible = VisibleRemainCountColumn_ToolStripMenuItem.Checked;
		}
		private void VisibleUnlockedCountColumn_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EquipmentView_Unlocked.Visible = VisibleUnlockedCountColumn_ToolStripMenuItem.Checked;
		}

		//更新(埋まっていたのをトップに出した)
		private void Reload_RToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (EquipmentView.Enabled && EquipmentView.SelectedRows != null && EquipmentView.SelectedRows.Count > 0)
			{
				//今選ばれているもののindexを取得しておき、
				//一覧を更新してから選びなおして詳細を更新する
				int v = 0;

				foreach(DataGridViewRow r in EquipmentView.SelectedRows)
				{
					v = r.Index;
					break;
				}
				UpdateView();
				EquipmentView.ClearSelection();
				EquipmentView.Rows[v].Selected = true;
				UpdateDetailView((int)EquipmentView[EquipmentView_ID.Index, EquipmentView.SelectedRows[0].Index].Value);
			}
			else
			{
				UpdateView();
			}
		}

		/// <summary>
		/// 「艦隊分析」装備情報反映用
		/// https://kancolle-fleetanalysis.firebaseapp.com/
		/// </summary>
		private void TopMenu_File_CopyToFleetAnalysis_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(
				"[" + string.Join(",", KCDatabase.Instance.Equipments.Values.Where(eq => eq?.IsLocked ?? false)
				.Select(eq => $"{{\"api_slotitem_id\":{eq.EquipmentID},\"api_level\":{eq.Level}}}")) + "]");
		}


		private void DialogEquipmentList_FormClosed(object sender, FormClosedEventArgs e)
		{

			ResourceManager.DestroyIcon(Icon);

		}
	}
}
