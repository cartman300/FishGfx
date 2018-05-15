﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using Clr = System.Drawing.Color;

namespace FishGfx.Graphics.Drawables {
	public class Terrain : IDrawable {
		Mesh3D TerrainMesh;
		Texture Texture;

		int Width;
		int Height;
		float[] HeightData;

		public Terrain() {
			TerrainMesh = new Mesh3D();
			TerrainMesh.PrimitiveType = PrimitiveType.Triangles;
		}

		/// <param name="Img">Heightmap</param>
		/// <param name="ScaleValue">Image heightmap range, 255 default</param>
		public void LoadFromImage(Image Img, float ScaleValue = 255) {
			Texture = Texture.FromImage(Img);
			Texture.SetFilterSmooth();

			Width = Img.Width;
			Height = Img.Height;
			HeightData = new float[Width * Height];

			// TODO: Direct pixel access, faster
			using (Bitmap Bmp = new Bitmap(Img)) {
				for (int y = 0; y < Bmp.Height; y++)
					for (int x = 0; x < Bmp.Width; x++) {
						Clr Pixel = Bmp.GetPixel(x, y);

						// Black 'nd white
						float NormalizedHeight = ((Pixel.R + Pixel.G + Pixel.B) / 3) / 255.0f;
						HeightData[y * Width + x] = NormalizedHeight * ScaleValue;
					}
			}

			List<Vertex3> Verts = new List<Vertex3>();

			for (int y = 0; y < Height; y++)
				for (int x = 0; x < Width; x++) {
					float u = x / (float)Width;
					float ustep = 1 / (float)Width;

					float v = (Height - y) / (float)Height;
					float vstep = (1 / (float)Height);

					float HCurrent = GetHeight(x, y);
					float HRight = GetHeight(x + 1, y);
					float HBottom = GetHeight(x, y - 1);
					float HBottomRight = GetHeight(x + 1, y - 1);

					Vertex3 Current = new Vector3(x, HCurrent, y);
					Current.UV = new Vector2(u, v);

					Vertex3 Right = new Vector3(x + 1, HRight, y);
					Right.UV = new Vector2(u + ustep, v);

					Vertex3 Bottom = new Vector3(x, HBottom, y - 1);
					Bottom.UV = new Vector2(u, v + vstep);

					Vertex3 BottomRight = new Vector3(x + 1, HBottomRight, y - 1);
					BottomRight.UV = new Vector2(u + ustep, v + vstep);

					// Emit quad
					Verts.Add(Current);
					Verts.Add(Right);
					Verts.Add(Bottom);
					Verts.Add(Bottom);
					Verts.Add(Right);
					Verts.Add(BottomRight);
				}

			TerrainMesh.SetVertices(Verts.ToArray());
		}

		float GetHeight(int X, int Y) {
			if (X < 0)
				X = 0;

			if (Y < 0)
				Y = 0;

			if (X >= Width)
				X = Width - 1;

			if (Y >= Height)
				Y = Height - 1;

			return HeightData[Y * Width + X];
		}

		public void Draw() {
			Texture.BindTextureUnit();
			TerrainMesh.Draw();
			Texture.UnbindTextureUnit();
		}
	}
}
