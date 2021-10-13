﻿using System;
using System.Collections.Generic;
using System.Text;
using EProcurement.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EProcurement.DataObjects
{
	public class AppDbTransactionContext : IAppDbTransactionContext
	{
		IConfiguration _configuration;

		public AppDbTransactionContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		
		public ApplicationDbContext GetDbContext()
		{
			return new ApplicationDbContext(_configuration);
		}
	}
}
