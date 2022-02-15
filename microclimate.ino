#include<ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>*

#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <DHT_U.h>

/* WIFI-CONNECTION SETTINGS */

// Имя wifi-сети
#define S_SID "Evgeny"

// Пароль wifi-сети
#define PASSWORD "0987654321"

/* HTTP-CONNECTION SETTINGS */

// IP-address
#define IP_ADDRESS "192.168.43.196"

// Port
#define PORT 57144 

/* Servers settings */

// Path from server for send request
#define PATH "/Main"

// Timer interval for send request to HTTP-server
#define INTERVAL 30

// Sensor identificator
#define SENSOR_ID "1"

#define PASS "pass"

/* Other settings */

// БОД
#define BOD 9600

// Режим отладки
#define _DEBUG 1

#if _DEBUG
#define DEBUG_PRINT(msg) Serial.print(msg);
#else
#define DEBUG_PRINT(msg) 
#endif // _DEBUG

/* Sensors settings */

// AM ****
#define DHTPIN     2 
#define DHTTYPE    DHT21     // DHT 21 (AM2301)

// mq135
#define MQ135PIN 0

DHT dht(DHTPIN, DHTTYPE);

// Connect & wait to WIFI
// if waitSecond if empty or 0, then endless waiting
bool connectToWifi(const char *ssid, const char *password, unsigned short waitSeconds=0);

// Send http-get request to server
// if timeout emmpty or 0,  then endless waiting
bool httpGetToServer(const char *host, unsigned int port, String url, unsigned short timeout = 0);

bool connectWifi;
float temp, humidity;
String data;
short valueMQ;
String stateGas;

void setup() {
  Serial.begin(BOD);
  Serial.println();
  
  connectWifi=false;
  
  temp=0;
  humidity=0;
  valueMQ=1;

  // mq135 init
  pinMode(MQ135PIN, INPUT);

  // AM2301 init
  dht.begin();  
}

bool connectToWifi(const char * ssid, const char * password, unsigned short waitSeconds){
  WiFi.begin(ssid,password);
  DEBUG_PRINT("connecting to \n")
  DEBUG_PRINT(ssid);
  DEBUG_PRINT(" using passowrd: '");
  DEBUG_PRINT(password);
  DEBUG_PRINT("'\n");

  unsigned long countSec = 0;
  
  while(WiFi.status()!=WL_CONNECTED)
  {
    if (waitSeconds != 0 && (countSec++ == waitSeconds)){
      return false;
    }   
    delay(1000);
    DEBUG_PRINT(".");
  }
  
  DEBUG_PRINT("\nConnected! LocalIp: ");
  DEBUG_PRINT(WiFi.localIP()); // IP
  DEBUG_PRINT("\n");
  return true;
}

bool httpGetToServer(const char *host, unsigned int port, String url, unsigned short timeoutValue){
    WiFiClient client;
    
    // Connect to server
    if (!client.connect(host, port)) {
      DEBUG_PRINT("connection to server failed!\n");
      return false;
    }
    
    client.print(String("GET ") + url + " HTTP/1.1\r\n" + "Host: " + host + " \r\n" + "Connection: close\r\n\r\n");
    unsigned long timeout = millis();
    
    while (client.available() == 0) {
      if (timeoutValue != 0 && (millis() - timeout > (timeoutValue*1000) ) )
      { 
          DEBUG_PRINT(">>> Client Timeout !");
          client.stop(); 
          return false; 
      } 
    }
    
    while (client.available())
    {
        DEBUG_PRINT(client.readStringUntil('\r'));
    }
    DEBUG_PRINT("closing connection\n");
    
    return true;
}

void loop() {

  // Connect to WIFI
  if (!connectWifi)
    connectWifi = connectToWifi(S_SID, PASSWORD, 5);
  
  if (connectWifi) {
    // Read temperature and humidity
    temp = dht.readTemperature();
    humidity = dht.readHumidity();
    
    DEBUG_PRINT("Temperature:");
    DEBUG_PRINT(temp);
    DEBUG_PRINT("\nHumidity:");
    DEBUG_PRINT(humidity);
    DEBUG_PRINT("\n");
    
    if (isnan(temp))
      temp=-100; // todo

    if (isnan(humidity))
      humidity=0;  

    // Read gas
    valueMQ = digitalRead(MQ135PIN);

    DEBUG_PRINT("\nMQ135:");
    DEBUG_PRINT(valueMQ);
    DEBUG_PRINT("\n");
    
    if (valueMQ==1)
      stateGas="false";
    else 
      stateGas="true";

    DEBUG_PRINT("\nGAS:");
    DEBUG_PRINT(valueMQ);
    DEBUG_PRINT("\n");

    String data = "/Get?Temp=" + String((int)temp) + "&Humidity="+ String((int)humidity) + "&Gas=" + stateGas + "&SensorID=" + SENSOR_ID + "&password=" + PASS;
    
    DEBUG_PRINT("\n");
    DEBUG_PRINT(data);
    DEBUG_PRINT("\n");
    
    // Send data from senosrs to server
    httpGetToServer(IP_ADDRESS, PORT, PATH + data, 1);
  }
  
  delay(INTERVAL*1000);
}
