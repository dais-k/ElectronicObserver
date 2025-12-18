using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using System;
using System.Linq;

namespace ElectronicObserver.Notifier
{

	public class NotifierConditionRepair : NotifierBase
	{

		// 通知済みフラグ
		private bool processedFlag15 = true;
		private bool processedFlag30 = true;

		public NotifierConditionRepair()
			: base()
		{
			Initialize();
		}

		public NotifierConditionRepair(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
			: base(config)
		{
			Initialize();
		}

		private void Initialize()
		{
			DialogData.Title = "母港給糧艦システム";

			// タイマ更新イベントに接続（既存）
			SystemEvents.UpdateTimerTick += UpdateTimerTick;

			// 母港に戻ったらフラグをクリアして再通知可能にする
			APIObserver o = APIObserver.Instance;
			o["api_port/port"].ResponseReceived += ClearFlag;
		}

		// api_port/port 受信で再通知可能にする
		void ClearFlag(string apiname, dynamic data)
		{
			processedFlag15 = false;
			processedFlag30 = false;
		}

		protected override void UpdateTimerTick()
		{
			var fleets = KCDatabase.Instance.Fleet;

			// 既に通知済みなら何もしない
			if (processedFlag15 && processedFlag30) return;

			// タイマがセットされていて、野崎など該当艦が存在するか確認
			if (fleets.ConditionRepairingTimer > DateTime.MinValue &&
				fleets.Fleets.Values.Any(f => f.IsConditionRepairedShip))
			{
				// 15分経過を判定 (AccelInterval を考慮)
				if (!processedFlag15 && (DateTime.Now - fleets.ConditionRepairingTimer).TotalMilliseconds + AccelInterval >= 15 * 60 * 1000)
				{
					DialogData.Message = "母港給糧艦システムの開始から15分が経過しました。";
					Notify();
					processedFlag15 = true;
				}
				if (processedFlag15 && (DateTime.Now - fleets.ConditionRepairingTimer).TotalMilliseconds + AccelInterval >= 30 * 60 * 1000)
				{
					DialogData.Message = "母港給糧艦システムの開始から30分が経過しました。";
					Notify();
					processedFlag30 = true;
				}
			}
		}

		public override void Notify()
		{
			base.Notify();
		}

	}
}