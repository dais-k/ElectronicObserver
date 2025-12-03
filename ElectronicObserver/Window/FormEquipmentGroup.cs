using ElectronicObserver.Data;
using ElectronicObserver.Data.EquipmentGroup;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{
	public partial class FormEquipmentGroup : DockContent
	{
		/// <summary>タブ背景色(アクティブ)</summary>
		private readonly Color TabActiveColor = Color.FromArgb(0xFF, 0xFF, 0xCC);

		/// <summary>タブ背景色(非アクティブ)</summary>
		private readonly Color TabInactiveColor = SystemColors.Control;

		// セル背景色
		private readonly Color CellColorRed = Color.FromArgb(0xFF, 0xBB, 0xBB);
		private readonly Color CellColorOrange = Color.FromArgb(0xFF, 0xDD, 0xBB);
		private readonly Color CellColorYellow = Color.FromArgb(0xFF, 0xFF, 0xBB);
		private readonly Color CellColorGreen = Color.FromArgb(0xBB, 0xFF, 0xBB);
		private readonly Color CellColorGray = Color.FromArgb(0xBB, 0xBB, 0xBB);
		private readonly Color CellColorCherry = Color.FromArgb(0xFF, 0xDD, 0xDD);

		//セルスタイル
		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter, CSDefaultRight,
			CSRedRight, CSOrangeRight, CSYellowRight, CSGreenRight, CSGrayRight, CSCherryRight,
			CSIsLocked;

		/// <summary>選択中のタブ</summary>
		private ImageLabel SelectedTab = null;

		/// <summary>選択中のグループ</summary>
		private EquipmentGroupData CurrentGroup => SelectedTab == null ? null : KCDatabase.Instance.EquipmentGroup[(int)SelectedTab.Tag];

		private bool IsRowsUpdating;
		private int _splitterDistance;

		public FormEquipmentGroup(FormMain parent)
		{
			InitializeComponent();

			ControlHelper.SetDoubleBuffered(EquipView);

			IsRowsUpdating = true;
			_splitterDistance = -1;

			foreach (DataGridViewColumn column in EquipView.Columns)
			{
				column.MinimumWidth = 2;
			}

			#region set CellStyle

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

			CSDefaultCenter = new DataGridViewCellStyle(CSDefaultLeft)
			{
				Alignment = DataGridViewContentAlignment.MiddleCenter
			};

			CSDefaultRight = new DataGridViewCellStyle(CSDefaultLeft)
			{
				Alignment = DataGridViewContentAlignment.MiddleRight
			};

			CSRedRight = new DataGridViewCellStyle(CSDefaultRight);
			CSRedRight.BackColor =
			CSRedRight.SelectionBackColor = CellColorRed;

			CSOrangeRight = new DataGridViewCellStyle(CSDefaultRight);
			CSOrangeRight.BackColor =
			CSOrangeRight.SelectionBackColor = CellColorOrange;

			CSYellowRight = new DataGridViewCellStyle(CSDefaultRight);
			CSYellowRight.BackColor =
			CSYellowRight.SelectionBackColor = CellColorYellow;

			CSGreenRight = new DataGridViewCellStyle(CSDefaultRight);
			CSGreenRight.BackColor =
			CSGreenRight.SelectionBackColor = CellColorGreen;

			CSGrayRight = new DataGridViewCellStyle(CSDefaultRight);
			CSGrayRight.ForeColor =
			CSGrayRight.SelectionForeColor = CellColorGray;

			CSCherryRight = new DataGridViewCellStyle(CSDefaultRight);
			CSCherryRight.BackColor =
			CSCherryRight.SelectionBackColor = CellColorCherry;

			CSIsLocked = new DataGridViewCellStyle(CSDefaultCenter);
			CSIsLocked.ForeColor =
			CSIsLocked.SelectionForeColor = Color.FromArgb(0xFF, 0x88, 0x88);

			EquipView.DefaultCellStyle = CSDefaultRight;
			EquipView_Name.DefaultCellStyle = CSDefaultLeft;

			#endregion

			SystemEvents.SystemShuttingDown += SystemShuttingDown;
		}

		private void FormEquipmentGroup_Load(object sender, EventArgs e)
		{
			EquipmentGroupManager groups = KCDatabase.Instance.EquipmentGroup;

			// 空(≒初期状態)の時、おなじみ全所属艦を追加
			if (groups.EquipmentGroups.Count == 0)
			{

				Utility.Logger.Add(3, "EquipmentGroup: グループが見つかりませんでした。デフォルトに戻すには、一旦終了後 " + EquipmentGroupManager.DefaultFilePath + " を削除してください。");

				var group = KCDatabase.Instance.EquipmentGroup.Add();
				group.Name = "全装備";

				for (int i = 0; i < EquipView.Columns.Count; i++)
				{
					var newdata = new EquipmentGroupData.ViewColumnData(EquipView.Columns[i]);
					if (SelectedTab == null)
						newdata.Visible = true;     //初期状態では全行が非表示のため
					group.ViewColumns.Add(EquipView.Columns[i].Name, newdata);
				}

				// 初期グループには全装備を含める
				if (KCDatabase.Instance.MasterEquipments != null && KCDatabase.Instance.MasterEquipments.Count > 0)
				{
					group.AddInclusionFilter(KCDatabase.Instance.MasterEquipments.Keys);
					group.UpdateMembers();
				}
			}

			foreach (var g in groups.EquipmentGroups.Values)
			{
				TabPanel.Controls.Add(CreateTabLabel(g.GroupID));
			}

			// 最初のタブを選択してビューを構築
			if (TabPanel.Controls.Count > 0)
			{
				var first = TabPanel.Controls.OfType<ImageLabel>().FirstOrDefault();
				if (first != null)
				{
					SelectedTab = first;
					SelectedTab.BackColor = TabActiveColor;
					ChangeEquipView(SelectedTab);
				}
			}

			{
				int columnCount = EquipView.Columns.Count;
				for (int i = 0; i < columnCount; i++)
				{
					EquipView.Columns[i].Visible = false;
				}
			}
			//*/

			ConfigurationChanged();

			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += APIUpdated;
			o.APIList["api_get_member/ship2"].ResponseReceived += APIUpdated;
			o.APIList["api_get_member/ship3"].ResponseReceived += APIUpdated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += APIUpdated;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += APIUpdated;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += APIUpdated;
			o.APIList["api_get_member/slot_item"].ResponseReceived += APIUpdated;
			o.APIList["api_req_hensei/preset_select"].ResponseReceived += APIUpdated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			IsRowsUpdating = false;
			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormShipGroup]);
			InitializeImprovementTooltip();
		}

		void ConfigurationChanged()
		{

			var config = Utility.Configuration.Config;

			// フォーム全体のフォントは設定から取得
			Font = config.UI.MainFont;
			StatusBar.Font = Font;

			// DataGridView は常に Meiryo 12px に固定する
			var dgvFont = new Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			EquipView.Font = dgvFont;

			// セルスタイルも DataGridView フォントに揃える
			CSDefaultLeft.Font =
			CSDefaultCenter.Font =
			CSDefaultRight.Font =
			CSRedRight.Font =
			CSOrangeRight.Font =
			CSYellowRight.Font =
			CSGreenRight.Font =
			CSGrayRight.Font =
			CSCherryRight.Font =
			CSIsLocked.Font =
				dgvFont;

			foreach (System.Windows.Forms.Control c in TabPanel.Controls)
				c.Font = Font;

			MenuGroup_AutoUpdate.Checked = config.FormEquipmentGroup.AutoUpdate;
			MenuGroup_ShowStatusBar.Checked = config.FormEquipmentGroup.ShowStatusBar;

		}

		// レイアウトロード時に呼ばれる
		public void ConfigureFromPersistString(string persistString)
		{

			string[] args = persistString.Split("?=&".ToCharArray());

			for (int i = 1; i < args.Length - 1; i += 2)
			{
				switch (args[i])
				{
					case "SplitterDistance":
						// 直接変えるとサイズが足りないか何かで変更が適用されないことがあるため、 Resize イベント中に変更する(ために値を記録する)
						// しかし Resize イベントだけだと呼ばれないことがあるため、直接変えてもおく
						// つらい
						splitContainer1.SplitterDistance = _splitterDistance = int.Parse(args[i + 1]);
						break;
				}
			}
		}

		protected override string GetPersistString() => "EquipmentGroup?SplitterDistance=" + splitContainer1.SplitterDistance;

		/// <summary>
		/// 指定したグループIDに基づいてタブ ラベルを生成します。
		/// </summary>
		private ImageLabel CreateTabLabel(int id)
		{

			ImageLabel label = new ImageLabel
			{
				Text = KCDatabase.Instance.EquipmentGroup[id].Name,
				Anchor = AnchorStyles.Left,
				Font = EquipView.Font,
				BackColor = TabInactiveColor,
				BorderStyle = BorderStyle.FixedSingle,
				Padding = new Padding(4, 4, 4, 4),
				Margin = new Padding(0, 0, 0, 0),
				ImageAlign = ContentAlignment.MiddleCenter,
				AutoSize = true,
				Cursor = Cursors.Hand
			};

			//イベントと固有IDの追加(内部データとの紐付)
			label.Click += TabLabel_Click;
			label.MouseDown += TabLabel_MouseDown;
			label.MouseMove += TabLabel_MouseMove;
			label.MouseUp += TabLabel_MouseUp;
			label.ContextMenuStrip = MenuGroup;
			label.Tag = id;

			return label;
		}

		void TabLabel_Click(object sender, EventArgs e)
		{
			ChangeEquipView(sender as ImageLabel);
		}

		private void APIUpdated(string apiname, dynamic data)
		{
			if (MenuGroup_AutoUpdate.Checked)
				ChangeEquipView(SelectedTab);
		}

		/// <summary>
		/// EquipView用の新しい行のインスタンスを作成します。
		/// （最適化: アイコンはキャッシュ経由で行作成時に設定し、装備配備先文字列は事前にまとめて計算したマップを利用）
		/// </summary>
		/// <param name="equip">追加する装備マスターデータ。</param>
		private DataGridViewRow CreateEquipViewRow(EquipmentDataMaster equip, Dictionary<int, string> shipsMap = null)
		{
			if (equip == null) return null;

			DataGridViewRow row = new DataGridViewRow();
			row.CreateCells(EquipView);
			row.Height = 21;

			// 今日の担当を取得。 "-" の場合は次回担当を表示する。
			string today = GetTodayImprovementNames(equip.Improvements);
			string improveDisplay = today == "-" ? GetNextImprovementNamesCombined(equip.Improvements) : today;

			// SetValues ではアイコン列には int (IconType) を入れておく（ソートのため）
			row.SetValues(
				equip.EquipmentID,
				equip.IconType,
				equip.Name,
				equip.CategoryTypeInstance?.Name ?? equip.CategoryType.ToString(),
				equip.CategoryTypeInstance2?.Name ?? equip.CategoryType2.ToString(),
				improveDisplay,
				Constants.GetRange(equip.Range),
				equip.Firepower,
				equip.Accuracy,
				equip.Evasion,
				equip.Bomber,
				equip.Torpedo,
				equip.LOS,
				equip.ASW,
				equip.AA,
				equip.Armor,
				equip.AircraftDistance,
				// EquipView_EquipedShips は後で shipsMap から設定するか、フォールバックで取得
				shipsMap != null && shipsMap.TryGetValue(equip.EquipmentID, out var ships) ? ships : GetShipsEquipping(equip.EquipmentID)
			);

			// 値は int のままにして、ツールチップと（任意で）Tag にキャッシュ画像を入れておく。
			try
			{
				var iconCell = row.Cells[EquipView_Icon.Index];

				// 表示用の画像は CellFormatting で int -> Image に変換される前提。
				// ただしツールチップはここで設定しておく。
				iconCell.ToolTipText = Constants.GetIconName(equip.IconType);

				// 任意: 描画時の GetCachedIcon 呼び出しを減らすため画像を Tag に入れておく
				var img = GetCachedIcon(equip.IconType);
				if (img != null)
				{
					iconCell.Tag = img;
				}
			}
			catch
			{
				// 無視
			}

			row.Tag = equip.EquipmentID;

			return row;
		}
		
		
		/// <summary>
		/// 指定したタブのグループのEquipViewを作成します。
		/// </summary>
		/// <param name="target">作成するビューのグループデータ</param>
		private void BuildEquipView(ImageLabel target)
		{
			if (target == null)
				return;

			EquipmentGroupData group = KCDatabase.Instance.EquipmentGroup[(int)target.Tag];

			// --- 追加: グループが空でマスター装備がロード済みなら全装備を自動追加 ---
			if ((group.Members == null || group.Members.Count == 0) &&
				(group.InclusionFilter == null || group.InclusionFilter.Count == 0))
			{
				if (KCDatabase.Instance.MasterEquipments != null && KCDatabase.Instance.MasterEquipments.Count > 0)
				{
					group.AddInclusionFilter(KCDatabase.Instance.MasterEquipments.Keys);
					group.UpdateMembers();
				}
				else
				{
					Utility.Logger.Add(3, $"EquipmentGroup: グループ '{group.Name}' が空であり、MasterEquipments は未ロードです。");
				}
			}
			

			IsRowsUpdating = true;
			EquipView.SuspendLayout();

			UpdateMembers(group);

			EquipView.Rows.Clear();

			var equips = group.MembersInstance;
			var rows = new List<DataGridViewRow>(equips.Count());

			// --- ここで装備ごとの配備先文字列マップを一括構築しておく（GetShipsEquipping を繰り返さない） ---
			var equipIds = equips.Select(e => e?.EquipmentID ?? -1).Where(id => id > 0).Distinct();
			var shipsMap = BuildEquipToShipsMap(equipIds);

			foreach (var eq in equips)
			{
				if (eq == null) continue;

				DataGridViewRow row = CreateEquipViewRow(eq, shipsMap);
				rows.Add(row);
			}

			for (int i = 0; i < rows.Count; i++)
				rows[i].Tag = i;

			EquipView.Rows.AddRange(rows.ToArray());

			// 設定に抜けがあった場合補充
			if (group.ViewColumns == null)
			{
				group.ViewColumns = new Dictionary<string, EquipmentGroupData.ViewColumnData>();
			}
			if (EquipView.Columns.Count != group.ViewColumns.Count)
			{
				foreach (DataGridViewColumn column in EquipView.Columns)
				{
					if (!group.ViewColumns.ContainsKey(column.Name))
					{
						var newdata = new EquipmentGroupData.ViewColumnData(column)
						{
							Visible = true     //初期状態でインビジだと不都合なので
						};

						group.ViewColumns.Add(newdata.Name, newdata);
					}
				}
			}

			ApplyViewData(group);
			ApplyAutoSort(group);

			EquipView.ResumeLayout();
			IsRowsUpdating = false;

		}

		/// <summary>
		/// 指定された装備ID集合について、装備を所持/装備している艦・基地・配置転換中の集計文字列を一括で作る。
		/// BuildEquipView の内部最適化用（GetShipsEquipping の個別走査を減らす）。
		/// </summary>
		private Dictionary<int, string> BuildEquipToShipsMap(IEnumerable<int> equipmentIDs)
		{
			var db = KCDatabase.Instance;
			var ids = new HashSet<int>(equipmentIDs.Where(i => i > 0));
			var result = ids.ToDictionary(i => i, i => new List<string>());

			// 所持艦（スロット/増設）を走査
			var equipmentsDict = db.Equipments;
			foreach (var s in db.Ships.Values)
			{
				if (s == null) continue;

				string displayName = s.MasterShip?.NameWithClass ?? $"ID:{s.MasterID}";
				string displayWithLv = $"{displayName} (Lv.{s.Level})";

				// マスターIDでの比較（主）
				try
				{
					if (s.SlotMaster != null)
					{
						foreach (var mid in s.SlotMaster)
						{
							if (ids.Contains(mid))
							{
								if (!result[mid].Contains(displayWithLv))
									result[mid].Add(displayWithLv);
							}
						}
					}
					if (s.ExpansionSlotMaster > 0 && ids.Contains(s.ExpansionSlotMaster))
					{
						if (!result[s.ExpansionSlotMaster].Contains(displayWithLv))
							result[s.ExpansionSlotMaster].Add(displayWithLv);
					}
				}
				catch
				{
					// 無視してインスタンス側の走査へ
				}

				// インスタンスID -> マスターID でのフォールバック
				if (s.Slot != null)
				{
					foreach (var instId in s.Slot)
					{
						if (instId <= 0) continue;
						if (equipmentsDict.TryGetValue(instId, out var inst) && inst != null)
						{
							int mid = inst.EquipmentID;
							if (ids.Contains(mid))
							{
								if (!result[mid].Contains(displayWithLv))
									result[mid].Add(displayWithLv);
							}
						}
					}
				}
				if (s.ExpansionSlot > 0)
				{
					if (equipmentsDict.TryGetValue(s.ExpansionSlot, out var expInst) && expInst != null)
					{
						int mid = expInst.EquipmentID;
						if (ids.Contains(mid))
						{
							if (!result[mid].Contains(displayWithLv))
								result[mid].Add(displayWithLv);
						}
					}
				}
			}

			// 基地航空隊：配備中の装備を集計
			foreach (var corps in db.BaseAirCorps.Values)
			{
				if (corps == null) continue;

				var counts = new Dictionary<int, int>();
				foreach (var sq in corps.Squadrons.Values)
				{
					if (sq == null) continue;
					int eqid = sq.EquipmentID;
					if (ids.Contains(eqid))
					{
						if (!counts.ContainsKey(eqid)) counts[eqid] = 0;
						counts[eqid]++;
					}
				}
				if (counts.Count > 0)
				{
					string corpsName = string.IsNullOrEmpty(corps.Name) ? "" : " " + corps.Name;
					foreach (var kv in counts)
					{
						var label = $"#{corps.MapAreaID}{corpsName}" + (kv.Value > 1 ? " x" + kv.Value : "");
						if (!result[kv.Key].Contains(label))
							result[kv.Key].Add(label);
					}
				}
			}

			// 配置転換中の装備
			var relocatingCounts = new Dictionary<int, int>();
			foreach (var r in db.RelocatedEquipments.Values)
			{
				var inst = r?.EquipmentInstance;
				if (inst == null) continue;
				int mid = inst.EquipmentID;
				if (ids.Contains(mid))
				{
					if (!relocatingCounts.ContainsKey(mid)) relocatingCounts[mid] = 0;
					relocatingCounts[mid]++;
				}
			}
			foreach (var kv in relocatingCounts)
			{
				var label = "配置転換中" + (kv.Value > 1 ? " x" + kv.Value : "");
				if (!result[kv.Key].Contains(label))
					result[kv.Key].Add(label);
			}

			// 重複排除・結合して文字列化
			var final = new Dictionary<int, string>();
			foreach (var id in ids)
			{
				var list = result[id];
				final[id] = list.Count == 0 ? "" : string.Join(", ", list.Distinct());
			}

			return final;
		}

		/// <summary>
		/// EquipViewを指定したタブに切り替えます。
		/// </summary>
		private void ChangeEquipView(ImageLabel target)
		{
			if (target == null) return;

			var group = KCDatabase.Instance.EquipmentGroup[(int)target.Tag];
			var currentGroup = CurrentGroup;

			int headIndex = 0;
			List<int> selectedIDList = new List<int>();

			if (group == null)
			{
				Utility.Logger.Add(3, "エラー：存在しないグループを参照しようとしました。開発者に連絡してください");
				return;
			}

			if (currentGroup != null)
			{
				UpdateMembers(currentGroup);

				if (CurrentGroup.GroupID != group.GroupID)
				{
					EquipView.Rows.Clear();      //別グループの行の並び順を引き継がせないようにする

				}
				else
				{
					headIndex = EquipView.FirstDisplayedScrollingRowIndex;
					selectedIDList = EquipView.SelectedRows.Cast<DataGridViewRow>().Select(r => (int)r.Cells[EquipView_ID.Index].Value).ToList();
				}
			}

			if (SelectedTab != null)
				SelectedTab.BackColor = TabInactiveColor;

			SelectedTab = target;

			BuildEquipView(SelectedTab);
			SelectedTab.BackColor = TabActiveColor;

			if (0 <= headIndex && headIndex < EquipView.Rows.Count)
			{
				try
				{
					EquipView.FirstDisplayedScrollingRowIndex = headIndex;
				}
				catch (InvalidOperationException)
				{
					// 1行も表示できないサイズのときに例外が出るので握りつぶす
				}
			}

			if (selectedIDList.Count > 0)
			{
				EquipView.ClearSelection();
				for (int i = 0; i < EquipView.Rows.Count; i++)
				{
					var row = EquipView.Rows[i];
					if (selectedIDList.Contains((int)row.Cells[EquipView_ID.Index].Value))
					{
						row.Selected = true;
					}
				}
			}
		}

		// 今日の担当だけを抽出する。-1 を発見したらその improvement はスキップ。
		private string GetTodayImprovementNames(List<EquipmentDataMaster.Improvement> imps)
		{
			if (imps == null || imps.Count == 0)
				return "";

			int today = (int)DateTime.Now.DayOfWeek;
			var parts = new List<string>();

			foreach (var imp in imps)
			{
				if (imp?.Req?.WeekConditions == null) continue;

				var weeks = imp.Req.WeekConditions;
				while (weeks.Count < 7) weeks.Add(new List<int>());

				var todayList = weeks.ElementAtOrDefault(today) ?? new List<int>();

				// この improvement が -1 を含むならスキップ
				if (todayList.Contains(-1))
					continue;

				foreach (var id in todayList)
				{
					if (id == 0)
					{
						parts.Add("任意2番艦");
					}
					else if (id > 0)
					{
						if (KCDatabase.Instance.MasterShips.TryGetValue(id, out var master) &&
							master != null && !string.IsNullOrEmpty(master.NameWithClass))
						{
							parts.Add(master.NameWithClass);
						}
						else
						{
							parts.Add($"ID:{id}");
						}
					}
				}
			}

			return parts.Count == 0 ? "-" : string.Join(", ", parts);
		}


		// 次回（-1 を検出した段階で検索する）担当を抽出する。
		// WeekConditions の定義順通りに名前化し、同じ曜日はまとめて一行にする。
		private string GetNextImprovementNamesCombined(List<EquipmentDataMaster.Improvement> imps)
		{
			if (imps == null || imps.Count == 0)
				return "";

			int today = (int)DateTime.Now.DayOfWeek;
			var parts = new List<string>();
			bool hasUnknownNext = false;

			foreach (var imp in imps)
			{
				if (imp?.Req?.WeekConditions == null) continue;

				var weeks = imp.Req.WeekConditions;
				while (weeks.Count < 7) weeks.Add(new List<int>());

				bool resolved = false;
				for (int offset = 1; offset <= 6 && !resolved; offset++)
				{
					int d = (today + offset) % 7;
					var list = weeks.ElementAtOrDefault(d) ?? new List<int>();

					if (list.Any(x => x != -1))
					{
						// 同じ曜日の候補をまとめる
						var names = new List<string>();
						foreach (var candidate in list)
						{
							if (candidate == -1) continue;

							string name;
							if (candidate == 0)
								name = "任意2番艦";
							else if (candidate > 0)
							{
								if (KCDatabase.Instance.MasterShips.TryGetValue(candidate, out var ship)
									&& ship != null
									&& !string.IsNullOrEmpty(ship.NameWithClass))
									name = ship.NameWithClass;
								else
									name = $"ID:{candidate}";
							}
							else
								name = $"ID:{candidate}";

							names.Add(name);
						}

						if (names.Count > 0)
						{
							// 先頭に曜日ラベルを付け、後続は艦名だけ並べる
							string line = $"次回({GetJapaneseWeekDay((DayOfWeek)d)}) {string.Join(", ", names)}";
							parts.Add(line);
						}

						resolved = true;
					}
				}

				if (!resolved)
					hasUnknownNext = true;
			}

			if (hasUnknownNext)
				parts.Add("次回(不明)");

			return parts.Count == 0 ? "" : string.Join(", ", parts);
		}

		/// <summary>
		/// DayOfWeek を日本語一文字で返す (例: 日, 月, 火 ...)
		/// </summary>
		private string GetJapaneseWeekDay(DayOfWeek d)
		{
			switch (d)
			{
				case DayOfWeek.Sunday: return "日";
				case DayOfWeek.Monday: return "月";
				case DayOfWeek.Tuesday: return "火";
				case DayOfWeek.Wednesday: return "水";
				case DayOfWeek.Thursday: return "木";
				case DayOfWeek.Friday: return "金";
				case DayOfWeek.Saturday: return "土";
				default: return "?";
			}
		}

		/// <summary>
		/// 指定した装備IDを装備している艦娘名をカンマ区切りで返します。
		/// 見つからなければ空文字を返します。
		/// </summary>
		private string GetShipsEquipping(int equipmentID)
		{
			if (equipmentID <= 0) return "";

			var names = new List<string>();

			// --- 所持艦（スロット/増設）を走査 ---
			foreach (var s in KCDatabase.Instance.Ships.Values)
			{
				if (s == null) continue;

				bool equipped = false;

				// マスターIDでの比較（主）
				try
				{
					if (s.SlotMaster != null && s.SlotMaster.Contains(equipmentID))
						equipped = true;
					else if (s.ExpansionSlotMaster == equipmentID)
						equipped = true;
				}
				catch
				{
					// 無視してフォールバック
				}

				// インスタンスID -> マスターID でのフォールバック
				if (!equipped)
				{
					if (s.Slot != null && s.Slot.Any(id => id > 0 &&
						(KCDatabase.Instance.Equipments.ContainsKey(id) && KCDatabase.Instance.Equipments[id]?.EquipmentID == equipmentID)))
					{
						equipped = true;
					}
					else if (s.ExpansionSlot > 0 &&
						(KCDatabase.Instance.Equipments.ContainsKey(s.ExpansionSlot) && KCDatabase.Instance.Equipments[s.ExpansionSlot]?.EquipmentID == equipmentID))
					{
						equipped = true;
					}
				}

				if (equipped)
				{
					var displayName = s.MasterShip?.NameWithClass ?? $"ID:{s.MasterID}";
					names.Add($"{displayName} (Lv.{s.Level})");
				}
			}

			// --- 基地航空隊：配備中の装備を集計 ---
			foreach (var corps in KCDatabase.Instance.BaseAirCorps.Values)
			{
				if (corps == null) continue;

				int count = corps.Squadrons.Values.Count(sq => sq != null && sq.EquipmentID == equipmentID);
				if (count > 0)
				{
					// 表示は "#{MapAreaID} {Name}"、複数なら " xN"
					string corpsName = string.IsNullOrEmpty(corps.Name) ? "" : " " + corps.Name;
					names.Add($"#{corps.MapAreaID}{corpsName}" + (count > 1 ? " x" + count : ""));
				}
			}

			// --- 配置転換中の装備 ---
			int relocatingCount = KCDatabase.Instance.RelocatedEquipments.Values
				.Select(r => r?.EquipmentInstance)
				.Where(eq => eq != null && eq.EquipmentID == equipmentID)
				.Count();

			if (relocatingCount > 0)
			{
				names.Add("配置転換中" + (relocatingCount > 1 ? " x" + relocatingCount : ""));
			}

			// 重複排除して返す
			var result = names.Distinct().ToList();
			return result.Count == 0 ? "" : string.Join(", ", result);
		}


		/// <summary>
		/// 現在選択している艦船のIDリストを求めます。
		/// </summary>
		private IEnumerable<int> GetSelectedShipID()
		{
			return EquipView.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index).Select(r => (int)r.Cells[EquipView_ID.Index].Value);
		}

	   
		/// <summary>
		/// 現在の表を基に、グループメンバーを更新します。
		/// </summary>
		private void UpdateMembers(EquipmentGroupData group)
		{
			group.UpdateMembers(EquipView.Rows.Cast<DataGridViewRow>().Select(r => (int)r.Cells[EquipView_ID.Index].Value));
		}

		private void EquipView_SelectionChanged(object sender, EventArgs e)
		{
			UpdateStatus();
		}

		private Dictionary<int, Image> _iconCache = new Dictionary<int, Image>();

		private Image GetCachedIcon(int iconId)
		{
			if (_iconCache.TryGetValue(iconId, out var cached))
			{
				// 既にキャッシュ済みならそれを返す
				return cached;
			}

			// 未キャッシュなら取得して保存
			var img = ResourceManager.GetEquipmentImage(iconId);
			if (img != null)
			{
				_iconCache[iconId] = img;
			}
			return img;
		}


		private void EquipView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == EquipView_Icon.Index)
			{
				if (e.Value is int iconId)
				{
					var img = GetCachedIcon(iconId); // キャッシュ経由で取得
					e.Value = img;
					var cell = EquipView.Rows[e.RowIndex].Cells[e.ColumnIndex];
					cell.ToolTipText = Constants.GetIconName(iconId);
					e.FormattingApplied = true;
				}
			}
			else if (e.ColumnIndex == EquipView_ImproveShips.Index)
			{
				string text = e.Value?.ToString() ?? "";

				if (text.Contains("次回"))
				{
					e.CellStyle.ForeColor = Color.FromArgb(150, 150, 150);
				}
				else
				{
					// デフォルトの ForeColor に戻す
					e.CellStyle.ForeColor = EquipView.DefaultCellStyle.ForeColor;
				}

				// 値自体は変更しないので FormattingApplied は false のままでも良いが、
				// 他のフォーマッタと競合しないよう True にしておく
				e.FormattingApplied = true;
				return;
			}
		}

		/// <summary>
		/// 集計と状態表示。対象は IsAbyssalEquipment == false の装備のみ。
		/// </summary>
		private void UpdateStatus()
		{
			var db = KCDatabase.Instance;

			// 装備中（艦載 or 基地航空隊配備）の MasterID を収集
			var equippedMasterIDs = new HashSet<int>();

			foreach (var ship in db.Ships.Values)
			{
				if (ship == null) continue;
				foreach (var eq in ship.AllSlotInstance)
				{
					if (eq != null)
						equippedMasterIDs.Add(eq.MasterID);
				}
			}

			foreach (var corps in db.BaseAirCorps.Values)
			{
				if (corps == null) continue;
				foreach (var sq in corps.Squadrons.Values)
				{
					if (sq == null) continue;
					var inst = sq.EquipmentInstance;
					if (inst != null)
						equippedMasterIDs.Add(inst.MasterID);
				}
			}

			var selectedRows = EquipView.SelectedRows;
			if (selectedRows.Count == 1)
			{
				// Tag は BuildEquipView 側で行インデックスに上書きされるため、
				// 装備IDはセルの EquipView_ID 列から取得する（フィルタやソート時に正しい）。
				int equipmentID = -1;
				var cellValue = selectedRows[0].Cells[EquipView_ID.Index].Value;
				if (cellValue is int idFromCell)
				{
					equipmentID = idFromCell;
				}
				else
				{
					// 念のため Tag に残っている場合をフォールバックとして使う
					if (selectedRows[0].Tag is int idFromTag)
						equipmentID = idFromTag;
				}

				if (equipmentID <= 0)
				{
					// 不正な値なら表示をクリア
					Status_Total.Text = "";
					Status_ByLevel.Text = "";
					Status_ByAircraftLevel.Text = "";
					return;
				}

				var instances = db.Equipments.Values
					.Where(eq => eq != null && eq.EquipmentID == equipmentID && eq.MasterEquipment != null && !eq.MasterEquipment.IsAbyssalEquipment)
					.ToList();

				int total = instances.Count;
				int equippedCount = instances.Count(i => equippedMasterIDs.Contains(i.MasterID));

				Status_Total.Text = $"所持数: {total} (装備中/配備中: {equippedCount})";

				var byLevel = instances
					.GroupBy(i => i.Level)
					.OrderBy(g => g.Key)
					.Select(g => $"★{g.Key}:{g.Count()}")
					.ToArray();

				Status_ByLevel.Text = "改修: " + (byLevel.Length > 0 ? string.Join(" /", byLevel) : "なし");

				var byAlvSimple = instances
					.GroupBy(i => i.AircraftLevel)
					.OrderBy(g => g.Key)
					.Select(g => $"☆{g.Key}:{g.Count()}")
					.ToArray();

				// ここで選択中の装備が航空機でない場合は熟練度表示を消す
				EquipmentDataMaster masterEq = null;
				if (db.MasterEquipments.ContainsKey(equipmentID))
					masterEq = db.MasterEquipments[equipmentID];

				if (masterEq != null && !masterEq.IsAircraft)
				{
					Status_ByAircraftLevel.Text = "";
				}
				else
				{
					Status_ByAircraftLevel.Text = "熟練度: " + (byAlvSimple.Length > 0 ? string.Join(" /", byAlvSimple) : "なし");
				}
			}
			else
			{
				var allInstances = db.Equipments.Values
					.Where(i => i != null && i.MasterEquipment != null && !i.MasterEquipment.IsAbyssalEquipment)
					.ToList();

				int totalAll = allInstances.Count;
				int equippedAll = allInstances.Count(i => equippedMasterIDs.Contains(i.MasterID));

				Status_Total.Text = $"全装備総数 (非深海装備): {totalAll} 個 (装備中/配備中: {equippedAll})";

				var byLevelAll = allInstances
					.GroupBy(i => i.Level)
					.OrderBy(g => g.Key)
					.Select(g => $"★{g.Key}:{g.Count()}")
					.Take(10)
					.ToArray();

				Status_ByLevel.Text = "改修(一部): " + (byLevelAll.Length > 0 ? string.Join(" / ", byLevelAll) : "なし");

				var byAlvAll = allInstances
					.GroupBy(i => i.AircraftLevel)
					.OrderBy(g => g.Key)
					.Select(g => $"{g.Key}:{g.Count()}")
					.Take(10)
					.ToArray();

				Status_ByAircraftLevel.Text = "熟練度(一部): " + (byAlvAll.Length > 0 ? string.Join(" / ", byAlvAll) : "なし");
			}
		}


		// EquipView 上のホバーでツールチップを表示して良いか判定するヘルパー
		private bool CanShowImproveTipAt(int rowIndex, int colIndex)
		{
			// 範囲外やヘッダは表示しない
			if (rowIndex < 0 || colIndex < 0) return false;
			if (rowIndex >= EquipView.Rows.Count || colIndex >= EquipView.Columns.Count) return false;

			// ImproveShips 列以外は既存動作に任せる（ここでは true を返す）
			if (colIndex != EquipView_ImproveShips.Index) return true;

			var cell = EquipView.Rows[rowIndex].Cells[colIndex];
			if (cell == null) return false;

			// FormattedValue を優先して取得（セル表示上何もない場合は空扱い）
			string text;
			try
			{
				text = cell.FormattedValue?.ToString() ?? cell.Value?.ToString();
			}
			catch
			{
				text = cell.Value?.ToString();
			}

			// 空白または "-" は表示しない
			if (string.IsNullOrWhiteSpace(text)) return false;
			if (text.Trim() == "-") return false;

			return true;
		}

		private void EquipView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			try
			{
				// カテゴリ列は表示文字列(50音順)ではなく Category / Category2 の値で比較する
				if (e.Column == EquipView_Category1 || e.Column == EquipView_Category2)
				{
					var db = KCDatabase.Instance;

					object val1 = null, val2 = null;
					int id1 = -1, id2 = -1;

					// EquipView の行から装備のマスターIDを取得して比較に使う
					try
					{
						val1 = EquipView.Rows[e.RowIndex1].Cells[EquipView_ID.Index].Value;
						val2 = EquipView.Rows[e.RowIndex2].Cells[EquipView_ID.Index].Value;
						if (val1 is int) id1 = (int)val1;
						if (val2 is int) id2 = (int)val2;
					}
					catch
					{
						// 取得失敗時はフォールバックで表示文字列で比較する
						id1 = id2 = -1;
					}

					int cat1 = int.MaxValue;
					int cat2 = int.MaxValue;

					if (id1 > 0 && db.MasterEquipments.TryGetValue(id1, out var m1) && m1 != null)
					{
						cat1 = (int)(e.Column == EquipView_Category1 ? m1.CategoryType : m1.CategoryType2);
					}
					if (id2 > 0 && db.MasterEquipments.TryGetValue(id2, out var m2) && m2 != null)
					{
						cat2 = (int)(e.Column == EquipView_Category1 ? m2.CategoryType : m2.CategoryType2);
					}

					// 比較
					e.SortResult = cat1.CompareTo(cat2);

					// 同値なら表示名(または装備名)で二次比較して安定化
					if (e.SortResult == 0)
					{
						string s1 = e.CellValue1?.ToString() ?? "";
						string s2 = e.CellValue2?.ToString() ?? "";

						// 可能ならマスターの名前で比較
						if (id1 > 0 && db.MasterEquipments.TryGetValue(id1, out var mm1) && mm1 != null)
							s1 = mm1.Name;
						if (id2 > 0 && db.MasterEquipments.TryGetValue(id2, out var mm2) && mm2 != null)
							s2 = mm2.Name;

						e.SortResult = string.Compare(s1, s2, StringComparison.CurrentCulture);
					}

					e.Handled = true;
					return;
				}
			}
			catch
			{
				// 失敗したら既定の比較にフォールバック（何もしない）
			}
		}

		private void EquipView_Sorted(object sender, EventArgs e)
		{
			int count = EquipView.Rows.Count;
			var direction = EquipView.SortOrder;

			for (int i = 0; i < count; i++)
				EquipView.Rows[i].Tag = i;
		}

		// 列のサイズ変更関連
		private void EquipView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (IsRowsUpdating)
				return;

			var group = CurrentGroup;
			if (group != null)
			{

				if (!group.ViewColumns[e.Column.Name].AutoSize)
				{
					group.ViewColumns[e.Column.Name].Width = e.Column.Width;
				}
			}
		}

		private void EquipView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (IsRowsUpdating)
				return;

			var group = CurrentGroup;
			if (group != null)
			{
				foreach (DataGridViewColumn column in EquipView.Columns)
				{
					group.ViewColumns[column.Name].DisplayIndex = column.DisplayIndex;
				}
			}
		}

		#region メニュー:グループ操作

		private void MenuGroup_Add_Click(object sender, EventArgs e)
		{
			using (var dialog = new DialogTextInput("グループを追加", "グループ名を入力してください："))
			{
				if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{
					var group = KCDatabase.Instance.EquipmentGroup.Add();

					group.Name = dialog.InputtedText.Trim();

					for (int i = 0; i < EquipView.Columns.Count; i++)
					{
						var newdata = new EquipmentGroupData.ViewColumnData(EquipView.Columns[i]);
						if (SelectedTab == null)
							newdata.Visible = true;     //初期状態では全行が非表示のため
						group.ViewColumns.Add(EquipView.Columns[i].Name, newdata);
					}

					TabPanel.Controls.Add(CreateTabLabel(group.GroupID));
				}
			}
		}

		private void MenuGroup_Copy_Click(object sender, EventArgs e)
		{
			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if (senderLabel == null)
				return;     //想定外

			using (var dialog = new DialogTextInput("グループをコピー", "グループ名を入力してください："))
			{
				if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{
					var group = KCDatabase.Instance.EquipmentGroup[(int)senderLabel.Tag].Clone();

					group.GroupID = KCDatabase.Instance.EquipmentGroup.GetUniqueID();
					group.Name = dialog.InputtedText.Trim();

					KCDatabase.Instance.EquipmentGroup.EquipmentGroups.Add(group);

					TabPanel.Controls.Add(CreateTabLabel(group.GroupID));
				}
			}
		}

		private void MenuGroup_Delete_Click(object sender, EventArgs e)
		{
			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if (senderLabel == null)
				return;     //想定外

			EquipmentGroupData group = KCDatabase.Instance.EquipmentGroup[(int)senderLabel.Tag];

			if (group != null)
			{
				if (MessageBox.Show(string.Format("グループ [{0}] を削除しますか？\r\nこの操作は元に戻せません。", group.Name), "確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
					== System.Windows.Forms.DialogResult.Yes)
				{
					if (SelectedTab == senderLabel)
					{
						EquipView.Rows.Clear();
						SelectedTab = null;
					}
					KCDatabase.Instance.EquipmentGroup.EquipmentGroups.Remove(group);
					TabPanel.Controls.Remove(senderLabel);
					senderLabel.Dispose();
				}
			}
			else
			{
				MessageBox.Show("このグループは削除できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void MenuGroup_Rename_Click(object sender, EventArgs e)
		{
			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if (senderLabel == null) return;

			EquipmentGroupData group = KCDatabase.Instance.EquipmentGroup[(int)senderLabel.Tag];

			if (group != null)
			{
				using (var dialog = new DialogTextInput("グループ名の変更", "グループ名を入力してください："))
				{
					dialog.InputtedText = group.Name;

					if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{
						group.Name = senderLabel.Text = dialog.InputtedText.Trim();
					}
				}
			}
			else
			{
				MessageBox.Show("このグループの名前を変更することはできません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void TabPanel_DoubleClick(object sender, EventArgs e)
		{
			MenuGroup_Add.PerformClick();
		}

		#endregion

		#region メニューON/OFF操作
		private void MenuGroup_Opening(object sender, CancelEventArgs e)
		{
			if (MenuGroup.SourceControl == TabPanel || SelectedTab == null)
			{
				MenuGroup_Add.Enabled = true;
				MenuGroup_Copy.Enabled = false;
				MenuGroup_Rename.Enabled = false;
				MenuGroup_Delete.Enabled = false;
			}
			else
			{
				MenuGroup_Add.Enabled = true;
				MenuGroup_Copy.Enabled = true;
				MenuGroup_Rename.Enabled = true;
				MenuGroup_Delete.Enabled = true;
			}
		}

		private void MenuMember_Opening(object sender, CancelEventArgs e)
		{
			if (SelectedTab == null)
			{
				e.Cancel = true;
				return;
			}

			if (KCDatabase.Instance.Ships.Count == 0)
			{
				MenuMember_Filter.Enabled = false;
				MenuMember_CSVOutput.Enabled = false;
			}
			else
			{
				MenuMember_Filter.Enabled = true;
				MenuMember_CSVOutput.Enabled = true;
			}

			if (EquipView.Rows.GetRowCount(DataGridViewElementStates.Selected) == 0)
			{
				MenuMember_AddToGroup.Enabled = false;
				MenuMember_CreateGroup.Enabled = false;
				MenuMember_Exclude.Enabled = false;
			}
			else
			{
				MenuMember_AddToGroup.Enabled = true;
				MenuMember_CreateGroup.Enabled = true;
				MenuMember_Exclude.Enabled = true;
			}
		}
		#endregion

		#region メニュー:メンバー操作
		
		private void MenuMember_ColumnFilter_Click(object sender, EventArgs e)
		{
			EquipmentGroupData group = CurrentGroup;

			if (group == null)
			{
				MessageBox.Show("このグループは変更できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			try
			{
				using (var dialog = new DialogEquipmentGroupColumnFilter(EquipView, group))
				{
					if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{

						group.ViewColumns = dialog.Result.ToDictionary(r => r.Name);
						group.ScrollLockColumnCount = dialog.ScrollLockColumnCount;

						ApplyViewData(group);
					}
				}
			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, "EquipmentGroup: 列の設定ダイアログでエラーが発生しました。");
			}
		}
		
		private void MenuMember_Filter_Click(object sender, EventArgs e)
		{
			var group = CurrentGroup;
			if (group != null)
			{
				try
				{
					if (group.Expressions == null)
						group.Expressions = new EqExpressionManager();

					using (var dialog = new DialogEquipmentGroupFilter(group))
					{
						if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
						{
							// replace
							int id = group.GroupID;
							group = dialog.ExportGroupData();
							group.GroupID = id;
							group.Expressions.Compile();

							KCDatabase.Instance.EquipmentGroup.EquipmentGroups.Remove(id);
							KCDatabase.Instance.EquipmentGroup.EquipmentGroups.Add(group);

							ChangeEquipView(SelectedTab);
						}
					}
				}
				catch (Exception ex)
				{
					Utility.ErrorReporter.SendErrorReport(ex, "EquipmentGroup: フィルタダイアログでエラーが発生しました。");
				}
			}
		}

		/// <summary>
		/// 表示設定を反映します。
		/// </summary>
		private void ApplyViewData(EquipmentGroupData group)
		{
			IsRowsUpdating = true;

			// いったん解除しないと列入れ替え時にエラーが起きる
			foreach (DataGridViewColumn column in EquipView.Columns)
			{
				column.Frozen = false;
			}

			foreach (var data in group.ViewColumns.Values.OrderBy(g => g.DisplayIndex))
			{
				data.ToColumn(EquipView.Columns[data.Name]);
			}

			int count = 0;
			foreach (var column in EquipView.Columns.Cast<DataGridViewColumn>().OrderBy(c => c.DisplayIndex))
			{
				column.Frozen = count < group.ScrollLockColumnCount;
				count++;
			}
	
			// --- 追加: 装備中の艦娘列が極端に狭くなるのを防ぐ ---
			// Designer 側は Fill になっているが、保存された設定で幅が 5 等になってしまうケースがあるため保護。
			var equipCol = EquipView.Columns.Cast<DataGridViewColumn>().FirstOrDefault(c => c.Name == "EquipView_EquipedShips");
			if (equipCol != null)
			{
				// 最小幅を設定しておく
				equipCol.MinimumWidth = Math.Max(equipCol.MinimumWidth, 60);

				// もし幅が異常に小さい場合は適切な値に直す
				if (equipCol.Width < 30)
				{
					equipCol.Width = 150;
				}

				// 常に表示領域を埋めるよう Fill にする（必要なら Designer の設定と一致させる）
				equipCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}

			IsRowsUpdating = false;
		}

		
		private void MenuMember_SortOrder_Click(object sender, EventArgs e)
		{
			var group = CurrentGroup;
			
			if (group != null)
			{
				try
				{
					using (var dialog = new DialogEquipmentGroupSortOrder(EquipView, group))
					{
						if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
						{
							group.AutoSortEnabled = dialog.AutoSortEnabled;
							group.SortOrder = dialog.Result;

							ApplyAutoSort(group);
						}
					}
				}
				catch (Exception ex)
				{

					Utility.ErrorReporter.SendErrorReport(ex, "EquipmentGroup: 自動ソート順設定ダイアログでエラーが発生しました。");
				}
			}
		}


		private void ApplyAutoSort(EquipmentGroupData group)
		{
			if (!group.AutoSortEnabled || group.SortOrder == null)
				return;

			// 一番上/最後に実行したほうが優先度が高くなるので逆順で
			for (int i = group.SortOrder.Count - 1; i >= 0; i--)
			{
				var order = group.SortOrder[i];
				ListSortDirection dir = order.Value;

				if (EquipView.Columns[order.Key].SortMode != DataGridViewColumnSortMode.NotSortable)
					EquipView.Sort(EquipView.Columns[order.Key], dir);
			}
		}

		private void MenuMember_AddToGroup_Click(object sender, EventArgs e)
		{
			using (var dialog = new DialogTextSelect("グループの選択", "追加するグループを選択してください：",
				KCDatabase.Instance.EquipmentGroup.EquipmentGroups.Values.ToArray()))
			{
				if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{
					var group = (EquipmentGroupData)dialog.SelectedItem;
					if (group != null)
					{
						group.AddInclusionFilter(GetSelectedShipID());

						if (group.ID == CurrentGroup.ID)
							ChangeEquipView(SelectedTab);
					}
				}
			}
		}

		private void MenuMember_CreateGroup_Click(object sender, EventArgs e)
		{
			var ships = GetSelectedShipID();
			if (ships.Count() == 0)
				return;

			using (var dialog = new DialogTextInput("グループの追加", "グループ名を入力してください："))
			{

				if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{

					var group = KCDatabase.Instance.EquipmentGroup.Add();

					group.Name = dialog.InputtedText.Trim();

					for (int i = 0; i < EquipView.Columns.Count; i++)
					{
						var newdata = new EquipmentGroupData.ViewColumnData(EquipView.Columns[i]);
						if (SelectedTab == null)
							newdata.Visible = true;     //初期状態では全行が非表示のため
						group.ViewColumns.Add(EquipView.Columns[i].Name, newdata);
					}

					group.AddInclusionFilter(ships);

					TabPanel.Controls.Add(CreateTabLabel(group.GroupID));
				}
			}
		}

		private void MenuMember_Exclude_Click(object sender, EventArgs e)
		{
			var group = CurrentGroup;
			if (group != null)
			{
				group.AddExclusionFilter(GetSelectedShipID());

				ChangeEquipView(SelectedTab);
			}
		}

		private static readonly string EquipmentCSVHeader = "装備ID,装備名,アイコン,カテゴリ,カテゴリ2,改修可,射程,火力,命中/対爆,回避/迎撃,爆装,雷装,索敵,対潜,対空,装甲,半径";

		private void MenuMember_CSVOutput_Click(object sender, EventArgs e)
		{
			IEnumerable<EquipmentDataMaster> equips;

			if (SelectedTab == null)
			{
				equips = KCDatabase.Instance.MasterEquipments.Values;
			}
			else
			{
				equips = EquipView.Rows.Cast<DataGridViewRow>().Select(r => KCDatabase.Instance.MasterEquipments[(int)r.Cells[EquipView_ID.Index].Value]);
			}

			if (SaveCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
					try
					{
						using (StreamWriter sw = new StreamWriter(SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding))
						{
							string header = EquipmentCSVHeader;
							sw.WriteLine(header);

							foreach (EquipmentDataMaster eq in equips.Where(s => s != null))
							{
									sw.WriteLine(string.Join(",",
										eq.EquipmentID,
										eq.Name,
										Constants.GetIconName(eq.IconType),
										eq.CategoryTypeInstance?.Name ?? eq.CategoryType.ToString(),
										eq.CategoryTypeInstance2?.Name ?? eq.CategoryType2.ToString(),
										eq.Improvable ? "○" : "-",
										Constants.GetRange(eq.Range),
										eq.Firepower,
										eq.Accuracy,
										eq.Evasion,
										eq.Bomber,
										eq.Torpedo,
										eq.LOS,
										eq.ASW,
										eq.AA,
										eq.Armor,
										eq.AircraftDistance
										));
								
							}
						}

						Utility.Logger.Add(2, "装備マスターデータグループ CSVを " + SaveCSVDialog.FileName + " に保存しました。");
					}
					catch (Exception ex)
					{
						Utility.ErrorReporter.SendErrorReport(ex, "装備マスターデータグループ CSV の出力に失敗しました。");
						MessageBox.Show("装備マスターデータグループ CSVの出力に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				
			}
		}

		#endregion

		#region タブ操作系

		private Point? _tempMouse = null;

		private void EquipView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			//ロードされる前やダブルクリックされたのがヘッダだったら何もしない
			if (EquipView.SelectedRows.Count != 0 && e.RowIndex != -1)
			{
				//ダブルクリックなので必ず1行選択になる
				int selectedId = GetSelectedShipID().First();
				new DialogAlbumMasterEquipment(selectedId).Show(Parent);

			}
		}

		void TabLabel_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				_tempMouse = TabPanel.PointToClient(e.Location);
			}
			else
			{
				_tempMouse = null;
			}
		}

		void TabLabel_MouseMove(object sender, MouseEventArgs e)
		{
			if (_tempMouse != null)
			{
				Rectangle move = new Rectangle(
					_tempMouse.Value.X - SystemInformation.DragSize.Width / 2,
					_tempMouse.Value.Y - SystemInformation.DragSize.Height / 2,
					SystemInformation.DragSize.Width,
					SystemInformation.DragSize.Height
					);

				if (!move.Contains(TabPanel.PointToClient(e.Location)))
				{
					TabPanel.DoDragDrop(sender, DragDropEffects.All);
					_tempMouse = null;
				}
			}
		}

		void TabLabel_MouseUp(object sender, MouseEventArgs e)
		{
			_tempMouse = null;
		}

		private void TabPanel_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ImageLabel)))
			{
				e.Effect = DragDropEffects.Move;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void TabPanel_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{

			//右クリックでキャンセル
			if ((e.KeyState & 2) != 0)
			{
				e.Action = DragAction.Cancel;
			}

		}

		private void TabPanel_DragDrop(object sender, DragEventArgs e)
		{

			//fixme:カッコカリ　範囲外にドロップすると端に行く

			Point mp = TabPanel.PointToClient(new Point(e.X, e.Y));

			var item = TabPanel.GetChildAtPoint(mp);

			int index = TabPanel.Controls.GetChildIndex(item, false);

			TabPanel.Controls.SetChildIndex((System.Windows.Forms.Control)e.Data.GetData(typeof(ImageLabel)), index);

			TabPanel.Invalidate();
		}

		#endregion

		private void MenuGroup_ShowStatusBar_CheckedChanged(object sender, EventArgs e)
		{
			StatusBar.Visible = MenuGroup_ShowStatusBar.Checked;
		}

		void SystemShuttingDown()
		{
			Utility.Configuration.Config.FormEquipmentGroup.AutoUpdate = MenuGroup_AutoUpdate.Checked;
			Utility.Configuration.Config.FormEquipmentGroup.ShowStatusBar = MenuGroup_ShowStatusBar.Checked;

			EquipmentGroupManager groups = KCDatabase.Instance.EquipmentGroup;

			List<ImageLabel> list = TabPanel.Controls.OfType<ImageLabel>().OrderBy(c => TabPanel.Controls.GetChildIndex(c)).ToList();

			for (int i = 0; i < list.Count; i++)
			{
				EquipmentGroupData group = groups[(int)list[i].Tag];
				if (group != null)
					group.GroupID = i + 1;
			}
		}

		private void FormEquipmentGroup_Resize(object sender, EventArgs e)
		{
			if (_splitterDistance != -1 && splitContainer1.Height > 0)
			{
				try
				{
					splitContainer1.SplitterDistance = _splitterDistance;
					_splitterDistance = -1;
				}
				catch (Exception)
				{
					// *ぷちっ*
				}
			}
		}
	}
}
