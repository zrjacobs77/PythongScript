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
    {
//MAIN-----------------------------------------------------------------------------------------------------------------------------------MAIN
        static void Main(string[] args){
            ExecuteServer();
        }
//MAIN-----------------------------------------------------------------------------------------------------------------------------------MAIN
        //private static SqlConnection connection;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;
        private static string connectionString;

        static void initializeDatabase(){
            server = "herdrtestdb.c42aqcn0bv1v.us-east-2.rds.amazonaws.com,3306";
            database = "herdr";
            uid = "herdru1";
            password = "ST3V3nordstrom<3";
            connectionString = "Server=" + server + ";" + "Database=" + 
            database + ";" + "User Id=" + uid + ";" + "Password=" + password + ";";

            //connection = new SqlConnection(connectionString);
            Console.WriteLine("INITIALIZED");
            //OpenConnection();
        }
        // private static bool OpenConnection()
        // {
        //     try
        //     {
        //         Console.WriteLine("TRY OPEN");
        //         connection.Open();
        //         Console.WriteLine("IT OPENED");
        //         //connection.ChangeDatabase("herdr");
        //         return true;
        //     }
        //     catch (SqlException ex)
        //     {
        //         Console.WriteLine("FAIL OPEN");
        //         //When handling errors, you can your application's response based 
        //         //on the error number.
        //         //The two most common error numbers when connecting are as follows:
        //         //0: Cannot connect to server.
        //         //1045: Invalid user name and/or password.
        //         switch (ex.Number)
        //         {
        //             case 0:
        //                 Console.WriteLine("Cannot connect to server.  Contact administrator");
        //                 break;

        //             case 1045:
        //                 Console.WriteLine("Invalid username/password, please try again");
        //                 break;
        //         }
        //         return false;
        //     }
        // }
        //Close connection
        // private bool CloseConnection()
        // {
        //     try
        //     {
        //         connection.Close();
        //         return true;
        //     }
        //     catch (SqlException ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //         return false;
        //     }
        // }
        
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
            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

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
                        case "1":
                            //query
                            // Console.WriteLine("CASE 1");
                            // SqlCommand command = new SqlCommand("GetProfile(01234567);", connection);
                            // using(SqlDataReader reader = command.ExecuteReader())
                            // {
                            //     while (reader.Read())
                            //     {
                            //         Console.WriteLine(String.Format("{0}, {1}",
                            //             reader[0], reader[1]));
                            //     }
                            // }
                            Console.WriteLine("CASE 1");
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                // Open the SqlConnection.
                                connection.Open();
                                Console.WriteLine("OPENED");

                                // This code uses an SqlCommand based on the SqlConnection.
                                using (SqlCommand command = new SqlCommand("GetProfile(01234567);", connection))
                                using (SqlDataReader reader = command.ExecuteReader()){
                                    while (reader.Read()){
                                        Console.WriteLine("{0} {1} {2}",
                                            reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                                    }
                                }
                            }
                            break;

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

                    byte[] message = Encoding.ASCII.GetBytes(data);

                    // Send a message to Client 
                    // using Send() method 
                    clientSocket.Send(message);

                    // Close client Socket using the 
                    // Close() method. After closing, 
                    // we can use the closed Socket 
                    // for a new Client Connection 
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    //connection.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
