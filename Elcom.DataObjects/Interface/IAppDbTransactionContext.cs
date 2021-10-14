using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.DataObjects
{
	public interface IAppDbTransactionContext
	{
		ApplicationDbContext GetDbContext();
	}
}
