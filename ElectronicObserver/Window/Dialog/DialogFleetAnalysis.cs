using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogFleetAnalysis : Form
	{
		Point? prevPosition = null; // グラフ上の位置（グラフデータ表示用）
		ToolTip tooltip = new ToolTip(); // ツールチップ（グラフデータ表示用）

		public DialogFleetAnalysis()
		{
			InitializeComponent();
		}

		private string GetLevelChartAxisString(int num)
		{
			string retStr = "";
			switch (num)
			{
				case 0:
					retStr = "2-39";
					break;
				case 1:
					retStr = "40-59";
					break;
				case 2:
					retStr = "60-79";
					break;
				case 3:
					retStr = "80-99";
					break;
				case 4:
					retStr = "100-127";
					break;
				case 5:
					retStr = "128-";
					break;
				default:
					break;
			}
			return retStr;
		}

		private void UpdateLevelChart()
		{
			//積み上げ棒グラフ
			for (int i = 0; i < dataGridView_Level.RowCount; i++)
			{
				//Seriesを追加する
				chart_Level.Series.Add(dataGridView_Level[1, i].Value.ToString());
				chart_Level.Series[i].ChartType = SeriesChartType.StackedColumn;
				//DataPoint設定
				for (int j = 2; j < dataGridView_Level.ColumnCount; j++)
				{
					chart_Level.Series[i].Points.AddXY(GetLevelChartAxisString(j - 2), Int32.Parse(dataGridView_Level[j, i].Value.ToString()));
					int maxPointNum = chart_Level.Series[i].Points.Count;
					chart_Level.Series[i].Points[maxPointNum - 1].ToolTip = dataGridView_Level[1, i].Value.ToString() + ":" + dataGridView_Level[j, i].Value.ToString();
				}
			}
		}

		private void UpdateShipTypeChart()
		{
			//レーダーチャート
			for (int i = 0; i < dataGridView_ShipTypes.RowCount; i++)
			{
				//項目のラベル
				chart_ShipTypes.ChartAreas[0].AxisX.CustomLabels.Add(i, i, dataGridView_ShipTypes[1, i].Value.ToString());
				//項目の値
				chart_ShipTypes.Series[0].Points.Add(new DataPoint(0, Int32.Parse(dataGridView_ShipTypes[5, i].Value.ToString()))); //最大Lv
				chart_ShipTypes.Series[1].Points.Add(new DataPoint(0, Int32.Parse(dataGridView_ShipTypes[3, i].Value.ToString()))); //平均Lv
				chart_ShipTypes.Series[2].Points.Add(new DataPoint(0, Int32.Parse(dataGridView_ShipTypes[4, i].Value.ToString()))); //最小Lv
			}
		}

		private void UpdateView()
		{
			//鎮守府の艦隊データを取得
			var ships = KCDatabase.Instance.Ships.Values;

			//################################
			//# 艦種別
			//################################
			//Lv1でない艦の数を数える
			var shipTypeCount = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Count(s => s.Level != 1));
			//Lv1でない艦の平均レベルを算出する
			var shipTypeLevelAvg = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Where(s => s.Level != 1).Average(s => s.Level));
			//Lv1でない艦の最小レベルを抽出する
			var shipTypeLevelMin = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Where(s => s.Level != 1).Min(s => s.Level));
			//Lv1でない艦の最大レベルを抽出する
			var shipTypeLevelMax = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Where(s => s.Level != 1).Max(s => s.Level));
			//Lv1でない艦の取得経験値合計を算出する
			var shipTypeExpSum = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Where(s => s.Level != 1).Sum(s => s.ExpTotal));

			foreach (var st in shipTypeCount.Keys)
			{
				//Console.WriteLine(st+ "|" + shipTypeCount[st]);
				dataGridView_ShipTypes.Rows.Add();
				int maxRowNum = dataGridView_ShipTypes.Rows.Count;
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[0].Value = maxRowNum; //デフォルトソートで困るので連番を入れておく(艦種のIDではない)
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[1].Value = st;
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[2].Value = shipTypeCount[st];
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[3].Value = Math.Round(shipTypeLevelAvg[st]);
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[4].Value = shipTypeLevelMin[st];
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[5].Value = shipTypeLevelMax[st];
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[6].Value = shipTypeExpSum[st];
			}

			//並び変え：1列目(通番)の昇順
			DataGridViewColumn sortColumnShipType = dataGridView_ShipTypes.Columns[0];
			dataGridView_ShipTypes.Sort(sortColumnShipType, ListSortDirection.Ascending);

			UpdateShipTypeChart();

			//########################################
			//# レベル帯別
			//########################################
			//2-39
			var shipTypeLevelRange1 = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Count(s => s.Level >= 2 && s.Level <= 39));
			//40-59
			var shipTypeLevelRange2 = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Count(s => s.Level >= 40 && s.Level <= 59));
			//60-79
			var shipTypeLevelRange3 = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Count(s => s.Level >= 60 && s.Level <= 79));
			//80-99
			var shipTypeLevelRange4 = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Count(s => s.Level >= 80 && s.Level <= 99));
			//100-127
			var shipTypeLevelRange5 = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Count(s => s.Level >= 100 && s.Level <= 128));
			//128-
			var shipTypeLevelRange6 = ships.GroupBy(s => s.MasterShip.ShipTypeName).ToDictionary(group => group.Key, group => group.Count(s => s.Level >= 128));

			foreach (var st in shipTypeLevelRange1.Keys)
			{
				dataGridView_Level.Rows.Add();
				int maxRowNum = dataGridView_Level.Rows.Count;
				dataGridView_Level.Rows[maxRowNum - 1].Cells[0].Value = maxRowNum; //デフォルトソートで困るので連番を入れておく
				dataGridView_Level.Rows[maxRowNum - 1].Cells[1].Value = st;
				dataGridView_Level.Rows[maxRowNum - 1].Cells[2].Value = shipTypeLevelRange1[st];
				dataGridView_Level.Rows[maxRowNum - 1].Cells[3].Value = shipTypeLevelRange2[st];
				dataGridView_Level.Rows[maxRowNum - 1].Cells[4].Value = shipTypeLevelRange3[st];
				dataGridView_Level.Rows[maxRowNum - 1].Cells[5].Value = shipTypeLevelRange4[st];
				dataGridView_Level.Rows[maxRowNum - 1].Cells[6].Value = shipTypeLevelRange5[st];
				dataGridView_Level.Rows[maxRowNum - 1].Cells[7].Value = shipTypeLevelRange6[st];
			}

			//並び変え：1列目(区分)の昇順
			DataGridViewColumn sortColumnLevel = dataGridView_Level.Columns[0];
			dataGridView_Level.Sort(sortColumnLevel, ListSortDirection.Ascending);

			UpdateLevelChart();
		}

		private void DialogFleetAnalysis_Load(object sender, EventArgs e)
		{
			FleetAnalysis_menuStrip.ShowItemToolTips = true;

			UpdateView();

			this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormResourceChart]);
		}

		private void SaveCSV_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var now = System.DateTime.Now;
			string dirNameParent = Directory.GetCurrentDirectory() + "\\FleetAnalysis";
			string dirNameDate = dirNameParent + "\\" + now.ToString("yyyyMMdd_HHmmss");

			//ディレクトリ作成
			Directory.CreateDirectory(dirNameParent);
			Directory.CreateDirectory(dirNameDate);

			//艦種別
			string savedir1 = dirNameDate + "\\ShipType.csv";
			StreamWriter sw1 = new StreamWriter(savedir1, false, Encoding.Default);
			sw1.Write("通番,艦種,隻数,平均Lv,最小Lv,最大Lv,取得経験値,");
			sw1.WriteLine();
			for (int i = 0; i < dataGridView_ShipTypes.RowCount; i++)
			{
				for (int j = 0; j < dataGridView_ShipTypes.ColumnCount; j++)
				{
					sw1.Write(dataGridView_ShipTypes[j, i].Value + ",");
				}
				sw1.WriteLine();
			}
			sw1.Close();

			//レベル帯別
			string savedir2 = dirNameDate + "\\Level.csv";
			StreamWriter sw2 = new StreamWriter(savedir2, false, Encoding.Default);
			sw2.Write("区分,艦種,0-39,40-59,60-79,80-99,100-127,128-,");
			sw2.WriteLine();
			for (int i = 0; i < dataGridView_Level.RowCount; i++)
			{
				for (int j = 0; j < dataGridView_Level.ColumnCount; j++)
				{
					sw2.Write(dataGridView_Level[j, i].Value + ",");
				}
				sw2.WriteLine();
			}
			sw2.Close();

			MessageBox.Show("保存完了\n" + savedir1 + "\n" + savedir2);
		}

		private void ClearData()
		{
			dataGridView_ShipTypes.Rows.Clear();
			chart_ShipTypes.ChartAreas[0].AxisX.CustomLabels.Clear();
			chart_ShipTypes.Series[0].Points.Clear();
			chart_ShipTypes.Series[1].Points.Clear();
			chart_ShipTypes.Series[2].Points.Clear();

			dataGridView_Level.Rows.Clear();
			chart_Level.Series.Clear();
		}

		private void Reload_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClearData();

			UpdateView();

			this.Text = "艦隊分析";
		}

		private void ShowLabelSwitch()
		{
			int count = 0;

			//いったん初期化
			chart_ShipTypes.ChartAreas[0].AxisY.LabelStyle.Enabled = true;
			for (int i = 0; i < chart_ShipTypes.Series.Count; i++)
			{
				for (int j = 0; j < chart_ShipTypes.Series[i].Points.Count; j++)
				{
					chart_ShipTypes.Series[i].Points[j].IsValueShownAsLabel = false;
				}
			}

			//有効になっているチェックを数える
			for (int i = 0; i < chart_ShipTypes.Series.Count; i++)
			{
				if (chart_ShipTypes.Series[i].Enabled)
				{
					count++;
				}
			}

			//1項目だけのときは値をラベル表示する
			if(count == 1)
			{
				chart_ShipTypes.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
				for(int i = 0; i < chart_ShipTypes.Series.Count; i++)
				{
					if (chart_ShipTypes.Series[i].Enabled)
					{
						for (int j = 0; j < chart_ShipTypes.Series[i].Points.Count; j++)
						{
							chart_ShipTypes.Series[i].Points[j].IsValueShownAsLabel = true;
						}
					}
				}
			}
		}
		private void checkBox_LvMax_CheckedChanged(object sender, EventArgs e)
		{
			chart_ShipTypes.Series[0].Enabled = checkBox_LvMax.Checked;
			ShowLabelSwitch();
		}

		private void checkBox_LvAvg_CheckedChanged(object sender, EventArgs e)
		{
			chart_ShipTypes.Series[1].Enabled = checkBox_LvAvg.Checked;
			ShowLabelSwitch();
		}

		private void checkBox_LvMin_CheckedChanged(object sender, EventArgs e)
		{
			chart_ShipTypes.Series[2].Enabled = checkBox_LvMin.Checked;
			ShowLabelSwitch();
		}

		private void ReadToGridDataView(DataGridView dataGridView, string fn)
		{
			StreamReader reader = new StreamReader(fn, Encoding.GetEncoding("shift_jis"));
			while (reader.Peek() >= 0)
			{
				string[] cols = reader.ReadLine().Split(',');
				if(cols[0] == "" || cols[0] == "区分" || cols[0] == "通番")
				{
					//最初の行はスキップ
					continue;
				}
				dataGridView.Rows.Add();
				int maxRowNum = dataGridView.Rows.Count;
				for (int i = 0;i < cols.Length-1;i++)
				{
					if(i == 1)
					{
						//艦種は文字列のまま
						dataGridView.Rows[maxRowNum - 1].Cells[i].Value = cols[i];
					}
					else
					{
						//他は数値
						dataGridView.Rows[maxRowNum - 1].Cells[i].Value = Int32.Parse(cols[i]);
					}
				}
			}
			reader.Close();

			//並び変え：1列目の昇順
			DataGridViewColumn sortColumnLevel = dataGridView.Columns[0];
			dataGridView.Sort(sortColumnLevel, ListSortDirection.Ascending);
		}

		private void ReadCSV_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string dirPath = "";

			ClearData();

			//フォルダ選択
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "フォルダを選択してください。";
			openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
			openFileDialog.FileName = "SelectFolder";
			openFileDialog.Filter = "Folder|.";
			openFileDialog.CheckFileExists = false;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				dirPath = Path.GetDirectoryName(openFileDialog.FileName);
			}

			// CSVファイルの読み込み、反映
			if (dirPath != "")
			{
				string fileShipTypePath = dirPath + "\\ShipType.csv";
				string fileLevelPath = dirPath + "\\Level.csv";

				ReadToGridDataView(dataGridView_ShipTypes, fileShipTypePath);
				ReadToGridDataView(dataGridView_Level, fileLevelPath);

				UpdateShipTypeChart();
				UpdateLevelChart();

				this.Text = "艦隊分析("+Path.GetFileName(dirPath)+")";
			}
		}
	}
}
