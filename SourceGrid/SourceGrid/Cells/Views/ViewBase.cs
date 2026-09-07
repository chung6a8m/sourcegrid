using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using DevAgeDrawing = DevAge.Drawing;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Base abstract class to manage the visual aspect of a cell. This class can be shared beetween multiple cells.
	/// </summary>
	[Serializable]
    public abstract class ViewBase : DevAgeDrawing.VisualElements.ContainerBase, IView
	{
        /// <summary>
        /// A default RectangleBorder instance with a 1 pixed LightGray border on bottom and right side
        /// </summary>
        public static DevAgeDrawing.RectangleBorder DefaultBorder = new DevAgeDrawing.RectangleBorder(new DevAgeDrawing.BorderLine(Color.LightGray, 1),
                                                                    new DevAgeDrawing.BorderLine(Color.LightGray, 1));

        public static DevAgeDrawing.Padding DefaultPadding = new DevAgeDrawing.Padding(2);
        public static Color DefaultBackColor = Color.FromKnownColor(KnownColor.Window);
        public static Color DefaultForeColor = Color.FromKnownColor(KnownColor.WindowText);
        public static DevAgeDrawing.ContentAlignment DefaultAlignment = DevAgeDrawing.ContentAlignment.MiddleLeft;


		#region Constructors

		/// <summary>
		/// Use default setting
		/// </summary>
		public ViewBase()
        {
            Background = new DevAgeDrawing.VisualElements.BackgroundSolid();


            Padding = DefaultPadding;
            ForeColor = DefaultForeColor;
            BackColor = DefaultBackColor;
            Border = DefaultBorder;
            TextAlignment = DefaultAlignment;
            ImageAlignment = DefaultAlignment;
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public ViewBase(ViewBase p_Source):base(p_Source)
		{
			ForeColor = p_Source.ForeColor;

            BackColor = p_Source.BackColor;
            Border = p_Source.Border;
            Padding = p_Source.Padding;

			//Duplicate the reference fields
			Font tmpFont = null;
			if (p_Source.Font != null)
    			tmpFont = (Font)p_Source.Font.Clone();

			Font = tmpFont;

            WordWrap = p_Source.WordWrap;
            TextAlignment = p_Source.TextAlignment;
            TrimmingMode = p_Source.TrimmingMode;
			ImageAlignment = p_Source.ImageAlignment;
            ImageStretch = p_Source.ImageStretch;
		}
		#endregion

		#region Format

		private bool m_ImageStretch = false;
		/// <summary>
		/// True to stretch the image to all the cell
		/// </summary>
		public bool ImageStretch
		{
			get{return m_ImageStretch;}
			set{m_ImageStretch = value;}
		}

		private DevAgeDrawing.ContentAlignment m_ImageAlignment;
		/// <summary>
		/// Image Alignment
		/// </summary>
		public DevAgeDrawing.ContentAlignment ImageAlignment
		{
			get{return m_ImageAlignment;}
			set{m_ImageAlignment = value;}
		}

		private Font m_Font = null;
		/// <summary>
		/// If null the grid font is used
		/// </summary>
		public Font Font
		{
			get{return m_Font;}
			set{m_Font = value;}
		}

		private Color m_ForeColor; 
		/// <summary>
		/// ForeColor of the cell
		/// </summary>
		public Color ForeColor
		{
			get{return m_ForeColor;}
			set{m_ForeColor = value;}
		}

        private bool mWordWrap = false;
		/// <summary>
		/// Word Wrap, default false.
		/// </summary>
		public bool WordWrap
		{
			get{return mWordWrap;}
			set{mWordWrap = value;}
		}

        private TrimmingMode mTrimmingMode = TrimmingMode.Char;
        /// <summary>
        /// TrimmingMode, default Char.
        /// </summary>
        public TrimmingMode TrimmingMode
        {
            get { return mTrimmingMode; }
            set { mTrimmingMode = value; }
        }

        private DevAgeDrawing.ContentAlignment mTextAlignment;
		/// <summary>
		/// Text Alignment.
		/// </summary>
		public DevAgeDrawing.ContentAlignment TextAlignment
		{
			get{return mTextAlignment;}
			set{mTextAlignment = value;}
		}

		/// <summary>
		/// Get the font of the cell, check if the current font is null and in this case return the grid font
		/// </summary>
		/// <param name="grid"></param>
		/// <returns></returns>
		public virtual Font GetDrawingFont(GridVirtual grid)
		{
			if (Font == null)
				return grid.Font;
			else
				return Font;
		}

        /// <summary>
        /// Returns the color of the Background. If the Background it isn't an instance of BackgroundSolid then returns DefaultBackColor
        /// </summary>
        public Color BackColor
        {
            get
            {
                if (Background is DevAgeDrawing.VisualElements.BackgroundSolid)
                    return ((DevAgeDrawing.VisualElements.BackgroundSolid)Background).BackColor;
                else
                    return DefaultBackColor;
            }
            set 
            {
                if (Background is DevAgeDrawing.VisualElements.BackgroundSolid)
                    ((DevAgeDrawing.VisualElements.BackgroundSolid)Background).BackColor = value;
            }
        }

        /// <summary>
        /// The border of the Cell. Usually it is an instance of the DevAge.Drawing.RectangleBorder structure.
        /// </summary>
        public new DevAgeDrawing.IBorder Border
        {
            get { return base.Border; }
            set { base.Border = value; }
        }

        /// <summary>
        /// The padding of the cell. Usually it is 2 pixel on all sides.
        /// </summary>
        public new DevAgeDrawing.Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        /// <summary>
        /// Gets or sets a property that specify how to draw the elements.
        /// </summary>
        public new DevAgeDrawing.ElementsDrawMode ElementsDrawMode
        {
            get { return base.ElementsDrawMode; }
            set { base.ElementsDrawMode = value; }
        }
		#endregion

		#region DrawCell
		/// <summary>
		/// Draw the cell specified
		/// </summary>
		/// <param name="cellContext"></param>
        /// <param name="graphics">Paint arguments</param>
		/// <param name="rectangle">Rectangle where draw the current cell</param>
        public void DrawCell(CellContext cellContext,
			DevAgeDrawing.GraphicsCache graphics,
            RectangleF rectangle)
        {
            PrepareView(cellContext);

            Draw(graphics, rectangle);
        }

        /// <summary>
        /// Prepare the current View before drawing. Override this method for customize the specials VisualModel that you need to create. Always calls the base PrepareView.
        /// </summary>
        /// <param name="context">Current context. Cell to draw. This property is set before drawing. Only inside the PrepareView you can access this property.</param>
        protected virtual void PrepareView(CellContext context)
        {
        }
		#endregion
	
		#region Measure (GetRequiredSize)
		/// <summary>
		/// Returns the minimum required size of the current cell, calculating using the current DisplayString, Image and Borders informations.
		/// </summary>
		/// <param name="cellContext"></param>
		/// <param name="maxLayoutArea">SizeF structure that specifies the maximum layout area for the text. If width or height are zero the value is set to a default maximum value.</param>
		/// <returns></returns>
		public Size Measure(CellContext cellContext,
			Size maxLayoutArea)
		{
            using (DevAgeDrawing.MeasureHelper measure = new DevAgeDrawing.MeasureHelper(cellContext.Grid))
            {
                PrepareView(cellContext);

                SizeF measureSize = Measure(measure, SizeF.Empty, maxLayoutArea);
                return Size.Ceiling(measureSize);
            }
		}
		#endregion

        /// <summary>
        /// Background of the cell. Usually it is an instance of BackgroundSolid
        /// </summary>
        public new DevAgeDrawing.VisualElements.IVisualElement Background
        {
            get { return base.Background; }
            set { base.Background = value; }
        }
	}
}
