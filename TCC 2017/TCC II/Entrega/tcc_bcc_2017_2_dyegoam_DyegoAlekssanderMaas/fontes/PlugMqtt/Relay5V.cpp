// 
// 
// 

#include "Relay5V.h"

void Relay5V::turnOn(int pin) const
{
	digitalWrite(pin, worksInLow ? LOW : HIGH);
}

void Relay5V::turnOff(int pin) const
{
	digitalWrite(pin, worksInLow ? HIGH : LOW);
}

void Relay5V::toggle(int pin)
{
	currentState = !currentState;
	if (currentState)
		turnOn(pin);
	else
		turnOff(pin);
}

bool Relay5V::isOn(int pin) const
{
  if (worksInLow)
    return digitalRead(pin) == 0;
  return digitalRead(pin) > 0;    
}

void DualRelay5V::turnOn(int pin1, int pin2) const
{
	relay1.turnOn(pin1);
	relay2.turnOn(pin2);
}

void DualRelay5V::turnOff(int pin1, int pin2) const
{
	relay1.turnOff(pin1);
	relay2.turnOff(pin2);
}

void DualRelay5V::toggle(int pin1, int pin2)
{
	relay1.toggle(pin1);
	relay2.toggle(pin2);
}
