// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------

namespace Microsoft.Expression.Drawing.Design
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.IO;
	using System.Windows;
	using System.Windows.Media.Imaging;
	using Microsoft.Windows.Design;
	using Microsoft.Windows.Design.Model;

	public abstract class ToolboxExample<T> : IToolboxExample
	{
		private Dictionary<string, byte[]> streamCache = new Dictionary<string, byte[]>();

		public virtual ModelItem CreateExample(EditingContext context)
		{
			ModelItem result = null;
			object example = null;
			
			if (this.ExampleInstance != null && (example = this.ExampleInstance()) != null)
			{
				result = ModelFactory.CreateItem(context, example);
			}
			else
			{
				result = ModelFactory.CreateItem(context, typeof(T));
			}

			this.PostProcess(result);
			return result;
		}

		/// <summary>
		/// Create an instance of T.  A ModelItem will be constructed based on it
		/// If this instance is null, a default(T) will be created as fallback.
		/// </summary>
		protected Func<object> ExampleInstance
		{
			get;
			set;
		}

		/// <summary>
		/// Post process the modelItem after it's created from factory.
		/// </summary>
		protected virtual void PostProcess(ModelItem modelItem)
		{
		}

		/// <summary>
		/// Localized display name in asset tool
		/// </summary>
		public string DisplayName
		{
			get;
			protected set;
		}

		/// <summary>
		/// Unique non-localized name to index image resources.
		/// </summary>
		protected string ResourceName
		{
			get;
			set;
		}

		protected virtual string GetImageStreamResourceName(double width, double height)
		{
			// A typical stream name: "Images.Microsoft.Expression.Shapes.Callout#Cloud_24x24.png"
			// NOTE: this is a resource name compiled in assembly, should not be localized.
			return string.Format(
				CultureInfo.InvariantCulture,
				"{0}.{1}#{2}_{3}x{4}.png",
				"Images",
				typeof(T).FullName,
				this.ResourceName,
				width,
				height);
		}

		/// <summary>
		/// Retrieve the image stream for the icon of example.
		/// Input size is typically 12x12 or 24x24 from Blend UI
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification="Don't crash on incorrect input image stream.  Fallback to null is well supported.")]
		public virtual Stream GetImageStream(Size desiredSize)
		{
			string resourceName = this.GetImageStreamResourceName(desiredSize.Width, desiredSize.Height);

			byte[] buffer;
			if (!this.streamCache.TryGetValue(resourceName, out buffer))
			{
				try
				{
					using (Stream stream = this.GetType().Assembly.GetManifestResourceStream(resourceName))
					{

						if (stream != null)
						{
#if DEBUG
							// Try to load this stream in Debug mode
							// make sure the stream is a valid image.
							BitmapImage image = new BitmapImage();
							image.BeginInit();
							image.StreamSource = stream;
							image.EndInit();
#endif

							buffer = new byte[stream.Length];
							stream.Read(buffer, 0, (int)stream.Length);
						}
					}
				}
				catch
				{
					buffer = null;
				}

				// Cache the byte buffer instead of the original stream because holding
				// an unmanaged stream in cache appears not reliable. Expression #99608
				this.streamCache[resourceName] = buffer;
			}

			return buffer == null ? null : new MemoryStream(buffer);
		}
	}
}
