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
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using System.Threading;

namespace TankGameClient
{

    public class ServerMessageArgs : EventArgs
    {
        public String msg;
    }


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // global game details
        int block_pixel_size = 40;
        int grid_size = 10;

        // decalring empty block 
        Block[,] blocks;


        // server
      //  IPAddress client_ip = IPAddress.Any;

        #region texture objects
        // declaring game texture objects

        SpriteFont title_f;
        Vector2 title_position;
        String title_text;
        Color title_color;

        // score board title
        SpriteFont score_title;
        Vector2 score_title_position;
        String score_title_text;
        Color score_title_color;

        // player details
        SpriteFont player;
        Vector2 player_position;
        Vector2 player_points_position;
        Vector2 player_coins_position;
        Vector2 player_health_position;
        String player_name_text;
        String player_health_text;
        String player_points_text;
        String player_coins_text;
        Color player_color;

        #endregion



        // details about our tank
        string player_id;

        public EventHandler<ServerMessageArgs> newMessage; //Represent the method that will handle the event (when server messages comes)

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = grid_size * block_pixel_size ;
            graphics.PreferredBackBufferWidth = grid_size * block_pixel_size + 300;
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
            #region grid

            blocks = new Block[grid_size, grid_size];
            // creating the block
            for (int i = 0; i < grid_size; i++)
            {
                for (int j = 0; j < grid_size; j++)
                {
                    blocks[i, j] = new Block(new Vector2(i * block_pixel_size, j * block_pixel_size ));
                }
            }

            #endregion
            #region text objects initializing
            // creating the title
            title_position = new Vector2((grid_size+1)*block_pixel_size, 10);
            title_text = "Tank Game Client Mode ";
            title_color = Color.White;

            // scoreboard title
            int title_left = (grid_size+1) * block_pixel_size;
            score_title_color = Color.Black;
            score_title_position = new Vector2((grid_size+2) * block_pixel_size, 60);
            score_title_text = "Score Board";

            // player details

            player_color = Color.Green;
            player_position = new Vector2(title_left, 100);
            player_points_position = new Vector2(title_left, 140);
            player_coins_position = new Vector2(title_left, 180); ;
            player_health_position = new Vector2(title_left, 220); ;
            player_name_text = "My Player"+this.player_id;
            player_coins_text += "My Coins: " ;
            player_points_text = "My Points: " ;
            player_health_text = "My Health: " ;

           

            #endregion

            Thread handleMsgThread = new Thread(handleMessages);
            handleMsgThread.Start();
            
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

            // loading the image of block
            for (int i = 0; i < grid_size; i++)
            {
                for (int j = 0; j < grid_size; j++)
                {
                    blocks[i, j].loadContent(Content);
                }
            }

            #region text_objects
            title_f = Content.Load<SpriteFont>("Fonts/title");
            score_title = Content.Load<SpriteFont>("Fonts/score_board_title");

            // player strings

            player= Content.Load<SpriteFont>("Fonts/player");
                         


            #endregion
            startCommunication();
             
        }

        #region communication
        public void startCommunication()
        {

            try
            {
                TcpClient sendSockect = new TcpClient();
                sendSockect.Connect("localhost", 6000);  //join the game
                String str = "JOIN#";
                NetworkStream serverStream = sendSockect.GetStream();

                ASCIIEncoding encode = new ASCIIEncoding();
                byte[] outStream = encode.GetBytes(str);
                serverStream.Write(outStream, 0, outStream.Length);
                Console.WriteLine("Sent Join#");

                serverStream.Close();
                sendSockect.Close();
                             

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        private void handleMessages()
        {
            TcpListener listnerSocket = new TcpListener(IPAddress.Any, 7000);
            Byte[] bytes = new Byte[1024];
            listnerSocket.Start();
            String data;

            while (true)
            {
                TcpClient gameServer = listnerSocket.AcceptTcpClient();
                data = null;
                NetworkStream serverStream = gameServer.GetStream();
                int i;
                while ((i = serverStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    formatMessage(data);    //format the message


                }
                gameServer.Close();
                serverStream.Close();
            }
        }
        
        #endregion

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
            GraphicsDevice.Clear(Color.SlateGray); //new Color(143, 143, 143)
            spriteBatch.Begin();
            #region grid
            // drawing the grid of blocks
            for (int i = 0; i < grid_size; i++)
            {
                for (int j = 0; j < grid_size; j++)
                {
                    blocks[i, j].draw(spriteBatch);
                }
            }
            #endregion

            #region text objects
            spriteBatch.DrawString(title_f, title_text, title_position, title_color);
            spriteBatch.DrawString(score_title, score_title_text, score_title_position, score_title_color);

            spriteBatch.DrawString(player, player_name_text, player_position, player_color);
            spriteBatch.DrawString(player, player_coins_text, player_coins_position, player_color);
            spriteBatch.DrawString(player, player_health_text, player_health_position, player_color);
            spriteBatch.DrawString(player, player_points_text, player_points_position, player_color);

            #endregion
            
            spriteBatch.End();
                     

            base.Draw(gameTime);
        }





        #region formatmessage
        private void formatMessage(String msg)
        {
            String[] firstpart = msg.Split('#');
            String[] parts = firstpart[0].Split(':');
            try
            {
                String msg_format = parts[0];
                if (msg_format.Equals("I")) // game instant received
                {
                    game_instant(parts);
                }
                else if (msg_format.Equals("G")) // global update received
                {
                   global_update(parts);
                }
                else if (msg_format.Equals("C")) // coin detail received
                {
                    coin_detail(parts);
                }
                else if (msg_format.Equals("L")) // Life Pack detail received
                {
                    lifePack_detail(parts);
                }
                else if (msg_format.Equals("S")) // acceptance received
                {
                    acceptance(parts);
                }
                else // mooving or shooting state received
                {
                    //mooving_shooting(parts);
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }


           private void game_instant(String[] parts)
        {

            this.player_id = parts[1];
            player_name_text = "My Player: " + this.player_id;
        
            // reading bricks
            String brick_map = parts[2];
            String[] bricks = brick_map.Split(';');
            foreach (String brick in bricks)
            {
                String[] brick_location = brick.Split(',');                
                blocks[int.Parse(brick_location[0]), int.Parse(brick_location[1])].change_type(1,Content,-100);
               
                
            }

            // reading stones
            String stone_map = parts[3];
            String[] stones = stone_map.Split(';');
            foreach (String stone in stones)
            {
                String[] stone_location = stone.Split(',');
                blocks[int.Parse(stone_location[0]), int.Parse(stone_location[1])].change_type(2,Content,-100);
              
            }

            // reading water
            String water_map = parts[4];
            String[] waters = water_map.Split(';');
            foreach (String water in waters)
            {
                String[] water_location = water.Split(',');
                blocks[int.Parse(water_location[0]), int.Parse(water_location[1])].change_type(3,Content,-100);
               
            }

        }
           private void global_update(String[] parts)
           {

               int player_no = 1;
               for (player_no = 1; player_no <= 5; player_no++)
               {
                   String player_code = parts[player_no];
                   if (player_code.Substring(0, 1).Equals("P")) // this is a player sub string
                   {
                       String[] player_details = player_code.Split(';');
                       String player_id = player_details[0];
                       String[] player_log = player_details[1].Split(',');
                       int direction = int.Parse(player_details[2]);
                       int shotted = int.Parse(player_details[3]);
                       int health = int.Parse(player_details[4]);
                       int coins = int.Parse(player_details[5]);
                       int points = int.Parse(player_details[6]);
                       remove_redundant_tanks(player_id);

                       bool our = false;
                       if (String.Compare(player_id, this.player_id) == 0) // if this is our tank
                       {
                           our = true;
                             player_coins_text = "My Coins: " + coins ;
                             player_points_text = "My Points: " + points; 
                             player_health_text = "My Health: " + health;
                       }

                       blocks[int.Parse(player_log[0]), int.Parse(player_log[1])].change_type(player_id, Content, direction, shotted, health, coins, points, our);

                   }
                   else
                       break;
               }
              check_life(parts[player_no]);

           }
           public void remove_redundant_tanks(String id)
           {
               for (int i = 0; i < grid_size; i++)
               {
                   for (int j = 0; j < grid_size; j++)
                   {
                       if (blocks[i, j].get_type() == 6) // if it is a tank
                       {
                           if (String.Compare(blocks[i, j].get_tank_id(), id) == 0) // if this block contains the tank we need
                           {
                               blocks[i, j].change_type(0, Content, -100); // change it to an empty block
                           }
                       }

                   }
               }
           }
           private void check_life(String msg)
           {
               String[] details = msg.Split(';');
               for (int i = 0; i < details.Length; i++) // iterating through bricks
               {
                   String[] brick = details[i].Split(',');
                   int x = int.Parse(brick[0]);
                   int y = int.Parse(brick[1]);
                   int life = int.Parse(brick[2]);
                   if (life == 4) // if the brick is destroid
                   {
                       blocks[x, y].change_type(0, Content, -100); // change it to an empty cell
                   }
                  
               }
           }
           private void coin_detail(String[] parts)
           {


               String[] location_str = parts[1].Split(',');
               blocks[int.Parse(location_str[0]), int.Parse(location_str[1])].change_type(4, Content, int.Parse(parts[2]));

               //   int val = int.Parse(parts[3]);
           }
           private void lifePack_detail(String[] parts)
           {


               String[] location_str = parts[1].Split(',');
               blocks[int.Parse(location_str[0]), int.Parse(location_str[1])].change_type(5, Content, int.Parse(parts[2]));


           }
           private void acceptance(String[] parts)
           {
               Console.Write(parts);
               // initializing player id
               String[] details = parts[1].Split(';');
               String player_id = details[0];
               String[] location_str = details[1].Split(',');
               int[] location = new int[] { int.Parse(location_str[0]), int.Parse(location_str[1]) };
               int direction = int.Parse(details[2]);
               //   ai.set_player_id(this.player_id);
               bool our = false;
              if (String.Compare(player_id, this.player_id) == 0) // if this is our tank
              {
                  our = true;
                  

              }
              blocks[int.Parse(location_str[0]), int.Parse(location_str[1])].change_type(player_id, Content, direction, -100, -100, -100, -100, our);

//               ai.start_ai();

               

           }
           private void mooving_shooting(String[] parts)
           {
               // send server msg to the ai
               ServerMessageArgs serverMessageArgs = new ServerMessageArgs();
               serverMessageArgs.msg = parts[0];
              // Console.WriteLine(parts[0]);
               newMessage(this, serverMessageArgs);
           }
        #endregion

        
        
    }
}
