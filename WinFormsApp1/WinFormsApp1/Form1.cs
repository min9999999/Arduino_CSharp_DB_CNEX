using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public class Data
        {
            public string Category;
            public int Value;
            public string Timestamp;
        }

        private const string BasePath = "https://arduinoconnectex-default-rtdb.firebaseio.com/"; //본인의 Firebase Database URL을 입력
        private const string FirebaseSecret = "E09Dva4dBklDbRCYPQi9TGPkvCU8Ut0vn9Iji737"; //본인의 Firebase Database 비밀번호를 입력
        private static FirebaseClient _client;

        SerialPort port = new SerialPort();

        public Form1()
        {
            InitializeComponent();
            _client = new FirebaseClient(BasePath, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(FirebaseSecret)
            });
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
                Category = "ON",
                Value = 1,
                Timestamp = currentDateTime
            };

            // 데이터 전송
            await _client
                 .Child("Arduino/Log") // 데이터가 저장될 경로를 지정
                 .PostAsync(data); // Firebase에 데이터 전송
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
               Category = "OFF",
               Value = 0,
               Timestamp = currentDateTime
           };

            await _client
                 .Child("Arduino/Log") // 데이터가 저장될 경로를 지정
                 .PostAsync(data); // Firebase에 데이터 전송
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
                var data = await _client
                    .Child("Arduino/Log") // 데이터가 저장된 경로를 지정
                    .OnceAsync<DataModel>(); // DataModel은 Firebase에서 가져올 데이터의 형식

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
                    foreach (var item in data)
                    {
                        var Category = item.Object.Category;
                        var value = item.Object.Value;
                        var timestamp = item.Object.Timestamp;
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



       