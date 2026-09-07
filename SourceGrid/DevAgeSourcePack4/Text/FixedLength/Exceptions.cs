using System;

namespace DevAge.Text.FixedLength
{
	public class InvalidFieldLengthException : DevAgeApplicationException
	{
		public InvalidFieldLengthException(int length):
			base("Invalid field length " + length.ToString() + " must be a positive number.")
		{
		}

	}

	public class ValueNotValidLengthException : DevAgeApplicationException
	{
		public ValueNotValidLengthException(string value, int expectedLength):
			base("Value " + value + " not valid, length must be " + expectedLength.ToString())
		{
		}

	}

	public class ValueNotSupportedException : DevAgeApplicationException
	{
		public ValueNotSupportedException(string value, Type type):
			base("Value " + value + " not supported, type is " + type.Name)
		{
		}

	}

	public class TypeNotSupportedException : DevAgeApplicationException
	{
		public TypeNotSupportedException(Type type):
			base("Type " + type.ToString() + " not supported")
		{
		}

	}

	public class RegExException : DevAgeApplicationException
	{
		public RegExException(string group):
			base("Regular expression group " + group + " not valid")
		{
		}

	}

	public class FieldParseException : DevAgeApplicationException
	{
		public FieldParseException(string name, string valToParse, Exception innerException):
			base("Failed to parse field " + name + " '" + valToParse + "' - " + innerException.Message, innerException)
		{
		}

	}

	public class FieldStringConvertException : DevAgeApplicationException
	{
		public FieldStringConvertException(string name, object value, Exception innerException):
			base("Failed to convert to string field " + name + " '" + FieldStringConvertException.ObjectToStringForError(value) + "' - " + innerException.Message, innerException)
		{
		}

		/// <summary>
		/// Returns a string used for error description for a specified object. Usually used when printing the object for the error message when there is a conversion error.
		/// </summary>
		/// <param name="val"></param>
		private static string ObjectToStringForError(object val)
		{
			try
			{
				if (val == null)
					return "<null>";
				else
					return val.ToString();
			}
			catch(Exception)
			{
				return "<object>";
			}
		}
	}


	public class FieldNotDefinedException : DevAgeApplicationException
	{
		public FieldNotDefinedException(int fieldIndex):
			base("Field " + fieldIndex.ToString() + " not defined.")
		{
		}

	}

	public class FailedPropertySetFieldException : DevAgeApplicationException
	{
		public FailedPropertySetFieldException(string field, Exception innerException):
			base("Failed to set property for field " + field + " - " + innerException.Message, innerException)
		{
		}

	}
	public class FailedPropertyGetFieldException : DevAgeApplicationException
	{
		public FailedPropertyGetFieldException(string field, Exception innerException):
			base("Failed to get property for field " + field + " - " + innerException.Message, innerException)
		{
		}

	}
}
