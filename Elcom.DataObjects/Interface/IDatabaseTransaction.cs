using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.DataObjects
{
	public interface IDatabaseTransaction: IDisposable
	{
		void Commit();
		void Rollback();
	}
}
