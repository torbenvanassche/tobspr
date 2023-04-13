I used gray-packing to optimize on performance a little bit. That is why the conveyor mask is just 3 solid colors.
This allows me to have a single mask texture instead of requiring 3 files, saving some hastle.
I sample the texture and split the RGB channels to use them independently.

I quickly made a model with ProBuilder, and unwrapped it in Blender (textured in PhotoShop).

The shader allows the speed of the belt to be provided via an exposed parameter, 
as well as a parameter for both the diffuse texture and mask texture.

In theory this method can be expanded on indefinitely for as many textures as is required.