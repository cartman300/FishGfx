﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FishGfx;

namespace FishGfx.Formats {
	public static class Obj {
		public static void Save(string FileName, Vertex3[] Verts) {
			using (StreamWriter SW = new StreamWriter(FileName)) {

				foreach (var Vert in Verts) {
					SW.WriteLine(string.Format(CultureInfo.InvariantCulture, "v {0} {1} {2}", Vert.Position.X, Vert.Position.Y, Vert.Position.Z));
					SW.WriteLine(string.Format(CultureInfo.InvariantCulture, "vt {0} {1}", Vert.UV.X, Vert.UV.Y));
				}

				for (int i = 0; i < Verts.Length; i += 3)
					SW.WriteLine("f {0}/{0} {1}/{1} {2}/{2}", i + 1, i + 2, i + 3);
			}
		}

		public static Vertex3[] Load(string FileName) {
			List<Vertex3> ObjVertices = new List<Vertex3>();
			string[] Lines = File.ReadAllLines(FileName);

			List<Vector3> Verts = new List<Vector3>();
			List<Vector2> UVs = new List<Vector2>();

			for (int j = 0; j < Lines.Length; j++) {
				string Line = Lines[j].Trim();

				if (Line.StartsWith("#"))
					continue;

				string[] Tokens = Line.Split(' ');
				switch (Tokens[0].ToLower()) {
					case "o":
						break;

					case "v": // Vertex
						Verts.Add(new Vector3(Tokens[1].ParseFloat(), Tokens[2].ParseFloat(), Tokens[3].ParseFloat()));
						break;

					case "vt": // Texture coordinate
						UVs.Add(new Vector2(Tokens[1].ParseFloat(), Tokens[2].ParseFloat()));
						break;

					case "vn": // Normal
						break;

					case "f": // Face
						for (int i = 2; i < Tokens.Length - 1; i++) {
							string[] V = Tokens[1].Split('/');
							ObjVertices.Add(new Vertex3(Verts[V[0].ParseInt() - 1], UVs[V[1].ParseInt() - 1]));

							V = Tokens[i].Split('/');
							ObjVertices.Add(new Vertex3(Verts[V[0].ParseInt() - 1], UVs[V[1].ParseInt() - 1]));

							V = Tokens[i + 1].Split('/');
							ObjVertices.Add(new Vertex3(Verts[V[0].ParseInt() - 1], UVs[V[1].ParseInt() - 1]));
						}

						break;

					default:
						break;
				}
			}

			return ObjVertices.ToArray();
		}
	}
}