using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using DevAgeDrawing = DevAge.Drawing;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Summary description for VisualModelCheckBox.
	/// </summary>
	[Serializable]
	public class CheckBox : Cell
	{
		/// <summary>
		/// Represents a default CheckBox with the CheckBox image align to the Middle Center of the cell. You must use this VisualModel with a Cell of type ICellCheckBox.
		/// </summary>
		public new readonly static CheckBox Default = new CheckBox();
		/// <summary>
		/// Represents a CheckBox with the CheckBox image align to the Middle Left of the cell
		/// </summary>
		public readonly static CheckBox MiddleLeftAlign;

		#region Constructors

		static CheckBox()
		{
			MiddleLeftAlign = new CheckBox();
			MiddleLeftAlign.CheckBoxAlignment = DevAgeDrawing.ContentAlignment.MiddleLeft;
			MiddleLeftAlign.TextAlignment = DevAgeDrawing.ContentAlignment.MiddleLeft;
		}

		/// <summary>
		/// Use default setting and construct a read and write VisualProperties
		/// </summary>
		public CheckBox()
		{
		}

		/// <summary>
		/// Copy constructor. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public CheckBox(CheckBox p_Source):base(p_Source)
		{
			m_CheckBoxAlignment = p_Source.m_CheckBoxAlignment;
            ElementCheckBox = (DevAgeDrawing.VisualElements.ICheckBox)ElementCheckBox.Clone();
		}
		#endregion

		private DevAgeDrawing.ContentAlignment m_CheckBoxAlignment = DevAgeDrawing.ContentAlignment.MiddleCenter;
		/// <summary>
		/// Image Alignment
		/// </summary>
		public DevAgeDrawing.ContentAlignment CheckBoxAlignment
		{
			get{return m_CheckBoxAlignment;}
			set{m_CheckBoxAlignment = value;}
		}

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            PrepareVisualElementCheckBox(context);
        }

        protected override IEnumerable<DevAgeDrawing.VisualElements.IVisualElement> GetElements()
        {
            if (ElementCheckBox != null)
                yield return ElementCheckBox;

            foreach (DevAgeDrawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;
        }
        private IEnumerable<DevAgeDrawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

        private DevAgeDrawing.VisualElements.ICheckBox mElementCheckBox = new DevAgeDrawing.VisualElements.CheckBoxThemed();
        /// <summary>
        /// Gets or sets the visual element used to draw the checkbox. Default is DevAge.Drawing.VisualElements.CheckBoxThemed.
        /// </summary>
        public DevAgeDrawing.VisualElements.ICheckBox ElementCheckBox
        {
            get { return mElementCheckBox; }
            set { mElementCheckBox = value; }
        }


        protected virtual void PrepareVisualElementCheckBox(CellContext context)
        {
            ElementCheckBox.AnchorArea = new DevAgeDrawing.AnchorArea(CheckBoxAlignment, false);

            Models.ICheckBox checkBoxModel = (Models.ICheckBox)context.Cell.Model.FindModel(typeof(Models.ICheckBox));
            Models.CheckBoxStatus checkBoxStatus = checkBoxModel.GetCheckBoxStatus(context);

            if (context.CellRange.Contains(context.Grid.MouseCellPosition))
            {
                if (checkBoxStatus.CheckEnable)
                    ElementCheckBox.Style = DevAgeDrawing.ControlDrawStyle.Hot;
                else
                    ElementCheckBox.Style = DevAgeDrawing.ControlDrawStyle.Disabled;
            }
            else
            {
                if (checkBoxStatus.CheckEnable)
                    ElementCheckBox.Style = DevAgeDrawing.ControlDrawStyle.Normal;
                else
                    ElementCheckBox.Style = DevAgeDrawing.ControlDrawStyle.Disabled;
            }

            ElementCheckBox.CheckBoxState = checkBoxStatus.CheckState;


            ElementText.Value = checkBoxStatus.Caption;
        }

		#region Clone
		/// <summary>
		/// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new CheckBox(this);
		}
		#endregion
	}
}
