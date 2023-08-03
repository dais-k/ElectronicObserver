using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou
{

	public class slot_deprive : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;

			int setid = (int)data.api_ship_data.api_set_ship.api_id;
			int unsetid = (int)data.api_ship_data.api_unset_ship.api_id;
			bool isRemodeled = false;

			ShipData ship = db.Ships[setid];

			// 念のため
			if (!db.Ships.ContainsKey(setid))
			{
				var a = new ShipData();
				a.LoadFromResponse(APIName, data.api_ship_data.api_set_ship);
				db.Ships.Add(a);
				isRemodeled = true;

			}
			else
			{
				db.Ships[setid].LoadFromResponse(APIName, data.api_ship_data.api_set_ship);
			}


			if (!db.Ships.ContainsKey(unsetid))
			{
				var a = new ShipData();
				a.LoadFromResponse(APIName, data.api_ship_data.api_unset_ship);
				db.Ships.Add(a);

			}
			else
			{
				db.Ships[unsetid].LoadFromResponse(APIName, data.api_ship_data.api_unset_ship);
			}

			// 装備シナジー検出カッコカリ
			if (!isRemodeled)
			{
				int firepower = ship.FirepowerTotal - ship.FirepowerBase;
				int torpedo = ship.TorpedoTotal - ship.TorpedoBase;
				int aa = ship.AATotal - ship.AABase;
				int armor = ship.ArmorTotal - ship.ArmorBase;
				int asw = ship.ASWTotal - (ship.MasterShip.ASW.GetEstParameterMin(ship.Level) + ship.ASWModernized);
				int evasion = ship.EvasionTotal - ship.MasterShip.Evasion.GetEstParameterMin(ship.Level);
				int los = ship.LOSTotal - ship.MasterShip.LOS.GetEstParameterMin(ship.Level);
				int luck = ship.LuckTotal - ship.LuckBase;
				int range = ship.MasterShip.Range;

				foreach (var eq in ship.AllSlotInstanceMaster.Where(eq => eq != null))
				{
					firepower -= eq.Firepower;
					torpedo -= eq.Torpedo;
					aa -= eq.AA;
					armor -= eq.Armor;
					asw -= eq.ASW;
					evasion -= eq.Evasion;
					los -= eq.LOS;
					luck -= eq.Luck;
					range = Math.Max(range, eq.Range);
				}

				range = ship.Range - range;

				if (firepower != 0 ||
					torpedo != 0 ||
					aa != 0 ||
					armor != 0 ||
					asw != 0 ||
					evasion != 0 ||
					los != 0 ||
					luck != 0 ||
					range != 0)
				{
					var sb = new StringBuilder();
					sb.Append("装備ボーナスを検出しました：");

					var a = new List<string>();
					if (firepower != 0)
						a.Add($"火力{firepower:+#;-#;0}");
					if (torpedo != 0)
						a.Add($"雷装{torpedo:+#;-#;0}");
					if (aa != 0)
						a.Add($"対空{aa:+#;-#;0}");
					if (armor != 0)
						a.Add($"装甲{armor:+#;-#;0}");
					if (asw != 0)
						a.Add($"対潜{asw:+#;-#;0}");
					if (evasion != 0)
						a.Add($"回避{evasion:+#;-#;0}");
					if (los != 0)
						a.Add($"索敵{los:+#;-#;0}");
					if (luck != 0)
						a.Add($"運{luck:+#;-#;0}");
					if (range != 0)
						a.Add($"射程{range:+#;-#;0}");

					sb.Append(string.Join(", ", a));

					sb.AppendFormat(" ; {0} [{1}]",
						ship.NameWithLevel,
						string.Join(", ", ship.AllSlotInstance.Where(eq => eq != null).Select(eq => eq.NameWithLevel)));

					Utility.Logger.Add(2, sb.ToString());
				}
			}
			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_req_kaisou/slot_deprive";
	}

}
