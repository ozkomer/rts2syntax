/*

The address for the MCP23016 is 0x20.
There are eight resistor/LED indicators on pins 21 - 28.
This demo produces a binary count of 0 to 255
on Port0 on the MCP23016.

*/

#include <Wire.h> // specify use of Wire.h library.

int i;
int j;

// Status de los relés 1 al 8
byte port0=0; 
// Status de los relés 9 al 16
byte port1=0;
  
void setup()  
{
  // open the serial port at 9600 bps:
  Serial.begin(9600);
  Serial.println("Bienvenidos a Zapatilla IP.");
  
  Wire.begin();
  Wire.beginTransmission(0x20);  // setup out direction registers
  Wire.send(0x06);  // pointer
  Wire.send(0x00);  // DDR Port0 all output
  Wire.send(0x00);  // DDR Port1 all output
  Wire.endTransmission(); 
  listarComandos ();
}
  
  int incomingByte = 0;	// for incoming serial data


void loop() {
  if (Serial.available() > 0) {
    // read the incoming byte:
    incomingByte = Serial.read();    
    // say what you got:
    Serial.print("I received: ");
    Serial.println(incomingByte);
    procesaComando(incomingByte);
    listarComandos ();
  }
} // end loop
  
void procesaComando(int linea)
{
  switch (linea)
  {
    case 49:
      port0=0;
      port1=0;
      Serial.print(" ... Apagando Todo.");
      updateRelays();
      break;
    case 50:
      port0=255;
      port1=255;
      Serial.print(" ... Encendiendo Todo.");
      updateRelays();
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
      i = ((Serial.read()));
      j = ((Serial.read()));
      if (i<58) { port0=(i-48); } else { port0= (i-87);}
      if (j<58) { port0= (port0 | ((j-48)<<4)); } else { port0 = ( port0 | ((j-87)<<4));}
      i = ((Serial.read()));
      j = ((Serial.read()));
      if (i<58) { port1=(i-48); } else { port1= (i-87);}
      if (j<58) { port1= (port1 | ((j-48)<<4)); } else { port1 = ( port1 | ((j-87)<<4));}
      
      Serial.print(" ... Seteando i=");
      Serial.print(i,HEX);
      Serial.print(" j=");
      Serial.print(j,HEX);
      Serial.println("");
      updateRelays();
      break;
  }
}

void listarComandos ()
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
  Wire.beginTransmission(0x20);  
  Wire.send(0x00); // Comando para acceder al puerto de datos GP0.
  
  Wire.send(port0); //Escribe status del port 0
  Wire.send(port1); //Escribe status del port 1, esto impacta en el GP1
  
  Wire.endTransmission();
}  
