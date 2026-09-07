using System;
using System.Drawing;
using System.Windows.Forms;
using DevAgeForms = DevAge.Windows.Forms;
namespace SourceGrid.Cells.Controllers
{
    /// <summary>
    /// Implementation RichTextBox behavior
    /// </summary>
    public class RichTextBox : ControllerBase
    {
        /// <summary>
        /// Default behavior RichTextBox
        /// </summary>
        public readonly static RichTextBox Default = new RichTextBox();

        /// <summary>
        /// Constructor
        /// </summary>
        public RichTextBox()
        {
        }
            
        #region Controller Events

        /// <summary>
        /// If ValueEventArgs is a font, use it to change SelectionFont
        /// If String, assign it as rtf
        /// If Int32, assign it as offset
        /// If HorizontalAlignment, assign as SelectionAlignment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnValueChanging(CellContext sender, ValueChangeEventArgs e)
        {
            base.OnValueChanging(sender, e);

            // only check if event args is of known type if event is not of type rich text
            if (!(e.NewValue is DevAgeForms.RichText))
            {
                Editors.RichTextBox richEditor = (Editors.RichTextBox)sender.Cell.Editor;
                DevAgeForms.DevAgeRichTextBox richTextBox = richEditor.Control;

                // if editor is not open, assign value and select all text
                if (sender.Cell.Editor.EditCell == null)
                {
                    richTextBox.Value = sender.Value as DevAgeForms.RichText;
                    richTextBox.SelectAll();
                }

                if (e.NewValue is Font)
                {
                    richTextBox.SelectionFont = (Font)e.NewValue;
                }
                else if (e.NewValue is Color)
                {
                    richTextBox.SelectionColor = (Color)e.NewValue;
                }
                else if (e.NewValue is Int32)
                {
                    richTextBox.SelectionCharOffset = (int)e.NewValue;
                }
                else if (e.NewValue is HorizontalAlignment)
                {
                    richTextBox.SelectionAlignment = (HorizontalAlignment)e.NewValue;
                }
                else if (e.NewValue is DevAgeForms.EffectType)
                {
                    richTextBox.SelectionEffect = (DevAgeForms.EffectType)e.NewValue;   
                }

                // if editor is not open, use changed value for cell
                if (sender.Cell.Editor.EditCell == null)
                {
                    sender.Value = richTextBox.Value;
                }
            }
        }

        #endregion
    }
}
