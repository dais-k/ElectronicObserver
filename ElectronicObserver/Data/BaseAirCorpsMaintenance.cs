using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	/// <summary>
	/// 基地航空隊の整備レベルデータを扱います。
	/// </summary>
	public class BaseAirCorpsMaintenance : APIWrapper, IIdentifiable
	{
		/// <summary>
		/// 飛行場が存在する海域ID
		/// </summary>
		public int MapAreaID => RawData.api_area_id() ? (int)RawData.api_area_id : -1;

		/// <summary>
		/// 整備レベル
		/// </summary>
		public int MaintenanceLevel => RawData.api_maintenance_level()? (int) RawData.api_maintenance_level : 0;

		public static int GetID(dynamic response)
			=> response.api_area_id() ? (int)response.api_area_id : -1;

		public int ID => MapAreaID;
	}
}
