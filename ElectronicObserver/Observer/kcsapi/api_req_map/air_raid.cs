using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_map
{
	public class air_raid : APIBase
	{
		private int _success;

		public override void OnRequestReceived(Dictionary<string, string> data)
		{
			_success = int.Parse(data["api_scc"]);

			base.OnRequestReceived(data);
		}

		public override void OnResponseReceived(dynamic data)
		{
			Utility.Logger.Add(
				2, 
				string.Format("超重爆基地空襲 [{0}]", 
					Constants.GetHeavyAirRaidButtonResult(_success)
				)
			);
			base.OnResponseReceived((object)data);
		}

		public override bool IsRequestSupported => true;

		public override string APIName => "api_req_map/air_raid";
	}
}
