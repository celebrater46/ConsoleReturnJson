using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ConsoleReturnJson
{
    class Program
    {
        // static void Main(string[] args)
        // {
        //     Console.WriteLine("Hello World!");
        // }
        
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            // Request Bodyに乗せるJSONを定義します。
            var json = "{\"question\":\"ちょうしはどう？\"}";
 
            // Response Bodyに含まれるJSONを格納するインスタンスを生成します。
            var answer = new Answer();
 
            using (var client = new HttpClient())
            {
                // Request BodyのContent-Typeや文字コードを定義します。
                var content = new StringContent(json, Encoding.UTF8, "application/json");
 
                // 先ほど定義したリクエストの内容に従って、指定したURLにPostメソッドでリクエストします。
                var httpResponse = await client.PostAsync("http://api.example.com?apiKey=hogehoge", content);
 
                // サーバーからのレスポンスをテキスト形式で受け取ります。ここで返ってくるレスポンスは
                // {"answer": "(´・ω・｀)"}というテキスト形式なので、ちょっと扱いにくいです。
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
 
                // 扱いにくいテキスト形式のJSONをオブジェクト(後に定義しているAnswerクラス)に変換します。
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(responseContent))) {
                    var ser = new DataContractJsonSerializer(answer.GetType());
                    answer = ser.ReadObject(ms) as Answer;
                }            
            }
 
            // JSONの内容をコンソールに表示します。
            Console.WriteLine(answer.answer);
        }
    }
    
    // {"answer": "(´・ω・｀)"}というJSONをオブジェクトに変換するためのクラスです。
    [DataContract]
    public class Answer
    {
        // JSONのフィールド名とプロパティ名は合わせなければいけません。
        // {"answer": "(´・ω・｀)"}のフィールド名はanswerなので、プロパティ名もanswerになります。
        [DataMember]
        public string answer;
    }
}