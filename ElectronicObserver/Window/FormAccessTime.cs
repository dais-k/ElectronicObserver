using DynaJson;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
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
using System.Windows.Forms.DataVisualization.Charting;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{
	public partial class FormAccessTime : DockContent
	{

		private string _currentAPIPath; 
		private DateTime _LastAccessTime;
		private DateTime _LastRequestReceivedTime;
		private string _Last4hoursIntervalUntil;
		private string _Last4hoursIntervalTo;

		public FormAccessTime(FormMain parent)
		{
			InitializeComponent();

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAccessTimer]);
		}

		private void FormAccessTime_Load(object sender, EventArgs e)
		{

			var c = Utility.Configuration.Config; 
			var o = APIObserver.Instance;
			DateTime dateTime;

			o.RequestReceived += RequestReceived;
			o.ResponseReceived += ResponseReceived;


			if (c.FormAccessTime.LastRequestReceivedTime != "" && DateTime.TryParse(c.FormAccessTime.LastRequestReceivedTime, out dateTime))
				_LastRequestReceivedTime = dateTime;
			else
				_LastRequestReceivedTime = DateTime.Now;

			_LastAccessTime = _LastRequestReceivedTime;
			_Last4hoursIntervalUntil = (c.FormAccessTime.Last4hoursIntervalUntil != "" && DateTime.TryParse(c.FormAccessTime.Last4hoursIntervalUntil, out dateTime)) ? c.FormAccessTime.Last4hoursIntervalUntil : "不明";
			_Last4hoursIntervalTo = (c.FormAccessTime.Last4hoursIntervalTo != "" && DateTime.TryParse(c.FormAccessTime.Last4hoursIntervalTo, out dateTime)) ? c.FormAccessTime.Last4hoursIntervalTo : "不明";


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		void RequestReceived(string apiname, dynamic data)
		{
			LoadRequest(apiname);
		}

		void ResponseReceived(string apiname, dynamic data)
		{
			LoadResponse(apiname);
		}

		private void LoadRequest(string apiname)
		{
			TimeViewer.Text = apiname + " : Request\r\n[" + DateTime.Now.ToString() + "]";
			_currentAPIPath = apiname;

		}

		void LoadResponse(string apiname)
		{
			var c = Utility.Configuration.Config;
			TimeSpan _TotalAccessTime;
			DateTime now = DateTime.Now;

			DateTime last =_LastRequestReceivedTime;
			TimeSpan _AccessInterval = now - last;

			if (_AccessInterval.TotalHours >= 4)
			{
				_Last4hoursIntervalTo = DateTimeHelper.TimeToCSVString(_LastRequestReceivedTime);
				c.FormAccessTime.Last4hoursIntervalTo = _Last4hoursIntervalTo;
				_Last4hoursIntervalUntil = DateTimeHelper.TimeToCSVString(now);
				c.FormAccessTime.Last4hoursIntervalUntil = _Last4hoursIntervalUntil;
			}
			
			if (_Last4hoursIntervalUntil == "不明" || _Last4hoursIntervalTo == "不明")
				_TotalAccessTime = now -_LastAccessTime;
			else
				_TotalAccessTime = now - DateTime.Parse(_Last4hoursIntervalUntil);

			TimeViewer.Text = (_currentAPIPath == apiname ? TimeViewer.Text += "\r\n\r\n" : "") + apiname + " : Response\r\n[" + now.ToString() + "]\r\n\r\n";
			TimeViewer.Text += (_TotalAccessTime.Days * 24 + _TotalAccessTime.Hours).ToString() + "時間" + _TotalAccessTime.Minutes.ToString() + "分" + _TotalAccessTime.Seconds.ToString() + "秒 連続アクセス中...";
			TimeViewer.Text += "\r\n" + "最後に艦これを4時間以上アクセスしなかった期間：\r\n[" + _Last4hoursIntervalTo + "～" + _Last4hoursIntervalUntil + "]";

			_LastRequestReceivedTime = now;
			c.FormAccessTime.LastRequestReceivedTime = DateTimeHelper.TimeToCSVString(_LastRequestReceivedTime);

		}

		void ConfigurationChanged()
		{
			var c = Utility.Configuration.Config;
			Font = TimeViewer.Font = c.UI.MainFont;
		}

		protected override string GetPersistString()
		{
			return "AccessTime";
		}
	}
}
