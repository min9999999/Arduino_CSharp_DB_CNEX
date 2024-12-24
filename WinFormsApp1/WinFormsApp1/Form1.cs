using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using Newtonsoft.Json;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using static WinFormsApp1.Form1;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using System.Reactive;

namespace WinFormsApp1
{ 
    public partial class Form1 : Form
    {
        class connection
        {
            // Firebase 클라이언트 세팅
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

        public Form1()
        {
            InitializeComponent();
        }

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
            if (!port.IsOpen) return;

            // 아두이노에 "1" 전송
            port.Write("1");

            // 현재 날짜와 시간을 가져옴
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Firebase에 데이터 저장
            var data = new Data
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
            if (!port.IsOpen) return;

            // 아두이노에 "0" 전송
            port.Write("0");

            // 현재 날짜와 시간을 가져옴
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Firebase에 데이터 저장
            var data = new Data
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
