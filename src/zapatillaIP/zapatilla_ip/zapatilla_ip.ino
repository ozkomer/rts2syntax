/*
Control de arreglo de reles por comandos seriales o TCP/IP
Eduardo Maureira, Enero 2012

*/

#include <Wire.h> // specify use of Wire.h library.
#include <SPI.h>
#include <Ethernet.h>

int i;
int j;

//.. This allows eight individual devices to be connected at one time with individual addresses of 0x20 through 0x27. (Hex numbers!) 
byte zxRelayAddres = 0x20;

// Status de los relés 1 al 8
byte port0=0; 
// Status de los relés 9 al 16
byte port1=0;

// the media access control (ethernet hardware) address for the shield:
byte mac[] = { 0x90, 0xA2, 0xDA, 0x00, 0x82, 0x47 }; 
//the IP address for the shield:
byte ip[] = { 139, 229, 65, 214 };// 139.229.65.214
//the gateway for the shield:
byte gateway[] = { 139, 229, 65, 193 };
//the subnet for the shield:
byte subnet[] = { 255, 255, 255, 224 };

// Servidor de la Zapatilla IP
EthernetServer server = EthernetServer(18008);
EthernetClient client;

void setup()  
{
  // open the serial port at 9600 bps:
  Serial.begin(9600);
  Serial.println("Bienvenidos a Zapatilla IP.");
  
  Wire.begin();
  Wire.beginTransmission(zxRelayAddres);  // setup out direction registers
  Wire.write((byte)0x06);  // pointer
  Wire.write((byte)0x00);  // DDR Port0 all output
  Wire.write((byte)0x00);  // DDR Port1 all output
  Wire.endTransmission(); 
  
  Ethernet.begin(mac,ip,gateway,subnet);

  // start listening for clients
  server.begin();
  listarComandosSerial ();
  readRelays();
}
  
  int incomingByte = 0;	// for incoming serial data


void loop() {
  if (Serial.available() > 0) {
    // read the incoming byte:
    incomingByte = Serial.read();    
    // say what you got:
    //Serial.print("I received: ");
    //Serial.println(incomingByte);
    procesaComandoSerial(incomingByte);
    listarComandosSerial ();
  }
  // if an incoming client connects, there will be bytes available to read:
  client = server.available();
  if (client == true) {
    // read bytes from the incoming client and write them back
    // to any clients connected to the server:
    byte letra;
    letra = client.read();
    procesaComandoTcpIP(letra);

  }
} // end loop
  
void apagarTodo()
{
  port0=0;
  port1=0;
  Serial.println(" ... Apagando Todo.");
  updateRelays();  
}

void encenderTodo()
{
  port0=255;
  port1=255;
  Serial.println(" ... Encendiendo Todo.");
  updateRelays();
}

void procesaComandoTcpIP(byte comando)
{
    byte chk;
    byte suma;
    if (comando==1)
    {
      port0=client.read();
      port1=client.read();
      chk = client.read();
      suma = (comando + port0 + port1);
      if (suma==chk)
      {
        updateRelays();
        readRelays();
      }
      else
      {
        Serial.print ("checksum Error;");
        Serial.print(suma);
        Serial.print("!=");
        Serial.println(chk);
      }
    }
}

void procesaComandoSerial(int linea)
{
  switch (linea)
  {
    case 49:
      apagarTodo();
      break;
    case 50:
      encenderTodo();
      break;
    case 76:// L
      i = ((Serial.read())-49);
      bitSet(port0,i); // port0 = ((port0) | (1<<i));
      Serial.print(" ... Apagando, rele= ");
      Serial.println(i);
      updateRelays();
      break;
    case 72:// H
      i = ((Serial.read())-49);
      bitSet(port1,i);// port1 = ((port1) | (1<<i));
      Serial.print(" ... Apagando, rele= ");
      Serial.println(i+8);
      updateRelays();
      break;
    case 108://l
      i = ((Serial.read())-49);
      bitClear(port0,i);// port0 = ((port0) - (1<<i));
      Serial.print(" ... Encendiendo, rele= ");
      Serial.println(i);
      updateRelays();
      break;
    case 104://h
      i = ((Serial.read())-49);
      bitClear(port1,i);//port1 = ((port1) - (1<<i));
      Serial.print(" ... Encendiendo, rele= ");
      Serial.println(i+8);
      updateRelays();
      break;
    case 115://s
      j = ((Serial.read()));
      i = ((Serial.read()));
      if (i<58) { port0=(i-48); } else { port0= (i-87);}
      if (j<58) { port0= (port0 | ((j-48)<<4)); } else { port0 = ( port0 | ((j-87)<<4));}
      j = ((Serial.read()));
      i = ((Serial.read()));
      if (i<58) { port1=(i-48); } else { port1= (i-87);}
      if (j<58) { port1= (port1 | ((j-48)<<4)); } else { port1 = ( port1 | ((j-87)<<4));}
      
      Serial.print(" ... Seteando port0=");
      Serial.print(port0,DEC);
      Serial.print(" port1=");
      Serial.print(port1,DEC);
      Serial.println("");
      updateRelays();
      break;
  }
}

void listarComandosSerial ()
{
  Serial.println("");
  Serial.println("*********Zapatilla IP.*********");
  Serial.println("1=Encender todo");
  Serial.println("2=Apagar todo");
  Serial.println("Li=Apagar el rele i, i es de 1 a 8");
  Serial.println("Hi=Apagar el rele (i+8), i es de 1 a 8");
  Serial.println("li=Enciende el rele i, i es de 1 a 8");
  Serial.println("hi=Enciende el rele (i+8), i es de 1 a 8");
  Serial.println("sijkl=Setea todos los reles, {i,j,k,l} son valores hexadecimales (0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f).");
  
  //Serial.println("S,i,j Setea el port0 con el valor de i, y el port1 con el valor de j.");
  Serial.println("?");
}

void updateRelays()
{
  Wire.beginTransmission(zxRelayAddres);  
  Wire.write((byte)0x00); // Comando para acceder al puerto de datos GP0.
  
  Wire.write((byte)port0); //Escribe status del port 0
  Wire.write((byte)port1); //Escribe status del port 1, esto impacta en el GP1
  
  Wire.endTransmission();
} 

void readRelays()
{
    // read port0, port1
    Wire.beginTransmission(zxRelayAddres);
    Wire.write((byte)0x00); // must act as a position pointer?
    Wire.endTransmission();
    Wire.requestFrom((byte)zxRelayAddres, (byte)2);    // request 1 byte
    port0 = Wire.read(); // receive a byte
    port1 = Wire.read(); // receive a byte
    Wire.endTransmission();
    
      Serial.print(" ... Status Actual port0=");
      Serial.print(port0,DEC);
      Serial.print(" port1=");
      Serial.print(port1,DEC);
      Serial.println("");
}
