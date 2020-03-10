# 999orange
Simple Currency Cheat for 100% Orange Juice [Steam]. All it does is set each currency type to their maximum values so you can buy whatever you want from the store. 

Note that this program will only be able to modify the following currency values stored locally:
- Stars
- Oranges
- Halloween Candy

(Depreciated: Christmas Candy, Valentines Chocolates - Read [the wiki](https://100orangejuice.gamepedia.com/Currency) for more information. Any currencies listed under Steam Inventory cannot be modified by this program)

With the steam inventory upgrade, more currencies are being moved to the steam inventory, which cannot be modified by this program, and thus you will not be able to buy all the cosmetics. I believe that fruitbat factory will eventually move all currencies (apart from stars) to the inventory, which will eventually render this cheat useless.

### [Latest Release (24/07/19)](https://github.com/tsuneko/Orange-Juice/releases/download/2.2.1/999percent.zip)
### Last verified working 11/03/20 ~ 100% Orange Juice 2.8.2

To find the currencies, 999orange utilises signature scanning to find where the currencies are stored in memory. As 999orange has been updated to use signature scanning rather than static offsets, if the game is updated then the offsets will not become outdated as easily.

### Latest Signatures:
Signatures are listed on the conf.ini file and are in the form `name=array,mask,offset,max_value`
- Stars: `stars=FF75C0E800000000FF35000000008BB3,xxxx????xx????xx,10,99999`
- Oranges: `oranges=81FF000000007CCAA1000000005335,xx????xxx????xx,9,999`
- Halloween Candy: `halloween=E8000000000FB6C0330500000000536A,x????xx?xx????xx,10,999`

A tutorial on finding signatures can be found [here](https://www.unknowncheats.me/forum/programming-for-beginners/171994-understanding-pattern-scanning-concept.html).

### Disclaimer:

100% Orange Juice is a VAC enabled game. I'm fairly sure VAC only checks process signatures when you are online in a match, so only use this program when you have the shop open. If you're scared of getting VAC banned, use cheat engine instead. If you're too scared to use cheat engine, I suggest you go earn your money legitimately.  Use entirely at own risk. I am not responsible for anything which can happen in regards to usage of my sources and/or releases. **Do not use the program while in an online match.**

As far as I am aware, this cheat will not result in a VAC ban as it only modifies currency values. There are no known cases of VAC bans due to this cheat.
