using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
	public interface IServiceUtility
	{
		DateTime GetCurrentTime(long orgId);
		string GetProjectSourcePrefix(ProjectProcessType processType);
	}
}
