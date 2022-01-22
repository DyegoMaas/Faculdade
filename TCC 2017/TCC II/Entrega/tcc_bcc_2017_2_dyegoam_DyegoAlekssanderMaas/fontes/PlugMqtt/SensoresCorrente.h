#ifndef _SENSORESCORRENTE_h
#define _SENSORESCORRENTE_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

namespace SensoresCorrente 
{
	enum ModuleType
	{
		_5A = 5,
		_20A = 20,
		_30A = 30
	};

	struct ACReading
	{
		float amps;
		float voltage;
		float power;
		ACReading(float current, float voltage, float power)
			: amps(current), voltage(voltage), power(power) {}
	};

	struct DCReading
	{
		float amps;
		float voltage;
		float power;
		DCReading(float current, float voltage, float power)
			: amps(current), voltage(voltage), power(power) {}
	};

	class ACS712
	{
		public:
			explicit ACS712(ModuleType moduleType);
			DCReading readDC(int analogInPin) const;
      ACReading readAC(int analogInPin);
      void calibrate(int analogInPin);

		private:
			float mVperAmp = 0.066; // use 100 for 20A Module and 185 for 5A Module
			const float ACSoffset = 1400; // equivalent to 0A
  		const float ACSVoltage = 2800; // equivalent to 3.3A
      bool isCalibrated = false;      
	};
}

#endif
