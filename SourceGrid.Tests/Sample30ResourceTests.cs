using System.Reflection;
using System.Windows.Forms;
using NUnit.Framework;

namespace SourceGrid.Tests
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class Sample30ResourceTests
    {
        [Test]
        public void SampleMenu_LoadsFourImagesInStableIndexOrder()
        {
            using (global::DevAge.TestApp.frmSample30 form = new global::DevAge.TestApp.frmSample30())
            {
                FieldInfo field = typeof(global::DevAge.TestApp.frmSample30).GetField(
                    "imageListMenu",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                ImageList imageList = (ImageList)field.GetValue(form);

                Assert.AreEqual(4, imageList.Images.Count);
                for (int index = 0; index < imageList.Images.Count; index++)
                {
                    Assert.AreEqual(16, imageList.Images[index].Width);
                    Assert.AreEqual(16, imageList.Images[index].Height);
                }
            }
        }
    }
}
