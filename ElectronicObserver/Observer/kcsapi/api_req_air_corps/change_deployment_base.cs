using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_air_corps
{
	public class change_deployment_base : APIBase
	{
		private int _currentArea;

		public override bool IsResponseSupported => true;

		public override bool IsRequestSupported => true;

		public override void OnRequestReceived(Dictionary<string, string> data)
		{
			// --- Store the current world for later
			_currentArea = int.Parse(data["api_area_id"]);

			base.OnRequestReceived(data);
		}

		public override void OnResponseReceived(dynamic data)
		{
			KCDatabase db = KCDatabase.Instance;

			foreach (var landBase in data.api_base_items)
			{
				// --- get the air corp id
				int baseId = BaseAirCorpsData.GetID(_currentArea, (int)(landBase?.api_rid));

				// --- Update the air corp
				db.BaseAirCorps[baseId].LoadFromResponse(APIName, landBase);
			}

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_req_air_corps/change_deployment_base";
	}
}
