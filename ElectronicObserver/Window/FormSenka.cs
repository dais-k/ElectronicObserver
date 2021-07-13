using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ElectronicObserver.Observer;
using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Resource;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{
	public partial class FormSenka : DockContent
	{
		//各EOの戦果リスト
		public Dictionary<Tuple<int, int>, int> EOSenka = new Dictionary<Tuple<int, int>, int>()
		{
			{ Tuple.Create( 1, 5 ), 75},
			{ Tuple.Create( 1, 6 ), 75},
			{ Tuple.Create( 2, 5 ), 100},
			{ Tuple.Create( 3, 5 ), 150},
			{ Tuple.Create( 4, 5 ), 180},
			{ Tuple.Create( 5, 5 ), 200},
			{ Tuple.Create( 6, 5 ), 250},
		};

		public FormSenka(FormMain parent)
		{
			InitializeComponent();

			ConfigurationChanged();

			//アイコンは使いまわし
			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormExpChecker]);
		}

		private void FormSenka_Load(object sender, EventArgs e)
		{
			APIObserver o = APIObserver.Instance;

			//出撃戦果の更新は母港帰投時
			o.APIList["api_port/port"].ResponseReceived += Updated;

			//EO海域の戦果合計の更新は出撃海域選択画面に進んだとき
			o.APIList["api_get_member/mapinfo"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		void ConfigurationChanged()
		{
			Font = Utility.Configuration.Config.UI.MainFont;
		}

		void Updated(string apiname, dynamic data)
		{
			KCDatabase db = KCDatabase.Instance;

			switch (apiname)
			{
				case "api_port/port":
					var res1 = RecordManager.Instance.Resource.GetRecordPrevious();
					if (res1 != null)
					{
						int diff = db.Admiral.Exp - res1.HQExp;
						labelRecordPrevious.Text = string.Format("今回: +{0} exp. / 戦果 {1:n2}", diff, diff * 7 / 10000.0);
					}
					var res2 = RecordManager.Instance.Resource.GetRecordDay();
					if (res2 != null)
					{
						int diff = db.Admiral.Exp - res2.HQExp;
						labelRecordDay.Text = string.Format("今日: +{0} exp. / 戦果 {1:n2}", diff, diff * 7 / 10000.0);
					}
					var res3 = RecordManager.Instance.Resource.GetRecordMonth();
					if (res3 != null)
					{
						int diff = db.Admiral.Exp - res3.HQExp;
						labelRecordMonth.Text = string.Format("今月: +{0} exp. / 戦果 {1:n2}", diff, diff * 7 / 10000.0);
					}
					break;
				case "api_get_member/mapinfo":
					int senkaEOSum = 0;
					StringBuilder sb = new StringBuilder();

					foreach (var map in KCDatabase.Instance.MapInfo.Values)
					{
						int gaugeType = -1;
						int current = 0;
						int max = 0;

						if (map.RequiredDefeatedCount != -1 && map.CurrentDefeatedCount < map.RequiredDefeatedCount)
						{
							gaugeType = 1;
							current = map.CurrentDefeatedCount;
							max = map.RequiredDefeatedCount;
						}
						else if (map.MapHPMax > 0)
						{
							gaugeType = map.GaugeType;
							current = map.MapHPCurrent;
							max = map.MapHPMax;
						}

						if (EOSenka.ContainsKey(Tuple.Create(map.MapAreaID, map.MapInfoID)))
						{
							if (map.IsCleared)
							{
								senkaEOSum += EOSenka[Tuple.Create(map.MapAreaID, map.MapInfoID)];

								sb.AppendLine(string.Format("{0}-{1}{2}: {3}[攻略済]",
								map.MapAreaID,
								map.MapInfoID,
								map.EventDifficulty > 0 ? $" [{Constants.GetDifficulty(map.EventDifficulty)}]" : "",
								map.CurrentGaugeIndex > 0 ? $"#{map.CurrentGaugeIndex} " : ""));
							}
							else
							{
								sb.AppendLine(string.Format("{0}-{1}{2}: {3}{4} {5} / {6}{7}",
								map.MapAreaID,
								map.MapInfoID,
								map.EventDifficulty > 0 ? $" [{Constants.GetDifficulty(map.EventDifficulty)}]" : "",
								map.CurrentGaugeIndex > 0 ? $"#{map.CurrentGaugeIndex} " : "",
								gaugeType == 1 ? "撃破" : gaugeType == 2 ? "HP" : "TP",
								current,
								max,
								gaugeType == 1 ? " 回" : ""));
							}
						}
					}

					labelEOSum.Text = string.Format("EO合計: 戦果 {0}", senkaEOSum);
					richTextBoxEODetails.Text = sb.ToString();

					break;
				default:
					break;
			}
		}

		//レイアウトを覚えるために必要
		protected override string GetPersistString()
		{
			return "Senka";
		}
	}
}
