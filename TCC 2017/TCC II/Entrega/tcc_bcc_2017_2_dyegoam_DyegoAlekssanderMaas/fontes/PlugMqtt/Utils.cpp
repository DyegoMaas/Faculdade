#include "Utils.h"

float Utils::computePower(float amps, float voltage)
{
	return amps * voltage;
}

///The Vrms is defined as square root of the mean of the squares of the values for the one time period of the sine wave
float Utils::computeVrms(float voltageAmplitude)
{
	//Vmax of a sine wave voltage waveform is defined as the Positive Amplitudes on the sine wave.
	//http://www.referencedesigner.com/rfcal/cal_04.php
	auto vMax = voltageAmplitude / 2.0;
	auto VRMS = vMax * 0.707;
	return VRMS;
}

