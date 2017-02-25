// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Expression.Interactivity.UnitTests
{
	using System;
    using Microsoft.Expression.Interactivity.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public sealed class ComparisonLogicTest
	{
		private const double DoubleOperand = 4.0d;
		private const string NonIntegerString = "Foo";

		#region Factory methods

		private StubComparableClass CreateStubComparableClass()
		{
			return new StubComparableClass();
		}

		#endregion

		#region Test methods

		[TestMethod]
		public void Compare_EqualIntegers_AreFoundEqual()
		{
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4,
														ComparisonConditionType.Equal,
														BehaviorTestUtilities.IntegerOperand4);
			Assert.IsTrue(result, "4 should equal 4.");
		}

		[TestMethod]
		public void Compare_DifferentIntegers_AreNotFoundEqual()
		{
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4,
														ComparisonConditionType.Equal,
														BehaviorTestUtilities.IntegerOperand5);
			Assert.IsFalse(result, "4 should not equal 5.");
		}

		[TestMethod]
		public void Compare_DifferentIntegers_AreNotEqual()
		{
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4,
														ComparisonConditionType.NotEqual,
														BehaviorTestUtilities.IntegerOperand5);
			Assert.IsTrue(result, "4 should not equal 5.");
		}

		[TestMethod]
		public void Compare_DifferentIntegers_FourLessThanFive()
		{
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4,
										ComparisonConditionType.LessThan,
										BehaviorTestUtilities.IntegerOperand5);
			Assert.IsTrue(result, "4 should be less than 5.");
		}

		[TestMethod]
		public void Compare_DifferentIntegers_FourNotGreaterThanFive()
		{
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4,
										ComparisonConditionType.GreaterThan,
										BehaviorTestUtilities.IntegerOperand5);
			Assert.IsFalse(result, "4 should not be greater than 5.");
		}

		[TestMethod]
		public void Compare_SameString_AreEqual()
		{
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.StringOperandLoremIpsum,
										ComparisonConditionType.Equal,
										BehaviorTestUtilities.StringOperandLoremIpsum);
			Assert.IsTrue(result, "Identical strings should be found equal.");
		}

		[TestMethod]
		public void Compare_DifferentStrings_AreNotEqual()
		{
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.StringOperandLoremIpsum,
										ComparisonConditionType.Equal,
										BehaviorTestUtilities.StringOperandNuncViverra);
			Assert.IsFalse(result, "Different strings should not be found equal.");
		}

		[TestMethod]
		public void Compare_SameObject_AreEqual()
		{
			StubClass stubClass = CreateStubClass();
			bool result = ComparisonLogic.EvaluateImpl(stubClass,
										ComparisonConditionType.Equal,
										stubClass);
			Assert.IsTrue(result, "An object should be equal to itself.");
		}

		private StubClass CreateStubClass()
		{
			return new StubClass();
		}

		[TestMethod]
		public void Compare_ObjectToIComparable_AreNotEqual()
		{
			StubClass stubClass = CreateStubClass();
			StubComparableClass stubComparableClass = CreateStubComparableClass();
			bool result = ComparisonLogic.EvaluateImpl(stubClass,
										ComparisonConditionType.NotEqual,
										stubComparableClass);
			Assert.IsTrue(result, "An arbitrary object should not be equal to another arbitrary object.");
		}

		[TestMethod]
		public void Compare_IntegerToObject_IsNotEqual()
		{
			StubClass stubClass = CreateStubClass();
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4,
										ComparisonConditionType.Equal,
										stubClass);
			Assert.IsFalse(result, "An integer should not be equal to an arbitrary object.");
		}

		[TestMethod]
		public void Compare_IntegerToEquivalentIComparableObject_IsEqual()
		{
			StubComparableConvertibleClass stubComparableConvertibleClass = new StubComparableConvertibleClass();
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4,
										ComparisonConditionType.Equal,
										stubComparableConvertibleClass);
			Assert.IsTrue(result, "An integer should be equal to an IComparableObject that reports the same value as the integer.");
		}

		[TestMethod]
		public void Compare_NullToNull_IsEqual()
		{
			bool result = ComparisonLogic.EvaluateImpl(null,
										ComparisonConditionType.Equal,
										null);
			Assert.IsTrue(result, "Null should be equal to null.");
		}

		[TestMethod]
		public void Compare_NullToInteger_IsNotEqual()
		{
			bool result = ComparisonLogic.EvaluateImpl(null,
							ComparisonConditionType.Equal,
							BehaviorTestUtilities.IntegerOperand4);
			Assert.IsFalse(result, "Null should not be equal to an integer.");
		}

		[TestMethod]
		public void Compare_IntegerWithNullNotEqual_IsTrue()
		{
			bool result = ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4, ComparisonConditionType.NotEqual, null);
			Assert.IsTrue(result, "4 is not equal to null, so NotEquals should return true");
		}

		[TestMethod]
		public void Compare_NullWithNullNotEqual_IsFalse()
		{
			bool result = ComparisonLogic.EvaluateImpl(null, ComparisonConditionType.NotEqual, null);
			Assert.IsFalse(result, "null is equal to null, so NotEquals should return false");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Compare_ComparableObjectWithNonComparableObjectGreaterThan_ThrowsArgumentException()
		{
			StubClass stubClass = CreateStubClass();
			ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4, ComparisonConditionType.GreaterThan, stubClass);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Compare_ComparableObjectWithNonComparableObjectGreaterThanOrEqual_ThrowsArgumentException()
		{
			StubClass stubClass = CreateStubClass();
			ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4, ComparisonConditionType.GreaterThanOrEqual, stubClass);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Compare_ComparableObjectWithNonComparableObjectLessThan_ThrowsArgumentException()
		{
			StubClass stubClass = CreateStubClass();
			ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4, ComparisonConditionType.LessThan, stubClass);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Compare_ComparableObjectWithNonComparableObjectLessThanOrEqual_ThrowsArgumentException()
		{
			StubClass stubClass = CreateStubClass();
			ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4, ComparisonConditionType.LessThanOrEqual, stubClass);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Compare_ComparableObjectWithNullLessThanOrEqual_ThrowsArgumentException()
		{
			ComparisonLogic.EvaluateImpl(BehaviorTestUtilities.IntegerOperand4, ComparisonConditionType.LessThanOrEqual, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Compare_NullWithComparableObjectLessThanOrEqual_ThrowsArgumentException()
		{
			ComparisonLogic.EvaluateImpl(null, ComparisonConditionType.LessThanOrEqual, BehaviorTestUtilities.IntegerOperand4);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Compare_NullWithNullLessThanOrEqual_ThrowsArgumentException()
		{
			ComparisonLogic.EvaluateImpl(null, ComparisonConditionType.LessThanOrEqual, null);
		}

		[TestMethod]
		public void Compare_BrushWithBrushString_AreEqual()
		{
			System.Windows.Media.SolidColorBrush brush = System.Windows.Media.Brushes.Red;
			bool result = ComparisonLogic.EvaluateImpl(brush, ComparisonConditionType.Equal, "Red");
			Assert.IsTrue(result, "Red brush should equal 'Red'");
		}

		[TestMethod]
		public void Compare_DoubleWithInconvertibleString_IsNotEqual()
		{
			bool result = ComparisonLogic.EvaluateImpl(ComparisonLogicTest.DoubleOperand, ComparisonConditionType.NotEqual, ComparisonLogicTest.NonIntegerString);
			Assert.IsTrue(result, "Double should not be equal to 'Foo'");
		}

		#endregion

		#region Helper classes

		private class StubClass
		{
		}

		private class StubComparableClass : IComparable
		{
			public int CompareTo(object obj)
			{
				if (obj.GetType().Equals(this.GetType()))
				{
					return 0;
				}

				return -1;
			}
		}

		private class StubComparableConvertibleClass : StubComparableClass, IConvertible
		{
			public TypeCode GetTypeCode()
			{
				throw new NotImplementedException();
			}

			public bool ToBoolean(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public byte ToByte(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public char ToChar(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public DateTime ToDateTime(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public decimal ToDecimal(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public double ToDouble(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public short ToInt16(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public int ToInt32(IFormatProvider provider)
			{
				return BehaviorTestUtilities.IntegerOperand4;
			}

			public long ToInt64(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public sbyte ToSByte(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public float ToSingle(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public string ToString(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public object ToType(Type conversionType, IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public ushort ToUInt16(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public uint ToUInt32(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}

			public ulong ToUInt64(IFormatProvider provider)
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}
