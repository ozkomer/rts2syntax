/*
  
  The address for the MCP23016 is 0x20.
  There are eight resistor/LED indicators on pins 21 - 28.
  This demo produces a binary count of 0 to 255
  on Port0 on the MCP23016.
  
*/

#include <Wire.h> // specify use of Wire.h library.

int i = 0;

void setup()

{
 
  Wire.begin();
  Wire.beginTransmission(0x20);  // setup out direction registers
  Wire.send(0x06);  // pointer
  Wire.send(0x00);  // DDR Port0 all output
  Wire.send(0xFF);  // DDR Port1 all input
  Wire.endTransmission(); 
  
  
    }

void loop() {
  
    i++; 
    Wire.beginTransmission(0x20);  // set mcp23016 for all output
    Wire.send(0x00); // begin here
    Wire.send(i); 
    Wire.endTransmission();
    if (i > 255) i = 0;
    delay(1000); // }

} // end loop
