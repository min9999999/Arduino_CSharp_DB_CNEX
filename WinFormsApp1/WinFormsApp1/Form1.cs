using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using Newtonsoft.Json;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using static WinFormsApp1.Form1;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.DataFormats;
using System.Text;


namespace WinFormsApp1
{ 
    public partial class Form1 : Form
    {
        public TcpListener server;
        public NetworkStream stream;
        public bool isRunning = false;
        string message = "0";
        string response = "";
        bool cDataProcessed = false; // C_DATA가 처리되었는지 여부를 나타내는 플래그


        // winform 실행시 서버를 켠다.
        public Form1()
        {
            InitializeComponent();

            server = new TcpListener(IPAddress.Any, 7000);
        }

      // Firebase 클라이언트 세팅
        class connection
        {
            public IFirebaseConfig fc = new FirebaseConfig()
            {
                AuthSecret = "E09Dva4dBklDbRCYPQi9TGPkvCU8Ut0vn9Iji737",
                BasePath = "https://arduinoconnectex-default-rtdb.firebaseio.com/"
            };

            public IFirebaseClient client;
            //Code to warn console if class cannot connect when called.
            public connection()
            {
                try
                {
                    client = new FireSharp.FirebaseClient(fc);
                }
                catch (Exception)
                {
                    Console.WriteLine("오류가 발생했습니다.");
                }
            }
        }
      
        public class Data
        {
            public string Timestamp { get; set; }
            public string Category { get; set; }
            public int Value { get; set; }
        }

        SerialPort port = new SerialPort();
        Crud crud = new Crud();


        class Crud
        {
            connection conn = new connection();

            //set datas to database
            public async Task SetData(string Category, int Value, string Timestamp)
            {
                try
                {
                    Data data = new Data()
                    {
                        Timestamp = Timestamp,
                        Category = Category,
                        Value = Value
                    };
                    var SetData = conn.client.Set("Arduino/Log/" + Timestamp, data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

            //Update datas
            public async Task UpdateData(string Category, int Value, string Timestamp)
            {
                try
                {
                    Data data = new Data()
                    {
                        Timestamp = Timestamp,
                        Category = Category,
                        Value = Value
                        
                    };
                    var SetData = conn.client.Update("Arduino/Log/" + Timestamp, data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            //List of the datas
            public Dictionary<string, Data> LoadData()
            {
                try
                {
                    FirebaseResponse al = conn.client.Get("Arduino/Log/");
                    Dictionary<string, Data> ListData = JsonConvert.DeserializeObject<Dictionary<string, Data>>(al.Body.ToString());
                    return ListData;
                }
                catch (Exception)
                {
                    Console.WriteLine("오류가 발생했습니다");
                    return null;
                }
            }
        }
         private async void button3_Click(object sender, EventArgs e)
        {
            // 아두이노가 연결이 된다는 조건하에 아래 내용들이 실행됨.
            if (!port.IsOpen) return;

            // 아두이노에 "1" 전송
            port.Write("1");

            // 유니티에 "1" 송신
            message = "1";
            cDataProcessed = true; // C_DATA가 처리되었는지 여부를 나타내는 플래그

            // 현재 날짜와 시간을 가져옴
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Firebase에 데이터 저장
            var data_Firebase = new Data
            {
                Timestamp = currentDateTime,
                Category = "ON",
                Value = 1

            };

            // 데이터 전송
            crud.SetData("ON", 1, currentDateTime);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            // 아두이노가 연결이 된다는 조건하에 아래 내용들이 실행됨.
            if (!port.IsOpen) return;

            // 아두이노에 "0" 전송
            port.Write("0");

            // 유니티에 "0" 송신
            message = "0";
            cDataProcessed = true; // C_DATA가 처리되었는지 여부를 나타내는 플래그

            // 현재 날짜와 시간을 가져옴
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Firebase에 데이터 저장
            var data_Firebase = new Data
            {
                Timestamp = currentDateTime,
                Category = "OFF",
                Value = 0

            };

            // 데이터 전송
            crud.SetData("OFF", 0, currentDateTime);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                messageLabel.Text = "이미 서버가 실행중 입니다.";
                return;
            }

            isRunning = true;

            server.Start();

            // Thread문 활용
            Thread thread = new Thread(update);
            thread.Start();

            messageLabel.Text = "서버가 시작되었습니다!";

            if (comboBox1.Text == "") return;
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                }
                else
                {
                    port.PortName = comboBox1.SelectedItem.ToString(); // "Com" 선택;
                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.StopBits = StopBits.One;                      // "StopBits": 정지비트의 수를 나타내는 열거형
                    port.Parity = Parity.None;                         // "Parity Bit": 데이터 전송 중 오류를 감지하는데 활용
                    port.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            button1.Text = port.IsOpen ? "Disconnect" : "Connect";
            comboBox1.Enabled = !port.IsOpen;
        }

            byte[] buffer = new byte[1024];
            int bytesRead;
            string messageFromClient;

        // 서버가 실행되고 나서 반복 실행이 되게끔.. Thread를 만들고 위에서 버튼 클릭시 무한 작동함.
        void update()
        {
            TcpClient client = server.AcceptTcpClient(); // 클라이언트가 서버로 들어갈때 사용하는 코드
            stream = client.GetStream(); // 서버로 들어온 후 클라이언트가 열은 소통창구인 stream

            while (isRunning)
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length); // 클라이언트와 서버간 소통 내용을 bytesRead에 담음
                messageFromClient = Encoding.UTF8.GetString(buffer, 0, bytesRead); // 서버 언어 -> 클라이언트 언어으로 통역

                if(messageFromClient.Contains("Connect")) // 서버로 들어온 클라이언트 언어가 connect일때 response으로 담음.
                {
                     response = "서버에 잘 연결 되었습니다.";
                }
                else if(messageFromClient.Contains("C_DATA")) // C_DATA가 처음 들어왔을 때만 처리
                {
                     response = message;
                    cDataProcessed = false; // 다음 C_DATA를 위해 플래그를 초기화
                }
                else if(messageFromClient.Contains("Disconnect"))
                 {
                     response = "서버가 종료되었습니다.";
                 }
                else
                {
                    response = "";
                }

                byte[] responseBytes = new byte[1024]; // 1024개의 바이트를 저장할 수 있는 배열 준비
                responseBytes = Encoding.UTF8.GetBytes(response); // response의 새로운 배열 준비
                stream.Write(responseBytes, 0, responseBytes.Length); // responseBytes의 내용을 stream에 작성함.

             }
         }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var item in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(item);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Firebase에서 데이터 가져오기
                foreach (var item in crud.LoadData())
                {
                    Console.WriteLine("Timestamp :" + item.Value.Timestamp);
                    Console.WriteLine("Category :" + item.Value.Category);
                    Console.WriteLine("Value :" + item.Value.Value);

                }
                // 바탕화면 경로 설정
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // 현재 날짜와 시간을 포맷하여 파일명 생성
                string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_data.csv";
                string filePath = Path.Combine(desktopPath, fileName);

                // CSV 파일로 저장
                using (var writer = new StreamWriter(filePath))
                {
                    // CSV 헤더 작성
                    await writer.WriteLineAsync("Timestamp,Category,Value,");

                    // 데이터 작성
                    foreach (var item in crud.LoadData())
                    {
                        var timestamp = item.Value.Timestamp;
                        var Category = item.Value.Category;
                        var value = item.Value.Value;
                        
                        await writer.WriteLineAsync($"{timestamp},{Category},{value}");
                    }
                }

                MessageBox.Show("CSV 파일이 성공적으로 저장되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }
        }
        public class DataModel
        {
            public string Timestamp { get; set; }
            public string Category { get; set; }
            public int Value { get; set; }
        }
    }
}
