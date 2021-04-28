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
        //private static string Library;
        private static string uid;
        private static string password;
        private static string connectionString;

        static void initializeDatabase(){
            /*
            server = "herdrtestdb.c42aqcn0bv1v.us-east-2.rds.amazonaws.com,3306";
            database = "herdr";
            uid = "herdru1";
            password = "ST3V3nordstrom<3";
            connectionString = "Server=" + server + ";" + "Database=" + 
            database + ";" + "User Id=" + uid + ";" + "Password=" + password + ";";*/

            //server = "tcp:3.23.163.170,3306";
            server = "tcp:127.0.0.1,3306";
            //Library = "DBMSSOCN"; 
            database = "herdrdb";

        //STANDARD USER
            //uid = "herdru1";
            //password = "ST3V3nordstrom<3";
        //ADMIN USER
            // uid = "admin";
            // password = "ctOqE9NPuC1WtJWXooSD";
        //NEW USER
            //uid = "ec2server";
            //password = "HavanaBanana!123";
        //NEWER USER
            uid = "dbadmin";
            password = "SeniorProject21";

            connectionString = 
                "Server=" + server + ";" + 
                //"Network=" + Library + ";" + 
                "Database=" + database + ";" +
                "User Id=" + uid + ";" + 
                "Password=" + password + ";";
            
            Console.WriteLine("Connection String: {0}", connectionString);
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
                    
                    //SPLIT DATA BY ";"
                    string[] messageIn = data.Split(";");

                    initializeDatabase();
                    Console.WriteLine("INITIALIZED");

                    //PROCESS DATA INTO QUERIES
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Open the SqlConnection.
                        connection.Open();
                        //connection.ChangeDatabase("herdr");
                        Console.WriteLine("OPENED");
                        
                        switch(messageIn[0]){
                            case "1":
                                Console.WriteLine("CASE 1");

                                // This code uses an SqlCommand based on the SqlConnection.
                                using (SqlCommand command = new SqlCommand("DESCRIBE hall;", connection))
                                using (SqlDataReader reader = command.ExecuteReader()){
                                    while (reader.Read()){
                                        Console.WriteLine("{0} {1}",
                                            reader.GetInt32(0), reader.GetString(1));
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
