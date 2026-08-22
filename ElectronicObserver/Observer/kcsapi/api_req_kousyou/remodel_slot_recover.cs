using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou
{

	public class remodel_slot_recover : APIBase
	{

		private int _equipmentID;

		public override bool IsRequestSupported => true;

		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			_equipmentID = int.Parse(data["api_slot_id"]);

			base.OnRequestReceived(data);
		}

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;

			//db.Material.LoadFromResponse(APIName, data.api_after_material);


			if (data.api_after_slot())
			{   //改修成功時のみ存在
				EquipmentData eq = db.Equipments[(int)data.api_after_slot.api_id];
				if (eq != null)
				{
					eq.LoadFromResponse(APIName, data.api_after_slot);

					if (Utility.Configuration.Config.Log.ShowSpoiler)
						Utility.Logger.Add(2, string.Format("{0} の改修値のリセットに成功しました。", eq.Name));

					db.UseItems[104].Count--;
				}

			}
			else if (Utility.Configuration.Config.Log.ShowSpoiler)
			{
				Utility.Logger.Add(2, string.Format("{0} の改修値のリセットに失敗しました。", db.Equipments[_equipmentID].NameWithLevel));
			}


			base.OnResponseReceived((object)data);
		}


		public override string APIName => "api_req_kousyou/remodel_slot_recover";
	}


}
