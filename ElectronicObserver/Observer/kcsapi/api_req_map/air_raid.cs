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
			CompassData compassData = KCDatabase.Instance.Battle.Compass;
			//NOTE：
			//敵機が全滅して攻撃が基地にまったく届かなかった場合、減らされた基地HPの情報自体が存在しない
			//敵機が全滅せずにたまたま攻撃が1つも当たらなかった場合、情報自体はあるがすべて0
			//
			//丙丁だと空襲が1波、乙だと2波、甲だと3波となっていて
			//その分だけapi_destruction_battleの配列要素が存在するため、存在する分をすべて表示する
			int max = ((DynaJson.JsonObject)data.api_destruction_battle).Length;
			for (int i = 0;i < max; i++)
			{
				int damage = 0; 
				
				if (data.api_destruction_battle[i].api_air_base_attack.api_stage3())
				{
					foreach (int dmg in (int[])data.api_destruction_battle[i].api_air_base_attack.api_stage3.api_fdam)
					{
						damage += dmg;
					}
				}
				Utility.Logger.Add
				(
					2,
					string.Format
					(
						"{0}-{1}-{2} で基地に超重爆空襲を受けました。( {3}/{4}# {5}, 被ダメージ合計: {6}, {7} )",
						compassData.RawData.api_maparea_id,
						compassData.RawData.api_mapinfo_no,
						compassData.RawData.api_no,
						i+1,
						max,
						Constants.GetHeavyAirRaidButtonResult(_success),
						damage,
						Constants.GetAirRaidDamage((int)data.api_destruction_battle[i].api_lost_kind)
					)
				);
			}

			base.OnResponseReceived((object)data);
		}

		public override bool IsRequestSupported => true;

		public override string APIName => "api_req_map/air_raid";
	}
}
