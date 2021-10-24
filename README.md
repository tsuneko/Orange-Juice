# 999% Orange Juice
Simple Currency Cheat for 100% Orange Juice [Steam]. All it does is set each currency type to their maximum values so you can buy whatever you want from the store. 

Note that this program will only be able to modify the following currency values stored locally:
- Stars
- Oranges
- Halloween Candy

(Depreciated: Christmas Candy, Valentines Chocolates - Read [the wiki](https://100orangejuice.gamepedia.com/Currency) for more information. Any currencies listed under Steam Inventory cannot be modified by this program)

With the steam inventory upgrade, more currencies are being moved to the steam inventory, which cannot be modified by this program, and thus you will not be able to buy all the cosmetics. I believe that fruitbat factory will eventually move all currencies (apart from stars) to the inventory, which will eventually render this cheat useless.

### [Latest Release (24/10/21)](https://github.com/tsuneko/Orange-Juice/archive/refs/tags/3.9.1+.zip)
### Last verified working 24/10/21 ~ 100% Orange Juice 3.9.1

**As the main 100% Orange Juice executable has been updated to use 64-bit architecture, please use the 32-bit executable (100orange_x86.exe) for this program to work.**
To find the currencies, 999percent utilises signature scanning to find where the currencies are stored in memory. As 999percent has been updated to use signature scanning rather than static offsets, if the game is updated then the offsets will not become outdated as easily.

### Latest Signatures:
Signatures are listed on the conf.ini file and are in the form `name=array,mask,offset,max_value`
- Stars: `stars=2B46080F48C1A3000000005E5D,xxxxxxx????xx,7,99999`
- Oranges: `oranges=5F0F49C6890D000000005E5D,xxxxxx????xx,6,999`
- Halloween Candy: `halloween=EB0233C0A3000000008985,xxxxx????xx,5,999`

A tutorial on finding signatures can be found [here](https://www.unknowncheats.me/forum/programming-for-beginners/171994-understanding-pattern-scanning-concept.html).

### Disclaimer:

100% Orange Juice is a VAC enabled game. VAC requires an active internet connection to function, and so it is usually safe to cheat if the game is launched without an internet connection.

Fruitbat Factory has [mentioned](https://steamcommunity.com/app/282800/discussions/0/1744480966997301464/) that you may be banned even without being in a match, however I believe their statement to be misleading as it conflicts with the design of VAC outlined [here](https://support.steampowered.com/kb_article.php?s=087dccfcc85be81977b078f1e4025bde&ref=7849-RADZ-6869).

### Do not attempt to play a match (singleplayer or multiplayer) with this cheat open.

For added safety, launch the game and use the cheat while disconnected from the internet.

999percent only modifies currency values stored locally, and so it is highly unlikely that its signature will be manually added to VAC's list of known cheats.

Use entirely at own risk. I am not responsible for anything which can happen in regards to usage of my sources and/or releases.

There are no known cases of VAC bans due to this cheat.
