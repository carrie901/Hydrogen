﻿#region Copyright Notice & License Information
//
// AudioStackItem.cs
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

namespace Hydrogen.Core
{
		/// <summary>
		/// A class represention of all information needed for an AudioClip to be played via the AudioStack.
		/// </summary>
		[System.Serializable]
		public sealed class AudioStackItem
		{
				/// <summary>
				/// The associated AudioClip
				/// </summary>
				public AudioClip Clip;
				/// <summary>
				/// Key to be used by the AudioStack
				/// </summary>
				/// <remarks>
				/// Should be unique to the AudioClip's file name.
				/// </remarks>
				public string Key;
				/// <summary>
				/// Duration of time to use when fading in an AudioClip.
				/// </summary>
				public float FadeInTime = 4.5f;
				/// <summary>
				/// Duration of time to use when fading out an AudioClip.
				/// </summary>
				public float FadeOutTime = 5.0f;
				/// <summary>
				/// Should the AudioSource fade between volume levels?
				/// </summary>
				public bool Fade;
				/// <summary>
				/// Should the AudioSource loop the clip?
				/// </summary>
				public bool Loop;
				/// <summary>
				/// The max volume for the item.
				/// </summary>
				public float MaxVolume = 1f;
				/// <summary>
				/// Should the Audio Clip be played automatically upon load?
				/// </summary>
				public bool PlayOnLoad = true;
				/// <summary>
				/// Do not remove when finished playing does not get removed, useful for menu clicks etc
				/// </summary>
				public bool Persistant;
				/// <summary>
				/// Should the AudioPoolItem be destroyed, freeing it's AudioSource when it's volume reaches 0 after fading
				/// </summary>
				public bool RemoveAfterFadeOut = true;
				/// <summary>
				/// The AudioSource associated to this AudioStackItem.
				/// </summary>
				[System.NonSerialized]
				public AudioSource Source;
				/// <summary>
				/// Reference to the parent stack.
				/// </summary>
				[System.NonSerialized]
				public AudioStack Stack;
				/// <summary>
				/// The volume to use when the sound is first played.
				/// </summary>
				public float StartVolume = 1f;
				/// <summary>
				/// The volume which the AudioSource should gravitate towards.
				/// </summary>
				public float TargetVolume = 1f;

				public AudioStackItem ()
				{
				}

				public AudioStackItem (AudioClip clip)
				{
						Clip = clip;
						Key = clip.name;
				}

				public AudioStackItem (AudioClip clip, string key)
				{
						Clip = clip;
						Key = key;
				}

				public void Process ()
				{
						if (Source == null)
								return;

						if (TargetVolume > MaxVolume)
								TargetVolume = MaxVolume;

						if (Source.volume != TargetVolume) {
								if (Fade) {
										if (Source.volume > TargetVolume) {
												Source.volume = 
														Mathf.Lerp (Source.volume, TargetVolume, FadeOutTime * Time.deltaTime);
										} else {
												Source.volume = 
														Mathf.Lerp (Source.volume, TargetVolume, FadeInTime * Time.deltaTime);
										}
								} else {
										Source.volume = TargetVolume;
								}
						}

						// Automatically remove finished processes, but only if they are not marked persistant.
						// Not checking for Loop as if someone sets it's TargetVolume to 0 its meant to go away.
						if (!Persistant && ((Fade && RemoveAfterFadeOut && Source.volume < 0.0001f) || Source.time == Clip.length)) {

								Stack.Remove (this);
						}
				}
		}
}