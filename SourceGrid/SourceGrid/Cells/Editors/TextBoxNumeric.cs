using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;
using DevAgeForms = DevAge.Windows.Forms;


namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// An editor that use a TextBoxTypedNumeric for editing support. You can customize the Control.NumericCharStyle property to enable char validation.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class TextBoxNumeric : TextBox
	{
		#region Constructor
		/// <summary>
		/// Construct a Model. Based on the Type specified the constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
		/// </summary>
		/// <param name="p_Type">The type of this model</param>
		public TextBoxNumeric(Type p_Type):base(p_Type)
		{
		}
		#endregion

		#region Edit Control
		/// <summary>
		/// Create the editor control
		/// </summary>
		/// <returns></returns>
		protected override Control CreateControl()
		{
			DevAgeForms.DevAgeTextBox editor = new DevAgeForms.DevAgeTextBox();
			editor.BorderStyle = BorderStyle.None;
			editor.AutoSize = false;
			editor.Validator = this;
			return editor;
		}

		/// <summary>
		/// Gets the control used for editing the cell.
		/// </summary>
		public new DevAgeForms.DevAgeTextBox Control
		{
			get
			{
                return (DevAgeForms.DevAgeTextBox)base.Control;
			}
		}
		#endregion
	}
}