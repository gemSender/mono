//
// RSAPKCS1KeyExchangeFormatter.cs: Handles PKCS#1 v.1.5 keyex encryption.
//
// Author:
//	Sebastien Pouliot <sebastien@ximian.com>
//
// (C) 2002 Motus Technologies Inc. (http://www.motus.com)
// (C) 2004 Novell (http://www.novell.com)
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
using System.Globalization;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography {
	
	// LAMESPEC: There seems no way to select a hash algorithm. The default 
	// algorithm, is SHA1 because the class use the PKCS1MaskGenerationMethod -
	// which default to SHA1.
	public class RSAPKCS1KeyExchangeFormatter: AsymmetricKeyExchangeFormatter
	{
		private RSA rsa;
		private RandomNumberGenerator random;
	
		public RSAPKCS1KeyExchangeFormatter ()
		{
		}
	
		public RSAPKCS1KeyExchangeFormatter (AsymmetricAlgorithm key)
		{
			SetKey (key);
		}
	
		public RandomNumberGenerator Rng 
		{
			get { return random; }
			set { random = value; }
		}
	
		public override string Parameters 
		{
			get { return "<enc:KeyEncryptionMethod enc:Algorithm=\"http://www.microsoft.com/xml/security/algorithm/PKCS1-v1.5-KeyEx\" xmlns:enc=\"http://www.microsoft.com/xml/security/encryption/v1.0\" />"; }
		}
	
		public override byte[] CreateKeyExchange (byte[] rgbData)
		{
			if (random == null)
				random = RandomNumberGenerator.Create ();  // create default
			return PKCS1.Encrypt_v15 (rsa, random, rgbData);
		}
	
		public override byte[] CreateKeyExchange (byte[] rgbData, Type symAlgType)
		{
			// documentation says that symAlgType is not used !?!
			return CreateKeyExchange (rgbData);
		}
	
		public override void SetKey (AsymmetricAlgorithm key)
		{
			rsa = (RSA) key;
		}
	}
}
