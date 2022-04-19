using System;
using System.Threading;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Player_2{
    class Program{

        static class Globals{
            public static string URL = "http://192.168.1.4:9000";
            public static string PLAY_URL = URL + "/play";
            public static string RESULT_URL = URL + "/results";
            public static char[] board_status_ticks = {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '};
        }

        static void Main(string[] args){
            Console.Write("Player 2, Please Enter Your Name: ");
            var p_name = Console.ReadLine();

            int index_pos;

            var tick_type = "";
            while (true){
                try{
                    Console.Write(string.Format("{0}, Please Enter Your Symbol choice b/w X and O: ", p_name));
                    tick_type = Console.ReadLine();
                    if (tick_type == "X" || tick_type == "O" || tick_type == "x" || tick_type == "o"){
                        Console.WriteLine("Success, Game Started!");
                        break;
                    }
                    else{
                        throw new Exception();
                    }
                }
                catch (Exception){
                    Console.WriteLine("Sorry this symbol is not available, Try Again!");
                }
            }
            int playFlag = 1;
            display_board();
            int move_count = 0;
            while(playFlag == 1){
                try{
                    if (move_count > 0){
                        Console.WriteLine("You have only 10 seconds for your move. ");
                        Console.Write("Enter your move within Range of: 1-9 (and 0 to Pass) --> ");
                            
                        try{
                            index_pos = Convert.ToInt32(ConsoleReadLine.ReadLine(10000));
                        }catch{
                            index_pos = 0;
                        }

                        if (index_pos != 0){
                            JObject jsonResponse = send_data(p_name, tick_type, index_pos);
                            playFlag = Convert.ToInt32(jsonResponse["flag"]);
                            display_board();
                        }else{
                            JObject jsonResponse = send_data(p_name, tick_type, 0);
                            playFlag = Convert.ToInt32(jsonResponse["flag"]);
                            Console.WriteLine("You lost your chance!");
                        }
                        Console.WriteLine("Wait until Player 1 makes their move....");
                        while(true){
                            try{
                                if (get_result(false) == 0){
                                    throw new Exception();
                                }
                                var result_tick = get_data(Convert.ToChar(tick_type));
                                if (Convert.ToChar(tick_type) != result_tick){
                                    display_board();
                                    Console.WriteLine();
                                    break;
                                }
                                Console.Write(" - ");
                                Thread.Sleep(1000);
                            }catch{
                                display_board();
                                Console.WriteLine();
                                Thread.Sleep(1000);
                                get_result(true);
                                playFlag = 0;
                                break;
                            }
                        }
                    }else{
                        Console.WriteLine("Wait until Player 1 makes their move....");
                        while(true){
                            try{
                                if (get_result(false) == 0){
                                    throw new Exception();
                                }
                                var result_tick = get_data(Convert.ToChar(tick_type));
                                if ((Convert.ToChar(tick_type) != result_tick)){
                                    display_board();
                                    Console.WriteLine();
                                    break;
                                }
                                Console.Write(" - ");
                                Thread.Sleep(1000);
                            }catch{
                                display_board();
                                Console.WriteLine();
                                Thread.Sleep(1000);
                                get_result(true);
                                playFlag = 0;
                                break;
                            }
                        }
                    }
                }
                catch(Exception e){
                    Console.WriteLine(e);
                }
                move_count += 1;
            }
        }

        static void display_board(){
            get_board_status();
            Console.WriteLine("\n\n\n\n\n\n\n\n\n");
            Console.WriteLine("     |     |     ");
            Console.WriteLine("  {0}  |  {1}  |  {2}  ", Globals.board_status_ticks[0], Globals.board_status_ticks[1], Globals.board_status_ticks[2]);
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine("  {0}  |  {1}  |  {2}  ", Globals.board_status_ticks[3], Globals.board_status_ticks[4], Globals.board_status_ticks[5]);
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine("  {0}  |  {1}  |  {2}  ", Globals.board_status_ticks[6], Globals.board_status_ticks[7], Globals.board_status_ticks[8]);
            Console.WriteLine("     |     |     ");
            Console.WriteLine("\n\n");
        }

        static JObject send_data(string p_name, string tick_type, int index_pos)
        {
            using (var client = new HttpClient()){
                var endpoint = new Uri(Globals.PLAY_URL);
                var newPost = new Post(){
                    p_name = p_name,
                    tick_type = tick_type,
                    index_pos = index_pos
                };
                var newPostJson = JsonConvert.SerializeObject(newPost);
                var payload = new StringContent(newPostJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
                JObject jsonObject = JObject.Parse(result);
                return jsonObject;
            }
        }

        static Char get_data(Char tick_type)
        {
            using (var client = new HttpClient()){
                try{
                    var endpoint = new Uri(Globals.PLAY_URL);
                    var result = client.GetAsync(endpoint).Result;
                    var jsonFile = result.Content.ReadAsStringAsync().Result;
                    JObject jsonObject = JObject.Parse(jsonFile);
                    var finalResponse = jsonObject["response"];
                    JObject lastElement = (JObject)finalResponse.Last;
                    return (char)lastElement["tick_type"];
                }catch{
                    return tick_type;
                }
            }
        }

        static int get_result(Boolean print)
        {
            using (var client = new HttpClient()){
                var endpoint = new Uri(Globals.RESULT_URL);
                var result = client.GetAsync(endpoint).Result.Content.ReadAsStringAsync().Result;;
                JObject jsonObject = JObject.Parse(result);
                if (print){
                    Console.WriteLine(jsonObject["response"]);
                }
                return Convert.ToInt32(jsonObject["game"]);
            }
        }

        static void get_board_status()
        {
            using (var client = new HttpClient()){
                var endpoint = new Uri(Globals.PLAY_URL);
                var result = client.GetAsync(endpoint).Result.Content.ReadAsStringAsync().Result;;
                JObject jsonObject = JObject.Parse(result);
                JArray responseObj = (JArray)jsonObject["response"];

                JArray ticks = new JArray();
                JArray indexes = new JArray();

                for(int idx = 0; idx < responseObj.Count; idx++){
                    if (Convert.ToInt32(responseObj[idx]["index_pos"]) != 0){
                        Globals.board_status_ticks[Convert.ToInt32(responseObj[idx]["index_pos"]) - 1] = (char)responseObj[idx]["tick_type"];
                    }
                }
            }
        }
    }
}
