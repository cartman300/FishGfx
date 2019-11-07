﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishGfx;
using FishGfx.Graphics;
using System.Diagnostics;

namespace FishGfx.Game {
	public abstract class FishGfxGame {
		protected RenderWindow Window;
		protected int Framerate = 60;


		public InputManager Input {
			get;
			private set;
		}

		protected virtual RenderWindow CreateWindow() {
			return new RenderWindow(800, 600, nameof(FishGfxGame));
		}

		protected ShaderProgram DefaultShader;

		protected virtual void CreateShaders() {
			DefaultShader = new ShaderProgram(new ShaderStage(ShaderType.VertexShader, "data/shaders/default3d.vert"), new ShaderStage(ShaderType.FragmentShader, "data/shaders/default.frag"));
		}

		protected virtual void CreateResources() {
			CreateShaders();
		}

		protected abstract void Init();

		protected abstract void Update(float Dt);

		protected abstract void Draw(float Dt);

		public static void Run(FishGfxGame Game) {
			Stopwatch SWatch = Stopwatch.StartNew();
			float Dt;

			Game.Window = Game.CreateWindow();
			Game.Input = new InputManager(Game.Window);

			ShaderUniforms.Current.Camera.SetOrthogonal(0, 0, Game.Window.WindowWidth, Game.Window.WindowHeight);

			Game.CreateResources();
			Game.Init();

			while (!Game.Window.ShouldClose) {
				if (Game.Framerate > 0)
					while (SWatch.ElapsedMilliseconds / 1000.0f < (1.0f / Game.Framerate))
						;

				Dt = SWatch.ElapsedMilliseconds / 1000.0f;
				SWatch.Restart();

				Game.Input.BeginNewFrame();
				Events.Poll();

				// TODO: Decouple draw and update

				Game.Draw(Dt);
				Game.Window.SwapBuffers();

				Game.Update(Dt);
			}
		}
	}
}
