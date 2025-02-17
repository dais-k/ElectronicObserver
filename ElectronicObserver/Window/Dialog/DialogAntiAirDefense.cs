using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogAntiAirDefense : Form
	{

		private class AACutinComboBoxData
		{
			public readonly int Kind;
			public AACutinComboBoxData(int kind)
			{
				Kind = kind;
			}

			public override string ToString() => $"{Kind}: {Constants.GetAACutinKind(Kind)}";


			public static implicit operator int(AACutinComboBoxData data)
			{
				if (data == null)
					return -1;
				return data.Kind;
			}
		}

		private class FormationComboBoxData
		{
			public readonly int Formation;
			public FormationComboBoxData(int formation)
			{
				Formation = formation;
			}

			public override string ToString() => Constants.GetFormation(Formation);


			public static implicit operator int(FormationComboBoxData data)
			{
				if (data == null)
					return -1;
				return data.Formation;
			}
		}


		/// <summary>
		/// NumericUpDown から Value を正しく取得できないことがあるため、一旦これにキャッシュする
		/// </summary>
		/// <remarks>https://github.com/andanteyk/ElectronicObserver/pull/197</remarks>
		private int enemySlotCountValue;


		public DialogAntiAirDefense()
		{
			InitializeComponent();
			enemySlotCountValue = (int)EnemySlotCount.Value;
		}

		private void DialogAntiAirDefense_Load(object sender, EventArgs e)
		{

			if (!KCDatabase.Instance.Fleet.IsAvailable)
			{
				MessageBox.Show("艦隊データが読み込まれていません。\r\n艦これを起動してから開いてください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
				return;
			}

			if (FleetID.SelectedIndex == -1)
				FleetID.SelectedIndex = 0;
			Formation.SelectedIndex = 0;
			AAFireAvoidance.SelectedIndex = 0;
			UpdateAACutinKind(ShowAll.Checked);
			UpdateFormation();
 
			APIObserver o = APIObserver.Instance;

			o["api_req_hensei/change"].RequestReceived += Updated;
			o["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o["api_req_member/updatedeckname"].RequestReceived += Updated;
			o["api_req_kaisou/remodeling"].RequestReceived += Updated;
			o["api_req_hensei/combined"].RequestReceived += Updated;
			o["api_req_kaisou/open_exslot"].RequestReceived += Updated;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_get_member/ship2"].ResponseReceived += Updated;
			o["api_get_member/ndock"].ResponseReceived += Updated;
			o["api_req_kousyou/getship"].ResponseReceived += Updated;
			o["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o["api_get_member/ship3"].ResponseReceived += Updated;
			o["api_req_kaisou/powerup"].ResponseReceived += Updated;        //requestのほうは面倒なのでこちらでまとめてやる
			o["api_get_member/deck"].ResponseReceived += Updated;
			o["api_get_member/slot_item"].ResponseReceived += Updated;
			o["api_get_member/ship_deck"].ResponseReceived += Updated;
			o["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
			o["api_get_member/require_info"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;
			o["api_req_kaisou/marriage"].ResponseReceived += Updated;

			this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAntiAirDefense]);
		}

		private void DialogAntiAirDefense_FormClosed(object sender, FormClosedEventArgs e)
		{
			ResourceManager.DestroyIcon(Icon);
		}


		public void SetFleetID(int id)
		{
			FleetID.SelectedIndex = id - 1;
		}

		private void Updated_List()
		{

			ShipData[] ships = GetShips().ToArray();
			int formation = Formation.SelectedItem as FormationComboBoxData;
			int aaCutinKind = AACutinKind.SelectedItem as AACutinComboBoxData;
			int enemyAircraftCount = enemySlotCountValue;
			int enemyAvoidance = (AAFireAvoidance.SelectedIndex < 0)? 0 : AAFireAvoidance.SelectedIndex;
			double[] avoid1 = new double[5] { 1.0, 0.6, 0.6, 0.5, 0.5 };
			double[] avoid2 = new double[5] { 1.0, 1.0, 0.7, 0.7, 0.5 };
			double[] avoid3 = new double[5] { 1.0, 1.0, 0.6, 0.4, 0.4 };
			double[] avoid4 = new double[5] { 1.0, 1.0, 1.0, 0.5, 0.5 };

			// 加重対空値
			double[] adjustedAAs = ships.Select(s => s == null ? 0.0 : Math.Floor(Calculator.GetAdjustedAAValue(s) * avoid1[enemyAvoidance])).ToArray();

			// 艦隊防空値
			double adjustedFleetAA = Calculator.GetAdjustedFleetAAValue(ships, formation, avoid2[enemyAvoidance]);

			// 割合撃墜率
			double[] proportionalAAs = adjustedAAs.Select((val, i) => Calculator.GetProportionalAirDefense(val, IsCombined ? (i < 6 ? 1 : 2) : -1)).ToArray();

			// 割合撃墜数
			int[] shootDownProportional = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			   Calculator.GetProportionalShootDown(enemyAircraftCount, proportionalAAs[i])).ToArray();

			// 固定撃墜
			int[] shootDownfixedAAs = adjustedAAs.Select((val, i) => Calculator.GetFixedAirDefense(val, adjustedFleetAA, aaCutinKind, IsCombined ? (i < 6 ? 1 : 2) : -1)).ToArray();

			// 最低保証数
			int[] shootDownFailed = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			   Calculator.GetMinimumShootDownCount(avoid3[enemyAvoidance], avoid4[enemyAvoidance], aaCutinKind)).ToArray();

			// 両方成功撃墜数
			int[] shootDownBoth = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			   shootDownfixedAAs[i]+ shootDownProportional[i]+ shootDownFailed[i]).ToArray();

			// 噴進弾幕
			double[] aaRocketBarrageProbability = ships.Select(ship => Calculator.GetAARocketBarrageProbability(ship)).ToArray();


			ResultView.Rows.Clear();
			var rows = new DataGridViewRow[ships.Length];
			for (int i = 0; i < ships.Length; i++)
			{
				if (ships[i] == null)
					continue;

				rows[i] = new DataGridViewRow();
				rows[i].CreateCells(ResultView);

				rows[i].SetValues(
					ships[i].Name,
					ships[i].AATotal,
					adjustedAAs[i],
					proportionalAAs[i],
					shootDownProportional[i],
					shootDownfixedAAs[i],
					shootDownFailed[i],
					shootDownBoth[i],
					aaRocketBarrageProbability[i]);

			}
			ResultView.Rows.AddRange(rows.Where(r => r != null).ToArray());

			AdjustedFleetAA.Text = adjustedFleetAA.ToString("0.00");
			{
				var allShootDown = shootDownBoth.Concat(shootDownProportional).Concat(shootDownFailed);
				AnnihilationProbability.Text = (allShootDown.Count(i => i >= enemyAircraftCount) / Math.Max(ships.Count(s => s != null) * 4, 1.0)).ToString("p1");
			}
		}


		private IEnumerable<ShipData> GetShips()
		{
			if (FleetID.SelectedIndex < 4)
				return KCDatabase.Instance.Fleet[FleetID.SelectedIndex + 1].MembersWithoutEscaped;
			else
				return KCDatabase.Instance.Fleet[1].MembersWithoutEscaped.Concat(KCDatabase.Instance.Fleet[2].MembersWithoutEscaped);
		}

		private bool IsCombined => FleetID.SelectedIndex == 4;


		private void UpdateAACutinKind(bool showAll)
		{

			AACutinComboBoxData[] list;

			if (showAll)
			{

				int max = Calculator.AACutinFixedBonusA.Keys.Max();
				list = Enumerable.Range(0, max + 1).Select(kind => new AACutinComboBoxData(kind)).ToArray();

			}
			else
			{
				var aacutintypelist = GetShips().Where(s => s !=null).Select(s => Calculator2.GetAACutinKind(s.ShipID, s.AllSlotMaster.ToArray(), s.ID)).SelectMany(x => x).Distinct().ToArray();
				var aacutinlist = new List<int>();

				foreach (var aac in aacutintypelist)
				{
					aacutinlist.Add(aac);
				}

				if (aacutinlist.Count >= 2)
				{
					int[,] listtemp = new int[aacutinlist.Count, 2];
					int index = 0;
					foreach (var item in aacutinlist)
					{
						listtemp[index, 0] = item;
						listtemp[index, 1] = (Calculator.AACutinPriority.ContainsKey(item) ? Calculator.AACutinPriority[item] : 0);
						index++;
					}
					int numRows = listtemp.GetLength(0);
					int numCols = listtemp.GetLength(1);
					var sortedRows = Enumerable.Range(0, numRows)
						.OrderBy(row => listtemp[row, 1])
						.Select(row => Enumerable.Range(0, numCols)
						.Select(col => listtemp[row, col])
						.ToArray())
						.ToArray();
					aacutinlist.Clear();
					foreach (var n in sortedRows)
					{
						aacutinlist.Add(n.ElementAt(0));
					}
				}
				aacutinlist.Insert(0,0);
				list = aacutinlist.Select(kind => new AACutinComboBoxData(kind)).ToArray();

			}

			AACutinKind.Items.Clear();
			AACutinKind.Items.AddRange(list);
			AACutinKind.SelectedIndex = 0;
		}

		private void UpdateFormation()
		{
			var items = (IsCombined ? Enumerable.Range(11, 4) : Enumerable.Range(1, 6))
				.Select(i => new FormationComboBoxData(i)).ToArray();

			int selected = Formation.SelectedItem as FormationComboBoxData;
			int index = Array.FindIndex(items, item => item == selected);

			Formation.Items.Clear();
			Formation.Items.AddRange(items);
			Formation.SelectedIndex = Math.Max(index, 0);
		}


		private void ResultView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			if (e.ColumnIndex == ResultView_ShootDownBoth.Index ||
				e.ColumnIndex == ResultView_ShootDownProportional.Index ||
				e.ColumnIndex == ResultView_ShootDownFailed.Index)
			{

				int value = e.Value as int? ?? 0;
				int enemySlot = enemySlotCountValue;

				e.Value = string.Format("{0}", value);
				e.FormattingApplied = true;
				e.CellStyle.BackColor = e.CellStyle.SelectionBackColor =
					value >= enemySlot ? Color.MistyRose : SystemColors.Window;
			}

		}


		private void Updated(string apiname, dynamic data)
		{
			ToolTipInfo.SetToolTip(AACutinKind, null);
			Updated_List();
			UpdateAACutinKind(ShowAll.Checked);
			UpdateFormation();
		}


		private void FleetID_SelectedIndexChanged(object sender, EventArgs e)
		{
			Updated_List();
			UpdateAACutinKind(ShowAll.Checked);
			UpdateFormation();
		}

		private void Formation_SelectedIndexChanged(object sender, EventArgs e)
		{
			Updated_List();
		}

		private void AACutinKind_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToolTipInfo.SetToolTip(AACutinKind,Constants.GetAACutinKind(AACutinKind.SelectedItem as AACutinComboBoxData));
			Updated_List();
		}

		private void EnemySlotCount_ValueChanged(object sender, EventArgs e)
		{
			enemySlotCountValue = (int)EnemySlotCount.Value;
			Updated_List();
		}

		private void ShowAll_CheckedChanged(object sender, EventArgs e)
		{
			ToolTipInfo.SetToolTip(AACutinKind, null);
			UpdateAACutinKind(ShowAll.Checked);
		}

		private void AAFireAvoidance_CheckedChanged(object sender, EventArgs e)
		{
			Updated_List();
		}

	}
}
