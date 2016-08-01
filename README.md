# WARNING: USE AT YOUR OWN RISK
Adjusting the value from the default may affect the lifetime of your screen.  You might end up looking into the abyss.  You have been WARNED!

# PWM
This is a utility to allow for adjust the PWM driver frequency delivered from Intel Graphics embedded GPU to the embedded LCD panel.

IntelPWMCA_Console.exe is compiled with the console output, IntelPWMCA.exe as windows app without console output.
It take two arguments:
1) the new PWM Frequency (from 50 to 20000Hz), if not present set the PWM frequency to 600Hz
2) (optional) set the check period of the setting in second. For example 1 mean that the program check every second if the pwm frequency is changed and if true it set back to the requested value.

Some examples:
Set the PWM frequency to 1500Hz and return 
D:\IntelPWMCA_Console 1500
Current PWM: 220 Hz
Set to: 1500 Hz

Set the PWM frequency to 2000Hz and check every 2 seconds if it has changed (the program never end): 
D:\IntelPWMCA_Console 2000 2
Current PWM: 1500 Hz
Set to: 2000 Hz
Current PWM: 2000 Hz

Tested on Lenovo T530 on windows 7 professional, intel driver version 10.18.10.4358. Compiled with Microsoft Visual studio community 2015. 
In my case the PWM frequency never reverts to default during britghness changes, screen power off or standby. It reverts only after a reboot, so the program can be called at startup and doesn't has to remain active (so no second parameter is needed).
The PWM frequency seems to be limited at 10kHz, if set >10KHz the pwm frequency is accepted but the actual frequency remain 10kHz (checked by the "whistle" tone of the screen). Any possibilities to override this limit? The old intel driver with the dmytrov/IntelPWMControl software doesn't have thi limit.

It should work on Windows 10 if recompiled with the igfxDHLibv2_0.dll DLL installed with the windows 10 intel driver (can be found in C:\Windows\System32)

It was inspired by this 
https://wiki.archlinux.org/index.php/Backlight#Backlight_PWM_modulation_frequency_.28Intel_i915_only.29
and this
http://devbraindom.blogspot.com/2013/03/eliminate-led-screen-flicker-with-intel.html
But its for Windows x64 instead.  

I think the value it reads back and writes is the frequency in Hz.  The actual output was not checked via high speed camera or anything like that, but it seems to have the same behavior as sending those intel_reg_write commands from linux with the computed values.  The driver actually has two configuration DWORDs,  the default for me for these two values in decimal is 2 and 220.  This utility just keeps the first value as read back from the driver and changes only the second value.  It's possible the first value is duty cycle or mode related, or something completely different.  You could experiment using the source code if you aren't afraid to explode.

# LICENSE
Public domain
