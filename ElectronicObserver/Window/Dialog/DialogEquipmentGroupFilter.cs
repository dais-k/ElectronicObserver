using ElectronicObserver.Data;
using ElectronicObserver.Data.EquipmentGroup;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogEquipmentGroupFilter : Form
	{

		private EquipmentGroupData _group;


		#region DataTable
		private DataTable _dtAndOr;
		private DataTable _dtLeftOperand;
		private DataTable _dtOperator;
		private DataTable _dtOperator_bool;
		private DataTable _dtOperator_number;
		private DataTable _dtOperator_string;
		private DataTable _dtOperator_array;
		private DataTable _dtRightOperand_bool;
		private DataTable _dtRightOperand_equipmentname;
		private DataTable _dtRightOperand_range;
		private DataTable _dtRightOperand_equipment;
		private DataTable _dtRightOperand_equipmentIcontype;
		private DataTable _dtRightOperand_equipmentCategory1;
		private DataTable _dtRightOperand_equipmentCategory2;
		#endregion

		public DialogEquipmentGroupFilter(EquipmentGroupData group)
		{
			InitializeComponent();

			{
				// 一部の列ヘッダを中央揃えにする
				var headercenter = new DataGridViewCellStyle(ExpressionView_Enabled.HeaderCell.Style)
				{
					Alignment = DataGridViewContentAlignment.MiddleCenter
				};
				ExpressionView_Enabled.HeaderCell.Style =
				ExpressionView_InternalAndOr.HeaderCell.Style =
				ExpressionView_ExternalAndOr.HeaderCell.Style =
				ExpressionView_Inverse.HeaderCell.Style =
				ExpressionView_Up.HeaderCell.Style =
				ExpressionView_Down.HeaderCell.Style =
				ExpressionDetailView_Enabled.HeaderCell.Style =
				ConstFilterView_Up.HeaderCell.Style =
				ConstFilterView_Down.HeaderCell.Style =
				ConstFilterView_Delete.HeaderCell.Style =
				headercenter;
			}


			#region init DataTable
			{
				_dtAndOr = new DataTable();
				_dtAndOr.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( bool ) ),
					new DataColumn( "Display", typeof( string ) ) });
				_dtAndOr.Rows.Add(true, "And");
				_dtAndOr.Rows.Add(false, "Or");
				_dtAndOr.AcceptChanges();

				ExpressionView_InternalAndOr.ValueMember = "Value";
				ExpressionView_InternalAndOr.DisplayMember = "Display";
				ExpressionView_InternalAndOr.DataSource = _dtAndOr;

				ExpressionView_ExternalAndOr.ValueMember = "Value";
				ExpressionView_ExternalAndOr.DisplayMember = "Display";
				ExpressionView_ExternalAndOr.DataSource = _dtAndOr;
			}
			{
				_dtLeftOperand = new DataTable();
				_dtLeftOperand.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( string ) ),
					new DataColumn( "Display", typeof( string ) ) });
				foreach (var lont in EqExpressionData.LeftOperandNameTable)
					_dtLeftOperand.Rows.Add(lont.Key, lont.Value);
				_dtLeftOperand.AcceptChanges();

				LeftOperand.ValueMember = "Value";
				LeftOperand.DisplayMember = "Display";
				LeftOperand.DataSource = _dtLeftOperand;
			}
			{
				_dtOperator = new DataTable();
				_dtOperator.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( EqExpressionData.ExpressionOperator ) ),
					new DataColumn( "Display", typeof( string ) ) });
				foreach (var ont in EqExpressionData.OperatorNameTable)
					_dtOperator.Rows.Add(ont.Key, ont.Value);
				_dtOperator.AcceptChanges();

				Operator.ValueMember = "Value";
				Operator.DisplayMember = "Display";
				Operator.DataSource = _dtOperator;
			}
			{
				_dtOperator_bool = new DataTable();
				_dtOperator_bool.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( EqExpressionData.ExpressionOperator ) ),
					new DataColumn( "Display", typeof( string ) ) });
				_dtOperator_bool.Rows.Add(EqExpressionData.ExpressionOperator.Equal, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.Equal]);
				_dtOperator_bool.Rows.Add(EqExpressionData.ExpressionOperator.NotEqual, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.NotEqual]);
				_dtOperator_bool.AcceptChanges();
			}
			{
				_dtOperator_number = new DataTable();
				_dtOperator_number.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( EqExpressionData.ExpressionOperator ) ),
					new DataColumn( "Display", typeof( string ) ) });
				_dtOperator_number.Rows.Add(EqExpressionData.ExpressionOperator.Equal, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.Equal]);
				_dtOperator_number.Rows.Add(EqExpressionData.ExpressionOperator.NotEqual, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.NotEqual]);
				_dtOperator_number.Rows.Add(EqExpressionData.ExpressionOperator.LessThan, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.LessThan]);
				_dtOperator_number.Rows.Add(EqExpressionData.ExpressionOperator.LessEqual, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.LessEqual]);
				_dtOperator_number.Rows.Add(EqExpressionData.ExpressionOperator.GreaterThan, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.GreaterThan]);
				_dtOperator_number.Rows.Add(EqExpressionData.ExpressionOperator.GreaterEqual, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.GreaterEqual]);
				_dtOperator_number.AcceptChanges();
			}
			{
				_dtOperator_string = new DataTable();
				_dtOperator_string.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( EqExpressionData.ExpressionOperator ) ),
					new DataColumn( "Display", typeof( string ) ) });
				_dtOperator_string.Rows.Add(EqExpressionData.ExpressionOperator.Equal, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.Equal]);
				_dtOperator_string.Rows.Add(EqExpressionData.ExpressionOperator.NotEqual, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.NotEqual]);
				_dtOperator_string.Rows.Add(EqExpressionData.ExpressionOperator.Contains, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.Contains]);
				_dtOperator_string.Rows.Add(EqExpressionData.ExpressionOperator.NotContains, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.NotContains]);
				_dtOperator_string.Rows.Add(EqExpressionData.ExpressionOperator.BeginWith, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.BeginWith]);
				_dtOperator_string.Rows.Add(EqExpressionData.ExpressionOperator.NotBeginWith, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.NotBeginWith]);
				_dtOperator_string.Rows.Add(EqExpressionData.ExpressionOperator.EndWith, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.EndWith]);
				_dtOperator_string.Rows.Add(EqExpressionData.ExpressionOperator.NotEndWith, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.NotEndWith]);
				_dtOperator_string.AcceptChanges();
			}
			{
				_dtOperator_array = new DataTable();
				_dtOperator_array.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( EqExpressionData.ExpressionOperator ) ),
					new DataColumn( "Display", typeof( string ) ) });
				_dtOperator_array.Rows.Add(EqExpressionData.ExpressionOperator.ArrayContains, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.ArrayContains]);
				_dtOperator_array.Rows.Add(EqExpressionData.ExpressionOperator.ArrayNotContains, EqExpressionData.OperatorNameTable[EqExpressionData.ExpressionOperator.ArrayNotContains]);
				_dtOperator_array.AcceptChanges();
			}
			{
				_dtRightOperand_bool = new DataTable();
				_dtRightOperand_bool.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( bool ) ),
					new DataColumn( "Display", typeof( string ) ) });
				_dtRightOperand_bool.Rows.Add(true, "○");
				_dtRightOperand_bool.Rows.Add(false, "×");
				_dtRightOperand_bool.AcceptChanges();
			}
			{
				_dtRightOperand_equipmentname = new DataTable();
				_dtRightOperand_equipmentname.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( int ) ),
					new DataColumn( "Display", typeof( string ) ) });
				foreach (var s in KCDatabase.Instance.MasterEquipments.Values.Where(s => !s.IsAbyssalEquipment).OrderBy(s => s.EquipmentID))
					_dtRightOperand_equipmentname.Rows.Add(s.EquipmentID, s.Name);
				_dtRightOperand_equipmentname.AcceptChanges();
			}
			{
				_dtRightOperand_range = new DataTable();
				_dtRightOperand_range.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( int ) ),
					new DataColumn( "Display", typeof( string ) ) });
				for (int i = 0; i <= 5; i++)
					_dtRightOperand_range.Rows.Add(i, Constants.GetRange(i));
				_dtRightOperand_range.AcceptChanges();
			}
			{
				_dtRightOperand_equipment = new DataTable();
				_dtRightOperand_equipment.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( int ) ),
					new DataColumn( "Display", typeof( string ) ) });
				_dtRightOperand_equipment.Rows.Add(-1, "(なし)");
				foreach (var eq in KCDatabase.Instance.MasterEquipments.Values.Where(eq => !eq.IsAbyssalEquipment).OrderBy(eq => eq.CategoryType))
					_dtRightOperand_equipment.Rows.Add(eq.EquipmentID, eq.Name);
				_dtRightOperand_equipment.Rows.Add(0, "(未開放)");
				_dtRightOperand_equipment.AcceptChanges();
			}
			{
				_dtRightOperand_equipmentCategory1 = new DataTable();
				_dtRightOperand_equipmentCategory1.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( string ) ),
					new DataColumn( "Display", typeof( string ) ) });
				// 表示名（string）を Value に格納する
				foreach (var category in KCDatabase.Instance.MasterEquipments.Values
					.Select(eq => (int)eq.CategoryType)
					.Distinct()
					.OrderBy(i => i))
				{
					var cat = KCDatabase.Instance.EquipmentTypes.ContainsKey(category) ? KCDatabase.Instance.EquipmentTypes[category] : null;
					var name = cat?.Name ?? category.ToString();
					_dtRightOperand_equipmentCategory1.Rows.Add(name, name);
				}
				_dtRightOperand_equipmentCategory1.AcceptChanges();
			}
			{
				_dtRightOperand_equipmentCategory2 = new DataTable();
				_dtRightOperand_equipmentCategory2.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( string ) ),
					new DataColumn( "Display", typeof( string ) ) });
				foreach (var category in KCDatabase.Instance.MasterEquipments.Values
					.Select(eq => (int)eq.CategoryType2)
					.Distinct()
					.OrderBy(i => i))
				{
					var cat = KCDatabase.Instance.EquipmentTypes.ContainsKey(category) ? KCDatabase.Instance.EquipmentTypes[category] : null;
					var name = cat?.Name ?? category.ToString();
					_dtRightOperand_equipmentCategory2.Rows.Add(name, name);
				}
				_dtRightOperand_equipmentCategory2.AcceptChanges();
			}
			{
				_dtRightOperand_equipmentIcontype = new DataTable();
				_dtRightOperand_equipmentIcontype.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( int ) ),
					new DataColumn( "Display", typeof( string ) ) });

				foreach (var icon in KCDatabase.Instance.MasterEquipments.Values.Select(eq => eq.IconType).Distinct().OrderBy(i => i))
				{
					string name = Constants.GetIconName(icon);
					_dtRightOperand_equipmentIcontype.Rows.Add(icon, name);
				}
				_dtRightOperand_equipmentIcontype.AcceptChanges();
			}

			RightOperand_ComboBox.ValueMember = "Value";
			RightOperand_ComboBox.DisplayMember = "Display";
			RightOperand_ComboBox.DataSource = _dtRightOperand_bool;

			SetExpressionSetter(EqExpressionData.LeftOperandNameTable.Keys.First());

			#endregion


			ConstFilterSelector.SelectedIndex = 0;

			ImportGroupData(group);
		}

		private void DialogEquipmentGroupFilter_Load(object sender, EventArgs e)
		{
			if (Owner != null)
				Icon = Owner.Icon;
		}



		/// <summary>
		/// グループデータをコピーし、UIを初期化します。
		/// </summary>
		/// <param name="group">対象となるグループ。コピーされるためこのインスタンスには変更は適用されません。</param>
		public void ImportGroupData(EquipmentGroupData group)
		{

			_group = group.Clone();

			UpdateExpressionView();
			UpdateConstFilterView();
		}


		/// <summary>
		/// 編集したグループデータを出力します。
		/// </summary>
		public EquipmentGroupData ExportGroupData()
		{
			// Expressions 内のカテゴリ右辺を表示名(string)に正規化してから返す
			if (_group?.Expressions?.Expressions != null)
			{
				foreach (var list in _group.Expressions.Expressions)
				{
					foreach (var ex in list.Expressions)
					{
						if (ex.LeftOperand == ".MasterEquipment.CategoryTypeInstance.Name" || ex.LeftOperand == ".MasterEquipment.CategoryTypeInstance2.Name")
						{
							if (ex.RightOperand != null)
							{
								if (ex.RightOperand is int ri)
								{
									var cat = KCDatabase.Instance.EquipmentTypes.ContainsKey(ri) ? KCDatabase.Instance.EquipmentTypes[ri] : null;
									ex.RightOperand = cat?.Name ?? ri.ToString();
								}
								else
								{
									ex.RightOperand = ex.RightOperand.ToString();
								}
							}
						}
					}
				}
			}
			return _group;
		}


		private DataGridViewRow GetExpressionViewRow(EqExpressionList exp)
		{
			var row = new DataGridViewRow();
			row.CreateCells(ExpressionView);

			row.SetValues(
				exp.Enabled,
				exp.ExternalAnd,
				exp.Inverse,
				exp.InternalAnd,
				exp.ToString()
				);

			return row;
		}

		private DataGridViewRow GetExpressionDetailViewRow(EqExpressionData exp)
		{
			var row = new DataGridViewRow();
			row.CreateCells(ExpressionDetailView);

			row.SetValues(
				exp.Enabled,
				exp.LeftOperand,
				exp.RightOperand,
				exp.Operator
				);

			return row;
		}


		private int GetSelectedRow(DataGridView dgv)
		{
			return dgv.SelectedRows.Count == 0 ? -1 : dgv.SelectedRows[0].Index;
		}



		private void UpdateExpressionView()
		{

			ExpressionView.Rows.Clear();


			var rows = new DataGridViewRow[_group.Expressions.Expressions.Count];
			for (int i = 0; i < rows.Length; i++)
			{
				rows[i] = GetExpressionViewRow(_group.Expressions.Expressions[i]);
			}

			ExpressionView.Rows.AddRange(rows.ToArray());

			ExpressionDetailView.Rows.Clear();

			LabelResult.Tag = false;
			UpdateExpressionLabel();

		}

		/// <summary>
		/// 包含/除外フィルタの表示を更新します。
		/// </summary>
		private void UpdateConstFilterView()
		{

			List<int> values = ConstFilterSelector.SelectedIndex == 0 ? _group.InclusionFilter : _group.ExclusionFilter;

			ConstFilterView.Rows.Clear();

			var rows = new DataGridViewRow[values.Count];
			for (int i = 0; i < rows.Length; i++)
			{
				rows[i] = new DataGridViewRow();
				rows[i].CreateCells(ConstFilterView);

				var equipment = KCDatabase.Instance.MasterEquipments[values[i]];
				rows[i].SetValues(values[i], equipment?.Name ?? "(未在籍)");
			}

			ConstFilterView.Rows.AddRange(rows);

		}

		private List<int> GetConstFilterFromUI()
		{
			return ConstFilterSelector.SelectedIndex == 0 ? _group.InclusionFilter : _group.ExclusionFilter;
		}


		/// <summary>
		/// 指定された式から、式UIを初期化します。
		/// </summary>
		/// <param name="left">左辺値。</param>
		/// <param name="right">右辺値。指定しなければ null。</param>
		/// <param name="ope">演算子。指定しなければ null。</param>
		private void SetExpressionSetter(string left, object right = null, EqExpressionData.ExpressionOperator? ope = null)
		{

			Type lefttype = EqExpressionData.GetLeftOperandType(left);

			bool isenumerable = lefttype != null && lefttype != typeof(string) && lefttype.GetInterface("IEnumerable") != null;
			if (isenumerable)
				lefttype = lefttype.GetElementType() ?? lefttype.GetGenericArguments().First();

			Description.Text = "";

			LeftOperand.SelectedValue = left;

			// 特殊判定(決め打ち)シリーズ
			if (left == ".MasterEquipment.Name")
			{
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_string;

				RightOperand_ComboBox.DataSource = _dtRightOperand_equipmentname;

				// right が int (EquipmentID) なら SelectedValue を使い、文字列なら Text を設定する
				if (right is int rid)
				{
					RightOperand_ComboBox.SelectedValue = rid;
				}
				else if (right != null)
				{
					RightOperand_ComboBox.Text = right.ToString();
				}
				else
				{
					// 初期表示は先頭の Display
					var first = _dtRightOperand_equipmentname.AsEnumerable().FirstOrDefault();
					if (first != null)
						RightOperand_ComboBox.Text = first["Display"]?.ToString() ?? "";
					else
						RightOperand_ComboBox.Text = "";
				}
			}
			else if (left == ".MasterEquipment.IconType")
			{
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;

				Operator.Enabled = true;
				// アイコンは数値比較（従来通り）なので数値用オペレータ
				Operator.DataSource = _dtOperator_number;

				RightOperand_ComboBox.DataSource = _dtRightOperand_equipmentIcontype;
				RightOperand_ComboBox.ValueMember = "Value";
				RightOperand_ComboBox.DisplayMember = "Display";

				// 右辺は数値（int）で比較する。設定値が存在すれば SelectedValue にセットする。
				if (right != null)
				{
					try
					{
						RightOperand_ComboBox.SelectedValue = Convert.ToInt32(right);
					}
					catch
					{
						// 万一文字列で来ても Value から探す（表示名からの選択はサポート）
						var row = _dtRightOperand_equipmentIcontype.AsEnumerable()
							.FirstOrDefault(r => string.Equals(r["Display"]?.ToString(), right.ToString(), StringComparison.CurrentCulture));
						if (row != null)
							RightOperand_ComboBox.SelectedValue = row["Value"];
						else
							RightOperand_ComboBox.SelectedIndex = 0;
					}
				}
				else
				{
					RightOperand_ComboBox.SelectedIndex = 0;
				}
			}
			else if (left == ".MasterEquipment.CategoryTypeInstance.Name")
			{
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				// カテゴリは文字列比較
				Operator.DataSource = _dtOperator_string;

				RightOperand_ComboBox.DataSource = _dtRightOperand_equipmentCategory1;

				// right を文字列として SelectedValue に設定（互換で int の場合は名前に変換）
				if (right != null)
				{
					try
					{
						RightOperand_ComboBox.SelectedValue = right.ToString();
					}
					catch
					{
						RightOperand_ComboBox.SelectedIndex = 0;
					}
				}
				else
				{
					RightOperand_ComboBox.SelectedIndex = 0;
				}
			}
			else if (left == ".MasterEquipment.CategoryTypeInstance2.Name")
			{
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_string;

				RightOperand_ComboBox.DataSource = _dtRightOperand_equipmentCategory2;

				if (right != null)
				{
					try
					{
						RightOperand_ComboBox.SelectedValue = right.ToString();
					}
					catch
					{
						RightOperand_ComboBox.SelectedIndex = 0;
					}
				}
				else
				{
					RightOperand_ComboBox.SelectedIndex = 0;
				}
			}
			else if (left == ".MasterEquipment.Range")
			{
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_ComboBox.DataSource = _dtRightOperand_range;
				RightOperand_ComboBox.SelectedValue = right ?? 1;

			}
			// 以下、汎用判定
			else if (lefttype == null)
			{
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = true;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = false;
				Operator.DataSource = _dtOperator;

				RightOperand_TextBox.Text = right == null ? "" : right.ToString();

			}
			else if (lefttype == typeof(int))
			{
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = true;
				RightOperand_NumericUpDown.Enabled = true;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_NumericUpDown.DecimalPlaces = 0;
				RightOperand_NumericUpDown.Increment = 1m;

				switch (left)
				{
					case ".MasterEquipment.EquipmentID":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 999999;
						break;
					default:
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 999;
						break;
				}
				RightOperand_NumericUpDown.Value = right == null ? RightOperand_NumericUpDown.Minimum : (int)right;
				UpdateDescriptionFromNumericUpDown();

			}
			else if (lefttype == typeof(long))
			{
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = true;
				RightOperand_NumericUpDown.Enabled = true;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_NumericUpDown.DecimalPlaces = 0;
				RightOperand_NumericUpDown.Increment = 1m;

				switch (left)
				{
					default:
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 9999;
						break;
				}
				RightOperand_NumericUpDown.Value = right == null ? RightOperand_NumericUpDown.Minimum : (long)right;
				UpdateDescriptionFromNumericUpDown();
			}
			else if (lefttype == typeof(double))
			{
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = true;
				RightOperand_NumericUpDown.Enabled = true;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				switch (left)
				{
					default:
						RightOperand_NumericUpDown.Maximum = int.MaxValue;
						RightOperand_NumericUpDown.Minimum = int.MinValue;
						RightOperand_NumericUpDown.DecimalPlaces = 0;
						RightOperand_NumericUpDown.Increment = 1m;
						break;
				}
				RightOperand_NumericUpDown.Value = right == null ? RightOperand_NumericUpDown.Minimum : Convert.ToDecimal(right);
				UpdateDescriptionFromNumericUpDown();

			}
			else if (lefttype == typeof(bool))
			{
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_bool;

				RightOperand_ComboBox.DataSource = _dtRightOperand_bool;
				RightOperand_ComboBox.SelectedValue = right ?? true;

			}
			else if (lefttype.IsEnum)
			{
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_bool;

				DataTable dt = new DataTable();
				dt.Columns.AddRange(new DataColumn[]{
					new DataColumn( "Value", typeof( string ) ),
					new DataColumn( "Display", typeof( string ) ) });
				var names = lefttype.GetEnumNames();
				var values = lefttype.GetEnumValues();
				for (int i = 0; i < names.Length; i++)
					dt.Rows.Add(values.GetValue(i), names[i]);
				dt.AcceptChanges();
				RightOperand_ComboBox.DataSource = dt;
				RightOperand_ComboBox.SelectedValue = right;

			}
			else
			{
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = true;
				RightOperand_TextBox.Enabled = true;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_string;

				RightOperand_TextBox.Text = right == null ? "" : right.ToString();

			}


			if (isenumerable)
			{
				Operator.DataSource = _dtOperator_array;
			}


			if (Operator.DataSource as DataTable != null)
			{
				if (ope == null)
				{
					Operator.SelectedValue = ((DataTable)Operator.DataSource).AsEnumerable().First()["Value"];
				}
				else
				{
					Operator.SelectedValue = (EqExpressionData.ExpressionOperator)ope;
				}
			}
		}



		/// <summary>
		/// 選択された行をもとに、 ExpressionDetailView を更新します。
		/// </summary>
		/// <param name="index">対象となる行のインデックス。</param>
		private void UpdateExpressionDetailView(int index)
		{

			if (index < 0 || _group.Expressions.Expressions.Count <= index) return;

			var ex = _group.Expressions.Expressions[index];


			// detail の更新と expression の初期化

			ExpressionDetailView.Rows.Clear();

			var rows = new DataGridViewRow[ex.Expressions.Count];
			for (int i = 0; i < rows.Length; i++)
			{
				rows[i] = GetExpressionDetailViewRow(ex.Expressions[i]);
			}

			ExpressionDetailView.Rows.AddRange(rows);
		}


		// 選択を基にUIの更新
		private void ExpressionView_SelectionChanged(object sender, EventArgs e)
		{

			UpdateExpressionDetailView(ExpressionView.SelectedRows.Count == 0 ? -1 : ExpressionView.SelectedRows[0].Index);

		}

		private void ExpressionDetailView_SelectionChanged(object sender, EventArgs e)
		{

			int index = ExpressionView.SelectedRows.Count == 0 ? -1 : ExpressionView.SelectedRows[0].Index;
			int detailIndex = ExpressionDetailView.SelectedRows.Count == 0 ? -1 : ExpressionDetailView.SelectedRows[0].Index;

			if (index < 0 || _group.Expressions.Expressions.Count <= index ||
				detailIndex < 0 || _group.Expressions[index].Expressions.Count <= detailIndex) return;

			EqExpressionData exp = _group.Expressions[index][detailIndex];

			SetExpressionSetter(exp.LeftOperand, exp.RightOperand, exp.Operator);

		}




		// Expression のボタン操作
		private void Expression_Add_Click(object sender, EventArgs e)
		{

			int insertrow = GetSelectedRow(ExpressionView);
			if (insertrow == -1) insertrow = ExpressionView.Rows.Count - 1;

			var exp = new EqExpressionList();

			_group.Expressions.Expressions.Insert(insertrow + 1, exp);
			ExpressionView.Rows.Insert(insertrow + 1, GetExpressionViewRow(exp));

			ExpressionUpdated();
		}

		private void Expression_Delete_Click(object sender, EventArgs e)
		{

			int selectedrow = GetSelectedRow(ExpressionView);

			if (selectedrow == -1)
			{
				MessageBox.Show("対象となる行を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			ExpressionDetailView.Rows.Clear();

			_group.Expressions.Expressions.RemoveAt(selectedrow);
			ExpressionView.Rows.RemoveAt(selectedrow);


			ExpressionUpdated();
		}


		private void ButtonOK_Click(object sender, EventArgs e)
		{

			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{

			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}




		/// <summary>
		/// UIの設定値からExpressionDataを構築します。
		/// </summary>
		private EqExpressionData BuildExpressionDataFromUI()
		{

			var exp = new EqExpressionData
			{
				LeftOperand = (string)LeftOperand.SelectedValue ?? LeftOperand.Text,
				Operator = (EqExpressionData.ExpressionOperator)Operator.SelectedValue
			};

			Type type = exp.GetLeftOperandType();
			if (type != null && type != typeof(string) && type.GetInterface("IEnumerable") != null)
				type = type.GetElementType() ?? type.GetGenericArguments().First();
			if (type != null && type.IsEnum)
				type = type.GetEnumUnderlyingType();

			// 左辺ごとの期待型の補正
			// 装備名は文字列（ユーザ入力や表示名）として扱う
			if (exp.LeftOperand == ".MasterEquipment.Name")
			{
				type = typeof(string);
			}
			// カテゴリは表示名(string)を期待
			else if (exp.LeftOperand == ".MasterEquipment.CategoryTypeInstance.Name" ||
					 exp.LeftOperand == ".MasterEquipment.CategoryTypeInstance2.Name")
			{
				type = typeof(string);
			}

			object ConvertFromComboBoxDisplay(ComboBox cb, Type targetType)
			{
				if (cb.DropDownStyle == ComboBoxStyle.DropDownList)
				{
					var sel = cb.SelectedValue ?? cb.Text;
					if (sel != null && targetType != null && targetType.IsInstanceOfType(sel))
						return sel;
					if (targetType == typeof(int))
					{
						if (sel is int) return sel;
						if (sel is string s && int.TryParse(s, out int v)) return v;
					}
					return targetType == null ? sel : Convert.ChangeType(sel, targetType);
				}
				else
				{
					string text = cb.Text ?? "";
					if (targetType == typeof(string)) return text;
					if (targetType == typeof(int))
					{
						if (int.TryParse(text, out int v)) return v;
						if (cb.DataSource is DataTable dt)
						{
							var row = dt.AsEnumerable().FirstOrDefault(r => string.Equals(r["Display"]?.ToString(), text, StringComparison.CurrentCulture));
							if (row != null)
							{
								var obj = row["Value"];
								if (obj is int) return obj;
								if (obj is string ss && int.TryParse(ss, out int v2)) return v2;
							}
						}
						var found = KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault(eq => string.Equals(eq.Name, text, StringComparison.CurrentCulture));
						if (found != null) return found.EquipmentID;
						return null;
					}
					return targetType == null ? (object)text : Convert.ChangeType(text, targetType);
				}
			}

			if (RightOperand_ComboBox.Enabled)
			{
				object val = ConvertFromComboBoxDisplay(RightOperand_ComboBox, type);

				if (val == null)
				{
					exp.RightOperand = RightOperand_ComboBox.Text;
				}
				else
				{
					if (type == typeof(string))
					{
						exp.RightOperand = val.ToString();
					}
					else
					{
						exp.RightOperand = (type == null || type.IsInstanceOfType(val)) ? val : Convert.ChangeType(val, type);
					}
				}
			}
			else if (RightOperand_NumericUpDown.Enabled)
			{
				if (type == typeof(int))
					exp.RightOperand = Convert.ToInt32(RightOperand_NumericUpDown.Value);
				else if (type == typeof(long))
					exp.RightOperand = Convert.ToInt64(RightOperand_NumericUpDown.Value);
				else if (type == typeof(double))
					exp.RightOperand = Convert.ToDouble(RightOperand_NumericUpDown.Value);
				else
					exp.RightOperand = Convert.ChangeType(RightOperand_NumericUpDown.Value, type);
			}
			else if (RightOperand_TextBox.Enabled)
			{
				if (type == typeof(string) || type == null)
					exp.RightOperand = RightOperand_TextBox.Text;
				else
					exp.RightOperand = Convert.ChangeType(RightOperand_TextBox.Text, type);
			}
			else
			{
				exp.RightOperand = null;
			}

			return exp;
		}


		// ExpressionDetail のボタン操作
		private void ExpressionDetail_Add_Click(object sender, EventArgs e)
		{

			int procrow = GetSelectedRow(ExpressionView);
			if (procrow == -1)
			{
				MessageBox.Show("対象となる式(左側)の行を選択してください。\r\n行が存在しない場合は追加してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			var exp = BuildExpressionDataFromUI();

			_group.Expressions.Expressions[procrow].Expressions.Add(exp);
			ExpressionDetailView.Rows.Add(GetExpressionDetailViewRow(exp));

			UpdateExpressionViewRow(procrow);
		}


		private void ExpressionDetail_Edit_Click(object sender, EventArgs e)
		{

			int procrow = GetSelectedRow(ExpressionView);
			if (procrow == -1)
			{
				MessageBox.Show("対象となる式列(左側)を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			int selectedrow = GetSelectedRow(ExpressionDetailView);
			if (selectedrow == -1)
			{
				MessageBox.Show("対象となる行を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			var exp = BuildExpressionDataFromUI();

			_group.Expressions.Expressions[procrow].Expressions[selectedrow] = exp;
			ExpressionDetailView.Rows.Insert(selectedrow, GetExpressionDetailViewRow(exp));
			ExpressionDetailView.Rows.RemoveAt(selectedrow + 1);

			UpdateExpressionViewRow(procrow);
		}


		private void ExpressionDetail_Delete_Click(object sender, EventArgs e)
		{

			int procrow = GetSelectedRow(ExpressionView);
			if (procrow == -1)
			{
				MessageBox.Show("対象となる式列(左側)を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			int selectedrow = GetSelectedRow(ExpressionDetailView);
			if (selectedrow == -1)
			{
				MessageBox.Show("対象となる行を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}

			_group.Expressions.Expressions[procrow].Expressions.RemoveAt(selectedrow);
			ExpressionDetailView.Rows.RemoveAt(selectedrow);

			UpdateExpressionViewRow(procrow);
		}


		// 左辺値変更時のUI変更
		private void LeftOperand_SelectedValueChanged(object sender, EventArgs e)
		{
			SetExpressionSetter((string)LeftOperand.SelectedValue ?? LeftOperand.Text);
		}


		// チェックボックス、コンボボックスの更新を即時反映する
		private void ExpressionView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (ExpressionView.IsCurrentCellDirty)
				ExpressionView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}


		private void ExpressionDetailView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (ExpressionDetailView.IsCurrentCellDirty)
				ExpressionDetailView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}


		// UI 操作(チェックボックス/コンボボックス)の反映
		private void ExpressionView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{

			if (e.RowIndex < 0) return;

			if (e.ColumnIndex == ExpressionView_Enabled.Index)
			{
				_group.Expressions[e.RowIndex].Enabled = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			}
			else if (e.ColumnIndex == ExpressionView_ExternalAndOr.Index)
			{
				_group.Expressions[e.RowIndex].ExternalAnd = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			}
			else if (e.ColumnIndex == ExpressionView_Inverse.Index)
			{
				_group.Expressions[e.RowIndex].Inverse = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			}
			else if (e.ColumnIndex == ExpressionView_InternalAndOr.Index)
			{
				_group.Expressions[e.RowIndex].InternalAnd = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			}

			UpdateExpressionViewRow(e.RowIndex);
		}

		private void ExpressionDetailView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{

			if (e.RowIndex < 0) return;

			int procrow = GetSelectedRow(ExpressionView);
			if (procrow == -1)
			{
				return;
			}

			if (e.ColumnIndex == ExpressionDetailView_Enabled.Index)
			{
				_group.Expressions[procrow].Expressions[e.RowIndex].Enabled = (bool)ExpressionDetailView[e.ColumnIndex, e.RowIndex].Value;
			}

			UpdateExpressionViewRow(procrow);
		}


		/// <summary>
		/// ExpressionView の指定された行の式表示を更新します。
		/// </summary>
		/// <param name="index">行インデックス。</param>
		private void UpdateExpressionViewRow(int index)
		{
			ExpressionView[ExpressionView_Expression.Index, index].Value = _group.Expressions[index].ToString();
			ExpressionUpdated();
		}

		/// <summary>
		/// 式が更新されたときの動作を行います。
		/// </summary>
		private void ExpressionUpdated()
		{
			UpdateExpressionLabel();
		}

		private void UpdateExpressionLabel()
		{
			if (LabelResult.Tag != null && (bool)LabelResult.Tag)
			{
				_group.Expressions.Compile();
				LabelResult.Text = _group.Expressions.ToExpressionString();
			}
			else
			{
				LabelResult.Text = _group.Expressions.ToString();
			}
		}



		// ボタン処理
		private void ExpressionView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

			if (e.RowIndex < 0) return;

			// fixme: 非選択セルで上下させると選択がちょっとちらつく  :(
			if (e.ColumnIndex == ExpressionView_Up.Index && e.RowIndex > 0)
			{
				_group.Expressions.Expressions.Insert(e.RowIndex - 1, _group.Expressions[e.RowIndex]);
				_group.Expressions.Expressions.RemoveAt(e.RowIndex + 1);

				ControlHelper.RowMoveUp(ExpressionView, e.RowIndex);
				ExpressionView.Rows[e.RowIndex - 1].Selected = true;

				ExpressionUpdated();


			}
			else if (e.ColumnIndex == ExpressionView_Down.Index && e.RowIndex < ExpressionView.Rows.Count - 1)
			{
				_group.Expressions.Expressions.Insert(e.RowIndex + 2, _group.Expressions[e.RowIndex]);
				_group.Expressions.Expressions.RemoveAt(e.RowIndex);

				ControlHelper.RowMoveDown(ExpressionView, e.RowIndex);
				ExpressionView.Rows[e.RowIndex + 1].Selected = true;

				ExpressionUpdated();

			}


			if (ExpressionView.SelectedRows.Count > 0)
				UpdateExpressionDetailView(ExpressionView.SelectedRows[0].Index);
		}

		private void ConstFilterView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

			// 上下動に意味があるかはおいておいて
			if (e.ColumnIndex == ConstFilterView_Up.Index && e.RowIndex > 0)
			{
				var list = GetConstFilterFromUI();
				list.Insert(e.RowIndex - 1, list[e.RowIndex]);
				list.RemoveAt(e.RowIndex + 1);

				ControlHelper.RowMoveUp(ConstFilterView, e.RowIndex);

			}
			else if (e.ColumnIndex == ConstFilterView_Down.Index && e.RowIndex < ConstFilterView.Rows.Count - 1)
			{
				var list = GetConstFilterFromUI();
				list.Insert(e.RowIndex + 2, list[e.RowIndex]);
				list.RemoveAt(e.RowIndex);

				ControlHelper.RowMoveDown(ConstFilterView, e.RowIndex);

			}
			else if (e.ColumnIndex == ConstFilterView_Delete.Index && e.RowIndex >= 0)
			{
				var list = GetConstFilterFromUI();
				list.RemoveAt(e.RowIndex);

				ConstFilterView.Rows.RemoveAt(e.RowIndex);
			}

		}


		// コンボボックスの即選択
		private void ExpressionView_CellClick(object sender, DataGridViewCellEventArgs e)
		{

			if (e.RowIndex < 0) return;

			if (ExpressionView.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
			{
				ExpressionView.BeginEdit(false);
				var edit = ExpressionView.EditingControl as DataGridViewComboBoxEditingControl;
				edit.DroppedDown = true;
			}

		}


		private void ExpressionDetailView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			if (e.RowIndex < 0) return;

			int procrow = GetSelectedRow(ExpressionView);
			if (procrow < 0 || procrow >= _group.Expressions.Expressions.Count ||
				e.RowIndex >= _group.Expressions[procrow].Expressions.Count)
			{
				return;
			}

			if (e.ColumnIndex == ExpressionDetailView_LeftOperand.Index)
			{
				e.Value = _group.Expressions[procrow].Expressions[e.RowIndex].LeftOperandToString();
				e.FormattingApplied = true;

			}
			else if (e.ColumnIndex == ExpressionDetailView_Operator.Index)
			{
				e.Value = _group.Expressions[procrow].Expressions[e.RowIndex].OperatorToString();
				e.FormattingApplied = true;

			}
			else if (e.ColumnIndex == ExpressionDetailView_RightOperand.Index)
			{
				e.Value = _group.Expressions[procrow].Expressions[e.RowIndex].RightOperandToString();
				e.FormattingApplied = true;
			}

		}



		// Description の変更
		private void RightOperand_NumericUpDown_ValueChanged(object sender, EventArgs e)
		{

			UpdateDescriptionFromNumericUpDown();
		}


		private void UpdateDescriptionFromNumericUpDown()
		{

			string left = ((string)LeftOperand.SelectedValue) ?? LeftOperand.Text;
			int intvalue = (int)RightOperand_NumericUpDown.Value;

			switch (left)
			{
				case ".MasterEquipment.EquipmentID":
					{
						var equipment = KCDatabase.Instance.MasterEquipments[intvalue];
						if (equipment != null)
						{
							Description.Text = equipment.Name;
						}
						else
						{
							Description.Text = "(未所持)";
						}
					}
					break;

			}

			if (left.Contains("Rate"))
			{
				Description.Text = RightOperand_NumericUpDown.Value.ToString("P0");
			}

		}

		private void LabelResult_Click(object sender, EventArgs e)
		{
			LabelResult.Tag = !(bool)LabelResult.Tag;
			UpdateExpressionLabel();
		}



		// ConstFilter 関連
		private void ConstFilterSelector_SelectedIndexChanged(object sender, EventArgs e)
		{

			if (_group != null)
			{
				UpdateConstFilterView();
			}

		}

		private void OptimizeConstFilter_Click(object sender, EventArgs e)
		{

			if (ConstFilterSelector.SelectedIndex == 0)
			{

				_group.InclusionFilter = _group.InclusionFilter.Intersect(KCDatabase.Instance.Ships.Keys).ToList();

			}
			else
			{

				_group.ExclusionFilter = _group.ExclusionFilter.Intersect(KCDatabase.Instance.Ships.Keys).ToList();
			}

			UpdateConstFilterView();
		}

		private void ClearConstFilter_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show(ConstFilterSelector.Text + " を初期化します。\r\nよろしいですか?", "初期化の確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
				== System.Windows.Forms.DialogResult.Yes)
			{

				if (ConstFilterSelector.SelectedIndex == 0)
				{
					_group.InclusionFilter.Clear();

				}
				else
				{
					_group.ExclusionFilter.Clear();
				}

				UpdateConstFilterView();
			}
		}

		private void ConvertToExpression_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show("現在の包含/除外リストを式に変換します。\r\n逆変換はできません。\r\nよろしいですか？", "確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
					== System.Windows.Forms.DialogResult.Yes)
			{

				if (_group.InclusionFilter.Count > 0)
				{
					_group.Expressions.Expressions.Add(new EqExpressionList(false, false, false));
					var exlist = _group.Expressions.Expressions.Last();
					// 変更点: 左辺を MasterEquipment をルートにしたものにする
					foreach (var id in _group.InclusionFilter)
					{
						exlist.Expressions.Add(new EqExpressionData(".MasterEquipment.EquipmentID", EqExpressionData.ExpressionOperator.Equal, id));
					}
					_group.InclusionFilter.Clear();
				}
				if (_group.ExclusionFilter.Count > 0)
				{
					_group.Expressions.Expressions.Add(new EqExpressionList(false, true, true));
					var exlist = _group.Expressions.Expressions.Last();

					// 変更点: 左辺を MasterEquipment をルートにしたものにする
					foreach (var id in _group.ExclusionFilter)
					{
						exlist.Expressions.Add(new EqExpressionData(".MasterEquipment.EquipmentID", EqExpressionData.ExpressionOperator.Equal, id));
					}
					_group.ExclusionFilter.Clear();
				}


				UpdateExpressionView();
				UpdateConstFilterView();

			}
		}


		private void ButtonMenu_Click(object sender, EventArgs e)
		{
			SubMenu.Show(ButtonMenu, ButtonMenu.Width / 2, ButtonMenu.Height / 2);
		}

		private void Menu_ImportFilter_Click(object sender, EventArgs e)
		{


			if (MessageBox.Show("クリップボードからフィルタをインポートします。\r\n現在のフィルタは破棄されます。(包含/除外フィルタは維持されます)\r\nよろしいですか？\r\n",
					"フィルタのインポートの確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== System.Windows.Forms.DialogResult.No)
				return;

			string data = Clipboard.GetText();

			if (string.IsNullOrEmpty(data))
			{
				MessageBox.Show("クリップボードが空です。\r\nフィルタデータをコピーしたうえで再度選択してください。\r\n",
					"インポートできません", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			try
			{

				using (var str = new StringReader(data))
				{
					var exp = (EqExpressionManager)_group.Expressions.Load(str);
					if (exp == null)
						throw new ArgumentException("インポートできないデータ形式です。");
					else
						_group.Expressions = exp;
				}

				UpdateExpressionView();

			}
			catch (Exception ex)
			{

				MessageBox.Show("フィルタのインポートに失敗しました。\r\n" + ex.Message, "インポートできません", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void Menu_ExportFilter_Click(object sender, EventArgs e)
		{

			try
			{

				StringBuilder str = new StringBuilder();
				_group.Expressions.Save(str);

				Clipboard.SetText(str.ToString());

				MessageBox.Show("フィルタをクリップボードにエクスポートしました。\r\n「フィルタのインポート」で取り込んだり、\r\nメモ帳等に貼り付けて保存したりしてください。\r\n",
					"フィルタのエクスポート", MessageBoxButtons.OK, MessageBoxIcon.Information);

			}
			catch (Exception ex)
			{

				MessageBox.Show("フィルタのエクスポートに失敗しました。\r\n" + ex.Message, "エクスポートできません", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}


	}
}
