# AuraSharp
[Created with ![heart](http://i.imgur.com/oXJmdtz.gif) in Poland by RapidDev](http://rapiddev.pl/)<br />
C# Library &amp; DLL Importer for Asus Aura Sync. This simple library facilitates establishing communication with Asus Aura Sync software.
It uses the original Aura SDK written in C++, and then moves its functions to C#.

![Presentation image](https://raw.githubusercontent.com/rapiddev/AuraSharp/master/AuraSharp/Assets/aurasharp-banner.png)

## What does this DLL do?
This library creates functions using the original Aura SDK, and then lets you easily administer the colors of the motherboard

## What doesn't this DLL do?
The Asus Aura SDK was last updated several years ago. The documentation contains information about other components such as DRAM support. BUT THESE FUNCTIONS DO NOT EXIST AND NEVER BE PUBLISHED (we love you Asus)

# How to use it?
1. If you compile a project in Visual Studio, put [AURA_SDK.dll](https://www.asus.com/campaign/aura/fl/SDK.html) in the Debug and Relase folders of your program.
2. Set your program to work in x86 (32 bit) mode, because the Aura SDK library is written for 32bit.
3. Use 'Add reference' to add a reference to AuraSharp.dll in your project.
4. Inform your program that you will use AuraSharp.
```c#
    using AuraSharp;
```
5. Create a new class instance.
```c#
    Aura Aura = new Aura();
```
6. That's all!

## Example program using the library
```c#
    class Program
    {
        static void Main(string[] args)
        {
            Aura Aura = new Aura();

            for (int i = 0; i < 5; i++)
            {
                Aura.setAll(AuraColor.red);
                System.Threading.Thread.Sleep(100);
                Aura.setAll(AuraColor.green);
                System.Threading.Thread.Sleep(100);
                Aura.setAll(AuraColor.blue);
                System.Threading.Thread.Sleep(100);
            }

            Aura.Reset();
            Console.ReadKey();
        }
    }
```

## How to change colors?
You can change colors in three ways.
1. Using RGB
```c#
    Aura.setAll(255, 192, 255);
```
2. Using ready library colors
```c#
    Aura.setAll(AuraColor.red);
```
3. Using the byte array
```c#
    byte[] my_colors = {255, 192, 255};
    Aura.setAll(my_colors);
```

## What works?
- [x] Control of 12V RGB stripes
- [x] Onboard leds like those mounted on the motherboard
- [x] Components on the motherboard (such as the logo on the sound card in Asus ROG 470-I)
## What is not working?
- Addressable RGB stripes
- DRAM
- GPU

### Thanks to [Andy Janata](https://github.com/ajanata) for creating AuraDriver. It helped me a lot in making this DLL 
