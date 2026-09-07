using System;

namespace DevAge.Configuration
{
	public class ConfigurationException : DevAgeApplicationException
	{
		public ConfigurationException(string p_strErrDescription):
			base(p_strErrDescription)
		{
		}
		public ConfigurationException(string p_strErrDescription, Exception p_InnerException):
			base(p_strErrDescription, p_InnerException)
		{
		}
	}
}
