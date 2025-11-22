using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;

namespace ElectronicObserver.Data.EquipmentGroup
{


	[DataContract(Name = "EqExpressionManager")]
	public sealed class EqExpressionManager : DataStorage, ICloneable
	{

		[DataMember]
		public List<EqExpressionList> Expressions { get; set; }

		[IgnoreDataMember]
		private Expression<Func<EquipmentData, bool>> predicate;

		[IgnoreDataMember]
		private Expression expression;


		public EqExpressionManager() : base()
		{
			Initialize();
		}

		public override void Initialize()
		{
			Expressions = new List<EqExpressionList>();
			predicate = null;
			expression = null;
		}


		public EqExpressionList this[int index]
		{
			get { return Expressions[index]; }
			set { Expressions[index] = value; }
		}


		public void Compile()
		{
			Expression ex = null;
			var paramex = Expression.Parameter(typeof(EquipmentData), "equip");

			foreach (var exlist in Expressions)
			{
				if (!exlist.Enabled)
					continue;

				if (ex == null)
				{
					ex = exlist.Compile(paramex);

				}
				else
				{
					if (exlist.ExternalAnd)
					{
						ex = Expression.AndAlso(ex, exlist.Compile(paramex));
					}
					else
					{
						ex = Expression.OrElse(ex, exlist.Compile(paramex));
					}
				}
			}


			if (ex == null)
			{
				ex = Expression.Constant(true, typeof(bool));       //:-P
			}

			predicate = Expression.Lambda<Func<EquipmentData, bool>>(ex, paramex);
			expression = ex;

		}


		public IEnumerable<EquipmentData> GetResult(IEnumerable<EquipmentData> list)
		{

			if (predicate == null)
				throw new InvalidOperationException("式がコンパイルされていません。");

			return list.AsQueryable().Where(predicate).AsEnumerable();
		}

		public bool IsAvailable => predicate != null;



		public override string ToString()
		{

			if (Expressions == null)
				return "(なし)";

			StringBuilder sb = new StringBuilder();
			foreach (var ex in Expressions)
			{
				if (!ex.Enabled)
					continue;
				else if (sb.Length == 0)
					sb.Append(ex.ToString());
				else
					sb.AppendFormat(" {0} {1}", ex.ExternalAnd ? "かつ" : "または", ex.ToString());
			}

			if (sb.Length == 0)
				sb.Append("(なし)");
			return sb.ToString();
		}

		public string ToExpressionString()
		{
			return expression.ToString();
		}



		public EqExpressionManager Clone()
		{
			var clone = (EqExpressionManager)MemberwiseClone();
			clone.Expressions = Expressions?.Select(e => e.Clone()).ToList();
			clone.predicate = null;
			clone.expression = null;
			return clone;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}


	}

}
