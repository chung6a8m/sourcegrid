using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using DevAgeDrawing = DevAge.Drawing;
using DevAgeForms = DevAge.Windows.Forms;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Class to manage the visual aspect of a cell. This class can be shared beetween multiple cells.
	/// </summary>
	[Serializable]
	public class Cell : ViewBase
	{
		/// <summary>
		/// Represents a default Model
		/// </summary>
		public readonly static Cell Default = new Cell();

		#region Constructors

		/// <summary>
		/// Use default setting and construct a read and write VisualProperties
		/// </summary>
		public Cell()
		{
            ElementsDrawMode = DevAgeDrawing.ElementsDrawMode.Align;
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public Cell(Cell p_Source):base(p_Source)
		{
            ElementImage = (DevAgeDrawing.VisualElements.IImage)p_Source.ElementImage.Clone();
            ElementText = (DevAgeDrawing.VisualElements.IText)p_Source.ElementText.Clone();
		}
		#endregion

		#region Clone
		/// <summary>
		/// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new Cell(this);
		}
		#endregion

        #region Visual elements

        protected override IEnumerable<DevAgeDrawing.VisualElements.IVisualElement> GetElements()
        {
            if (ElementImage != null)
                yield return ElementImage;

            if (ElementText != null)
                yield return ElementText;
        }

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            PrepareVisualElementText(context);

            PrepareVisualElementImage(context);
        }

        private DevAgeDrawing.VisualElements.IText mElementText = new DevAgeDrawing.VisualElements.TextGDI();
        /// <summary>
        /// Gets or sets the IText visual element used to draw the cell text.
        /// Default is DevAge.Drawing.VisualElements.TextGDI
        /// </summary>
        public DevAgeDrawing.VisualElements.IText ElementText
        {
            get { return mElementText; }
            set { mElementText = value; }
        }

        /// <summary>
        /// Apply to the VisualElement specified the Image properties of the current View.
        /// Derived class can call this method to apply the settings to custom VisualElement.
        /// </summary>
        protected virtual void PrepareVisualElementText(CellContext context)
        {
            if (ElementText is DevAgeDrawing.VisualElements.TextRenderer)
            {
                DevAgeDrawing.VisualElements.TextRenderer elementText = (DevAgeDrawing.VisualElements.TextRenderer)ElementText;

                elementText.TextFormatFlags = TextFormatFlags.Default | TextFormatFlags.NoPrefix;
                if (WordWrap)
                    elementText.TextFormatFlags |= TextFormatFlags.WordBreak;
                if (TrimmingMode == TrimmingMode.Char)
                    elementText.TextFormatFlags |= TextFormatFlags.EndEllipsis;
                else if (TrimmingMode == TrimmingMode.Word)
                    elementText.TextFormatFlags |= TextFormatFlags.WordEllipsis;
                elementText.TextFormatFlags |= DevAgeForms.Utilities.ContentAligmentToTextFormatFlags(TextAlignment);
            }
            else if (ElementText is DevAgeDrawing.VisualElements.TextGDI)
            {
                DevAgeDrawing.VisualElements.TextGDI elementTextGDI = (DevAgeDrawing.VisualElements.TextGDI)ElementText;

                if (WordWrap)
                    elementTextGDI.StringFormat.FormatFlags = (StringFormatFlags)0;
                else
                    elementTextGDI.StringFormat.FormatFlags = StringFormatFlags.NoWrap;
                if (TrimmingMode == TrimmingMode.Char)
                    elementTextGDI.StringFormat.Trimming = StringTrimming.EllipsisCharacter;
                else if (TrimmingMode == TrimmingMode.Word)
                    elementTextGDI.StringFormat.Trimming = StringTrimming.EllipsisWord;
                else
                    elementTextGDI.StringFormat.Trimming = StringTrimming.None;
                elementTextGDI.Alignment = TextAlignment;
            }

            ElementText.Font = GetDrawingFont(context.Grid);
            ElementText.ForeColor = ForeColor;
            //I have already set the TextFormatFlags for the alignment so the Anchor is not necessary. I have removed this code for performance reasons.
            //element.AnchorArea = new DevAge.Drawing.AnchorArea(TextAlignment, false);

            ElementText.Value = context.DisplayText;
        }

        private DevAgeDrawing.VisualElements.IImage mElementImage = new DevAgeDrawing.VisualElements.Image();
        /// <summary>
        /// Gets or sets the IImage visual element used to draw the cell image.
        /// Default is DevAge.Drawing.VisualElements.Image
        /// </summary>
        public DevAgeDrawing.VisualElements.IImage ElementImage
        {
            get { return mElementImage; }
            set { mElementImage = value; }
        }

        /// <summary>
        /// Apply to the VisualElement specified the Image properties of the current View.
        /// Derived class can call this method to apply the settings to custom VisualElement.
        /// </summary>
        protected virtual void PrepareVisualElementImage(CellContext context)
        {
            ElementImage.AnchorArea = new DevAgeDrawing.AnchorArea(ImageAlignment, ImageStretch);

            //Read the image
            System.Drawing.Image img = null;
            Models.IImage imgModel = (Models.IImage)context.Cell.Model.FindModel(typeof(Models.IImage));
            if (imgModel != null)
                img = imgModel.GetImage(context);
            ElementImage.Value = img;
            //ElementImage.AnchorArea = AnchorArea.HasBottom
        }
        #endregion  
	}


}
