// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Expression.Interactivity
{
	using System.ComponentModel;
	using System;
	using System.Diagnostics.CodeAnalysis;

	internal static class TypeConverterHelper
	{	
		internal static object DoConversionFrom(TypeConverter converter, object value)
		{
			object returnValue = value;

			try
			{
				if (converter != null && value != null && converter.CanConvertFrom(value.GetType()))
				{
					returnValue = converter.ConvertFrom(value);
				}
			}
			catch (Exception e)
			{
				if (!TypeConverterHelper.ShouldEatException(e))
				{
					throw;
				}
			}

			return returnValue;
		}

		private static bool ShouldEatException(Exception e)
		{
			bool shouldEat = false;
			
			if (e.InnerException != null)
			{
				shouldEat |= ShouldEatException(e.InnerException);
			}

			shouldEat |= e is FormatException;
			return shouldEat;
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification="Activator.CreateInstance could be calling user code which we don't want to bring us down.")]
		internal static TypeConverter GetTypeConverter(Type type)
		{
			return TypeDescriptor.GetConverter(type);
		}
	}
}
