﻿using Newtonsoft.Json;

namespace EProcurement.Common
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

		public ErrorDetails(string message)
		{
			Message = message;
		}
	}
}