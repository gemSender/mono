//
// System.Runtime.Remoting.Metadata.W3cXsd2001.SoapDuration
//
// Authors:
//      Martin Willemoes Hansen (mwh@sysrq.dk)
//      Lluis Sanchez Gual (lluis@ximian.com)
//
// (C) 2003 Martin Willemoes Hansen
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

using System;
using System.Text;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001 
{
	public sealed class SoapDuration
	{
		public SoapDuration()
		{
		}
		
		public static string XsdType {
			get { return "duration"; }
		}

		public static TimeSpan Parse (string s)
		{
			if (s.Length == 0)
				throw new ArgumentException ("Invalid format string for duration schema datatype.");

			int start = 0;
			if (s [0] == '-')
				start = 1;
			bool minusValue = (start == 1);

			if (s [start] != 'P')
				throw new ArgumentException ("Invalid format string for duration schema datatype.");
			start++;

			int parseStep = 0;
			int days = 0;
			bool isTime = false;
			int hours = 0;
			int minutes = 0;
			int seconds = 0;

			bool error = false;

			int i = start;
			while (i < s.Length) {
				if (s [i] == 'T') {
					isTime = true;
					parseStep = 4;
					i++;
					start = i;
					continue;
				}
				for (; i < s.Length; i++) {
					if (!Char.IsDigit (s [i]))
						break;
				}
				int value = int.Parse (s.Substring (start, i - start));
				switch (s [i]) {
				case 'Y':
					days += value * 365;
					if (parseStep > 0)
						error = true;
					else
						parseStep = 1;
					break;
				case 'M':
					if (parseStep < 2) {
						days += 365 * (value / 12) + 30 * (value % 12);
						parseStep = 2;
					} else if (isTime && parseStep < 6) {
						minutes = value;
						parseStep = 6;
					}
					else
						error = true;
					break;
				case 'D':
					days += value;
					if (parseStep > 2)
						error = true;
					else
						parseStep = 3;
					break;
				case 'H':
					hours = value;
					if (!isTime || parseStep > 4)
						error = true;
					else
						parseStep = 5;
					break;
				case 'S':
					seconds = value;
					if (!isTime || parseStep > 6)
						error = true;
					else
						parseStep = 7;
					break;
				default:
					error = true;
					break;
				}
				if (error)
					break;
				++i;
				start = i;
			}
			if (error)
				throw new ArgumentException ("Invalid format string for duration schema datatype.");
			TimeSpan ts = new TimeSpan (days, hours, minutes, seconds);
			return minusValue ? -ts : ts;
		}

		public static string ToString (TimeSpan value)
		{
			StringBuilder builder = new StringBuilder();
			if (value.Ticks < 0) {
				builder.Append('-');
				value = value.Negate();
			}
			builder.Append('P');
			if (value.Days > 0) builder.Append(value.Days).Append('D');
			if (value.Days > 0 || value.Minutes > 0 || value.Seconds > 0 || value.Milliseconds > 0) {
				builder.Append('T');
				if (value.Hours > 0) builder.Append(value.Hours).Append('D');
				if (value.Minutes > 0) builder.Append(value.Minutes).Append('M');
				if (value.Seconds > 0 || value.Milliseconds > 0) {
					builder.Append(value.Seconds);
					if (value.Milliseconds > 0) builder.Append('.').Append(String.Format("{0:000}", value.Milliseconds));
					builder.Append('S');
				}
			}
			return builder.ToString();
		}
	}
}
