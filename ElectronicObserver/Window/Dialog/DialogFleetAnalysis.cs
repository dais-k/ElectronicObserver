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
		public DialogFleetAnalysis()
		{
			InitializeComponent();
		}

		private void UpdateView()
		{
			var ships = KCDatabase.Instance.Ships.Values;
			var equipments = KCDatabase.Instance.Equipments.Values;
			var masterEquipments = KCDatabase.Instance.MasterEquipments;
			int masterCount = masterEquipments.Values.Count(eq => !eq.IsAbyssalEquipment);

			//var allCount = equipments.GroupBy(eq => eq.EquipmentID).ToDictionary(group => group.Key, group => group.Count());
			//var remainCount = new Dictionary<int, int>(allCount);

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
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[0].Value = maxRowNum;
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[1].Value = st;
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[2].Value = shipTypeCount[st] ;
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[3].Value = Math.Round(shipTypeLevelAvg[st]);
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[4].Value = shipTypeLevelMin[st];
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[5].Value = shipTypeLevelMax[st];
				dataGridView_ShipTypes.Rows[maxRowNum - 1].Cells[6].Value = shipTypeExpSum[st];
			}

			//並び変え：1列目(通番)の昇順
			DataGridViewColumn sortColumn = dataGridView_ShipTypes.Columns[0];
			dataGridView_ShipTypes.Sort(sortColumn, ListSortDirection.Ascending);

			//レーダーチャート
			for (int i = 0; i < dataGridView_ShipTypes.RowCount; i++)
			{
				//項目のラベル
				chart_ShipTypes.ChartAreas[0].AxisX.CustomLabels.Add(i, i, dataGridView_ShipTypes[1, i].Value.ToString());
				//項目の値
				chart_ShipTypes.Series[0].Points.Add(new DataPoint(0, Int32.Parse(dataGridView_ShipTypes[5, i].Value.ToString()))); //最高Lv
				chart_ShipTypes.Series[1].Points.Add(new DataPoint(0, Int32.Parse(dataGridView_ShipTypes[3, i].Value.ToString())));	//平均Lv
				chart_ShipTypes.Series[2].Points.Add(new DataPoint(0, Int32.Parse(dataGridView_ShipTypes[4, i].Value.ToString())));	//最小Lv
			}
		}

		private void DialogFleetAnalysis_Load(object sender, EventArgs e)
		{
			UpdateView();

			this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormResourceChart]);
		}

		private void SaveCSV_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var now = System.DateTime.Now;
			string savedir = Directory.GetCurrentDirectory() + "\\Record\\FleetAnalysis_"+ now.ToString("yyyyMMddHHmmss") + ".csv";

			StreamWriter sw = new StreamWriter(savedir, false, Encoding.Default);
			sw.Write("通番,艦種,隻数,平均Lv,最小Lv,最大Lv,取得経験値,");
			sw.WriteLine();
			for (int i = 0; i < dataGridView_ShipTypes.RowCount; i++)
			{
				for (int j = 0; j < dataGridView_ShipTypes.ColumnCount; j++)
				{
					sw.Write(dataGridView_ShipTypes[j, i].Value + ",");
				}
				sw.WriteLine();
			}
			sw.Close();

			MessageBox.Show("保存完了："+savedir);
		}

		private void Reload_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dataGridView_ShipTypes.Rows.Clear();
			chart_ShipTypes.ChartAreas[0].AxisX.CustomLabels.Clear();
			chart_ShipTypes.Series[0].Points.Clear();
			chart_ShipTypes.Series[1].Points.Clear();
			chart_ShipTypes.Series[2].Points.Clear();
			UpdateView();
		}
	}
}
