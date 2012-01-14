/*

The address for the MCP23016 is 0x20.
There are eight resistor/LED indicators on pins 21 - 28.
This demo produces a binary count of 0 to 255
on Port0 on the MCP23016.

*/

#include <Wire.h> // specify use of Wire.h library.

int i = 0;

// Status de los relés 1 al 8
byte port0=0; 
// Status de los relés 9 al 16
byte port1=0;
  
void setup()  
{
  Wire.begin();
  Wire.beginTransmission(0x20);  // setup out direction registers
  Wire.send(0x06);  // pointer
  Wire.send(0x00);  // DDR Port0 all output
  Wire.send(0x00);  // DDR Port1 all output
  Wire.endTransmission(); 
}
  
void loop() {
  if (i<8)
  { 
    port0 = (1<<i);
    port1 = 0;
  }
  else
  {
    port0 = (0);
    port1 = (1<<(i-8));
  }
  updateRelays();
  i++; 
  if (i > 16) i = 0;
  delay(1000);
} // end loop
  
void updateRelays()
{
  Wire.beginTransmission(0x20);  
  Wire.send(0x00); // Comando para acceder al puerto de datos GP0.
  
  Wire.send(port0); //Escribe status del port 0
  Wire.send(port1); //Escribe status del port 1, esto impacta en el GP1
  
  Wire.endTransmission();
}  
