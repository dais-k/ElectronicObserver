using ElectronicObserver.Data.EquipmentGroup;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ElectronicObserver.Data
{
	/// <summary>
	/// 装備グループのデータを保持します。
	/// ShipGroupData と同様の機能を装備（EquipmentDataMaster）向けに実装しています。
	/// </summary>
	[DataContract(Name = "EquipmentGroupData")]
	[DebuggerDisplay("[{GroupID}] : {Name} ({Members.Count} equips)")]
	public sealed class EquipmentGroupData : DataStorage, IIdentifiable, ICloneable
	{
		[DataContract(Name = "ViewColumnData")]
		public class ViewColumnData : ICloneable
		{
			/// <summary>
			/// 列名
			/// </summary>
			[DataMember]
			public string Name { get; set; }

			/// <summary>
			/// 幅
			/// </summary>
			[DataMember]
			public int Width { get; set; }

			/// <summary>
			/// 表示される順番
			/// </summary>
			[DataMember]
			public int DisplayIndex { get; set; }

			/// <summary>
			/// 可視かどうか
			/// </summary>
			[DataMember]
			public bool Visible { get; set; }

			/// <summary>
			/// 自動幅調整を行うか
			/// </summary>
			[DataMember]
			public bool AutoSize { get; set; }

			
			public ViewColumnData(string name)
			{
				Name = name;
			}
 
			public ViewColumnData(string name, int width, int displayIndex, bool visible, bool autoSize)
			{
				Name = name;
				Width = width;
				DisplayIndex = displayIndex;
				Visible = visible;
				AutoSize = autoSize;
			}

			public ViewColumnData(DataGridViewColumn column)
			{
				FromColumn(column);
			}

			/// <summary>
			/// 現在の設定を、列に対して適用します。
			/// </summary>
			/// <param name="column">対象となる列。</param>
			public void ToColumn(DataGridViewColumn column)
			{
				if (column.Name != Name) throw new ArgumentException("設定する列と Name が異なります。");
				column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
				column.Width = Width;
				column.DisplayIndex = DisplayIndex;
				column.Visible = Visible;
				column.AutoSizeMode = AutoSize ? System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader : System.Windows.Forms.DataGridViewAutoSizeColumnMode.NotSet;
			}

			/// <summary>
			/// 現在の列の状態から、設定を生成します。
			/// </summary>
			/// <param name="column">対象となる列。</param>
			/// <returns>このインスタンス自身を返します。</returns>
			public ViewColumnData FromColumn(DataGridViewColumn column)
			{
				Name = column.Name;
				Width = column.Width;
				DisplayIndex = column.DisplayIndex;
				Visible = column.Visible;
				AutoSize = column.AutoSizeMode == System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
				return this;
			}

			public ViewColumnData Clone()
			{
				return (ViewColumnData)MemberwiseClone();
			}

			object ICloneable.Clone()
			{
				return Clone();
			}
		}

		[DataMember] public int GroupID { get; internal set; }
		[DataMember] public string Name { get; set; }

		[IgnoreDataMember] public Dictionary<string, ViewColumnData> ViewColumns { get; set; }
		[DataMember]
		private IEnumerable<ViewColumnData> ViewColumnsSerializer
		{
			get => ViewColumns.Values;
			set => ViewColumns = value.ToDictionary(v => v.Name);
		}

		[DataMember] public int ScrollLockColumnCount { get; set; }

		[IgnoreDataMember] public List<KeyValuePair<string, ListSortDirection>> SortOrder { get; set; }
		[DataMember]
		private List<SerializableKeyValuePair<string, ListSortDirection>> SortOrderSerializer
		{
			get => SortOrder?.Select(s => new SerializableKeyValuePair<string, ListSortDirection>(s)).ToList();
			set => SortOrder = value?.Select(s => new KeyValuePair<string, ListSortDirection>(s.Key, s.Value)).ToList();
		}

		[DataMember] public bool AutoSortEnabled { get; set; }

		// 式フィルタ（Ship 用 ExpressionManager と互換性がないため未使用／将来拡張用）
		[DataMember] public EqExpressionManager Expressions { get; set; }

		[IgnoreDataMember] public List<int> InclusionFilter { get; set; }
		[DataMember] private SerializableList<int> InclusionFilterSerializer { get => InclusionFilter; set => InclusionFilter = value; }

		[IgnoreDataMember] public List<int> ExclusionFilter { get; set; }
		[DataMember] private SerializableList<int> ExclusionFilterSerializer { get => ExclusionFilter; set => ExclusionFilter = value; }

		[IgnoreDataMember] public List<int> Members { get; private set; }
		[IgnoreDataMember] public IEnumerable<EquipmentDataMaster> MembersInstance => Members.Select(id => KCDatabase.Instance.MasterEquipments.ContainsKey(id) ? KCDatabase.Instance.MasterEquipments[id] : null);

		[DataMember] private SerializableList<int> MembersSerializer { get => Members; set => Members = value; }

		public EquipmentGroupData(int groupID)
		{
			Initialize();
			GroupID = groupID;
		}

		public override void Initialize()
		{
			GroupID = -1;
			ViewColumns = new Dictionary<string, ViewColumnData>();
			Name = "no title";
			ScrollLockColumnCount = 0;
			AutoSortEnabled = true;
			SortOrder = new List<KeyValuePair<string, ListSortDirection>>();
			Expressions = null;
			InclusionFilter = new List<int>();
			ExclusionFilter = new List<int>();
			Members = new List<int>();
		}

		// ラッパ型: 式が ".MasterEquipment.xxx" を期待しているため、MasterEquipment を公開する簡易型を作る
		private class MasterEquipWrapper
		{
			public EquipmentDataMaster MasterEquipment { get; }
			public MasterEquipWrapper(EquipmentDataMaster m) { MasterEquipment = m; }
		}

		/// <summary>
		/// Inclusion/Exclusion フィルタに基づいて Members を更新します。
		/// ShipGroupData の Expressions 相当は現在未実装（将来拡張）。
		/// </summary>
		public void UpdateMembers(IEnumerable<int> previousOrder = null)
		{
			if (InclusionFilter == null) InclusionFilter = new List<int>();
			if (ExclusionFilter == null) ExclusionFilter = new List<int>();

			ValidateFilter();

			IEnumerable<int> newdataIds;

			bool useExpressions = Expressions != null && Expressions.Expressions != null && Expressions.Expressions.Count > 0;

			if (!useExpressions)
			{
				// 既存挙動: InclusionFilter のみを結果とする
				newdataIds = InclusionFilter.Except(ExclusionFilter);
			}
			else
			{
				// Expressions がある場合は MasterEquipments を式で評価して結果集合を得る
				try
				{
					// パラメータは MasterEquipWrapper 型（.MasterEquipment を辿れる）
					var paramex = System.Linq.Expressions.Expression.Parameter(typeof(MasterEquipWrapper), "e");
					System.Linq.Expressions.Expression ex = null;

					// 各 EqExpressionList を外部条件(AND/OR)に従って結合する
					foreach (var exlist in Expressions.Expressions)
					{
						if (!exlist.Enabled) continue;

						var listExpr = exlist.Compile(paramex);
						if (ex == null)
						{
							ex = listExpr;
						}
						else
						{
							if (exlist.ExternalAnd)
								ex = System.Linq.Expressions.Expression.AndAlso(ex, listExpr);
							else
								ex = System.Linq.Expressions.Expression.OrElse(ex, listExpr);
						}
					}

					if (ex == null)
						ex = System.Linq.Expressions.Expression.Constant(true, typeof(bool));

					var lambda = System.Linq.Expressions.Expression.Lambda<Func<MasterEquipWrapper, bool>>(ex, paramex).Compile();

					var matches = KCDatabase.Instance.MasterEquipments.Values
						.Where(me => me != null && lambda(new MasterEquipWrapper(me)))
						.Select(me => me.EquipmentID);

					// 式で見つかったものに InclusionFilter を合算し、ExclusionFilter を除外
					newdataIds = matches.Except(ExclusionFilter).Distinct();
				}
				catch
				{
					// 万一式評価で失敗したら従来挙動にフォールバック
					newdataIds = InclusionFilter.Except(ExclusionFilter);
				}
			}

			IEnumerable<int> prev = (previousOrder != null && previousOrder.Any()) ? previousOrder : (Members ?? new List<int>());

			// 前の並び順を保ちつつ新規を追加
			Members = prev.Except(prev.Except(newdataIds)).Union(newdataIds).ToList();
		}

		public void AddInclusionFilter(IEnumerable<int> list)
		{
			InclusionFilter = InclusionFilter.Union(list).ToList();
			ExclusionFilter = ExclusionFilter.Except(list).ToList();
		}

		public void AddExclusionFilter(IEnumerable<int> list)
		{
			InclusionFilter = InclusionFilter.Except(list).ToList();
			ExclusionFilter = ExclusionFilter.Union(list).ToList();
		}

		public void ValidateFilter()
		{
			if (KCDatabase.Instance.MasterEquipments != null && KCDatabase.Instance.MasterEquipments.Count > 0)
			{
				var keys = KCDatabase.Instance.MasterEquipments.Keys;
				InclusionFilter = InclusionFilter.Intersect(keys).Distinct().ToList();
				ExclusionFilter = ExclusionFilter.Intersect(keys).Distinct().ToList();
			}
		}

		public int ID => GroupID;
		public override string ToString() => Name;

		public EquipmentGroupData Clone()
		{
			var clone = (EquipmentGroupData)MemberwiseClone();
			clone.GroupID = -1;
			clone.ViewColumns = ViewColumns.Select(p => p.Value.Clone()).ToDictionary(p => p.Name);
			clone.SortOrder = new List<KeyValuePair<string, ListSortDirection>>(SortOrder);
			// Expressions は深いコピーする（ダイアログで編集しても元データに影響を与えないため）
			clone.Expressions = Expressions?.Clone();
			clone.InclusionFilter = new List<int>(InclusionFilter);
			clone.ExclusionFilter = new List<int>(ExclusionFilter);
			clone.Members = new List<int>(Members);
			return clone;
		}

		object ICloneable.Clone() => Clone();
	}
}
