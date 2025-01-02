void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(13, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
 if(Serial.available())
 {
  switch(Serial.read())
  {
    case'1':
     String data = Serial.readStringUntil('\n'); // 데이터 읽기
     Serial.println(data); // 수신한 데이터 다시 송신
     digitalWrite(13, 1); break;
    case'0':
     String data = Serial.readStringUntil('\n'); // 데이터 읽기
     Serial.println(data); // 수신한 데이터 다시 송신
     digitalWrite(13, 0); break;
  }
 }
}
