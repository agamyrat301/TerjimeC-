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

            MySqlConnection dbConn = new MySqlConnection("server=localhost;database=fromdurdy;uid=root;pwd=;");
            dbConn.Open();
            var cmd = new MySqlCommand("select ID,QUESTION_TK,ANSWER_1_TK,ANSWER_2_TK,ANSWER_3_TK,ANSWER_4_TK " +
                "from questions", dbConn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                row[num] = reader.GetString(0).Trim() + "<r>" + reader.GetString(1) + "<r>" +
                    reader.GetString(2) + "<r>" + reader.GetString(3) + "<r>" + reader.GetString(4) + "<r>" + reader.GetString(5) + "<r>" + "<wroww>";

                if (num == 3)
                {
                    string respbody = TranslateQuestion("tk", "ru", row[0] + row[1] + row[2] + row[3]);
                    string[] MyArr = respbody.Split(new string[] { "<wroww>" }, StringSplitOptions.None);
                        foreach (var arr in MyArr)
                        {
                            string[] yoww = arr.Split(new string[] { "<r>" }, StringSplitOptions.None);
                            if (yoww.Length > 5)
                                UpdateQuestion(yoww[0], yoww[1], yoww[2], yoww[3], yoww[4], yoww[5]);
                        }
                    num = 0;

                }
                num++;
            }
            Console.ReadKey();
        }

        private static void UpdateQuestion(string id, string question, string answer_1, string answer_2, string answer_3, string answer_4)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;database=fromdurdy;uid=root;pwd=;"))
            {
                string Upquery = "UPDATE questions SET QUESTION_RU=@question, ANSWER_1_RU=@answer_1, ANSWER_2_RU=@answer_2,ANSWER_3_RU=@answer_3, ANSWER_4_RU=@answer_4 WHERE ID=@id";
                con.Open();
                MySqlCommand newCmd = new MySqlCommand();
                newCmd.CommandText = Upquery;
                newCmd.Connection = con;
                newCmd.Parameters.AddWithValue("@id", id.Trim());
                newCmd.Parameters.AddWithValue("@question", question);
                newCmd.Parameters.AddWithValue("@answer_1", answer_1);
                newCmd.Parameters.AddWithValue("@answer_2", answer_2);
                newCmd.Parameters.AddWithValue("@answer_3", answer_3);
                newCmd.Parameters.AddWithValue("@answer_4", answer_4);
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
