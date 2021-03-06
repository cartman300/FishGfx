﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using FishGfx.Graphics.Drawables;
using FishGfx.Graphics;
using System.Numerics;
using FishGfx.Game;

namespace Test {
	class LevelTile {
		public int X;
		public int Y;
		public int ID;
	}

	class LevelLayer {
		public string Name;
		// public string Tileset; 

		public LevelEntity[] Entities;
		public LevelTile[] Tiles;

		public override string ToString() {
			return string.Format("Layer '{0}'", Name);
		}
	}

	class GameLevel {
		public int Width;
		public int Height;

		[JsonProperty("time_limit")]
		public int TimeLimit;

		public string Background;

		public LevelLayer[] Layers;

		public static GameLevel FromFile(string FileName) {
			const bool InvertY = true;

			string JsonSrc = File.ReadAllText(FileName);
			GameLevel Lvl = JsonConvert.DeserializeObject<GameLevel>(JsonSrc);


			for (int i = 0; i < Lvl.Layers.Length; i++) {
				LevelLayer L = Lvl.Layers[i];

				if (L.Name == "entities")
					for (int j = 0; j < L.Entities.Length; j++) {
						LevelEntity Ent = L.Entities[j];
						Ent.Y = Lvl.Height - Ent.Y - 1;
					}
			}

			return Lvl;
		}

		public Tilemap LayerFore;
		public Tilemap LayerMain;
		public Tilemap LayerBack;

		void FillTilemap(Tilemap Map, LevelTile[] Tiles) {
			Texture TileAtlas = Map.GetTileAtlas();
			//int TileCountX = TileAtlas.Width / Map.TileSize;

			for (int i = 0; i < Tiles.Length; i++) {
				int TileX = Tiles[i].X;
				int TileY = Map.Height - Tiles[i].Y - 1;
				int TileIdx = Tiles[i].ID;

				Map.SetTile(TileX, TileY, TileIdx);
			}
		}

		public IEnumerable<LevelEntity> GetAllEntities() {
			for (int i = 0; i < Layers.Length; i++) {
				if (Layers[i].Name == "entities")
					for (int j = 0; j < Layers[i].Entities.Length; j++) {
						yield return Layers[i].Entities[j];
					}
			}
		}

		public IEnumerable<LevelEntity> GetEntitiesByName(string Name) {
			foreach (var E in GetAllEntities()) {
				if (E.Name == Name)
					yield return E;
			}
		}

		public void Init(ShaderProgram Shader) {
			Texture TileTex = Texture.FromFile("data/textures/tilemap.png");
			int TileSize = 16;

			int TilesX = Width / TileSize;
			int TilesY = Height / TileSize;

			for (int i = 0; i < Layers.Length; i++) {
				LevelLayer Layer = Layers[i];

				switch (Layer.Name) {
					case "foreground":
						LayerFore = new Tilemap(TileSize, TilesX, TilesY, TileTex);
						LayerFore.Shader = Shader;
						FillTilemap(LayerFore, Layer.Tiles);
						break;

					case "main":
						LayerMain = new Tilemap(TileSize, TilesX, TilesY, TileTex);
						LayerMain.Shader = Shader;
						FillTilemap(LayerMain, Layer.Tiles);
						break;

					case "background":
						LayerBack = new Tilemap(TileSize, TilesX, TilesY, TileTex);
						LayerBack.Shader = Shader;
						FillTilemap(LayerBack, Layer.Tiles);
						break;

					case "entities":
						break;

					default:
						throw new NotImplementedException();
				}
			}
		}

		public bool GetSolid(Vector2 Pos, out int TileID) {
			TileID = -1;

			if (LayerMain.TryWorldPosToTile(Pos, out int X, out int Y)) {
				if ((TileID = LayerMain.GetTile(X, Y)) != -1)
					return true;
			}

			return false;
		}
	}
}
