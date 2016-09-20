## Pull Requests
Please, always work on the BleedingEdge branch and make sure it is fully resolved before doing Pull Request from your fork.

## DRM Free downloads ##
Please select a verion from branch and download the ZIP in MOD directory.

## Rimworld LED Technology

A pack consisting of LED Technology.  [Version 0.7]
     
- LED Strip (8W) 
- LED Corner Strip (8W)
- LED 45 Degree Spot Light (25W)
- LED Security Light (50W) 
- LED Hydroponics (100W) 
 
## GlowFlooder Framework

1. Create a class that inherits from `: IGlowFlooder` 
2. Implement the interface code. Use the existing code as a guideline. Caching is very important aspect to keep the game smooth!
3. In your building register the glowFlooder using `CustomGlowFloodManager.RegisterFlooder(yourFlooder);`

Vanilla GlowComp will still work. They are processed first using the vanilla GlowFlooder. Then the customGlowflooder will process each next implementation as required, by calling the implemented interface method. 

Generally when you instantiate your glow flooder in a thing you should do all the calculations and save the "cell" grid into the cache object. The cache object will then be used to assign the colour into the vanilla glow grid.

Colour collisions are not handled by the customGlowFlooder. Custom implementations will overwrite existing glowComps, unless you specifically write your own glow collision detector and do with the colour as you please. You should also do some collision detection. There is not right or wrong way but generally speeking the more loops you have the longer it takes to execute. Think about shortcutting calculations and using as best optimised logic as possible.

# Have Fun!
