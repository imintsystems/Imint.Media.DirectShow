﻿// 
//  Network.cs
//  
//  Author:
//       Simon Mika <simon.mika@imint.se>
//  
//  Copyright (c) 2012-2013 Imint AB
// 
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//  the documentation and/or other materials provided with the distribution.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
using System;
using Uri = Kean.Core.Uri;
using Kean.Core.Extension;
using Serialize = Kean.Core.Serialize;

namespace Imint.Media.DirectShow.Axis
{
	public class Network :
		DirectShow.Stream,
		Media.Player.ICapture
	{
		protected override DirectShow.Binding.IGraph Open(Uri.Locator name)
		{
			DirectShow.Binding.IGraph result = null;
			if (name.Scheme.Head == "axrtsphttp" && name.Authority.NotNull() && name.Query["video"].IsNull())
			{
				name = name.Copy();
				result = new DirectShow.Binding.Graph(this.Application);
				if (this.Open(result, name))
				{
					result.Play();
					//result.Stop();
					//result.Play();
				}
				else
				{
					result.Close();
					result = null;
				}
			}
			return result;
		}
		bool Open(DirectShow.Binding.IGraph graph, Uri.Locator name)
		{
			bool result = graph.Open(new Filters.Net.RTPSource(name, new DirectShow.Binding.Filters.Decoder.Mpeg2(new DirectShow.Binding.Filters.Dmo.ColorConverter(new DirectShow.Binding.Filters.SampleGrabber.All() { FuzzyMatch = true } ))) { Output = 0 });
			graph.Save("file:///./test.grf");
			return result;
		}

		public System.Collections.Generic.IEnumerable<Resource> Devices
		{
			// TODO: Enumerate network streams
			get { yield return new Resource(ResourceType.Capture, "Axis Camera", "axrtsphttp://192.168.1.21/axis-media/media.amp?videocodec=h264"); }
		}
	}
}
