// Relay5V.h

#ifndef _RELAY5V_h
#define _RELAY5V_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class Relay5V
{
public:
	explicit Relay5V(bool worksInLow) : worksInLow(worksInLow) {}
	void turnOn(int pin) const;
	void turnOff(int pin) const;
	void toggle(int pin);
  bool isOn(int pin) const;
private:
	bool worksInLow = true; //relays are activated on LOW
	bool currentState = true;
};

class DualRelay5V
{
public:
	explicit DualRelay5V(bool worksInLow) : 
		relay1(Relay5V(worksInLow)), 
		relay2(Relay5V(worksInLow))
	{
	}

	void turnOn(int pin1, int pin2) const;
	void turnOff(int pin1, int pin2) const;
	void toggle(int pin1, int pin2);
private:
	Relay5V relay1;
	Relay5V relay2;
};

#endif
