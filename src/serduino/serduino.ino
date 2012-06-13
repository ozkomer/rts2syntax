/* 
 * Driver for Arduino used as multipurpose sensor.
 * Copyright (C) 2010 Petr Kubanek, Insitute of Physics <kubanek@fzu.cz>
 *
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

#define SIZE_A  30



// array for average values of sensors
int avals[6][SIZE_A];
double asums[6];
double accel[6]; // RA (x,y,z) ; DEC (x,y,z) accelerations

int avals_index = 0;
int pinLimitRA=7;

// Vector unitario del acelerometro del eje de declinacion medido en un Zenith (Alt=90) de Pointing Calculado por astrometria.
const double Zenith[]     = { 
  -0.2116771931 , 0.975214028  , 0.0644233307 };

// Vector unitario del acelerometro del eje de ascension recta, medido en la mitad de la carrera del switch RA_HOME
const double SouthPole[]  = { 
  0.3780225716  , 0.3874084811 , -0.84084101  };

// Angulo entre el Zenith y la posición del tubo del telescopio. [en radianes].
double zenithAngle;

// Si el zenithAngle excede este valor [ en radianes ], se apagará la montura.
const double zenithAngleLimit = 1.8325957146; //== 105 [grados sexagesimales]

// Angulo del eje contrapeso. [en radianes].
double counterWeightAngle;

void setup()
{
  // must switch to external reference
  analogReference(EXTERNAL);

  Serial.begin(9600);

  int i;

  pinMode(pinLimitRA,OUTPUT);
  digitalWrite (pinLimitRA,LOW);

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
  static const double VZEROG = 1.5;//[Volts]
  static const double VoltsPerCount = 0.0029325513196480938;//(VREF / MaxAnalogRead);//[volts]
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
  int sensorValue = 0;
  int i;
  for (i = 8; i < 11; i++)
  {
    sensorValue = sensorValue << 1;
    sensorValue |= digitalRead(i);
  }


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

  Serial.println();
}

void loop()
{
  if  ( (digitalRead(8)==1) //  Si se alcanza un LimitSwitch en RA
      ||                    // o
        (zenithAngle>zenithAngleLimit) ) // El telescopio está definivamente mirando por debajo del horizonte
  {
    digitalWrite (pinLimitRA,HIGH); // Se informa al frigobar a traves del pinLimitRA
  }
  if (Serial.available())
  {
    char ch = Serial.read();
    switch (ch)
    {
    case '?':
      sendSensor();
      break;
    default:
      digitalWrite (pinLimitRA,LOW);
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
