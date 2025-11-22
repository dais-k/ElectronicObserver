using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
	
	/// <summary>
	/// 装備グループのデータを管理します。ShipGroupManager と同様の基本機能を提供します。
	/// </summary>
	[DataContract(Name = "EquipmentGroupManager")]
	public sealed class EquipmentGroupManager : DataStorage
	{

		public const string DefaultFilePath = @"Settings\EquipmentGroups.xml";


		/// <summary>
		/// 装備グループリスト
		/// </summary>
		[IgnoreDataMember]
		public IDDictionary<EquipmentGroupData> EquipmentGroups { get; private set; }


		[DataMember]
		public IEnumerable<EquipmentGroupData> EquipmentGroupsSerializer
		{
			get { return EquipmentGroups.Values.OrderBy(g => g.ID); }
			set { EquipmentGroups = new IDDictionary<EquipmentGroupData>(value); }
		}

		public EquipmentGroupManager()
		{
			Initialize();
		}


		public override void Initialize()
		{
			EquipmentGroups = new IDDictionary<EquipmentGroupData>();
		}

		public EquipmentGroupData this[int index] => EquipmentGroups[index];

		public EquipmentGroupData Add()
		{
			int key = GetUniqueID();
			var group = new EquipmentGroupData(key);
			EquipmentGroups.Add(group);
			return group;
		}

		public int GetUniqueID()
		{
			return EquipmentGroups.Count > 0 ? EquipmentGroups.Keys.Max() + 1 : 1;
		}

		public EquipmentGroupManager Load()
		{
			ResourceManager.CopyFromArchive(DefaultFilePath.Replace("\\", "/"), DefaultFilePath, true, false);
			return (EquipmentGroupManager)Load(DefaultFilePath);
		}

		public void Save()
		{
			var dir = Path.GetDirectoryName(DefaultFilePath);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			Save(DefaultFilePath);
		}
	}

}
