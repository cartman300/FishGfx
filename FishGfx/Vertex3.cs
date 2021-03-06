﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FishGfx {
	public unsafe struct Vertex3 {
		public Vector3 Position;
		public Vector2 UV;
		public Color Color;

		public Vertex3(Vertex3 Clone) : this(Clone.Position, Clone.UV, Clone.Color) {
		}

		public Vertex3(Vertex3 Clone, Color NewClr) : this(Clone) {
			Color = NewClr;
		}

		public Vertex3(Vector3 Pos, Vector2 UV, Color Clr) {
			Position = Pos;
			this.UV = UV;
			Color = Clr;
		}

		public Vertex3(float X, float Y, float Z) : this(new Vector3(X, Y, Z)) {
		}

		public Vertex3(Vector3 Pos) : this(Pos, Vector2.Zero, Color.White) {
		}

		public Vertex3(Vector3 Pos, Color Clr) : this(Pos, Vector2.Zero, Clr) {
		}

		public Vertex3(Vector3 Pos, Vector2 UV) : this(Pos, UV, Color.White) {
		}

		public byte[] ToByteArray() {
			byte[] Bytes = new byte[sizeof(Vertex3)];

			fixed (Vertex3* ThisPtr = &this) {
				byte* ThisBytes = (byte*)ThisPtr;

				for (int i = 0; i < Bytes.Length; i++)
					Bytes[i] = ThisBytes[i];
			}

			return Bytes;
		}

		public static implicit operator Vertex3(Vector3 Pos) {
			return new Vertex3(Pos);
		}

		public static Vertex3[] FromFloatArray(float[] Positions) {
			Vertex3[] Result = new Vertex3[Positions.Length / 3];

			for (int i = 0; i < Result.Length; i++) {
				int idx = i * 3;
				Result[i] = new Vertex3(new Vector3(Positions[idx], Positions[idx + 1], Positions[idx + 2]));
			}

			return Result;
		}

		public static Vertex3 FromByteArray(byte[] Bytes) {
			Vertex3 Vtx = new Vertex3();
			byte* VtxPtr = (byte*)&Vtx;

			for (int i = 0; i < Bytes.Length; i++)
				VtxPtr[i] = Bytes[i];

			return Vtx;
		}
	}
}
