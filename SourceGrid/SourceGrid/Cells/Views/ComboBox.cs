using System;
using System.Collections.Generic;
using System.Text;
using DevAgeDrawing = DevAge.Drawing;
namespace SourceGrid.Cells.Views
{
    public class ComboBox : Cell
    {
        /// <summary>
        /// Represents a default CheckBox with the CheckBox image align to the Middle Center of the cell. You must use this VisualModel with a Cell of type ICellCheckBox.
        /// </summary>
        public new readonly static ComboBox Default = new ComboBox();

        #region Constructors

        static ComboBox()
        {
        }

        /// <summary>
        /// Use default setting and construct a read and write VisualProperties
        /// </summary>
        public ComboBox()
        {
            ElementDropDown.AnchorArea = new DevAgeDrawing.AnchorArea(float.NaN, 0, 0, 0, false, false);
        }

        /// <summary>
        /// Copy constructor. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <param name="p_Source"></param>
        public ComboBox(ComboBox p_Source)
            : base(p_Source)
        {
            ElementDropDown = (DevAgeDrawing.VisualElements.IDropDownButton)p_Source.ElementDropDown.Clone();
        }
        #endregion

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            PrepareVisualElementDropDown(context);
        }

        protected override IEnumerable<DevAgeDrawing.VisualElements.IVisualElement> GetElements()
        {
            if (ElementDropDown != null)
                yield return ElementDropDown;

            foreach (DevAgeDrawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;
        }
        private IEnumerable<DevAgeDrawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

        private DevAgeDrawing.VisualElements.IDropDownButton mElementDropDown = new DevAgeDrawing.VisualElements.DropDownButtonThemed();
        /// <summary>
        /// Gets or sets the visual element used to draw the checkbox. Default is DevAge.Drawing.VisualElements.CheckBoxThemed.
        /// </summary>
        public DevAgeDrawing.VisualElements.IDropDownButton ElementDropDown
        {
            get { return mElementDropDown; }
            set { mElementDropDown = value; }
        }


        protected virtual void PrepareVisualElementDropDown(CellContext context)
        {
            if (context.CellRange.Contains(context.Grid.MouseCellPosition))
            {
                ElementDropDown.Style = DevAgeDrawing.ButtonStyle.Hot;
            }
            else
            {
                ElementDropDown.Style = DevAgeDrawing.ButtonStyle.Normal;
            }
        }

        #region Clone
        /// <summary>
        /// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new ComboBox(this);
        }
        #endregion
    }
}
