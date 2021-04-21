// A C# Program for Server 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data.SqlClient;
//using Renci.SshNet;

namespace Server
{
    
    class Program
    {/*
        struct SSHUser
        {
            string user;
            string pass;
            string host;
        };
        // Main Method 
        static void Main(string[] args)
        {
            SSHUser sshUser;
            sshUser.user = "herdru1";
            sshUser.pass = "ST3V3nordstrom<3";
            sshUser.host = = "herdrtestdb.c42aqcn0bv1v.us-east-2.rds.amazonaws.com";
            var client = new SshClient(sshUser);
            //Set up the SSH connection
            using (client)
            {
                //Start the connection
                client.Connect();
                var output = client.RunCommand("echo test");
                client.Disconnect();
                Console.WriteLine(output.Result);
            };
            ExecuteServer();
        }
        */
//MAIN-----------------------------------------------------------------------------------------------------------------------------------MAIN
        static void Main(string[] args){
            ExecuteServer();
        }
//MAIN-----------------------------------------------------------------------------------------------------------------------------------MAIN
        private static SqlConnection connection;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;

        static void initializeDatabase(){
            server = "herdrtestdb.c42aqcn0bv1v.us-east-2.rds.amazonaws.com";
            database = "herdrtestdb";
            uid = "herdru1";
            password = "ST3V3nordstrom<3";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + 
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new SqlConnection(connectionString);
        }
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        
        public static void ExecuteServer()
        {
            // Establish the local endpoint 
            // for the socket. Dns.GetHostName 
            // returns the name of the host 
            // running the application. 
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            // Creation TCP/IP Socket using 
            // Socket Class Costructor 
            Socket listener = new Socket(ipAddr.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Using Bind() method we associate a 
                // network address to the Server Socket 
                // All client that will connect to this 
                // Server Socket must know this network 
                // Address 
                listener.Bind(localEndPoint);

                // Using Listen() method we create 
                // the Client list that will want 
                // to connect to Server 
                listener.Listen(10);

                while (true) //LOOP AWAITING CONNECTIONS
                {

                    Console.WriteLine("Waiting connection ... ");

                    // Suspend while waiting for 
                    // incoming connection Using 
                    // Accept() method the server 
                    // will accept connection of client 
                    Socket clientSocket = listener.Accept();
                    // Data buffer 
                    byte[] bytes = new Byte[16];
                    string data = null;

                    while (true) //READ DATA IN UNTIL "<EOF>"
                    {

                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    
                    Console.WriteLine("Text received -> {0} ", data);
                    //byte[] message = Encoding.ASCII.GetBytes("Test Server tell me if you get this.");
                    
                    //SPLIT DATA BY ";"
                    string[] messageIn = data.Split(";");

                    initializeDatabase();

                    //PROCESS DATA INTO QUERIES
                    switch(messageIn[0]){
                        case "Log In Request":

                            break;

                        case "Get LocalUser":

                            break;
                        
                        case "Request more Profiles":

                            break;

                        case "Confirm Match":

                            break;

                        case "Block User 2 for user 1.":

                            break;

                        case "Report User for: ":

                            break;

                        case "Refresh Notifications":

                            break;

                        case "Create Profile":

                            break;

                        case "Delete Profile":

                            break;
                        
                        case "Send Contact Request":

                            break;
                        
                        case "Update Profile":

                            break;

                        case "Search For User":

                            break;

                        default:
                            Console.WriteLine("DEFAULT CASE\n");
                            break;
                    }

                    byte[] message = BitConverter.GetBytes(bool.Parse(data));

                    // Send a message to Client 
                    // using Send() method 
                    clientSocket.Send(message);

                    // Close client Socket using the 
                    // Close() method. After closing, 
                    // we can use the closed Socket 
                    // for a new Client Connection 
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
