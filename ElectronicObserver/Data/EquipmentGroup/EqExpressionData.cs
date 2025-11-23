using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ElectronicObserver.Data;

namespace ElectronicObserver.Data.EquipmentGroup
{

	/// <summary>
	/// 装備フィルタの式データ
	/// </summary>
	[DataContract(Name = "EqExpressionData")]
	public class EqExpressionData : ICloneable
	{

		public enum ExpressionOperator
		{
			Equal,
			NotEqual,
			LessThan,
			LessEqual,
			GreaterThan,
			GreaterEqual,
			Contains,
			NotContains,
			BeginWith,
			NotBeginWith,
			EndWith,
			NotEndWith,
			ArrayContains,
			ArrayNotContains,
		}


		[DataMember]
		public string LeftOperand { get; set; }

		[DataMember]
		public ExpressionOperator Operator { get; set; }

		[DataMember]
		public object RightOperand { get; set; }


		[DataMember]
		public bool Enabled { get; set; }


		[IgnoreDataMember]
		private static readonly Regex regex_index = new Regex(@"\.(?<name>\w+)(\[(?<index>\d+?)\])?", RegexOptions.Compiled);

		[IgnoreDataMember]
		public static readonly Dictionary<string, string> LeftOperandNameTable = new Dictionary<string, string>() {
			{ ".MasterEquipment.EquipmentID", "装備ID" },
			{ ".MasterEquipment.IconType", "アイコン" },
			{ ".MasterEquipment.Name", "装備名" },
			{ ".MasterEquipment.CategoryTypeInstance.Name", "カテゴリ" },
			{ ".MasterEquipment.CategoryTypeInstance2.Name", "カテゴリ2" },
			{ ".MasterEquipment.Improvable", "改修可能" },
			{ ".MasterEquipment.Range", "射程" },
			{ ".MasterEquipment.Firepower", "火力" },
			{ ".MasterEquipment.Accuracy", "命中" },
			{ ".MasterEquipment.Evasion", "回避" },
			{ ".MasterEquipment.Bomber", "爆装" },
			{ ".MasterEquipment.Torpedo", "雷装" },
			{ ".MasterEquipment.LOS", "索敵" },
			{ ".MasterEquipment.ASW", "対潜" },
			{ ".MasterEquipment.AA", "対空" },
			{ ".MasterEquipment.Armor", "装甲" },
			{ ".MasterEquipment.AircraftDistance", "戦闘行動半径" },
			{ ".MasterEquipment.IsAbyssalEquipment", "深海装備" },
			{ ".MasterEquipment.IsAircraftOnlyAirbase", "基地航空隊専用" },
			{ ".MasterEquipment.IsOwned", "所持している" },
			{ ".MasterEquipment.ImprovableToday", "本日改修可能" },
		};

		private static Dictionary<string, Type> ExpressionTypeTable = new Dictionary<string, Type>();
		

		[IgnoreDataMember]
		public static readonly Dictionary<ExpressionOperator, string> OperatorNameTable = new Dictionary<ExpressionOperator, string>() {
			{ ExpressionOperator.Equal, "と等しい" },
			{ ExpressionOperator.NotEqual, "と等しくない" },
			{ ExpressionOperator.LessThan, "より小さい" },
			{ ExpressionOperator.LessEqual, "以下" },
			{ ExpressionOperator.GreaterThan, "より大きい" },
			{ ExpressionOperator.GreaterEqual, "以上" },
			{ ExpressionOperator.Contains, "を含む" },
			{ ExpressionOperator.NotContains, "を含まない" },
			{ ExpressionOperator.BeginWith, "から始まる" },
			{ ExpressionOperator.NotBeginWith, "から始まらない" },
			{ ExpressionOperator.EndWith, "で終わる" },
			{ ExpressionOperator.NotEndWith, "で終わらない" },
			{ ExpressionOperator.ArrayContains, "を含む" },
			{ ExpressionOperator.ArrayNotContains, "を含まない" },

		};



		public EqExpressionData()
		{
			Enabled = true;
		}

		public EqExpressionData(string left, ExpressionOperator ope, object right)
			: this()
		{
			LeftOperand = left;
			Operator = ope;
			RightOperand = right;
		}


		public Expression Compile(ParameterExpression paramex)
		{

			Expression memberex = null;

			// 特別な左辺の処理
			if (LeftOperand == ".MasterEquipment.IsOwned" || LeftOperand == ".MasterEquipment.ImprovableToday")
			{
				// paramex の MasterEquipment プロパティを渡してヘルパーで判定する:
				// EqExpressionDataHelpers.IsOwned(paramex.MasterEquipment)
				// EqExpressionDataHelpers.IsImprovableToday(paramex.MasterEquipment)
				var methodName = LeftOperand == ".MasterEquipment.IsOwned" ? nameof(EqExpressionDataHelpers.IsOwned) : nameof(EqExpressionDataHelpers.IsImprovableToday);
				memberex = Expression.Call(
					typeof(EqExpressionDataHelpers).GetMethod(methodName),
					Expression.PropertyOrField(paramex, "MasterEquipment")
				);

				// この memberex は bool 型扱いにするため以降の memberType 設定は下で行う
			}
			else
			{

				Match match = regex_index.Match(LeftOperand);
				if (match.Success)
				{

					do
					{

						if (memberex == null)
						{
							memberex = Expression.PropertyOrField(paramex, match.Groups["name"].Value);
						}
						else
						{
							memberex = Expression.PropertyOrField(memberex, match.Groups["name"].Value);
						}

						if (int.TryParse(match.Groups["index"].Value, out int index))
						{
							memberex = Expression.Property(memberex, "Item", Expression.Constant(index, typeof(int)));
						}

					} while ((match = match.NextMatch()).Success);

				}
				else
				{
					memberex = Expression.PropertyOrField(paramex, LeftOperand);
				}
			}

			// member の最終型を取得（enum や IEnumerable<> の判定に使う）
			Type memberType = memberex.Type;

			// enum は基底型に変換して比較する（基底型は int 以外の可能性もある）
			if (memberType.IsEnum)
			{
				var underlying = Enum.GetUnderlyingType(memberType);
				memberex = Expression.Convert(memberex, underlying);
				memberType = underlying;
			}

			// constex を member 型に合わせて作成（可能なら変換する）
			Expression constex;
			if (RightOperand == null)
			{
				constex = Expression.Constant(null, memberType.IsValueType ? typeof(object) : memberType);
			}
			else
			{
				try
				{
					// Try convert RightOperand to memberType
					var converted = Convert.ChangeType(RightOperand, memberType);
					constex = Expression.Constant(converted, memberType);
				}
				catch
				{
					// Fallback: keep original runtime type
					constex = Expression.Constant(RightOperand, RightOperand.GetType());
				}
			}

			Expression condex;
			switch (Operator)
			{
				case ExpressionOperator.Equal:
					condex = Expression.Equal(memberex, constex);
					break;
				case ExpressionOperator.NotEqual:
					condex = Expression.NotEqual(memberex, constex);
					break;
				case ExpressionOperator.LessThan:
					condex = Expression.LessThan(memberex, constex);
					break;
				case ExpressionOperator.LessEqual:
					condex = Expression.LessThanOrEqual(memberex, constex);
					break;
				case ExpressionOperator.GreaterThan:
					condex = Expression.GreaterThan(memberex, constex);
					break;
				case ExpressionOperator.GreaterEqual:
					condex = Expression.GreaterThanOrEqual(memberex, constex);
					break;
				case ExpressionOperator.Contains:
					condex = Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotContains:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.BeginWith:
					condex = Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotBeginWith:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.EndWith:
					condex = Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotEndWith:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.ArrayContains:
					{
						// member が string の場合は string.Contains
						if (memberex.Type == typeof(string))
						{
							var sval = RightOperand == null ? "" : RightOperand.ToString();
							var constStr = Expression.Constant(sval, typeof(string));
							condex = Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constStr);
						}
						else
						{
							// 要素型を探す（配列、ジェネリック引数、IEnumerable<T> インターフェイス）
							Type elemType = memberex.Type.GetElementType();
							if (elemType == null)
							{
								var ga = memberex.Type.GetGenericArguments();
								if (ga != null && ga.Length > 0)
									elemType = ga[0];
							}
							if (elemType == null)
							{
								var ie = memberex.Type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
								if (ie != null)
									elemType = ie.GetGenericArguments()[0];
							}
							if (elemType == null)
							{
								condex = Expression.Constant(false);
								break;
							}

							// constex を要素型に合わせる
							if (RightOperand == null || !elemType.IsInstanceOfType(RightOperand))
							{
								try
								{
									var converted = Convert.ChangeType(RightOperand, elemType);
									constex = Expression.Constant(converted, elemType);
								}
								catch
								{
									constex = Expression.Constant(RightOperand, RightOperand?.GetType() ?? typeof(object));
								}
							}
							else
							{
								constex = Expression.Constant(RightOperand, elemType);
							}

							condex = Expression.Call(typeof(Enumerable), "Contains", new Type[] { elemType }, memberex, constex);
						}
					}
					break;
				case ExpressionOperator.ArrayNotContains:
					{
						if (memberex.Type == typeof(string))
						{
							var sval = RightOperand == null ? "" : RightOperand.ToString();
							var constStr = Expression.Constant(sval, typeof(string));
							condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constStr));
						}
						else
						{
							Type elemType = memberex.Type.GetElementType();
							if (elemType == null)
							{
								var ga = memberex.Type.GetGenericArguments();
								if (ga != null && ga.Length > 0)
									elemType = ga[0];
							}
							if (elemType == null)
							{
								var ie = memberex.Type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
								if (ie != null)
									elemType = ie.GetGenericArguments()[0];
							}
							if (elemType == null)
							{
								condex = Expression.Constant(true);
								break;
							}

							if (RightOperand == null || !elemType.IsInstanceOfType(RightOperand))
							{
								try
								{
									var converted = Convert.ChangeType(RightOperand, elemType);
									constex = Expression.Constant(converted, elemType);
								}
								catch
								{
									constex = Expression.Constant(RightOperand, RightOperand?.GetType() ?? typeof(object));
								}
							}
							else
							{
								constex = Expression.Constant(RightOperand, elemType);
							}

							condex = Expression.Not(Expression.Call(typeof(Enumerable), "Contains", new Type[] { elemType }, memberex, constex));
						}
					}
					break;

				default:
					throw new NotImplementedException();
			}

			return condex;
		}



		public static Type GetLeftOperandType(string left)
		{

			// 特別な左辺の型を明示
			if (left == ".MasterEquipment.IsOwned")
				return typeof(bool);

			if (left == ".MasterEquipment.ImprovableToday")
				return typeof(bool);

			if (ExpressionTypeTable.ContainsKey(left))
			{
				return ExpressionTypeTable[left];

			}
			else if (KCDatabase.Instance.MasterEquipments.Count > 0 || KCDatabase.Instance.Equipments.Count > 0)
			{

				// Use a sample object that matches the parameter type used in Compile (EquipmentData).
				// Many LeftOperand strings start with ".MasterEquipment", so the root must be EquipmentData
				// (which has a MasterEquipment property). If no EquipmentData exists, fall back to MasterEquipments.
				object obj = null;

				if (KCDatabase.Instance.Equipments.Count > 0)
					obj = KCDatabase.Instance.Equipments.Values.First();
				else if (KCDatabase.Instance.MasterEquipments.Count > 0)
					obj = KCDatabase.Instance.MasterEquipments.Values.First();
				else
					return null;

				Match match = regex_index.Match(left);
				if (match.Success)
				{

					do
					{

						if (int.TryParse(match.Groups["index"].Value, out int index))
						{
							obj = ((dynamic)obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null))[index];
						}
						else
						{
							object obj2 = obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null);
							if (obj2 == null)
							{   // プロパティはあるけど null -> 型情報だけ取りたい
								var prop = obj.GetType().GetProperty(match.Groups["name"].Value);
								if (prop != null)
								{
									var type = prop.PropertyType;
									ExpressionTypeTable.Add(left, type);
									return type;
								}
								else
								{
									// property not found
									return null;
								}
							}
							else
							{
								obj = obj2;
							}
						}

					} while (obj != null && (match = match.NextMatch()).Success);


					if (obj != null)
					{
						ExpressionTypeTable.Add(left, obj.GetType());
						return obj.GetType();
					}
				}

			}

			return null;
		}

		public Type GetLeftOperandType()
		{
			return GetLeftOperandType(LeftOperand);
		}



		public override string ToString() => $"{LeftOperandToString()} は {RightOperandToString()} {OperatorToString()}";



		/// <summary>
		/// 左辺値の文字列表現を求めます。
		/// </summary>
		public string LeftOperandToString()
		{
			if (LeftOperandNameTable.ContainsKey(LeftOperand))
				return LeftOperandNameTable[LeftOperand];
			else
				return LeftOperand;
		}

		/// <summary>
		/// 演算子の文字列表現を求めます。
		/// </summary>
		public string OperatorToString()
		{
			return OperatorNameTable[Operator];
		}

		/// <summary>
		/// 右辺値の文字列表現を求めます。
		/// </summary>
		public string RightOperandToString()
		{

			if (LeftOperand == ".MasterEquipment.EquipmentID")
			{
				var equip = KCDatabase.Instance.MasterEquipments[(int)RightOperand];
				if (equip != null)
					return $"{equip.EquipmentID} ({equip.Name})";
				else
					return $"{(int)RightOperand} (未所持)";

			}
			else if (LeftOperand == ".MasterEquipment.IconType")
			{
				if (RightOperand is int)
				{
					int icon = (int)RightOperand;
					string name = Constants.GetIconName(icon);
					return $"{name}";
				}
				else
				{
					return RightOperand?.ToString() ?? "(未指定)";
				}
			}
			else if (LeftOperand == ".MasterEquipment.CategoryTypeInstance.Name")
			{
				if (RightOperand is int)
				{
					var cat = KCDatabase.Instance.EquipmentTypes[(int)RightOperand];
					if (cat != null)
						return cat.Name;
					else
						return $"{(int)RightOperand} (未定義)";
				}
				else
				{
					return RightOperand?.ToString() ?? "(未指定)";
				}
			}
			else if (LeftOperand == ".MasterEquipment.CategoryTypeInstance2.Name")
			{
				if (RightOperand is int)
				{
					var cat = KCDatabase.Instance.EquipmentTypes[(int)RightOperand];
					if (cat != null)
						return cat.Name;
					else
						return $"{(int)RightOperand} (未定義)";
				}
				else
				{
					return RightOperand?.ToString() ?? "(未指定)";
				}
			}
			else if (LeftOperand == ".MasterEquipment.Range")
			{
				return Constants.GetRange((int)RightOperand);

			}
			else if (RightOperand is bool)
			{
				return ((bool)RightOperand) ? "○" : "×";

			}
			else
			{
				return RightOperand.ToString();

			}

		}


		public EqExpressionData Clone()
		{
			var clone = MemberwiseClone();      //checkme: 右辺値に参照型を含む場合死ぬ
			return (EqExpressionData)clone;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}


	/// <summary>
	/// EqExpressionData 用の補助メソッド（式木で直接呼び出せる static メソッドをここに置く）
	/// </summary>
	internal static class EqExpressionDataHelpers
	{
		/// <summary>
		/// 指定された MasterEquip が所持されているかを返す。
		/// （KCDatabase.Instance.Equipments を参照して MasterEquipment.EquipmentID を持つインスタンスが存在するかを判定）
		/// </summary>
		public static bool IsOwned(EquipmentDataMaster master)
		{
			if (master == null) return false;
			// Equipments: Dictionary&lt;int, EquipmentData&gt; の Values を走査して装備IDが一致するものがあるか
			return KCDatabase.Instance.Equipments.Values.Any(e => e != null && e.EquipmentID == master.EquipmentID);
		}

		/// <summary>
		/// 指定された MasterEquip が「本日」改修担当のある改修情報を持つかを返す。
		/// FormEquipmentGroup の表示ロジックに合わせ、-1 を含む改善はスキップする。
		/// </summary>
		public static bool IsImprovableToday(EquipmentDataMaster master)
		{
			if (master == null) return false;
			if (master.Improvements == null || master.Improvements.Count == 0) return false;

			int today = (int)DateTime.Now.DayOfWeek;

			foreach (var imp in master.Improvements)
			{
				if (imp?.Req?.WeekConditions == null) continue;

				var weeks = imp.Req.WeekConditions;
				while (weeks.Count < 7) weeks.Add(new List<int>());

				var todayList = weeks.ElementAtOrDefault(today) ?? new List<int>();

				// -1 を含む improvement はスキップ
				if (todayList.Contains(-1))
					continue;

				// 有効な担当があれば true
				foreach (var id in todayList)
				{
					if (id == 0 || id > 0)
						return true;
				}
			}

			return false;
		}
	}

}
