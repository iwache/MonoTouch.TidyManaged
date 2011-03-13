// Extended for MonoTouch by TSWE-Technische Softwareentwicklung
// Copyright (c) 2011 Immo Wache
//
// Copyright (c) 2009 Mark Beaton
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TidyManaged
{
	internal class OutputSink : IDisposable
	{
		public Stream Stream  { get; private set; }

		private GCHandle data;
		private bool disposed;
		
		[MarshalAs(UnmanagedType.Struct)]
		internal Interop.TidyOutputSink TidyOutputSink;

		internal OutputSink(Stream stream)
		{
			this.Stream = stream;
			this.data = GCHandle.Alloc(this);
			IntPtr sinkData = GCHandle.ToIntPtr(data);
			this.TidyOutputSink = new Interop.TidyOutputSink(sinkData, 
				new Interop.TidyPutByteFunc(OutputSinkCallback.OnPutByte));
		}

		#region IDisposable Members

		/// <summary>
		/// Disposes of all unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// Disposes of all unmanaged resources.
		/// </summary>
		/// <param name="disposing">Indicates whether the the document is already being disposed of.</param>
		protected virtual void Dispose(bool disposing)
		{
     		if (disposing && !disposed) 
			{
        		disposed = true;

      			if (data.IsAllocated) 
				{
        			data.Free ();
        			data = new GCHandle ();
      			}
			}
		}

		#endregion
	}
}
