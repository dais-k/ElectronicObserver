using DynaJson;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
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

		private Regex _apiPattern;
		private string _currentAPIPath; 
		private string _LastAccessTime;
		private string _LastRequestReceivedTime;
		private string _Last4hoursIntervalUntil;
		private string _Last4hoursIntervalTo;
		private string _NowAccessTime;

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

			o.RequestReceived += RequestReceived;
			o.ResponseReceived += ResponseReceived;


			if (c.FormAccessTime.LastRequestReceivedTime != "")
				_LastRequestReceivedTime = c.FormAccessTime.LastRequestReceivedTime;
			else
				_LastRequestReceivedTime = DateTime.Now.ToString();

			_LastAccessTime = _LastRequestReceivedTime;
			_Last4hoursIntervalUntil = (c.FormAccessTime.Last4hoursIntervalUntil != "") ? c.FormAccessTime.Last4hoursIntervalUntil : "不明";
			_Last4hoursIntervalTo = (c.FormAccessTime.Last4hoursIntervalTo != "") ? c.FormAccessTime.Last4hoursIntervalTo : "不明";


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		void RequestReceived(string apiname, dynamic data)
		{
			if (_apiPattern != null && !_apiPattern.Match(apiname).Success)
				return;

			LoadRequest(apiname, data);
		}

		void ResponseReceived(string apiname, dynamic data)
		{
			if (_apiPattern != null && !_apiPattern.Match(apiname).Success)
				return;


			LoadResponse(apiname, data);
		}

		private void LoadRequest(string apiname, Dictionary<string, string> data)
		{
			_NowAccessTime = DateTime.Now.ToString();
			TimeViewer.Text = apiname + " : Request\r\n[" + _NowAccessTime + "]";
			_currentAPIPath = apiname;

		}

		void LoadResponse(string apiname, dynamic data)
		{
			var c = Utility.Configuration.Config;
			TimeSpan _TotalAccessTime;
			DateTime now = DateTime.Now;

			DateTime last = DateTime.Parse(_LastRequestReceivedTime);
			TimeSpan _AccessInterval = now - last;

			if (_AccessInterval.TotalHours >= 4)
			{
				_Last4hoursIntervalTo = _LastRequestReceivedTime;
				c.FormAccessTime.Last4hoursIntervalTo = _Last4hoursIntervalTo;
				_Last4hoursIntervalUntil = now.ToString();
				c.FormAccessTime.Last4hoursIntervalUntil = _Last4hoursIntervalUntil;
			}
			
			if (_Last4hoursIntervalUntil == "不明" || _Last4hoursIntervalTo == "不明")
				_TotalAccessTime = now - DateTime.Parse(_LastAccessTime);
			else
				_TotalAccessTime = now - DateTime.Parse(_Last4hoursIntervalUntil);

			TimeViewer.Text = (_currentAPIPath == apiname ? TimeViewer.Text += "\r\n\r\n" : "") + apiname + " : Response\r\n[" + now.ToString() + "]\r\n\r\n";
			TimeViewer.Text += (_TotalAccessTime.Days * 24 + _TotalAccessTime.Hours).ToString() + "時間" + _TotalAccessTime.Minutes.ToString() + "分" + _TotalAccessTime.Seconds.ToString() + "秒 連続アクセス中...";
			TimeViewer.Text += "\r\n" + "最後に艦これを4時間以上アクセスしなかった期間：\r\n[" + _Last4hoursIntervalTo + "～" + _Last4hoursIntervalUntil + "]";

			_LastRequestReceivedTime = now.ToString();
			c.FormAccessTime.LastRequestReceivedTime = _LastRequestReceivedTime;

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
