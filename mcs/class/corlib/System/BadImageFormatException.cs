//
// System.BadImageFormatException.cs
//
// Authors:
//   Sean MacIsaac (macisaac@ximian.com)
//   Duncan Mak (duncan@ximian.com)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//
// (C) 2001 Ximian, Inc.
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Runtime.Serialization;

namespace System
{
	[Serializable]
	public class BadImageFormatException : SystemException
	{
		const int Result = unchecked ((int)0x8007000B);

		// Fields
		private string fileName;
		private string fusionLog;

		// Constructors
		public BadImageFormatException ()
			: base (Locale.GetText ("Invalid file image."))
		{
			HResult = Result;
		}

		public BadImageFormatException (string message)
			: base (message)
		{
			HResult = Result;
		}

		protected BadImageFormatException (SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
			fileName = info.GetString ("BadImageFormat_FileName");
			fusionLog = info.GetString ("BadImageFormat_FusionLog");
		}

		public BadImageFormatException (string message, Exception innerException)
			: base (message, innerException)
		{
			HResult = Result;
		}

		public BadImageFormatException (string message, string fileName)
			: base (message)
		{
			this.fileName = fileName;
			HResult = Result;
		}

		public BadImageFormatException (string message, string fileName, Exception innerException)
			: base (message, innerException)
		{
			this.fileName = fileName;
			HResult = Result;
		}

		// Properties
		public override string Message
		{
			get { return base.Message; }
		}

		public string FileName
		{
			get { return fileName; }
		}

		[MonoTODO ("Probably not entirely correct. fusionLog needs to be set somehow (we are probably missing internal constuctor)")]
		public string FusionLog
		{
			get { return fusionLog; }
		}

		// Methods
		public override void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue ("BadImageFormat_FileName", fileName);
			info.AddValue ("BadImageFormat_FusionLog", fusionLog);
		}

		public override string ToString ()
		{
			if (fileName != null)
				return Locale.GetText ("Filename: ") + fileName;
			return base.ToString ();
		}
	}
}
