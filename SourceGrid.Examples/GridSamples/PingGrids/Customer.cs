
using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Isam.Esent.Collections.Generic;


namespace WindowsFormsSample.GridSamples.PingGrids
{
	[Serializable]
	public struct Customer
	{
		public string Name {get;set;}
		public int TotalRevenue {get;set;}
	}
}
