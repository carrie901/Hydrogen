﻿#region Copyright Notice & License Information
//
// hWebPool.cs
//
// Author:
//       Matthew Davey <matthew.davey@dotbunny.com>
//
// Copyright (c) 2013 dotBunny Inc. (http://www.dotbunny.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using UnityEngine;

/// <summary>
/// A drop in implementation of the Hydrogen.Core.WebPool. This simply makes the class an accessible singleton 
/// with some very simple additional functionality.
/// </summary>
[AddComponentMenu ("Hydrogen/Web Pool Manager")]
public sealed class hWebPool : Hydrogen.Core.WebPool
{
		/// <summary>
		/// Should this web pool survive scene switches?
		/// </summary>
		public bool Presistant = true;
		/// <summary>
		/// Internal fail safe to maintain instance across threads.
		/// </summary>
		/// <remarks>
		/// Multithreaded Safe Singleton Pattern.
		/// </remarks>
		/// <description>
		/// http://msdn.microsoft.com/en-us/library/ms998558.aspx
		/// </description>
		static readonly System.Object _syncRoot = new System.Object ();
		/// <summary>
		/// Internal reference to the static instance of the web pool.
		/// </summary>
		static volatile hWebPool _staticInstance;

		/// <summary>
		/// Gets the web pool instance, creating one if none is found.
		/// </summary>
		/// <value>
		/// The Web Pool.
		/// </value>
		public static hWebPool Instance {
				get {
						if (_staticInstance == null) {
								lock (_syncRoot) {
										_staticInstance = FindObjectOfType (typeof(hWebPool)) as hWebPool;

										// If we don't have it, lets make it!
										if (_staticInstance == null) {
												var go = GameObject.Find (Hydrogen.Components.DefaultSingletonName) ??
												         new GameObject (Hydrogen.Components.DefaultSingletonName);

												go.AddComponent<hObjectPool> ();
												_staticInstance = go.GetComponent<hWebPool> ();	
										}
								}
						}
						return _staticInstance;
				}
		}

		/// <summary>
		/// Does a Web Pool already exist?
		/// </summary>
		public static bool Exists ()
		{
				return _staticInstance != null;
		}

		/// <summary>
		/// Unity's Awake Event
		/// </summary>
		protected override void Awake ()
		{
				// Make sure to do the object pools normal initialization
				base.Awake ();

				// Should this gameObject be kept around :) I think so.
				if (Presistant)
						DontDestroyOnLoad (gameObject);
		}
}