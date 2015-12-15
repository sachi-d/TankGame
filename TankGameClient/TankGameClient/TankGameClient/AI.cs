using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TankGameClient
{
    class AI
    {

        #region variables
        TcpClient serv;
        NetworkStream stm;
        String server_ip = "localhost";

        String player_id;

        // mssages form the server
        string server_msg = "";

        #endregion

        #region messagepassingwithserver

        // accepting the server messages
        public void new_sever_msg(Object sender, ServerMessageArgs e)
        {
            this.server_msg = e.msg;
        }

        public void set_player_id(String id)
        {
            this.player_id = id;
        }

        public void send_message_to_server(String str)
        {

            try
            {
                serv = new TcpClient();
                serv.Connect(server_ip, 6000);
                stm = serv.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                stm.Close();
                serv.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
        #endregion






    }
}