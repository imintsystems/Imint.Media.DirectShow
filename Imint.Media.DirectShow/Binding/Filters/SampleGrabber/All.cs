﻿// 
//  All.cs
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
using Bitmap = Kean.Draw.Raster;

namespace Imint.Media.DirectShow.Binding.Filters.SampleGrabber
{
	public class All :
		Filters.Abstract
	{
		public Kean.Math.Fraction Rate { get; set; }
		public All() :
			this(new NullRenderer())
		{ }
		public All(params Filters.Abstract[] next) :
			base("All Sample Grabbers", new Yvu420(next), new Yuv420(next), new Yuyv(next), new Bgr(next), new Bgra(next))
		{ }
		public override bool Build(DirectShowLib.IPin source, IBuild build)
		{
			foreach (Filters.Abstract candidate in this.Next)
				candidate.FuzzyMatch = this.FuzzyMatch;
			bool result = false;
			foreach (Abstract candidate in this.Next)
			{
				candidate.Rate = this.Rate;
				if (result = candidate.Build(source, build))
					break;
			}
			return result;
		}
	}
}
