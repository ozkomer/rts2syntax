/*
 * Driver for Arduino used as multipurpose sensor.
 * Copyright (C) 2010 Petr Kubanek, Insitute of Physics <kubanek@fzu.cz>
 * Modified by Eduardo Maureira, September 2012. <emaureir@das.uchile.cl>
 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */
#include <Wire.h> // specify use of Wire.h library. 
#define SIZE_A  30

//.. This allows eight individual devices to be connected at one time with individual addresses of 0x20 through 0x27. (Hex numbers!) 
byte zxRelayAddres = 0x20;
// Status de los relÃ©s 1 al 8
byte port0=0; 
// Status de los relÃ©s 9 al 16
byte port1=0;

// array for average values of sensors
int avals[6][SIZE_A];
double asums[6];
double accel[6]; // RA (x,y,z) ; DEC (x,y,z) accelerations

int avals_index = 0;
boolean protectMount;
boolean monturaEncendida;

// Vector unitario del acelerometro del eje de declinacion medido en un Zenith (Alt=90) de Pointing Calculado por astrometria.
const double Zenith[]     = { 
0.043428553544686100, 0.997336304799117000, 0.058521246693571100 };
		
// Vector unitario del acelerometro del eje de ascension recta, medido en la mitad de la carrera del switch RA_HOME
const double SouthPole[]  = { 
0.333013222037250, 0.385916999673910, -0.8603114832215930 };

// Angulo entre el Zenith y la posiciÃ³n del tubo del telescopio. [en radianes].
double zenithAngle;

// Si el zenithAngle excede este valor [ en radianes ], se apagarÃ¡ la montura.
const double zenithAngleLimit = 1.8325957146; //== 105 [grados sexagesimales]

//7000>Valor obtenido haciendo un arranque de la montura desde park1 hasta el meridiano opuesto(forzando un transito).
// El valor obtenido fue 3247, pero se permitira una holgura de 7000 conteos de posiciones mas alla  del limite del Zenith.
//10>Ahora se ha optado por usar solo park3, esto evita siempre (en teoria), que la montura pase por debajo del horizonte.
const unsigned long zenithLimitCounter = 10;//7000;

// Angulo del eje contrapeso. [en radianes].
double counterWeightAngle;

double counterWeightAngleMin = 1.77150919;// == 101.5  [grados sexagesimales] 
double counterWeightAngleMax = 4.75602221;// == 272.5  [grados sexagesimales] 


//Cada vez que el zenithAngle excede el zenithAngleLimit, esta variable se incrementa en 1.
// si esta variable supera el valor zenithLimitCounter, se ordena apagar la montura, y se setea esta variable en cero
unsigned long  zenithCounter;

void setup()
{
  // must switch to external reference
  analogReference(EXTERNAL);
  
  
  protectMount = true;
  
  //Preparamos las comunicaciones I2C
  Wire.begin();
  Wire.beginTransmission(zxRelayAddres);  // setup out direction registers
  Wire.write((byte)0x06);  // pointer
  Wire.write((byte)0x00);  // DDR Port0 all output
  Wire.write((byte)0x00);  // DDR Port1 all output
  Wire.endTransmission(); 

  Serial.begin(9600);

  refreshMonturaEncendida();
  

  zenithCounter = 0;
  zenithAngle =0;
  int i;

//  pinMode(pinLimitRA,OUTPUT);
//  digitalWrite (pinLimitRA,LOW);

  for (i = 8; i < 11; i++)
  {
    pinMode(i, INPUT);
  }

  // fill sensor array
  for (i = 0; i < 6; i++)
  {
    asums[i] = 0;
    accel[i]=0;
  }
  for (i=0; i < SIZE_A; i++)
  {
    for (int j = 0; j < 6; j++)
    {
      avals[j][i] = analogRead(j);
      asums[j] += avals[j][i];
    }
  }
  avals_index = 0;
}

void refreshCounterWeightAngle ()
{
  double cross;
  cross =  ( (accel[0] * SouthPole[0]) + (accel[1] * SouthPole[1]) + (accel[2] * SouthPole[2]) );
  counterWeightAngle = acos (cross); 
}

void refreshZenithAngle ()
{
  double cross;
  cross =  ( (accel[3] * Zenith[0]) + (accel[4] * Zenith[1]) + (accel[5] * Zenith[2]) );
  zenithAngle = acos (cross); 
}


void refreshAccelerations()
{
  static const double VZEROG = (1.665);//[Volts]
  static const double VoltsPerCount = (0.003251953125);//(VREF / MaxAnalogRead);//[volts]
  static const double Sensitivity = 0.33;//0.55;//[Volt/g]
  // Convertimos los voltajes de "AnalogRead" en Aceleraciones acordes con la especificacion del acelerometro
  for (int i=0;i<6;i++)
  {
    accel[i] = (( (asums[i] / SIZE_A) * VoltsPerCount) - (VZEROG)) / Sensitivity;
  }
  // Normalizamos las aceleraciones convirtiendolas en vectores unitarios.
  double cRoot;
  cRoot =  sqrt((accel[0] * accel[0]) + (accel[1] * accel[1]) + (accel[2] * accel[2]));
  accel[0] /= cRoot;
  accel[1] /= cRoot;
  accel[2] /= cRoot;
  cRoot =  sqrt((accel[3] * accel[3]) + (accel[4] * accel[4]) + (accel[5] * accel[5]));
  accel[3] /= cRoot;
  accel[4] /= cRoot;
  accel[5] /= cRoot;
}

void sendSensor()
{
  byte sensorValue;
  sensorValue = 0;
  bitWrite(sensorValue,0,digitalRead(8));
  bitWrite(sensorValue,1,digitalRead(9));
  bitWrite(sensorValue,2,digitalRead(10));
  bitWrite(sensorValue,3,monturaEncendida);
  bitWrite(sensorValue,4,protectMount);

  int i; // loop iterator
//  
//  for (i = 8; i < 11; i++)
//  {
//    sensorValue = sensorValue << 1;
//    sensorValue |= digitalRead(i);
//  }

  Serial.print(sensorValue, DEC);
  Serial.print(" ");

  for (i = 0; i < 6; i++)
  {
    Serial.print(asums[i] / SIZE_A, DEC);
    Serial.print(" ");
  }
  refreshCounterWeightAngle ();
  Serial.print(counterWeightAngle, DEC);
  Serial.print(" ");
  Serial.print(zenithAngle, DEC);
  Serial.print(" ");
  Serial.print(zenithCounter, DEC);
  Serial.println();  
}

void loop()
{
  if (zenithAngle>zenithAngleLimit) // Si el telescopio esta en una posicion insegura
  { 
    zenithCounter++; // incrementamos el contador
  }
  
  if (zenithAngle<1.57079633) // Si el telescopio ha vuelto a una posicion segura
  {
    zenithCounter = 0; // Reiniciamos el contador
  }

  if  ( (digitalRead(8)==1) //  Si se alcanza un LimitSwitch en RA
      ||                    // o
        (zenithCounter>zenithLimitCounter)  // El telescopio estÃ¡ hace tiempo definivamente mirando por debajo del horizonte
//      ||
//        (counterWeightAngle<counterWeightAngleMin) // El Telescopio cruzo el limitSwitch del East
//      ||
//        (counterWeightAngle>counterWeightAngleMax) // El Telescopio cruzo el limitSwitch del West
      )  
  {
    //digitalWrite (pinLimitRA,HIGH); // Se informa al frigobar a traves del pinLimitRA
    if ((protectMount) && (monturaEncendida))
    {
      apagarMontura();
      zenithCounter = 0;
      protectMount=false;
    }
  }
    
  if (Serial.available())
  {
    refreshMonturaEncendida(); 
    char ch = Serial.read();
    switch (ch)
    {
    case '?':
      sendSensor();
      break;
    case 'A':
      protectMount=true;
      break;
    case '_':
      protectMount=false;
      break;
    default:
      protectMount=true;
      //digitalWrite (pinLimitRA,LOW);
      Serial.println("E unknow command");
    }   
  }
  for (int i = 0; i < 6; i++)
  {
    asums[i] -= avals[i][avals_index];
    int r = analogRead(i);
    avals[i][avals_index] = r;
    asums[i] += r;
  }
  avals_index ++;
  avals_index %= SIZE_A;
  refreshAccelerations();
  
  refreshZenithAngle ();
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
  
//  Serial.print(" ... Status Actual port0=");
//  Serial.print(port0,DEC);
//  Serial.print(" port1=");
//  Serial.print(port1,DEC);
//  Serial.println("");
}

void refreshMonturaEncendida()
{
  readRelays();
  monturaEncendida = ((bitRead(port0,2))==0);
}

void apagarMontura()
{
//  refreshMonturaEncendida();
//  if (!monturaEncendida) //Si la montura ya esta apagada
//  {
//    return; // no apagar la montura
//  }
  //do {
    bitSet(port0,2);// La montura esta en el rele 3.
    Serial.println(" ... Apagando Montura.");
    updateRelays();
    refreshMonturaEncendida();
  //} while (monturaEncendida);
}

void updateRelays()
{
  Wire.beginTransmission(zxRelayAddres);  
  Wire.write((byte)0x00); // Comando para acceder al puerto de datos GP0.
  Wire.write((byte)port0); //Escribe status del port 0
  Wire.write((byte)port1); //Escribe status del port 1, esto impacta en el GP1
  Wire.endTransmission();
} 