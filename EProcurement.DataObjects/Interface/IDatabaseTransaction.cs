using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.DataObjects
{
	public interface IDatabaseTransaction: IDisposable
	{
		void Commit();
		void Rollback();
	}
}
