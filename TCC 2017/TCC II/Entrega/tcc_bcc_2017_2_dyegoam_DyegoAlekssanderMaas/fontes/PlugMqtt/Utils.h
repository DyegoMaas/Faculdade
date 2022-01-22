// Utils.h

#ifndef _UTILS_h
#define _UTILS_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class Utils
{
public:
	static float computePower(float amps, float voltage);
	static float computeVrms(float voltageAmplitude);
};


#endif