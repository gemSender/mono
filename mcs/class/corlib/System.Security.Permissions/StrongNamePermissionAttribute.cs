//
// System.Security.Permissions.StrongNameIdentityPermissionAttribute.cs
//
// Duncan Mak <duncan@ximian.com>
//
// (C) 2002 Ximian, Inc.			http://www.ximian.com
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

namespace System.Security.Permissions {

	[AttributeUsage (AttributeTargets.Assembly | AttributeTargets.Class |
			 AttributeTargets.Struct | AttributeTargets.Constructor |
			 AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
	[Serializable]
	public sealed class StrongNameIdentityPermissionAttribute : CodeAccessSecurityAttribute	{

		// Fields
		private string name;
		private string key;
		private string version;
		
		// Constructor
		public StrongNameIdentityPermissionAttribute (SecurityAction action) : base (action) {}
		
		// Properties
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string PublicKey
		{
			get { return key; }
			set { key = value; }
		}

		public string Version
		{
			get { return version; }
			set { version = value; }
		}
			 
		// Methods
		public override IPermission CreatePermission ()
		{
			if (this.Unrestricted)
				throw new ArgumentException ("Unsupported PermissionState.Unrestricted");

			StrongNameIdentityPermission perm = null;
			if ((name == null) && (key == null) && (version == null))
				perm = new StrongNameIdentityPermission (PermissionState.None);
			else {
				if (key == null)
					throw new ArgumentException ("PublicKey is required");

				byte[] keyblob = Convert.FromBase64String (key);
				StrongNamePublicKeyBlob blob = new StrongNamePublicKeyBlob (keyblob);
				
				Version v = null;
				if (version != null)
					v = new Version (version);
				else
					v = new Version ();

				if (name == null)
					name = String.Empty;

				perm = new StrongNameIdentityPermission (blob, name, v);
			}
			return perm;
		}
	}
}
