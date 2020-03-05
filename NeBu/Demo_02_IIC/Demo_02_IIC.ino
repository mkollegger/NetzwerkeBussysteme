// (C) 2020 Michael Kollegger
// 
// Kontakt mike@fotec.at / www.fotec.at
// 
// Erstversion vom 04.03.2020 20:17
// Entwickler      Michael Kollegger
// Projekt         NeBu

#include <Wire.h>

byte counter = 0;


const int SDAPin =	2;
const int SCLPin = 3;


void IIC_delay (void)
{
byte i;

	for (i = 0; i < 3; i++)
	{
		delay(1);
	}
}

void IIC_start (void) 
{
	digitalWrite(SCLPin, HIGH);
	digitalWrite(SDAPin, HIGH);
	IIC_delay();
	digitalWrite(SDAPin, LOW);
	IIC_delay();
	digitalWrite(SCLPin, LOW);
}

void IIC_stop (void) 
{
	digitalWrite(SCLPin, LOW);
	digitalWrite(SDAPin, LOW);
	IIC_delay();
	digitalWrite(SCLPin, HIGH);
	IIC_delay();
	digitalWrite(SDAPin, HIGH);
}

byte IIC_send_byte (byte b) 
{
byte 	lauf,
		ret = 0;
	
	for (lauf = 0;lauf < 8;lauf++) 
	{
		if ((b & 0x80) > 0) 
		{
			digitalWrite(SDAPin, HIGH);
			
		}
		else 
		{
			digitalWrite(SDAPin, LOW);
		}
		b = b << 1;
		IIC_delay();
		digitalWrite(SCLPin, HIGH);
      	IIC_delay();
		digitalWrite(SCLPin, LOW);
	}

	//Acknowledge abfragen (9. Bit)
	IIC_delay();
	pinMode(SDAPin,INPUT);
	digitalWrite(SDAPin, HIGH);
	digitalWrite(SCLPin, HIGH);
	
	IIC_delay();
	
	if (digitalRead(SDAPin) == HIGH) ret = 0xFF;
	digitalWrite(SCLPin, LOW);
	IIC_delay();
	pinMode(SDAPin,OUTPUT);
	return (ret);
}

byte IIC_read_byte (byte ack) 
{
byte 	lauf,
		a = 128,
		c = 0;

	pinMode(SDAPin,INPUT);

	for (lauf = 0;lauf < 8;lauf++) 
	{
		IIC_delay();
		digitalWrite(SCLPin, HIGH);
		IIC_delay();
		if (digitalRead(SDAPin) == HIGH) c = c + a;
		a = a/2;			
		digitalWrite(SCLPin, LOW);
	}

	pinMode(SDAPin,OUTPUT);
	//Acknowledge bestätigen ... oder nicht (9. Bit)
	IIC_delay();

	digitalWrite(SDAPin, ack);
	digitalWrite(SCLPin, HIGH);
	IIC_delay();
	digitalWrite(SCLPin, LOW);
	digitalWrite(SDAPin, LOW);

	return(c);
}

void initIICGpio()
{
  pinMode(SDAPin,OUTPUT);
  pinMode(SCLPin,OUTPUT);
}

void initIICChip()
{
	Wire.begin();
	Wire.beginTransmission(0x20);
	Wire.write(byte(0xFF));
	Wire.endTransmission();
}


// the setup function runs once when you press reset or power the board
void setup() {
	// initialize digital pin 13 as an output.
	pinMode(13, OUTPUT);

	initIICGpio();
	initIICChip();
}


// the loop function runs over and over again forever
void loop() {

	digitalWrite(13, HIGH);   // turn the LED on (HIGH is the voltage level)
  delay(200);              // wait for a second
  digitalWrite(13, LOW);    // turn the LED off by making the voltage LOW
  delay(200);              // wait for a second

  counter ++;
  Wire.beginTransmission(0x20);
  Wire.write(byte(counter));
  Wire.endTransmission();

  IIC_start();
  IIC_send_byte(0x40);
  IIC_send_byte(counter);
  IIC_stop();
}




