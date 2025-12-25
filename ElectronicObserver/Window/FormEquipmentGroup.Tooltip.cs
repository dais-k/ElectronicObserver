using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window
{
	// データ抽出・格納のみを担当する partial 実装
	public partial class FormEquipmentGroup
	{
		// ホバー検出用（既存）
		private Timer _hoverTimer;
		private int _hoverRow = -1;
		private int _hoverCol = -1;

		// 現在表示対象の抽出済みデータ
		private ImprovementTooltipData _currentTooltipData;

		// フィールド名 "_tip_form" を "_tipForm" に統一
		private Form _tipForm;

		private const int HoverDelay = 400;

		private void InitializeImprovementTooltip()
		{
			if (_hoverTimer != null) return;

			_hoverTimer = new Timer { Interval = HoverDelay };
			_hoverTimer.Tick += HoverTimer_Tick;

			EquipView.CellMouseMove += EquipView_CellMouseMove;
			EquipView.CellMouseLeave += EquipView_CellMouseLeave;
			EquipView.MouseLeave += EquipView_MouseLeave;
		}

		private void DisposeImprovementTooltip()
		{
			if (_hoverTimer != null)
			{
				_hoverTimer.Tick -= HoverTimer_Tick;
				_hoverTimer.Stop();
				_hoverTimer.Dispose();
				_hoverTimer = null;
			}

			if (EquipView != null)
			{
				EquipView.CellMouseMove -= EquipView_CellMouseMove;
				EquipView.CellMouseLeave -= EquipView_CellMouseLeave;
				EquipView.MouseLeave -= EquipView_MouseLeave;
			}

			HideImprovementTip();
		}

		private void EquipView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				_hoverTimer?.Stop();
				_hoverRow = _hoverCol = -1;
				HideImprovementTip();
				return;
			}

			if (e.RowIndex != _hoverRow || e.ColumnIndex != _hoverCol)
			{
				_hoverRow = e.RowIndex;
				_hoverCol = e.ColumnIndex;

				HideImprovementTip();

				_hoverTimer?.Stop();
				_hoverTimer?.Start();
			}
		}

		private void EquipView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			_hoverTimer?.Stop();
			_hoverRow = _hoverCol = -1;
			HideImprovementTip();
		}

		private void EquipView_MouseLeave(object sender, EventArgs e)
		{
			_hoverTimer?.Stop();
			_hoverRow = _hoverCol = -1;
			HideImprovementTip();
		}

		private void HoverTimer_Tick(object sender, EventArgs e)
		{
			_hoverTimer?.Stop();

			if (_hoverRow < 0 || _hoverCol < 0) return;
			if (_hoverRow >= EquipView.Rows.Count || _hoverCol >= EquipView.Columns.Count) return;

			// ImproveShips 列のみ対象（必要なら列判定を変更）
			if (_hoverCol == EquipView_ImproveShips.Index)
			{
				// FormEquipmentGroup 側で表示可否を判定する
				if (this.CanShowImproveTipAt(_hoverRow, _hoverCol))
				{
					TryPrepareAndShowTooltip(_hoverRow, _hoverCol);
				}
				else
				{
					// 空セルならツールチップは出さない（既に閉じる処理は HoverTimer の外で行われているが念のため）
					_currentTooltipData = null;
					HideImprovementTip();
				}
			}
		}

		// データを抽出して _currentTooltipData に格納する（UI表示は別）
		private void TryPrepareAndShowTooltip(int rowIndex, int colIndex)
		{
			try
			{
				if (rowIndex < 0 || rowIndex >= EquipView.Rows.Count) return;

				var idObj = EquipView.Rows[rowIndex].Cells[EquipView_ID.Index].Value;
				if (!(idObj is int equipmentID)) return;

				if (!KCDatabase.Instance.MasterEquipments.TryGetValue(equipmentID, out var master) || master == null) return;

				// 各候補ごとのデータ化（要求どおり各文字列を作成して格納）
				var entries = new List<ImprovementEntry>();
				if (master.Improvements != null)
				{
					foreach (var imp in master.Improvements)
					{
						var entry = new ImprovementEntry
						{
							Upgrade = imp.Upgrade != null ? new List<string>(imp.Upgrade) : new List<string>(),
							Resource = imp.Resource, // ResourceBlock は参照で保持
						};

						// 特別処理: Upgrade が "false" の場合の指定値を設定する
						bool upgradeFlagFalse = imp.Upgrade != null && imp.Upgrade.Count > 0 && imp.Upgrade[0] == "false";

						// NameImp: "master.Name + " → " + Upgrade" （ただし "false" の場合は master.Name のみ）
						try
						{
							if (upgradeFlagFalse)
							{
								entry.NameImp = master.Name;
							}
							else
							{
								string upName = "";
								if (imp.Upgrade != null && imp.Upgrade.Count > 0)
								{
									var u0 = imp.Upgrade[0]; // string 型
									if (int.TryParse(u0, out int uid))
									{
										if (KCDatabase.Instance.MasterEquipments.TryGetValue(uid, out var upm) && upm != null)
											upName = upm.Name;
										else
											upName = $"ID:{uid}";
									}
									else if (!string.IsNullOrEmpty(u0))
									{
										if (u0.StartsWith("consumable_"))
										{
											var numPart = u0.Substring("consumable_".Length);
											if (int.TryParse(numPart, out int cid))
											{
												upName = Constants.GetImprovementItemName(cid);
											}
											else
											{
												upName = u0;
											}
										}
										else
										{
											upName = u0;
										}
									}
								}
								entry.NameImp = string.IsNullOrEmpty(upName) ? master.Name : master.Name + " → " + upName;
							}
						}
						catch
						{
							entry.NameImp = master.Name;
						}

						// TodayShip, NextShip
						try
						{
							entry.TodayShip = this.GetTodayImprovementNames(new List<EquipmentDataMaster.Improvement> { imp });
						}
						catch { entry.TodayShip = ""; }
						try
						{
							entry.NextShip = this.GetNextImprovementNamesCombined(new List<EquipmentDataMaster.Improvement> { imp });
						}
						catch { entry.NextShip = ""; }

						// BaseRes
						if (imp.Resource?.BaseResource != null)
						{
							var r = imp.Resource.BaseResource;
							entry.BaseRes = $"必要資材    燃料:{r.Mat1}  弾薬:{r.Mat2}  鋼材:{r.Mat3}  ボーキ:{r.Mat4}";
						}
						else
						{
							entry.BaseRes = "";
						}

						// ExtraRes strings and ExtraItem strings
						for (int i = 0; i < 3; i++)
						{
							var er = imp.Resource?.ExtraResources != null && imp.Resource.ExtraResources.Count > i
								? imp.Resource.ExtraResources[i]
								: null;

							string extraRes = "";
							string extraItems = "";

							if (er != null)
							{
								extraRes = $"開発: {er.Mat1} / {er.Mat2}   改修: {er.Mat3} / {er.Mat4}";

								// Build ExtraItem lines: each item -> name xNeedCount (所持数)
								if (er.Items != null && er.Items.Count > 0)
								{
									var lines = new List<string>();

									foreach (var it in er.Items.Take(4)) // 表示は先頭4個まで
									{
										string id = it.Id ?? "";
										string itemName = id;
										string haveSuffix = "";

										// consumable_
										if (id.StartsWith("consumable_"))
										{
											var numPart = id.Substring("consumable_".Length);
											if (int.TryParse(numPart, out int consId))
											{
												try
												{
													itemName = Constants.GetImprovementItemName(consId);
												}
												catch
												{
													itemName = $"item#{consId}";
												}
											}
											else
											{
												itemName = id;
											}
										}
										else if (int.TryParse(id, out int iid))
										{
											// 装備ID の場合はマスター名取得と所持数算出
											if (KCDatabase.Instance.MasterEquipments.TryGetValue(iid, out var masterEq) && masterEq != null)
											{
												itemName = masterEq.Name;
											}
											else
											{
												itemName = $"ID:{iid}";
											}
											// 所持数
											int owned = KCDatabase.Instance.Equipments.Values.Count(e => e != null && e.EquipmentID == iid);
											haveSuffix = $" (所持:{owned})";
										}
										else
										{
											// 非数値かつ非 consumable_: 表示そのまま
											itemName = id;
										}

										if (id == "-1")
											lines.Add($"-");
										else
											lines.Add($"{itemName}×{it.NeedCount}{haveSuffix}");
									}

									extraItems = string.Join(Environment.NewLine, lines);
								}
							}

							// assign to entry fields
							switch (i)
							{
								case 0:
									entry.ExtraRes1 = extraRes;
									entry.ExtraItem1 = extraItems;
									break;
								case 1:
									entry.ExtraRes2 = extraRes;
									entry.ExtraItem2 = extraItems;
									break;
								case 2:
									// 特別条件: Upgrade が "false" の場合は指定どおり上書きする
									if (upgradeFlagFalse)
									{
										entry.ExtraRes3 = "-";
										entry.ExtraItem3 = "";
									}
									else
									{
										entry.ExtraRes3 = extraRes;
										entry.ExtraItem3 = extraItems;
									}
									break;
							}
						}

						entries.Add(entry);
					}
				}

				// 格納
				_currentTooltipData = new ImprovementTooltipData
				{
					EquipmentID = equipmentID,
					MasterName = master.Name,
					Entries = entries
				};

				// UI 表示呼び出しは分離（ここでは _currentTooltipData が準備されたことを保証する）
				ShowPreparedTooltipAt(rowIndex, colIndex);
			}
			catch
			{
				_currentTooltipData = null;
			}
		}

		private void ShowPreparedTooltipAt(int rowIndex, int colIndex)
		{
			var cellRect = EquipView.GetCellDisplayRectangle(colIndex, rowIndex, true);
			var screen = EquipView.PointToScreen(new Point(cellRect.Right, cellRect.Bottom));

			HideImprovementTip();

			_tipForm = new ImprovementTipForm(_currentTooltipData);
			_tipForm.StartPosition = FormStartPosition.Manual;

			int x = screen.X - _tipForm.Width;
			if (x < 0) x = 0;
			int y = screen.Y + 4;
			_tipForm.Location = new Point(x, y);

			_tipForm.Show();
		}

		private void HideImprovementTip()
		{
			if (_tipForm != null && !_tipForm.IsDisposed)
			{
				try
				{
					_tipForm.Close();
					_tipForm.Dispose();
				}
				catch { }
			}
			_tipForm = null;
		}

		// 抽出データのモデル
		private class ImprovementTooltipData
		{
			public int EquipmentID { get; set; }
			public string MasterName { get; set; }
			public List<ImprovementEntry> Entries { get; set; } = new();
		}

		private class ImprovementEntry
		{
			// Upgrade は文字列リストに変更
			public List<string> Upgrade { get; set; } = new();
			// Resource は EquipmentDataMaster.ResourceBlock 型の参照保持
			public EquipmentDataMaster.ResourceBlock Resource { get; set; }

			// 追加出力フィールド
			public string NameImp { get; set; }
			public string TodayShip { get; set; }
			public string NextShip { get; set; }
			public string BaseRes { get; set; }
			public string ExtraRes1 { get; set; }
			public string ExtraRes2 { get; set; }
			public string ExtraRes3 { get; set; }
			public string ExtraItem1 { get; set; }
			public string ExtraItem2 { get; set; }
			public string ExtraItem3 { get; set; }
		}

		// UI 実装: エントリー毎にラベル + DataGridView(3x3) を組み立てるフォーム（右余白を詰めた版）
		private class ImprovementTipForm : Form
		{
			private readonly int _borderThickness = 1; // 枠線太さ（必要に応じて変更）
			private readonly Color _borderColor = Color.FromArgb(200, 120, 120, 120);

			public ImprovementTipForm(ImprovementTooltipData data)
			{
				FormBorderStyle = FormBorderStyle.None;
				ShowInTaskbar = false;
				StartPosition = FormStartPosition.Manual;
				BackColor = SystemColors.Window;
				TopMost = true;

				// ちらつき防止
				DoubleBuffered = true;

				AutoSize = true;
				AutoSizeMode = AutoSizeMode.GrowAndShrink;

				// フォーム内の余白を小さくして右側の余白を詰める
				Padding = new Padding(2 + _borderThickness, 2 + _borderThickness, 2 + _borderThickness, 2 + _borderThickness);

				var main = new FlowLayoutPanel
				{
					FlowDirection = FlowDirection.TopDown,
					AutoSize = true,
					WrapContents = false,
					BackColor = BackColor,
					Margin = new Padding(2),
					Padding = new Padding(2)
				};

				if (data == null || data.Entries.Count == 0)
				{
					var none = new Label
					{
						Text = "改修情報なし",
						AutoSize = true,
						Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 9f),
						BackColor = BackColor,
						Padding = new Padding(4)
					};
					main.Controls.Add(none);
					Controls.Add(main);
					return;
				}

				foreach (var entry in data.Entries)
				{
					// NameImp (フォント12)
					var nameLbl = new Label
					{
						Text = entry.NameImp ?? data.MasterName,
						AutoSize = true,
						Font = new Font("メイリオ", 11f, FontStyle.Regular),
						BackColor = BackColor,
						Padding = new Padding(0, 4, 2, 2),
						Margin = new Padding(0)
					};
					main.Controls.Add(nameLbl);

					if (!string.IsNullOrEmpty(entry.TodayShip))
					{
						var tLbl = new Label
						{
							Text = $"{entry.TodayShip}",
							AutoSize = true,
							Font = new Font("メイリオ", 9f),
							BackColor = BackColor,
							Padding = new Padding(2, 2, 2, 0),
							Margin = new Padding(0)
						};
						main.Controls.Add(tLbl);
					}

					if (!string.IsNullOrEmpty(entry.NextShip))
					{
						var nLbl = new Label
						{
							Text = $"{entry.NextShip}",
							AutoSize = true,
							Font = new Font("メイリオ", 9f),
							BackColor = BackColor,
							ForeColor = Color.FromArgb(150, 150, 150),
							Padding = new Padding(2, 0, 2, 0),
							Margin = new Padding(0)
						};
						main.Controls.Add(nLbl);
					}

					if (!string.IsNullOrEmpty(entry.BaseRes))
					{
						var bLbl = new Label
						{
							Text = entry.BaseRes,
							AutoSize = true,
							Font = new Font("メイリオ", 9f),
							BackColor = BackColor,
							Padding = new Padding(2, 2, 2, 2),
							Margin = new Padding(0)
						};
						main.Controls.Add(bLbl);
					}

					// DataGridView 構築とサイズ調整（改行表示対応、右余白を詰める）
					var dgv = new DataGridView
					{
						AutoSize = true,
						AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
						// 行高さは自動拡張させる
						AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
						RowHeadersVisible = false,
						ColumnHeadersVisible = false,
						AllowUserToAddRows = false,
						AllowUserToDeleteRows = false,
						AllowUserToResizeRows = false,
						AllowUserToResizeColumns = false,
						ReadOnly = true,
						BackgroundColor = BackColor,
						GridColor = BackColor,
						DefaultCellStyle = new DataGridViewCellStyle
						{
							WrapMode = DataGridViewTriState.True, // 折り返し有効
							Font = new Font("メイリオ", 9f),
							Alignment = DataGridViewContentAlignment.TopLeft,
							BackColor = BackColor,
							NullValue = ""
						},
						BorderStyle = BorderStyle.None,
						CellBorderStyle = DataGridViewCellBorderStyle.None,
						// 左マージンは少しだけ確保、右は 0 にして余白を減らす
						Margin = new Padding(0, 0, 0, 0),
						ScrollBars = ScrollBars.None,
						EnableHeadersVisualStyles = false
					};

					// 選択色無効化（青ハイライト対策）
					var selBack = dgv.DefaultCellStyle.BackColor;
					var selFore = dgv.DefaultCellStyle.ForeColor;
					dgv.DefaultCellStyle.SelectionBackColor = selBack;
					dgv.DefaultCellStyle.SelectionForeColor = selFore;
					dgv.RowsDefaultCellStyle.SelectionBackColor = selBack;
					dgv.RowsDefaultCellStyle.SelectionForeColor = selFore;
					dgv.SelectionChanged += (s, e) => { try { dgv.ClearSelection(); } catch { } };

					// 列定義
					dgv.Columns.Clear();
					dgv.Columns.Add(new DataGridViewTextBoxColumn()
					{
						Name = "colLabel",
						ReadOnly = true,
						SortMode = DataGridViewColumnSortMode.NotSortable,
						DefaultCellStyle = { Font = new Font("メイリオ", 9f, FontStyle.Bold), Alignment = DataGridViewContentAlignment.TopLeft, WrapMode = DataGridViewTriState.False }
					});
					dgv.Columns.Add(new DataGridViewTextBoxColumn()
					{
						Name = "colRes",
						ReadOnly = true,
						SortMode = DataGridViewColumnSortMode.NotSortable,
						DefaultCellStyle = { Alignment = DataGridViewContentAlignment.TopLeft, WrapMode = DataGridViewTriState.True }
					});
					dgv.Columns.Add(new DataGridViewTextBoxColumn()
					{
						Name = "colItems",
						ReadOnly = true,
						SortMode = DataGridViewColumnSortMode.NotSortable,
						DefaultCellStyle = { Alignment = DataGridViewContentAlignment.TopLeft, WrapMode = DataGridViewTriState.True }
					});

					foreach (DataGridViewColumn col in dgv.Columns)
					{
						col.DefaultCellStyle.SelectionBackColor = selBack;
						col.DefaultCellStyle.SelectionForeColor = selFore;
					}

					// 行追加
					string[] labels = new[] { "★0～★5", "★6～★9", "更新" };
					string[] res = new[] { entry.ExtraRes1 ?? "", entry.ExtraRes2 ?? "", entry.ExtraRes3 ?? "" };
					string[] items = new[] { entry.ExtraItem1 ?? "", entry.ExtraItem2 ?? "", entry.ExtraItem3 ?? "" };

					for (int i = 0; i < 3; i++)
					{
						dgv.Rows.Add(labels[i], res[i], items[i]);
						dgv.Rows[i].DefaultCellStyle.BackColor = BackColor;
						dgv.Rows[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
						dgv.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
					}

					// 行高さを自動調整（折り返しの計算を反映）
					try
					{
						dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
					}
					catch
					{
						// 安全に無視して続行
					}

					// 列幅は MeasureString の余白を減らして計算（右余白を詰める）
					using (var g = CreateGraphics())
					{
						int padding = 4; // 以前は 8。右余白を詰めるため小さくする。
						for (int col = 0; col < dgv.Columns.Count; col++)
						{
							int maxW = 0;
							for (int row = 0; row < dgv.Rows.Count; row++)
							{
								var value = dgv.Rows[row].Cells[col].Value?.ToString() ?? "";
								SizeF sz = g.MeasureString(value, dgv.DefaultCellStyle.Font);
								int w = (int)Math.Ceiling(sz.Width) + padding;
								if (w > maxW) maxW = w;
							}
							maxW = Math.Max(maxW, 30); // 最低幅を少し小さく
							dgv.Columns[col].Width = maxW;
						}
					}

					// 合計高さを行高合計で決定（折り返し反映済み）
					int totalHeight = dgv.ColumnHeadersHeight + dgv.Rows.Cast<DataGridViewRow>().Sum(r => r.Height);
					dgv.Height = Math.Min(600, totalHeight);

					// 合計幅（右の余白を詰めるため余分な +4 を削除）
					int totalWidth = dgv.Columns.Cast<DataGridViewColumn>().Sum(c => c.Width) + dgv.RowHeadersWidth;
					dgv.Width = Math.Min(600, totalWidth);

					// 選択解除
					dgv.ClearSelection();
					if (dgv.CurrentCell != null) dgv.CurrentCell = null;

					main.Controls.Add(dgv);
				}

				Controls.Add(main);
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);

				// 角丸なしの完全な矩形で枠を描画：四辺を塗りつぶす方式で確実に表示
				if (_borderThickness <= 0) return;

				var g = e.Graphics;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

				using (var brush = new SolidBrush(_borderColor))
				{
					int w = Width;
					int h = Height;
					int t = _borderThickness;

					// 上
					g.FillRectangle(brush, 0, 0, w, t);
					// 左
					g.FillRectangle(brush, 0, t, t, Math.Max(0, h - t * 2));
					// 右
					g.FillRectangle(brush, Math.Max(0, w - t), t, t, Math.Max(0, h - t * 2));
					// 下
					g.FillRectangle(brush, 0, Math.Max(0, h - t), w, t);
				}
			}
		}
	}
}