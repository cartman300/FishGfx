﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

using System.Numerics;

namespace FishGfx.Graphics {
	public static class Gfx {
		static CommandList List = null;

		public static void BeginRecord(CommandList L = null) {
			if (List != null)
				throw new Exception("Recording already in progress");

			List = L != null ? L : new CommandList();
		}

		public static CommandList EndRecord() {
			if (List == null)
				throw new Exception("Recording not in progress");

			CommandList Ret = List;
			List = null;
			return Ret;
		}

		public static void Clear() {
			if (List != null) {
				List.Enqueue(() => Clear());
				return;
			}

			Gl.ClearColor(69 / 255.0f, 112 / 255.0f, 56 / 255.0f, 1.0f);
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		public static void Line(Vertex2 Start, Vertex2 End) {
			if (List != null) {
				List.Enqueue(() => Line(Start, End));
				return;
			}

			// TODO
		}

		public static void LineStrip(Vertex2[] Points) {
			if (List != null) {
				List.Enqueue(() => LineStrip(Points));
				return;
			}

			// TODO
		}

		public static void Bezier(Vector2 Start, Vector2 End) {
			if (List != null) {
				List.Enqueue(() => Bezier(Start, End));
				return;
			}

			// TODO
		}
	}
}
