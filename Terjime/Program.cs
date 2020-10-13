using MySql.Data.MySqlClient;
using System;
using System.Net.Http;

namespace Terjime
{
    class Program
    {
        public static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            string[] row = new string[4];
            int num = 0;

            MySqlConnection dbConn = new MySqlConnection("server=localhost;database=durdysese;uid=root;pwd=;");
            dbConn.Open();
            var cmd = new MySqlCommand("select ID,QUESTION_TK,ANSWER_1_TK,ANSWER_2_TK,ANSWER_3_TK,ANSWER_4_TK " +
                "from questions where ID = 474242", dbConn);
            var reader = cmd.ExecuteReader();


            while (reader.Read())
            {


              string target = reader.GetString(0) + "<r>" + reader.GetString(1) + "<r>" + 
                    reader.GetString(2) + "<r>" + reader.GetString(3) + "<r>" + reader.GetString(4) + "<r>" + reader.GetString(5);

                Console.WriteLine(target);

                //string respbody = TranslateQuestion("tk", "ru", target);
                //Console.WriteLine(respbody);


                //row[num] = reader.GetString(0).Trim() + "<r>" + reader.GetString(1) + "<r>" +
                //     reader.GetString(2) + "<r>" + reader.GetString(3) + "<r>" + reader.GetString(4) + "<r>" + reader.GetString(5) + "<wroww>";




                //if (num == 3)
                //{

                //    string respbody = TranslateQuestion("tk", "ru", row[0] + row[1] + row[2] + row[3]);

                //    Console.WriteLine(respbody);
                //    string[] MyArr = respbody.Split("<wroww>");

                //    foreach (var arr in MyArr)
                //    {
                //        string[] yoww = arr.Split("<r>");
                //        Console.WriteLine(yoww[0]);
                //    }


                //    num = 0;

                //}


                //num++;


            }
            Console.ReadKey();

        }




        private static void UpdateQuestion(string id, string question, string answer_1, string answer_2, string answer_3, string answer_4)
        {

            using (MySqlConnection con = new MySqlConnection("server=localhost;database=fromdurdy;uid=root;pwd=;"))
            {
                string Upquery = "UPDATE questions SET QUESTION_TK=@question, ANSWER_1_TK=@answer_1, " +
                "ANSWER_2_TK=@answer_2,ANSWER_3_TK=@answer_3, ANSWER_4_TK=@answer_4 WHERE id=@id";
                con.Open();
                MySqlCommand newCmd = new MySqlCommand();
                newCmd.CommandText = Upquery;
                newCmd.Connection = con;
                newCmd.Parameters.AddWithValue("@id", id);
                newCmd.Parameters.AddWithValue("@question_tm", question);
                newCmd.Parameters.AddWithValue("@answer_1_tm", answer_1);
                newCmd.Parameters.AddWithValue("@answer_2_tm", answer_2);
                newCmd.Parameters.AddWithValue("@answer_3_tm", answer_3);
                newCmd.Parameters.AddWithValue("@answer_4_tm", answer_4);
                newCmd.ExecuteNonQuery();
                con.Close();
            }
        }



        public static String TranslateQuestion(string langFrom, string langTo, string AnswerTxt)
        {
            string web_app_url1 = "https://script.google.com/macros/s/AKfycbw34EB9D-RFgRlKQvoRmH7p6RArcw7QK7oIXuAZ3viDIjG0dSE/exec";
            string web_app_url2 = "https://script.google.com/macros/s/AKfycbw9uZCaBAejBJYmMYyWmuv9O5XSZoNS3HCdGzTveCbizi3qDJk/exec";
            string web_app_url3 = "https://script.google.com/macros/s/AKfycbz6sYfM2kZpjoVDOBwoYFbnxN_4c9PCTqvDrvHwLkyhbSwniwwe/exec";

            string url = web_app_url3 + "?q="
                + AnswerTxt + "&target=" + langTo + "&source=" + langFrom;


            HttpResponseMessage response = client.GetAsync(url).Result;
            var resultBody = response.Content.ReadAsStringAsync().Result;
            return resultBody;


        }




    }
}
