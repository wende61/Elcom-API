using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common
{
	public class BulkAction 
	{
		public List<long> Ids { get; set; }
		public RecordStatus RecordStatus { get; set; }
	}
}
