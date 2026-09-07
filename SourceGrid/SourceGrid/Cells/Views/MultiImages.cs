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
	public class MultiImages : Cell
	{
		#region Constructors

		/// <summary>
		/// Use default setting
		/// </summary>
		public MultiImages()
		{
            ElementsDrawMode = DevAgeDrawing.ElementsDrawMode.Covering;
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
        /// <param name="other"></param>
        public MultiImages(MultiImages other)
            : base(other)
		{
            mImages = (DevAgeDrawing.VisualElements.VisualElementList)other.mImages.Clone();
		}
		#endregion

        private DevAgeDrawing.VisualElements.VisualElementList mImages = new DevAgeDrawing.VisualElements.VisualElementList();
		/// <summary>
		/// Images of the cells
		/// </summary>
        public DevAgeDrawing.VisualElements.VisualElementList SubImages
		{
            get { return mImages; }
		}

        protected override IEnumerable<DevAgeDrawing.VisualElements.IVisualElement> GetElements()
        {
            foreach (DevAgeDrawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;

            foreach (DevAgeDrawing.VisualElements.IVisualElement v in SubImages)
                yield return v;
        }
        private IEnumerable<DevAgeDrawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

		#region Clone
		/// <summary>
		/// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new MultiImages(this);
		}
		#endregion
	}
}
