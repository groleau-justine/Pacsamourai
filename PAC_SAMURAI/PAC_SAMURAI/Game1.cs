using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PAC_SAMURAI
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Pacsamourai : Microsoft.Xna.Framework.Game
    {
        const int tailleWidth = 32;
        const int tailleHeight = 32;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map useMap;
        Objet mur;

        public Pacsamourai()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            useMap = new Map(0);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            // Read the file and put it in useMap
            mur = new Objet(Content.Load<Texture2D>("mur01"));
            useMap.loadMap();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (int x = 0; x < useMap.MaxMapX; x++)
            {
                for (int y = 0; y < useMap.MaxMapY; y++)
                {
                    Vector2 coord = new Vector2(y * tailleWidth, x * tailleHeight);

                    if (useMap.MapGame[x, y] == 0)
                        spriteBatch.Draw(mur.Texture, coord, Color.White);
                    else if (useMap.MapGame[x, y] == 1)
                        spriteBatch.Draw(mur.Texture, coord, Color.Black);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
